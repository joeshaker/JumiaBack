using Jumia_Api.Domain.Interfaces.UnitOfWork;
using Jumia_Api.Infrastructure.Presistence.Context;

namespace Jumia_Api.Infrastructure.Presistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JumiaDbContext _context;



        //private ExamRepo? _examRepo;
        //private QuestionRepo? _questionRepo;
        //private ChoiceRepo? _choiceRepo;
        //private UserExamResultRepo? _userExamResultRepo;
        //private IUserRepo? _userRepo;
        private readonly Dictionary<Type, object> _repositories = new();



        public UnitOfWork(JumiaDbContext context)
        {
            _context = context;

        }


        //public IUserRepo UserRepo => _userRepo ??= new UserRepo(_context);



        //public IExamRepo ExamRepo => _examRepo ??= new ExamRepo(_context);

        //public IQuestionRepo QuestionRepo => _questionRepo ?? new QuestionRepo(_context);

        //public IUserExamResultRepo ExamResultRepo => _userExamResultRepo ?? new UserExamResultRepo(_context);

        //public IChoiceRepo ChoiceRepo => _choiceRepo ?? new ChoiceRepo(_context);

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