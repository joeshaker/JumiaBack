using Jumia_Api.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        
        
        IProductRepo ProductRepo { get; }
        IGenericRepo<T> Repository<T>() where T : class;
         ICategoryRepo CategoryRepo { get; }

        Task<int> SaveChangesAsync();


    }
}
