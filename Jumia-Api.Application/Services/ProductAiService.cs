using Jumia_Api.Application.Dtos.ProductDtos;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Jumia_Api.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Services
{
    public class ProductAiService : IProductAiService
    {
        //    private readonly IOpenAiClient _openAiClient;
        //    private readonly IUnitOfWork _unitOfWork;


        //    public ProductAiService(IOpenAiClient openAiClient,
        //                            IUnitOfWork unitOfWork)
        //    {
        //        _openAiClient = openAiClient;
        //        _unitOfWork = unitOfWork;

        //    }

        //    public async Task<ProductAttributeValueDto> ParseQueryToFilterAsync(string query)
        //    {
        //        var categories = await _unitOfWork.CategoryRepo.GetAllAsync();
        //        var prompt = BuildPrompt(query, categories);

        //        var response = await _openAiClient.CompleteAsync(prompt);
        //        return JsonConvert.DeserializeObject<ProductAttributeValueDto>(response);
        //    }

        //    public async Task<ProductAttributeValueDto> GetSimilarProductsFilterAsync(int productId)
        //    {
        //        var product = await _unitOfWork.ProductRepo.GetByIdAsync(productId);
        //        if (product == null) throw new Exception("Product not found");

        //        var prompt = $"""
        //    You are an AI assistant. Create a product filter JSON to find items similar to:
        //    - Name: "{product.Name}"
        //    - Attributes: {JsonConvert.SerializeObject(product.productAttributeValues)}
        //    - Category: {product.Category.Name}
        //    """;

        //        var response = await _openAiClient.CompleteAsync(prompt);
        //        return JsonConvert.DeserializeObject<ProductAttributeValueDto>(response);
        //    }

        //    private string BuildPrompt(string query, IEnumerable<Category> categories)
        //    {
        //        var categoryList = JsonConvert.SerializeObject(categories.Select(c => c.Name));
        //        return $@"
        //    You are a smart e-commerce assistant.
        //    Available categories: {categoryList}

        //    User Query: ""{query}""

        //    Extract category, attributes, price, and sorting in JSON:
        //    {{
        //        ""categoryIds"": [int],
        //        ""attributes"": {{""Color"": ""Red"", ""Size"": ""M""}},
        //        ""minPrice"": decimal?,
        //        ""maxPrice"": decimal?,
        //        ""onlyAvailable"": bool,
        //        ""sortBy"": ""priceAsc|priceDesc|newest|popularity""
        //    }}";
        //    }


        //    public async Task<string> AnswerProductQuestionAsync(string question, int? productId = null)
        //    {
        //        string context;

        //        if (productId.HasValue)
        //        {
        //            var product = await _unitOfWork.ProductRepo.GetByIdAsync(productId.Value);
        //            if (product == null) throw new Exception("Product not found");

        //            context = $"""
        //    Product Details:
        //    Name: {product.Name}
        //    Description: {product.Description}
        //    Price: {product.BasePrice:C}
        //    Attributes: {JsonConvert.SerializeObject(product.productAttributeValues.Select(a => new { a.ProductAttribute.Name, a.Value }))}
        //    Variants: {JsonConvert.SerializeObject(product.ProductVariants.Select(v => new { v.VariantName, v.Price, v.StockQuantity }))}
        //    """;
        //        }
        //        else
        //        {
        //            var allProducts = await _unitOfWork.ProductRepo.GetAllAsync();
        //            context = $"""
        //    Catalog Summary:
        //    {string.Join("\n", allProducts.Select(p => $"- {p.Name}: {p.Description} (${p.BasePrice})"))}
        //    """;
        //        }

        //        var prompt = $"""
        //You are a helpful e-commerce assistant.
        //Context:
        //{context}

        //User Question: "{question}"

        //Answer concisely and use only the provided context.
        //""";

        //        var response = await _openAiClient.CompleteAsync(prompt);
        //        return response;
        //    }
        public Task<string> AnswerProductQuestionAsync(string question, int? productId = null)
        {
            throw new NotImplementedException();
        }

        public Task<ProductAttributeDto> GetSimilarProductsFilterAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductAttributeDto> ParseQueryToFilterAsync(string query)
        {
            throw new NotImplementedException();
        }
    }
}
