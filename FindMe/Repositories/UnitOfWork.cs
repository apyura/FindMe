using AutoMapper;
using FindMe.Models;

namespace FindMe.Repositories
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ApplicationContext _applicationContext;
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UnitOfWork(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }

        public void Save()
        {
            _applicationContext.SaveChanges();
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository = _userRepository
                        ?? new UserRepository(_applicationContext, _mapper);
            }
        }
    }
}
