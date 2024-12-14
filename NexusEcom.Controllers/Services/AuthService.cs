using NexusEcom.DataAccess.DataTransferObjects;
using NexusEcom.DataAccess.Entities;
using NexusEcom.DataAccess.Repositories;
using NexusEcom.Utils;
//using Solutaris.InfoWARE.ProtectedBrowserStorage.Extensions;
using Solutaris.InfoWARE.ProtectedBrowserStorage.Services;
using System.Text.RegularExpressions;

namespace NexusEcom.Controllers.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
       private  readonly IIWLocalStorageService _localStorage;

        public AuthService(UserRepository userRepository, IIWLocalStorageService localStorage)
        {
            this._userRepository = userRepository;
           this._localStorage = localStorage;
        }

        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginDto.Email) || !IsValidEmail(loginDto.Email))
                {
                    throw new ArgumentException("Invalid email format.");
                }

                if (string.IsNullOrWhiteSpace(loginDto.Password))
                {
                    throw new ArgumentException("Password is required.");
                }

                var user = await _userRepository.GetByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    throw new InvalidOperationException("Invalid email or password.");
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    throw new InvalidOperationException("Invalid email or password.");
                }

                return DefaultConfigs.GenerateToken(user.Email, user.Role.ToLower());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                throw; // Propagate the exception 
            }
        }


       
        public async Task<bool> Register(RegisterDto register)
        {
            try
            {
                if (!IsValidEmail(register.Email)) { return false; }

                if (string.IsNullOrWhiteSpace(register.Password) || register.Password.Length < 6)
                {
                    return false;
                }

                if (await _userRepository.GetByEmailAsync(register.Email) != null)
                {
                    throw new InvalidOperationException("Email is already registered.");
                }

                var user = new User
                {
                    Email = register.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password),
                    Role = register.Role,
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepository.AddUserAsync(user);
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error during registration: {ex.Message}");
                
                return false;
            } }

        public async Task<bool> IsUserLoggedIn()
        {
            var tokenResult = await _localStorage.GetItemAsync<string>("authToken");
            return !string.IsNullOrEmpty(tokenResult);
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 8; // Simple check for now (minimum 8 characters)
        }
    }
}


