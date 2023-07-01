using FindMe.DTO;
using FindMe.Error;
using FindMe.Models;

namespace FindMe.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool IsUsersExist();
        Task<User> AddUser(User newUser);
        Task<Result<UserInfoDTO>> UpdateUser(int id, User user, SaltHashedPassword? newHashedPassword);
    }
}
