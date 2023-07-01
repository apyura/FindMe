using AutoMapper;
using FindMe.DTO;
using FindMe.Error;
using FindMe.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace FindMe.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IMapper _mapper;
        public UserRepository(ApplicationContext applicationContext, IMapper mapper)
           : base(applicationContext)
        {
            _mapper = mapper;
        }

        public bool IsUsersExist()
        {
            return _applicationContext.Users.Any();
        }

        public async Task<User> AddUser(User newUser)
        {
            var result = _applicationContext.Users.Add(newUser);
            await _applicationContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Result<UserInfoDTO>> UpdateUser(int id, User user, SaltHashedPassword? newHashedPassword)
        {
            try
            {
                user.Id = id;
                Expression<Func<SetPropertyCalls<User>, SetPropertyCalls<User>>> setPropertyCalls 
                    = (q  => q.SetProperty(b => b.Email, user.Email).SetProperty(b => b.PhoneNumber, user.PhoneNumber));
                if (newHashedPassword != null)
                {
                    setPropertyCalls
                    = (s => s
                            .SetProperty(b => b.Email, user.Email)
                            .SetProperty(b => b.Hash, newHashedPassword.hash)
                            .SetProperty(b => b.PhoneNumber, user.PhoneNumber)
                            .SetProperty(b => b.Salt, newHashedPassword.salt));
                }

                var numberOfLines = await _applicationContext.Users.Where(p => p.Id == id)
                                                        .ExecuteUpdateAsync(setPropertyCalls);
                

                if (numberOfLines == 1)
                {
                    _applicationContext.SaveChanges();
                    return Result<UserInfoDTO>.GetSuccess(_mapper.Map<UserInfoDTO>(user));
                }
                else
                {
                    return Result<UserInfoDTO>.GetError(ErrorCode.NotFound, String.Format("There is not user with {0} id.", user.Id));
                }
            }
            catch (Exception ex)  //DbUpdateException
            {
                return Result<UserInfoDTO>.GetError(ErrorCode.DBError, ex.Message);
            }
        }
    }
}
