using NexusEcom.DataAccess.DataTransferObjects;
using NexusEcom.DataAccess.Entities;

namespace NexusEcom.Controllers.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByEmail(string email);

        Task AddUserAsync(UserDto user);
    }
}
