using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MinimalJwtAuth.Models;
using MinimalJwtAuth.Repositories;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MinimalJwtAuth.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public User Get(UserLogin userLogin)
        {
            User user = UserRepository.Users.FirstOrDefault(o => o.Username.Equals(userLogin.Username, StringComparison.OrdinalIgnoreCase) && o.Password.Equals(userLogin.Password));

            return user;
        }

        public User GetCurrentUser()
        {
            
             User user = UserRepository.Users.FirstOrDefault(o => o.Username.Equals(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            Console.WriteLine(user);
            return user;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
