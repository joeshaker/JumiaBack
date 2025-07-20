using Jumia_Api.Domain.Models;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Jumia_Api.Application.Interfaces;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Jumia_Api.Application.Dtos.PaymentDtos;
using EllipticCurve.Utils;
using Jumia_Api.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Jumia_Api.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _apiKey;
        private readonly string _integrationId;
        private readonly string _iframeId;
        private readonly UserManager<AppUser> _userManager;

        public PaymentService(HttpClient httpClient, IConfiguration config, IUnitOfWork unitOfWork,UserManager<AppUser> userManager)
        {
            _httpClient = httpClient;
            _config = config;
            _unitOfWork = unitOfWork;
            _apiKey = _config["Paymob:ApiKey"];
            _integrationId = _config["Paymob:IntegrationId"];
            _iframeId = _config["Paymob:CardIframeId"];
            _userManager = userManager;
        }

        public async Task<PaymentResponseDto> InitiatePaymentAsync(PaymentRequetsDto request)
        {
            try
            {
                var order = await _unitOfWork.OrderRepo.GetByIdAsync(request.OrderId);
                if (order == null)
                    return new() { Success = false, Message = "Order not found." };

                var token = await GetAuthTokenAsync();
                var paymobOrderId = await RegisterOrderAsync(token, request);
                var paymentKey = await GeneratePaymentKeyAsync(token, paymobOrderId, request);
                var iframeUrl = GetPaymentUrl(paymentKey, request.PaymentMethod);

                return new PaymentResponseDto
                {
                    Success = true,
                    PaymentUrl = iframeUrl,
                    TransactionId = paymobOrderId,
                    Message = "Payment initiated successfully"
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponseDto
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        private async Task<string> GetAuthTokenAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/auth/tokens", new { api_key = _apiKey });
            var json = await response.Content.ReadFromJsonAsync<JsonDocument>();
            return json.RootElement.GetProperty("token").GetString();
        }

        private async Task<string> RegisterOrderAsync(string token, PaymentRequetsDto request)
        {
            var orderRequest = new
            {
                auth_token = token,
                delivery_needed = "false",
                amount_cents = (int)(request.Amount * 100),
                currency = request.Currency,
                items = new[] {
                new {
                    name = $"Order #{request.OrderId}",
                    amount_cents = (int)(request.Amount * 100),
                    description = "Jumia Clone Order",
                    quantity = 1
                }
            }
            };

            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders", orderRequest);
            var json = await response.Content.ReadFromJsonAsync<JsonDocument>();
            return json.RootElement.GetProperty("id").ToString();
        }

        private async Task<string> GeneratePaymentKeyAsync(string token, string paymobOrderId, PaymentRequetsDto request)
        {
            var order = await _unitOfWork.OrderRepo.GetByIdAsync(request.OrderId);
            if (order == null)
                throw new Exception("Order not found.");

            var customer = await _unitOfWork.CustomerRepo.GetByIdAsync(order.CustomerId);
            if (customer == null)
                throw new Exception("Customer not found.");

            var user = await _userManager.FindByIdAsync(customer.UserId);

            if (user == null)
                throw new Exception("User not found.");

            var address = await _unitOfWork.AddressRepo.GetByIdAsync(order.AddressId);
            if (address == null)
                throw new Exception("Billing address not found.");

            var billing = new
            {
                first_name = user.FirstName ?? "N/A",              // Ensure FirstName exists in AppUser
                last_name = user.LastName ?? "N/A",                // Same here
                email = user.Email ?? "email@example.com",
                phone_number = address.PhoneNumber ?? "+201000000000",
                street = address.StreetAddress ?? "N/A",
                building = "N/A",                                   // Add building if available
                floor = "N/A",                                      // Add floor if stored
                apartment = "N/A",                                  // Add apartment if stored
                city = address.City ?? "N/A",
                state = address.State ?? address.City ?? "N/A",
                country = address.Country ?? "EG",
                postal_code = address.PostalCode ?? "00000"
            };

            var keyRequest = new
            {
                auth_token = token,
                amount_cents = (int)(request.Amount * 100),
                expiration = 3600,
                order_id = paymobOrderId,
                billing_data = billing,
                currency = request.Currency,
                integration_id = int.Parse(_integrationId)
            };

            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/acceptance/payment_keys", keyRequest);
            var json = await response.Content.ReadFromJsonAsync<JsonDocument>();
            return json.RootElement.GetProperty("token").GetString();
        }


        private string GetPaymentUrl(string paymentKey, string method)
        {
            return method.ToLower() switch
            {
                "card" => $"https://accept.paymob.com/api/acceptance/iframes/{_iframeId}?payment_token={paymentKey}",
                "vodafone" => $"https://accept.paymob.com/api/acceptance/payments/pay?payment_token={paymentKey}&source=mobile_wallet",
                "paypal" => $"https://accept.paymob.com/api/acceptance/payments/pay?payment_token={paymentKey}&source=paypal",
                _ => throw new ArgumentException("Unsupported payment method.")
            };
        }

        //public async Task<bool> ValidatePaymentCallback(string payload)
        //{
        //    // TODO: Add actual validation logic later
        //    return true;
        //}

        public async Task<bool> ValidatePaymentCallback(string payload)
        {
            try
            {
                var json = JsonDocument.Parse(payload);
                var root = json.RootElement;

                // Paymob sends "obj" or "order" in callback payload
                var success = root.GetProperty("success").GetBoolean();
                if (!success) return false;

                var orderId = root.GetProperty("order").GetProperty("merchant_order_id").GetInt32(); // or order.id if you stored Paymob Order ID

                var order = await _unitOfWork.OrderRepo.GetByIdAsync(orderId);
                if (order == null) return false;

                // ✅ Mark the order as paid
                order.Status = "Paid";
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}
