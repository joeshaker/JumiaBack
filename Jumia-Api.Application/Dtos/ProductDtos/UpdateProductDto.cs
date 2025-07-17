using Jumia_Api.Application.Dtos.ProductDtos.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.ProductDtos
{
    public class UpdateProductDto:AddProductDto
    {
        public int ProductId { get; set; }
    }
}
