using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.ProductDtos
{
    public class ProductFilterRequestDto
    {
        public List<int> CategoryIds { get; set; }
        public Dictionary<string, string>? AttributeFilters { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
