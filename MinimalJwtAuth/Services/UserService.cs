using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MinimalJwtAuth.Models;
using MinimalJwtAuth.Repositories;
using System.Security.Claims;
using System.Security.Cryptography;
using MinimalJwtAuth.Exceptions;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

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
            var username = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = UserRepository.Users.FirstOrDefault(o => o.Username.Equals(username));
            return user;
        }
        public User GetCurrentUserById()
        {
            var id = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Sid);
            User user = UserRepository.Users.FirstOrDefault(o => o.Id.Equals(Int32.Parse(id)));
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
        public User Create(User user)
        {
            User userNameAlready = UserRepository.Users.FirstOrDefault(o => o.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase));
            User emailAlready = UserRepository.Users.FirstOrDefault(o => o.EmailAddress.Equals(user.EmailAddress, StringComparison.OrdinalIgnoreCase));
            if (userNameAlready != null && emailAlready != null) {
                throw new UserException("Username and Email already exist");
            }
            else if (userNameAlready != null)
            {
                throw new UserException("Username already exists");
            }
            else if (emailAlready != null)
            {
                throw new UserException("Email is Already in use");
            }
            UserRepository.Users.Add(user);
            return user;
        }

        public User Update(User newUser)
        {
            User oldUser = this.GetCurrentUserById();
            if (oldUser == null)
            {
                throw new UserException("Current user couldn't be found ");
            }
            User userNameAlready = UserRepository.Users.FirstOrDefault(o => o.Username.Equals(newUser.Username, StringComparison.OrdinalIgnoreCase));
            User emailAlready = UserRepository.Users.FirstOrDefault(o => o.EmailAddress.Equals(newUser.EmailAddress, StringComparison.OrdinalIgnoreCase));
            if ((userNameAlready != null && newUser.Username!= oldUser.Username) && (emailAlready != null && newUser.EmailAddress != oldUser.EmailAddress))
            {
                throw new UserException("Username and Email already exist");
            }
            else if (userNameAlready != null && newUser.Username != oldUser.Username)
            {
                throw new UserException("Username already exists");
            }
            else if (emailAlready != null && newUser.EmailAddress != oldUser.EmailAddress)
            {
                throw new UserException("Email is Already in use");
            }

            oldUser.Username = newUser.Username;
            oldUser.EmailAddress = newUser.EmailAddress;
            oldUser.Surname = newUser.Surname;
            oldUser.GivenName = newUser.GivenName;
            oldUser.Role = newUser.Role;
            if (!string.IsNullOrWhiteSpace(newUser.Password))
            {
                oldUser.Password = newUser.Password;
            }
            var returnable = this.MakeReturnable(oldUser);
            return returnable;
        }

        public Boolean Delete() 
        {
            User user = this.GetCurrentUserById();
            if (user is null) return false;

            UserRepository.Users.Remove(user);
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
