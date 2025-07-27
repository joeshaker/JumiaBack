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
using Jumia_Api.Application.Dtos.OrderDtos;
using AutoMapper;

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
        private IMapper _mapper;

        public PaymentService(IMapper mapper,HttpClient httpClient, IConfiguration config, IUnitOfWork unitOfWork,UserManager<AppUser> userManager)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _config = config;
            _unitOfWork = unitOfWork;
            _apiKey = _config["Paymob:ApiKey"];
            _integrationId = _config["Paymob:IntegrationId"];
            _iframeId = _config["Paymob:CardIframeId"];
            _userManager = userManager;
        }

        public async Task<PaymentResponseDto> InitiatePaymentAsync(CreateOrderDTO orderDto)
        {
            try
            {
                // 1. Map and save order with suborders + items
                var newOrder = _mapper.Map<Order>(orderDto);

                await _unitOfWork.OrderRepo.AddAsync(newOrder);
                await _unitOfWork.SaveChangesAsync(); // OrderId is now generated

                // 2. Use the generated OrderId to build PaymentRequestDto internally
                var paymentRequest = new PaymentRequetsDto
                {

                    Order = orderDto,
                    OrderId = newOrder.OrderId,
                    Amount = newOrder.FinalAmount,
                    Currency = "EGP",
                    PaymentMethod = newOrder.PaymentMethod

                };

                // 3. Continue Paymob flow
                var token = await GetAuthTokenAsync();

                var paymobOrderId = await RegisterOrderAsync(token, paymentRequest);

                var paymentKey = await GeneratePaymentKeyAsync(token, paymobOrderId, paymentRequest);

                var iframeUrl = GetPaymentUrl(paymentKey, paymentRequest.PaymentMethod);

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
                merchant_order_id = $"{request.OrderId}_{request.Amount}_{request.Currency}_{request.PaymentMethod}",


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

            Console.WriteLine("fffffffffffffffff", json);
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            if (!json.RootElement.TryGetProperty("id", out var idElement))
                throw new Exception("Paymob order registration response did not contain 'id'.");

            return idElement.ToString();
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
                Console.WriteLine("Validating callback payload: " + payload);
                var json = JsonDocument.Parse(payload);
                var root = json.RootElement;

                // ✅ Get success flag
                bool success = root.TryGetProperty("success", out var successElement)
                               && bool.TryParse(successElement.GetString(), out var isSuccess)
                               && isSuccess;

                // ✅ Extract order ID from merchant_order_id
                if (!root.TryGetProperty("merchant_order_id", out var merchantIdElement))
                    return false;

                var merchantIdStr = merchantIdElement.GetString();
                var parts = merchantIdStr?.Split('_');
                if (parts == null || parts.Length == 0 || !int.TryParse(parts[0], out int orderId))
                    return false;

                // ✅ Load order
                var order = await _unitOfWork.OrderRepo.GetByIdAsync(orderId);
                if (order == null) return false;

                if (success)
                {
                    order.Status = "Paid";
                    order.PaymentStatus = "Paid";
                }
                else
                {
                    _unitOfWork.OrderRepo.Delete(order.OrderId); // Ensure cascade delete is enabled
                }
                await _unitOfWork.CartRepo.ClearCartAsync(order.CustomerId); // Clear cart after payment
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Callback validation failed: " + ex.Message);
                return false;
            }
        }



    }

}
