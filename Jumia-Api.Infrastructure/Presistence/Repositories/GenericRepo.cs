using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Infrastructure.Presistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Infrastructure.Presistence.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {

        protected readonly JumiaDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepo(JumiaDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        virtual public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);


        public virtual async Task Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            _dbSet.Remove(entity);

        }

        virtual public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();


        virtual public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);


        virtual public void Update(T entity) => _dbSet.Update(entity);



    }
}
