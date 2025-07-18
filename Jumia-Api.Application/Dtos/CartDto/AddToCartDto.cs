using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.CartDto
{
    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int? VariantId { get; set; } // null if no variant
        public int Quantity { get; set; }
    }

}
