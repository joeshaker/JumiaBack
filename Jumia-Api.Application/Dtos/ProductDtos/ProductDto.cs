using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.ProductDtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool IsAvailable { get; set; }
        public int StockQuantity { get; set; }

        public string MainImageUrl { get; set; }
        public List<string> AdditionalImageUrls { get; set; } = new();

        public List<ProductVariantDto> Variants { get; set; } = new();
        public List<ProductAttributeValueDto> Attributes { get; set; } = new();
    }

}
