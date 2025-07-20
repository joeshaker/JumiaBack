using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    public class RatingRepo : GenericRepo<Rating>, IRatingRepo
    {

        public RatingRepo(JumiaDbContext context):base(context){
        }
        public async Task AddAsync(Rating entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Delete(int id)
        {
            var result= await GetByIdAsync(id);
            if (result != null) {
                _dbSet.Remove(result);
            }
        }

        public async Task<IEnumerable<Rating>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetAllByProductId(int productId)
        {
            return await _dbSet.Where(r => r.ProductId == productId).ToListAsync();
        }

        public async Task<Rating?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.RatingId == id);
        }

        public async void Update(Rating entity)
        {
             _dbSet.Update(entity);
        }
    }
}
