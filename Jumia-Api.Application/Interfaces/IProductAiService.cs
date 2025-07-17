using Jumia_Api.Application.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Interfaces
{
    public interface IProductAiService
    {
        Task<ProductAttributeDto> ParseQueryToFilterAsync(string query);
        Task<ProductAttributeDto> GetSimilarProductsFilterAsync(int productId);
        Task<string> AnswerProductQuestionAsync(string question, int? productId = null);
    }
}
