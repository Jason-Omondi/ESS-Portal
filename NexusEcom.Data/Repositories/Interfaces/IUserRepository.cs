using NexusEcom.DataAccess.DataTransferObjects;
using NexusEcom.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusEcom.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        //Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllUsersAsync();
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
