using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jumia_Api.Application.Dtos.OrderDtos;
using Jumia_Api.Application.Dtos.PaymentDtos;

namespace Jumia_Api.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> InitiatePaymentAsync(CreateOrderDTO request);
        Task<bool> ValidatePaymentCallback(string payload);
    }
}
