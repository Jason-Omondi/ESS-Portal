using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NexusEcom.Utils
{
    public class DefaultConfigs
    {
        public static readonly int STATUS_SUCCESS = 1;
        public static readonly int STATUS_ERROR = 2;
        public static readonly int STATUS_FAIL = 0;
        public static readonly string DEFAULT_DATE_FORMAT = "yyyy-MM-dd";
        public static readonly string DEFAULT_DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public static readonly string ERROR_MESSAGE = "An error occured while processing your request";

        private readonly IConfiguration _configuration;

        private static string secretKey;
        private static string _issuer;
        private static string _audience;
        private static int _tokenExpiryInHours;

        // Method to initialize settings
        public static void Initialize(IConfiguration configuration)
        {
            secretKey = configuration["JwtSettings:SecretKey"];
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
            _tokenExpiryInHours = int.Parse(configuration["JwtSettings:TokenExpiryInHours"]);
        }

        public struct DefaultResponse
        {
            public int status { get; set; }
            public string message { get; set; }
            public bool res { get; set; }
            public string return_value { get; set; }
            public object data { get; set; }

            public DefaultResponse(int status, string message, string return_value, object data = null, bool res = false)
            {
                this.status = status;
                this.message = message;
                this.return_value = return_value;
                this.data = data;
                this.res = res;
            }
        }

        public static string GenerateToken(string email, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_tokenExpiryInHours),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
