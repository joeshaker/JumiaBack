using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.ProductDtos.Get
{
    public class ProductsUIDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public decimal DiscountPercentage { get; set; } 
        public string ImageUrl { get; set; }

    }
}
