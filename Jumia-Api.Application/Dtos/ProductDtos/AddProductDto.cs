using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.ProductDtos
{
    public class AddProductDto
    {
       
        public int SellerId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public int StockQuantity { get; set; }
        public string MainImageUrl { get; set; }

        public List<ProductAttributeValueDto> Attributes { get; set; }
        public List<ProductVariantDto> Variants { get; set; }
        public List<string> AdditionalImageUrls { get; set; }
    }
}
