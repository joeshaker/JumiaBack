using Jumia_Api.Application.Common.Results;
using Jumia_Api.Application.Dtos.ProductDtos;
using Jumia_Api.Application.Dtos.ProductDtos.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.AiDtos
{
    public class AiQueryResponseDto
    {
        public string OriginalQuery { get; set; }
        public ProductFilterRequestDto Filters { get; set; }
        public PagedResult<ProductDetailsDto> Products { get; set; }
    }

}
