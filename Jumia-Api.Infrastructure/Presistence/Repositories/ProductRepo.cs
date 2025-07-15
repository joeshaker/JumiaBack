using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        public ProductRepo(JumiaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
            => await _dbSet
                .Where(p => p.IsAvailable)
                .AsNoTracking()
                .ToListAsync();

       

        public async Task<List<Product>> GetProductsByCategoryIdsAsync(List<int> categoryIds,
                                                                Dictionary<string, string> attributeFilters = null,
                                                                decimal? minPrice = null,
                                                                decimal? maxPrice = null)
        {
            var query = _dbSet
                .Where(p => categoryIds.Contains(p.CategoryId) && p.IsAvailable)
                .AsQueryable();

            if (attributeFilters != null && attributeFilters.Any())
            {
                foreach (var filter in attributeFilters)
                {
                    string attributeName = filter.Key;
                    string attributeValue = filter.Value;

                    query = query.Where(p => p.AttributeValues
                        .Any(av => av.ProductAttribute.Name == attributeName && av.Value == attributeValue));
                }

            }
            if (minPrice.HasValue)
                query = query.Where(p => p.BasePrice >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.BasePrice <= maxPrice.Value);

            return await query.AsNoTracking().ToListAsync();
        }
    }
    }

