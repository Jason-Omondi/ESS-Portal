﻿
using NexusEcom.Data.DataTransferObjects;
using NexusEcom.Data.Entities;

namespace NexusEcom.Controllers.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByEmail(string email);

        //Task AddUserAsync(UserDto user);
        Task<List<UserDto>> GetAllUsersAsync();

        Task<bool> RegisterUserAsync(UserDto userDto);
        Task<bool> ValidateUserLoginAsync(string email, string password);
        Task<bool> ResetPasswordAsync(int userId, string newPassword);
        Task<bool> UpdateUserAsync(string employeeNo, UserDto updatedUser);
        Task<bool> DeleteUserAsync(string employeeNumber);
        Task<List<UserDto>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10);
    }
}
