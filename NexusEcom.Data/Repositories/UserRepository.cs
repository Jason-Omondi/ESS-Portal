﻿
using Microsoft.EntityFrameworkCore;
using NexusEcom.Data.Context;
using NexusEcom.Data.DataTransferObjects;
using NexusEcom.Data.Entities;
using NexusEcom.Data.Repositories.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace NexusEcom.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                return await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)

            {
                Console.WriteLine($"Error retreiving user: {ex.Message}");
                return null;
            }
        }

        public async Task<User?> GetByEmpNoAsync(string empNo)
        {
            try
            {
                return await appDbContext.Users.FirstOrDefaultAsync(u => u.EmployeeNumber == empNo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retreiving user: {ex.Message}");
                return null;
            }
        }



        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                await appDbContext.Users.AddAsync(user);
                await appDbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database update error while creating user: {ex.Message}");
                return false; // Operation failed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating a user: {ex.Message}");
                return false;
            }

        }

        public async Task<bool> UpdateUserAsync(UserDto user)
        {
            try
            {
                if (user == null) 
                {
                    Console.WriteLine("Cannot update empty user bro!");
                    return false;
                }

                var existingUser = await appDbContext.Users.FindAsync(user.EmployeeNumber);
                if (existingUser != null)
                {
                    Console.WriteLine($"User with ID {user.Email} not found.");
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(user.Email))
                    existingUser.Email = user.Email;

                //if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                //    existingUser.PasswordHash = user.PasswordHash;

                if (!string.IsNullOrWhiteSpace(user.Role))
                    existingUser.Role = user.Role;

                if (!string.IsNullOrWhiteSpace(user.EmployeeNumber))
                    existingUser.EmployeeNumber = user.EmployeeNumber;

                appDbContext.Users.Update(existingUser);
                await appDbContext.SaveChangesAsync();
                return true;

            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database update error while updating user ID {user.EmployeeNumber}: {ex.Message}");
                return false; // Operation failed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in UpdateUserAsync: {ex.Message}");
                return false;
            }

        }

        public async Task<bool> DeleteUserAsync(string employeeId)
        {
            try
            {
                var user = await appDbContext.Users.FindAsync(employeeId);
                if (user == null)
                {
                    Console.WriteLine($"User with ID {employeeId} not found.");
                    return false; 
                }

                appDbContext.Users.Remove(user);
                await appDbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database update error while deleting user ID {employeeId}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in DeleteUserAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<List<UserDto>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var list = await appDbContext.Users
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Map each User entity to a UserDto
                var userDtos = list.Select(user => new UserDto
                {
                    Email = user.Email,
                    EmployeeNumber = user.EmployeeNumber,
                    Role = user.Role,
                    //UserId = user.UserId,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    //CreatedAt = user.CreatedAt
                }).ToList();

                return userDtos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving users: {ex.Message}");
                return new List<UserDto>();
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var data = await appDbContext.Users.ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving users: {ex.Message}");
                return new List<User>(); // Return an empty list to ensure the calling code doesn't break
            }
        }

        public async Task<List<User>> GetAllUsersWithFiltersAsync(string? role = null, string? emailContains = null)
        {
            try
            {
                var query = appDbContext.Users.AsQueryable();

                if (!string.IsNullOrWhiteSpace(role))
                    query = query.Where(u => u.Role.Equals(role, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(emailContains))
                    query = query.Where(u => u.Email.Contains(emailContains, StringComparison.OrdinalIgnoreCase));

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving filtered users: {ex.Message}");
                return new List<User>();
            }
        }


        public async Task<bool> ValidateUserPasswordAsync(string email, string password)
        {
            try
            {

                var data = await appDbContext.Users.FirstOrDefaultAsync(u => u.EmployeeNumber == email);
                if (data == null)
                {
                    return false; // User not found
                }


                //extract current user by email
                var user = await GetByEmailAsync(data.Email);
                if (user == null)
                {
                    return false; // User not found
                }

                return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash); // Validate password hash
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating password: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
        {
            //TODO: email verification b4 password reset
            try
            {
                var user = await appDbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    Console.WriteLine($"User with ID {userId} not found.");
                    return false;
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                appDbContext.Users.Update(user);
                await appDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting password: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            try
            {
                return await appDbContext.Users.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if email is registered: {ex.Message}");
                return false;
            }
        }
    
        public async Task<bool> RegisterUserIfUserExists(string employeeNo, string password)
        {
            try
            {
                var existingUser = await appDbContext.Users.FirstOrDefaultAsync(u => u.EmployeeNumber == employeeNo);

                if (existingUser == null)
                {
                    Console.WriteLine($"User is not part of us!");
                    return false;
                }

                //check if the existing user has a password, if so then user is already registered, if not then we can set password
                if(!string.IsNullOrWhiteSpace(existingUser.PasswordHash))
                {
                    Console.WriteLine($"User {employeeNo} is already registered!");
                    return false;
                }


                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

                appDbContext.Users.Update(existingUser);
                await appDbContext.SaveChangesAsync();
                return true;

            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database error during registration: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if email is registered: {ex.Message}");
                return false;
            }
        }
    
    }
}


//public async Task<User?> GetByEmailAsync(string email)
//{
//    try
//    {
//        return await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
//    }
//    catch (Exception ex) 
//    {
//    Console.WriteLine(ex.Message);
//        throw;
//    }
//}

//public async Task AddUserAsync(User user)
//{
//    try
//    {
//        await appDbContext.Users.AddAsync(user);
//        await appDbContext.SaveChangesAsync();
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.ToString());
//        throw;
//    }

//}


//public async Task<bool> RegisterUserAsync(UserDto userDto)
//{
//    try
//    {
//        if (await IsEmailRegisteredAsync(userDto.Email))
//        {
//            Console.WriteLine("Email is already registered.");
//            return false;
//        }

//        var user = new User
//        {
//            Email = userDto.Email,
//            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
//            Role = userDto.Role,
//            EmployeeNumber = userDto.EmployeeNumber ?? string.Empty,
//            CreatedAt = DateTime.UtcNow,
//            FullName = userDto.FullName,
//            PhoneNumber = userDto.PhoneNumber
//        };

//        if (!await AddUserAsync(user))
//        {
//            return false;
//        }
//        return true;

//    }
//    catch (Exception ex) 
//    {
//        Console.WriteLine($"not my codes fault: {ex.Message}");
//        return false;
//    }
//}

