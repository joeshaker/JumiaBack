using Jumia_Api.Application.Dtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Common.Results
{
    public class OrderCreationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public OrderDTO Order { get; set; }
        public Dictionary<string, object> ErrorDetails { get; set; } 

        public static OrderCreationResult SuccessResult(OrderDTO order)
        {
            return new OrderCreationResult { Success = true, Order = order };
        }

        public static OrderCreationResult FailureResult(string errorMessage, Dictionary<string, object> errorDetails = null)
        {
            return new OrderCreationResult { Success = false, ErrorMessage = errorMessage, ErrorDetails = errorDetails };
        }
    }
}
