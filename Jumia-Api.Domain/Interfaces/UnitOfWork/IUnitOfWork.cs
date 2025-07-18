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

        IProductAttributeRepo ProductAttributeRepo { get; }
        IProductRepo ProductRepo { get; }
        IAddressRepo AddressRepo { get; }
        ICartRepo CartRepo { get; }
        IGenericRepo<T> Repository<T>() where T : class;
         ICategoryRepo CategoryRepo { get; }
        ICartItemRepo CartItemRepo { get; }
        ICustomerRepo CustomerRepo { get; }

        Task<int> SaveChangesAsync();


    }
}
