using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jumia_Api.Application.Dtos.OrderDtos;

namespace Jumia_Api.Application.Dtos.PaymentDtos
{
    public class PaymentRequetsDto
    {
        public OrderDTO Order { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string PaymentMethod { get; set; } = "card";
    }
}
