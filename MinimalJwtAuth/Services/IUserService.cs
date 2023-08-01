using MinimalJwtAuth.Models;

namespace MinimalJwtAuth.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
        public User GetCurrentUser();
        public string GenerateRefreshToken();
    }
}
