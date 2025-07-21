using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.ProductDtos.Post
{
    public class AddProductDto
    {

        public int SellerId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal BasePrice { get; set; }
        [SwaggerSchema("Main product image")]
        public IFormFile MainImageUrl { get; set; }
        [SwaggerSchema("Additional images for the product")]
        public List<IFormFile> AdditionalImageUrls { get; set; } = new();
        public List<ProductAttributeDto> Attributes { get; set; } = new();
        public List<AddProductVariantDto> Variants { get; set; } = new();
    }
}
