using Jumia_Api.Application.Dtos.ProductDtos;
using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProductsByCategoriesAsync(ProductFilterRequestDto productFilterRequestDto);

    }
}
