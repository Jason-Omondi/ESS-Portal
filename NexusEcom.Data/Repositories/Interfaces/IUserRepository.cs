

using NexusEcom.Data.DataTransferObjects;
using NexusEcom.Data.Entities;

namespace NexusEcom.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        //Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetByEmpNoAsync(string employeeNo);
        Task<List<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize);
        //Task<List<User>> GetAllUsersWithFiltersAsync(string? role = null, string? emailContains = null);

        Task<bool> UpdateUserAsync(UserDto updatedUser); // Update user details

        // Authentication Operations
        //Task<bool> RegisterUserAsync(UserDto registerDto); 

        Task<bool> RegisterUserIfUserExists(string employeeNo, string password);
        Task<bool> ValidateUserPasswordAsync(string email, string password);
        Task<bool> ResetPasswordAsync(int userId, string newPassword); // Reset user password

        Task<bool> DeleteUserAsync(string employeeId);

        // Helper Methods
        Task<bool> IsEmailRegisteredAsync(string email);

    }
}
