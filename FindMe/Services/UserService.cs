using AutoMapper;
using FindMe.DTO;
using FindMe.Error;
using FindMe.Models;
using FindMe.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace FindMe.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashPassword _hashPassword;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IHashPassword hashPassword)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashPassword = hashPassword;

            if (!_unitOfWork.UserRepository.IsUsersExist())
            {
                var hashedPassword1 = _hashPassword.GetHashedPassword("asdfghyt");
                _unitOfWork.UserRepository.Add(new User { Email = "Tom33@gmail.com", Salt = hashedPassword1.salt, Hash = hashedPassword1.hash, PhoneNumber = "885521477" });
                _unitOfWork.UserRepository.Add(new User { Email = "Tom22@gmail.com", Salt = RandomNumberGenerator.GetBytes(128 / 8), Hash = "qweddjkii", PhoneNumber = "885521455" });
                _unitOfWork.Save();
            }
        }
          
        public async Task<Result<List<UserInfoDTO>>> GetAllUsers()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var activeUsers = users.Select(p => p).Where(p => p.Active);

            return Result<List<UserInfoDTO>>
                .GetSuccess(_mapper.Map<List<UserInfoDTO>>(users));
        }

        public async Task<Result<UserInfoDTO>> GetUserById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if(user == null)
            {
                return Result<UserInfoDTO>
                    .GetError(ErrorCode.NotFound, "The user is not found.");
            }

            return Result<UserInfoDTO>.GetSuccess(_mapper.Map<UserInfoDTO>(user));
        }

        public async Task<Result<UserInfoDTO>> AddNewUser(UserDTO user)
        {
            if (IsEmailAndPhoneNumberAreNull(user.Email, user.PhoneNumber))
            {
                return Result<UserInfoDTO>
                    .GetError(ErrorCode.EmailAndPhoneNumberNull, "Enter email or phone number.");
            }

            User result;

            try
            {
                result = await _unitOfWork.UserRepository.AddUser(GetUserWithHashedPassword(user)); // поменять на Add
            }
            catch (DbUpdateException e)
            {
                string message = e.Message;
                if (e.InnerException != null && e.InnerException is SqlException)
                {
                    message = e.InnerException.Message;
                }

                return Result<UserInfoDTO>.GetError(ErrorCode.DBError, message);
            }
            //catch
            //{
            //    //log
            //}

            return Result<UserInfoDTO>.GetSuccess(_mapper.Map<UserInfoDTO>(result));
        }

        public async Task<Result<UserInfoDTO>> UpdateUser(int id, UserDTO user)
        {
            if (IsEmailAndPhoneNumberAreNull(user.Email, user.PhoneNumber))
            {
                return Result<UserInfoDTO>
                    .GetError(ErrorCode.EmailAndPhoneNumberNull, "Enter email or phone number.");
            }

            if(user.NewPassword != null)
            {
                var hashedPassword = _hashPassword.GetHashedPassword(user.NewPassword);
                return await _unitOfWork.UserRepository.UpdateUser(id, GetUserWithHashedPassword(user), hashedPassword);
            }

            return await _unitOfWork.UserRepository.UpdateUser(id, GetUserWithHashedPassword(user), null);
        }

        public async Task<Result<UserInfoDTO>> ActivateUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if(user == null)
            {
                return Result<UserInfoDTO>
                    .GetError(ErrorCode.NotFound, "The user is not found.");
            }

            user.Active = true;
            _unitOfWork.Save();

            return Result<UserInfoDTO>.GetSuccess(_mapper.Map<UserInfoDTO>(user));
        }

        public async Task<Result<UserInfoDTO>> DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (user == null)
            {
                return Result<UserInfoDTO>
                    .GetError(ErrorCode.NotFound, "The user is not found.");
            }

            user.Active = false;
            _unitOfWork.Save();

            return Result<UserInfoDTO>.GetSuccess(_mapper.Map<UserInfoDTO>(user));
        }

        private static bool IsEmailAndPhoneNumberAreNull(string? Email, string? PhoneNumber) =>
            Email == null && PhoneNumber == null;

        private User GetUserWithHashedPassword(UserDTO user)
        {
            var hashedPassword = _hashPassword.GetHashedPassword(user.Password);

            var newUser = _mapper.Map<User>(user);
            newUser.Salt = hashedPassword.salt;
            newUser.Hash = hashedPassword.hash;

            return newUser;
        }
    }
}
