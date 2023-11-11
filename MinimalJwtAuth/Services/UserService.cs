using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MinimalJwtAuth.Models;
using MinimalJwtAuth.Repositories;
using System.Security.Claims;
using System.Security.Cryptography;
using MinimalJwtAuth.Exceptions;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace MinimalJwtAuth.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository) 
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }
        public User Get(UserLogin userLogin)
        {
            User user = _userRepository.Login(userLogin);

            return user;
        }

        public User GetCurrentUser()
        {
            var username = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = UserRepository.Users.FirstOrDefault(o => o.Username.Equals(username));
            return user;
        }
        public User GetCurrentUserById()
        {
            var id = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Sid);
            if (id == null) { return null; }
            User user = _userRepository.GetCurrentUserById(Int32.Parse(id));
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
        public NewRefreshToken SaveTokens(User user) 
        {
            var newTokens = new NewRefreshToken();
            newTokens.RefreshToken = GenerateRefreshToken();
            newTokens.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            newTokens = _userRepository.CreateRefreshToken(user.Id, newTokens);
            return newTokens;
        }
        public User Create(User user)
        {
            try 
            { 
                _userRepository.CreateNewUser(user);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                   
                    throw new UserException("Username and email must be unique");
                }
            }
            return user;
        }

        public User Update(User oldUser)
        {
            var userToModify = GetCurrentUserById();
            if (userToModify == null)
            {
                throw new UserException("Current user invalid");
            }
            var newUser = new User();
            oldUser.Id = userToModify.Id;
            try
            {
                newUser = _userRepository.UpdateUser(oldUser);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {

                    throw new UserException("Username and email must be unique");
                }
            }
            return newUser;
        }

        public bool Delete() 
        {
            User user = this.GetCurrentUserById();
            if (user is null) return false;

            _userRepository.DeleteUser(user);
            return true;
        }

        public User MakeReturnable(User user)
        {
            var returnable = new User
            {
                Username = user.Username,
                EmailAddress = user.EmailAddress,
                GivenName = user.GivenName,
                Surname = user.Surname,
                Role = user.Role,
            };
            return returnable;
        }
    }
}
