//using AutoMapper;
//using Jumia_Api.Application.Dtos.ProductDtos.Get;
//using Jumia_Api.Application.Interfaces;
//using Jumia_Api.Domain.Interfaces.UnitOfWork;
//using Jumia_Api.Domain.Models;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Json;
//using System.Text;
//using System.Threading.Tasks;

//namespace Jumia_Api.Infrastructure.External_Services
//{
//    public class RecommendationService : IRecommendationService
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        private readonly TimeSpan _aiTimeout = TimeSpan.FromSeconds(10);
//        private readonly TimeSpan _basicTimeout = TimeSpan.FromSeconds(15);

//        public RecommendationService(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }

//        public async Task<IEnumerable<ProductsUIDto>> GetRecommendationsAsync(int userId)
//        {
//            try
//            {
//                // First try AI recommendations
//                var aiRecommendations = await GetAIRecommendationsWithTimeoutAsync(userId);
//                if (aiRecommendations.Any())
//                    return aiRecommendations;
//            }
//            catch
//            {
//                // AI failed, continue to basic
//            }

//            // Fallback to basic recommendations
//            var basicRecommendations = await GetBasicRecommendationsWithTimeoutAsync(userId);
//            if (basicRecommendations.Any())
//                return basicRecommendations;

//            // Final fallback to random products
//            return await GetRandomProductsAsync(6);
//        }

//        private async Task<IEnumerable<ProductsUIDto>> GetAIRecommendationsWithTimeoutAsync(int userId)
//        {
//            using var cts = new CancellationTokenSource(_aiTimeout);
//            try
//            {
//                return await GetAIRecommendationsCoreAsync(userId, cts.Token);
//            }
//            catch (OperationCanceledException)
//            {
//                return Enumerable.Empty<ProductsUIDto>();
//            }
//            catch
//            {
//                return Enumerable.Empty<ProductsUIDto>();
//            }
//        }

//        private async Task<IEnumerable<ProductsUIDto>> GetAIRecommendationsCoreAsync(int userId, CancellationToken ct)
//        {
//            // Get user interaction data
//            var cart = await _unitOfWork.CartRepo.GetCustomerCartAsync(userId);
//            var orders = await _unitOfWork.OrderRepo.GetByCustomerIdAsync(userId);
//            var wishlist = await _unitOfWork.WishlistRepo.GetCustomerWishlistAsync(userId);

//            // Get product catalog
//            var allProducts = await _unitOfWork.ProductRepo.GetAllAsync();

//            // Construct prompt
//            var prompt = ConstructPrompt(cart, orders, wishlist, allProducts);

//            // Get AI response
//            var aiResponse = await GetAIResponseAsync(prompt, ct);

//            // Parse response
//            var recommendedProducts = ParseAIResponse(aiResponse, allProducts);
//            return _mapper.Map<IEnumerable<ProductsUIDto>>(recommendedProducts);
//        }

//        private string ConstructPrompt(
//            Cart cart,
//            IEnumerable<Order> orders,
//            Wishlist wishlist,
//            IEnumerable<Product> allProducts)
//        {
//            var sb = new StringBuilder();
//            sb.AppendLine("Generate 6 product recommendations based on user interactions:");

//            // Extract products from cart
//            var cartProducts = cart?.CartItems?.Select(i => i.Product) ?? Enumerable.Empty<Product>();
//            AppendProductSection(sb, "Cart", cartProducts);

//            // Extract products from orders
//            var orderProducts = orders?
//                .SelectMany(o => o.SubOrders)
//                .SelectMany(so => so.OrderItems)
//                .Select(oi => oi.Product)
//                .DistinctBy(p => p.ProductId)
//                ?? Enumerable.Empty<Product>();
//            AppendProductSection(sb, "Orders", orderProducts);

//            // Extract products from wishlist
//            var wishlistProducts = wishlist?.WishlistItems?.Select(i => i.Product) ?? Enumerable.Empty<Product>();
//            AppendProductSection(sb, "Wishlist", wishlistProducts);

//            sb.AppendLine("\n**Available Products (ID, Name, Category, Price):**");
//            foreach (var p in allProducts)
//            {
//                sb.AppendLine($"{p.ProductId}: {p.Name} | {p.Category?.Name} | ${p.BasePrice}");
//            }

//            sb.AppendLine("\n**Instructions:**");
//            sb.AppendLine("- Recommend 6 relevant products from available products");
//            sb.AppendLine("- Return only product IDs in comma-separated format");
//            sb.AppendLine("- Example: 123,456,789,101,112,113");

//            return sb.ToString();
//        }

//        private void AppendProductSection(StringBuilder sb, string title, IEnumerable<Product> products)
//        {
//            if (products == null || !products.Any()) return;

//            sb.AppendLine($"{title} ({products.Count()} items):");
//            foreach (var p in products)
//            {
//                sb.AppendLine($"- {p.Name} (ID: {p.ProductId}, Category: {p.Category?.Name}, Price: ${p.BasePrice})");
//            }
//        }

//        private async Task<string> GetAIResponseAsync(string prompt, CancellationToken ct)
//        {
//            using var client = new HttpClient();
//            client.Timeout = TimeSpan.FromSeconds(10);

//            var request = new
//            {
//                model = "qwen3:0.6b",
//                prompt,
//                stream = false
//            };

//            var response = await client.PostAsJsonAsync("http://localhost:11434/api/generate", request, ct);
//            response.EnsureSuccessStatusCode();

//            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
//            return result?.Response ?? string.Empty;
//        }

//        private IEnumerable<Product> ParseAIResponse(string response, IEnumerable<Product> allProducts)
//        {
//            try
//            {
//                var productIds = response.Split(',')
//                    .Select(id => int.TryParse(id.Trim(), out var result) ? result : -1)
//                    .Where(id => id > 0)
//                    .Distinct()
//                    .Take(6)
//                    .ToArray();

//                return allProducts
//                    .Where(p => productIds.Contains(p.ProductId))
//                    .Take(6);
//            }
//            catch
//            {
//                return Enumerable.Empty<Product>();
//            }
//        }

//        private async Task<IEnumerable<ProductsUIDto>> GetBasicRecommendationsWithTimeoutAsync(int userId)
//        {
//            using var cts = new CancellationTokenSource(_basicTimeout);
//            try
//            {
//                var recommendations = await GetUserBasedRecommendationsAsync(userId, cts.Token);
//                return _mapper.Map<IEnumerable<ProductsUIDto>>(recommendations);
//            }
//            catch (OperationCanceledException)
//            {
//                return Enumerable.Empty<ProductsUIDto>();
//            }
//            catch
//            {
//                return Enumerable.Empty<ProductsUIDto>();
//            }
//        }

//        private async Task<IEnumerable<Product>> GetUserBasedRecommendationsAsync(int userId, CancellationToken ct)
//        {
//            // Get user interactions in parallel
//            var cartTask = _unitOfWork.CartRepo.GetCustomerCartAsync(userId);
//            var ordersTask = _unitOfWork.OrderRepo.GetByCustomerIdAsync(userId);
//            var wishlistTask = _unitOfWork.WishlistRepo.GetCustomerWishlistAsync(userId);

//            await Task.WhenAll(cartTask, ordersTask, wishlistTask);

//            var cart = await cartTask;
//            var orders = await ordersTask;
//            var wishlist = await wishlistTask;

//            // Extract interacted product IDs
//            var cartProductIds = cart?.CartItems?.Select(i => i.ProductId) ?? Enumerable.Empty<int>();
//            var orderProductIds = orders?
//                .SelectMany(o => o.SubOrders)
//                .SelectMany(so => so.OrderItems)
//                .Select(oi => oi.ProductId)
//                ?? Enumerable.Empty<int>();
//            var wishlistProductIds = wishlist?.WishlistItems?.Select(i => i.ProductId) ?? Enumerable.Empty<int>();

//            var allInteractedIds = cartProductIds
//                .Concat(orderProductIds)
//                .Concat(wishlistProductIds)
//                .Distinct()
//                .ToList();

//            if (!allInteractedIds.Any())
//                return Enumerable.Empty<Product>();

//            // Get interacted products details
//            var interactedProducts = await _unitOfWork.ProductRepo.GetbyIdsWithVariantsAndAttributesAsync(allInteractedIds);
//            var recommendations = new List<Product>();

//            foreach (var product in interactedProducts)
//            {
//                var similar = await GetSimilarProductsAsync(product.ProductId);
//                recommendations.AddRange(similar);
//            }

//            // Remove duplicates and interacted products
//            return recommendations
//                .Where(p => !allInteractedIds.Contains(p.ProductId))
//                .DistinctBy(p => p.ProductId)
//                .Take(6)
//                .ToList();
//        }

//        private async Task<IEnumerable<Product>> GetSimilarProductsAsync(int productId)
//        {
//            var targetProduct = await _unitOfWork.ProductRepo.GetByIdAsync(productId);
//            if (targetProduct == null) return Enumerable.Empty<Product>();

//            var allProducts = (await _unitOfWork.ProductRepo.GetAllAsync()).ToList();

//            return allProducts
//                .Where(p => p.ProductId != productId)
//                .OrderByDescending(p =>
//                    (p.CategoryId == targetProduct.CategoryId ? 1 : 0) +
//                    (Math.Abs(p.BasePrice - targetProduct.BasePrice) <= 10 ? 1 : 0) +
//                    (p.SellerId == targetProduct.SellerId ? 1 : 0))
//                .Take(6)
//                .ToList();
//        }

//        private async Task<IEnumerable<ProductsUIDto>> GetRandomProductsAsync(int count)
//        {
//            var allProducts = await _unitOfWork.ProductRepo.GetAllAsync();
//            var randomProducts = allProducts.OrderBy(r => Guid.NewGuid()).Take(count);
//            return _mapper.Map<IEnumerable<ProductsUIDto>>(randomProducts);
//        }

//        private class OllamaResponse
//        {
//            [JsonProperty("response")]
//            public string Response { get; set; }
//        }
//    }
//}
using AutoMapper;
using AutoMapper;
using Jumia_Api.Application.Dtos.ProductDtos.Get;
using Jumia_Api.Application.Interfaces;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Jumia_Api.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jumia_Api.Infrastructure.External_Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly TimeSpan _aiTimeout = TimeSpan.FromSeconds(10);
        private readonly TimeSpan _basicTimeout = TimeSpan.FromSeconds(15);

        public RecommendationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDetailsDto>> GetRecommendationsAsync(int userId)
        {
            try
            {
                var aiRecommendations = await GetAIRecommendationsWithTimeoutAsync(userId);
                if (aiRecommendations.Any()) return aiRecommendations;
            }
            catch { /* AI failed, continue to basic */ }

            var basicRecommendations = await GetBasicRecommendationsWithTimeoutAsync(userId);
            if (basicRecommendations.Any()) return basicRecommendations;

            return await GetFallbackRecommendationsAsync(userId);
        }

        #region Fallback Recommendation Logic
        private async Task<IEnumerable<ProductDetailsDto>> GetFallbackRecommendationsAsync(int userId)
        {
            try
            {
                // Get user's interaction products
                var interactionProducts = await GetUserInteractionProductsAsync(userId);

                if (interactionProducts.Any())
                {
                    return PickRandomProducts(interactionProducts, 6);
                }

                // If no interaction history, return top-selling products
                return await GetTopSellingProductsAsync(6);
            }
            catch
            {
                // Final safety net
                return await GetRandomProductsAsync(6);
            }
        }

        private async Task<IEnumerable<Product>> GetUserInteractionProductsAsync(int userId)
        {
            var cartTask = _unitOfWork.CartRepo.GetCustomerCartAsync(userId);
            var ordersTask = _unitOfWork.OrderRepo.GetByCustomerIdAsync(userId);
            var wishlistTask = _unitOfWork.WishlistRepo.GetCustomerWishlistAsync(userId);

            await Task.WhenAll(cartTask, ordersTask, wishlistTask);

            var cart = await cartTask;
            var orders = await ordersTask;
            var wishlist = await wishlistTask;

            // Extract product IDs from all sources
            var cartProductIds = cart?.CartItems?.Select(i => i.ProductId) ?? Enumerable.Empty<int>();
            var orderProductIds = orders?
                .SelectMany(o => o.SubOrders)
                .SelectMany(so => so.OrderItems)
                .Select(oi => oi.ProductId)
                ?? Enumerable.Empty<int>();
            var wishlistProductIds = wishlist?.WishlistItems?.Select(i => i.ProductId) ?? Enumerable.Empty<int>();

            // Combine and remove duplicates
            var allProductIds = cartProductIds
                .Concat(orderProductIds)
                .Concat(wishlistProductIds)
                .Distinct()
                .ToList();

            if (!allProductIds.Any()) return Enumerable.Empty<Product>();

            // Fetch current product details with categories
            return await _unitOfWork.ProductRepo.GetbyIdsWithVariantsAndAttributesAsync(allProductIds);
        }

        private async Task<IEnumerable<ProductDetailsDto>> GetTopSellingProductsAsync(int count)
        {
            // Get all orders with details
            var allOrders = await _unitOfWork.OrderRepo.GetAllWithDetailsAsync();

            // Extract and aggregate order items
            var allOrderItems = allOrders
                .SelectMany(o => o.SubOrders)
                .SelectMany(so => so.OrderItems);

            // Calculate total quantities sold per product
            var productSales = allOrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(ps => ps.TotalQuantity)
                .Take(100) // Take extra to account for out-of-stock items
                .ToList();

            // Get product details for top sellers
            var productIds = productSales.Select(ps => ps.ProductId).ToList();
            var products = (await _unitOfWork.ProductRepo.GetbyIdsWithVariantsAndAttributesAsync(productIds))
                .Where(p => p.StockQuantity > 0) // Filter out out-of-stock
                .OrderByDescending(p =>
                    productSales.FirstOrDefault(ps => ps.ProductId == p.ProductId)?.TotalQuantity ?? 0)
                .Take(count)
                .ToList();

            return _mapper.Map<IEnumerable<ProductDetailsDto>>(products);
        }

        private IEnumerable<ProductDetailsDto> PickRandomProducts(IEnumerable<Product> products, int count)
        {
            var availableProducts = products
                .Where(p => p.StockQuantity > 0) // Ensure stock available
                .ToList();

            if (!availableProducts.Any())
                return Enumerable.Empty<ProductDetailsDto>();

            var random = new Random();
            return _mapper.Map<IEnumerable<ProductDetailsDto>>(
                availableProducts
                    .OrderBy(p => random.Next())
                    .Take(count)
            );
        }
        #endregion

        #region AI Recommendation Logic
        private async Task<IEnumerable<ProductDetailsDto>> GetAIRecommendationsWithTimeoutAsync(int userId)
        {
            using var cts = new CancellationTokenSource(_aiTimeout);
            try
            {
                return await GetAIRecommendationsCoreAsync(userId, cts.Token);
            }
            catch
            {
                return Enumerable.Empty<ProductDetailsDto>();
            }
        }

        private async Task<IEnumerable<ProductDetailsDto>> GetAIRecommendationsCoreAsync(int userId, CancellationToken ct)
        {
            var cart = await _unitOfWork.CartRepo.GetCustomerCartAsync(userId);
            var orders = await _unitOfWork.OrderRepo.GetByCustomerIdAsync(userId);
            var wishlist = await _unitOfWork.WishlistRepo.GetCustomerWishlistAsync(userId);
            var allProducts = await _unitOfWork.ProductRepo.GetAllAsync();

            var prompt = ConstructPrompt(cart, orders, wishlist, allProducts);
            var aiResponse = await GetAIResponseAsync(prompt, ct);
            var recommendedProducts = ParseAIResponse(aiResponse, allProducts);

            // Filter out out-of-stock products and map to DTO
            return _mapper.Map<IEnumerable<ProductDetailsDto>>(
                recommendedProducts.Where(p => p.StockQuantity > 0)
            );
        }

        private string ConstructPrompt(
            Cart cart,
            IEnumerable<Order> orders,
            Wishlist wishlist,
            IEnumerable<Product> allProducts)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Generate 6 product recommendations based on user interactions:");

            // Include stock information in prompt
            void AppendProductSection(StringBuilder sb, string title, IEnumerable<Product> products)
            {
                if (products == null || !products.Any()) return;
                sb.AppendLine($"{title} ({products.Count()} items):");
                foreach (var p in products)
                {
                    sb.AppendLine($"- {p.Name} (ID: {p.ProductId}, Category: {p.Category?.Name}, " +
                                  $"Price: ${p.BasePrice}, Stock: {p.StockQuantity})");
                }
            }

            // Existing section appending logic
            AppendProductSection(sb, "Cart", cart?.CartItems?.Select(i => i.Product));
            AppendProductSection(sb, "Orders", orders?
                .SelectMany(o => o.SubOrders)
                .SelectMany(so => so.OrderItems)
                .Select(oi => oi.Product)
                .DistinctBy(p => p.ProductId));
            AppendProductSection(sb, "Wishlist", wishlist?.WishlistItems?.Select(i => i.Product));

            sb.AppendLine("\n**Available Products (ID, Name, Category, Price, Stock):**");
            foreach (var p in allProducts)
            {
                sb.AppendLine($"{p.ProductId}: {p.Name} | {p.Category?.Name} | ${p.BasePrice} | {p.StockQuantity} in stock");
            }

            sb.AppendLine("\n**Instructions:**");
            sb.AppendLine("- Recommend 6 relevant IN-STOCK products (stock > 0)");
            sb.AppendLine("- Return only product IDs in comma-separated format");
            sb.AppendLine("- Example: 123,456,789,101,112,113");

            return sb.ToString();
        }

        private async Task<string> GetAIResponseAsync(string prompt, CancellationToken ct)
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var response = await client.PostAsJsonAsync(
                "http://localhost:11434/api/generate",
                new { model = "qwen3:0.6b", prompt, stream = false },
                ct
            );

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
            return result?.Response ?? string.Empty;
        }

        private IEnumerable<Product> ParseAIResponse(string response, IEnumerable<Product> allProducts)
        {
            try
            {
                var productIds = response.Split(',')
                    .Select(id => int.TryParse(id.Trim(), out var result) ? result : -1)
                    .Where(id => id > 0)
                    .Distinct()
                    .Take(6)
                    .ToArray();

                return allProducts
                    .Where(p => productIds.Contains(p.ProductId))
                    .Take(6);
            }
            catch
            {
                return Enumerable.Empty<Product>();
            }
        }
        #endregion

        #region Basic Recommendation Logic
        private async Task<IEnumerable<ProductDetailsDto>> GetBasicRecommendationsWithTimeoutAsync(int userId)
        {
            using var cts = new CancellationTokenSource(_basicTimeout);
            try
            {
                return await GetUserBasedRecommendationsAsync(userId, cts.Token);
            }
            catch
            {
                return Enumerable.Empty<ProductDetailsDto>();
            }
        }

        private async Task<IEnumerable<ProductDetailsDto>> GetUserBasedRecommendationsAsync(int userId, CancellationToken ct)
        {
            // Get user interactions in parallel
            var cartTask = _unitOfWork.CartRepo.GetCustomerCartAsync(userId);
            var ordersTask = _unitOfWork.OrderRepo.GetByCustomerIdAsync(userId);
            var wishlistTask = _unitOfWork.WishlistRepo.GetCustomerWishlistAsync(userId);
            await Task.WhenAll(cartTask, ordersTask, wishlistTask);

            // Extract interacted product IDs
            var cartProductIds = (await cartTask)?.CartItems?.Select(i => i.ProductId) ?? Enumerable.Empty<int>();
            var orderProductIds = (await ordersTask)?
                .SelectMany(o => o.SubOrders)
                .SelectMany(so => so.OrderItems)
                .Select(oi => oi.ProductId)
                ?? Enumerable.Empty<int>();
            var wishlistProductIds = (await wishlistTask)?.WishlistItems?.Select(i => i.ProductId) ?? Enumerable.Empty<int>();

            var allInteractedIds = cartProductIds
                .Concat(orderProductIds)
                .Concat(wishlistProductIds)
                .Distinct()
                .ToList();

            if (!allInteractedIds.Any()) return Enumerable.Empty<ProductDetailsDto>();

            // Get interacted products with categories
            var interactedProducts = (await _unitOfWork.ProductRepo.GetbyIdsWithVariantsAndAttributesAsync(allInteractedIds))
                .Where(p => p.StockQuantity > 0) // Filter out-of-stock
                .ToList();

            var recommendations = new List<Product>();
            foreach (var product in interactedProducts)
            {
                var similar = await GetSimilarProductsAsync(product.ProductId);
                recommendations.AddRange(similar);
            }

            // Remove duplicates and interacted products
            return _mapper.Map<IEnumerable<ProductDetailsDto>>(
                recommendations
                    .Where(p => !allInteractedIds.Contains(p.ProductId))
                    .DistinctBy(p => p.ProductId)
                    .Take(6)
            );
        }

        private async Task<IEnumerable<Product>> GetSimilarProductsAsync(int productId)
        {
            var targetProduct = await _unitOfWork.ProductRepo.GetByIdAsync(productId);
            if (targetProduct == null) return Enumerable.Empty<Product>();

            var allProducts = (await _unitOfWork.ProductRepo.GetAllAsync())
                .Where(p => p.StockQuantity > 0) // Only in-stock products
                .ToList();

            return allProducts
                .Where(p => p.ProductId != productId)
                .OrderByDescending(p =>
                    (p.CategoryId == targetProduct.CategoryId ? 1 : 0) +
                    (Math.Abs(p.BasePrice - targetProduct.BasePrice) <= 10 ? 1 : 0) +
                    (p.SellerId == targetProduct.SellerId ? 1 : 0))
                .Take(6)
                .ToList();
        }
        #endregion

        #region Utility Methods
        private async Task<IEnumerable<ProductDetailsDto>> GetRandomProductsAsync(int count)
        {
            var allProducts = await _unitOfWork.ProductRepo.GetAllAsync();
            var availableProducts = allProducts
                .Where(p => p.StockQuantity > 0) // Filter out-of-stock
                .ToList();

            var random = new Random();
            var randomProducts = availableProducts
                .OrderBy(p => random.Next())
                .Take(count);

            return _mapper.Map<IEnumerable<ProductDetailsDto>>(randomProducts);
        }

        private class OllamaResponse
        {
            [JsonProperty("response")]
            public string Response { get; set; }
        }
        #endregion
    }
}