using Jumia_Api.Domain.Interfaces.Repositories;
using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Jumia_Api.Domain.Models;
using Jumia_Api.Infrastructure.Presistence.Context;
using Jumia_Api.Infrastructure.Presistence.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Jumia_Api.Infrastructure.Presistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JumiaDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private ICategoryRepo? _categoryRepo;
        private IProductRepo? _productRepo;
        private IProductAttributeRepo? _productAttributeRepo;

        private IOrderRepository? _orderRepository;

        private IAddressRepo? _addressRepo;

        private readonly Dictionary<Type, object> _repositories = new();



        public UnitOfWork(JumiaDbContext context)
        {
            _context = context;

        }

        public IProductRepo ProductRepo => _productRepo ??= new ProductRepo(_context);


   
        public ICategoryRepo CategoryRepo => _categoryRepo ?? new CategoryRepository(_context);


        public IProductAttributeRepo ProductAttributeRepo => _productAttributeRepo ?? new ProductAttributeRepo(_context);


        public IOrderRepository OrderRepo => _orderRepository ?? new OrderRepository(_context);

        public IAddressRepo AddressRepo => _addressRepo?? new AddressRepo(_context);    


        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepo<T> Repository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repo))
                return (IGenericRepo<T>)repo;

            var newRepo = new GenericRepo<T>(_context);
            _repositories.Add(typeof(T), newRepo);
            return newRepo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }




    }
}