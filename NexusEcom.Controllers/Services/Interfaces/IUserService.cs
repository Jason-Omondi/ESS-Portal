using NexusEcom.DataAccess.DataTransferObjects;
using NexusEcom.DataAccess.Entities;

namespace NexusEcom.Controllers.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByEmail(string email);

        //Task AddUserAsync(UserDto user);

        Task<bool> RegisterUserAsync(UserDto userDto);
        Task<bool> ValidateUserLoginAsync(string email, string password);
        Task<bool> ResetPasswordAsync(int userId, string newPassword);
        Task<bool> UpdateUserAsync(string employeeNo, UserDto updatedUser);
        Task<bool> DeleteUserAsync(string employeeNumber);
        Task<List<UserDto>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10);
    }
}
