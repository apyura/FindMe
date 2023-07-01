using FindMe.DTO;
using FindMe.Error;

namespace FindMe.Services
{
    public interface IUserService
    {
        Task<Result<List<UserInfoDTO>>> GetAllUsers();
        Task<Result<UserInfoDTO>> GetUserById(int id);
        Task<Result<UserInfoDTO>> AddNewUser(UserDTO user);
        Task<Result<UserInfoDTO>> UpdateUser(int id, UserDTO user);
        Task<Result<UserInfoDTO>> ActivateUser(int id);
        Task<Result<UserInfoDTO>> DeleteUser(int id);
    }
}
