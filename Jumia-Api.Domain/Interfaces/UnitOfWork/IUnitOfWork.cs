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
        IGenericRepo<T> Repository<T>() where T : class;
         ICategoryRepo CategoryRepo { get; }
        ICouponRepo CouponRepo { get; }

        IUserCouponRepo UserCouponRepo { get; }



        Task<int> SaveChangesAsync();


    }
}
