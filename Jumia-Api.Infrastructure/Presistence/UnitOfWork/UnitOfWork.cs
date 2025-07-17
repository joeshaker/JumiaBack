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

        private IUserRepository _userRep;

        private IProductAttributeRepo? _productAttributeRepo;

        private readonly Dictionary<Type, object> _repositories = new();



        public UnitOfWork(JumiaDbContext context)
        {
            _context = context;

        }

        public IProductRepo ProductRepo => _productRepo ??= new ProductRepo(_context);


        //public IUserRepo UserRepo => _userRepo ??= new UserRepo(_context);



        //public IExamRepo ExamRepo => _examRepo ??= new ExamRepo(_context);

        //public IQuestionRepo QuestionRepo => _questionRepo ?? new QuestionRepo(_context);

        //public IUserExamResultRepo ExamResultRepo => _userExamResultRepo ?? new UserExamResultRepo(_context);

        //public IChoiceRepo ChoiceRepo => _choiceRepo ?? new ChoiceRepo(_context);
        public ICategoryRepo CategoryRepo => _categoryRepo ?? new CategoryRepository(_context);
        public IUserRepository UserRepo =>_userRep ??= new UserRepository(_userManager);

        public IProductAttributeRepo ProductAttributeRepo => _productAttributeRepo ?? new ProductAttributeRepo(_context);

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