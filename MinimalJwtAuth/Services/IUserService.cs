using MinimalJwtAuth.Models;

namespace MinimalJwtAuth.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
        public NewRefreshToken SaveTokens(User user);
        public User GetCurrentUser();
        public User GetCurrentUserById();
        public string GenerateRefreshToken();
        public User Create(User user);
        public User Update(User user);
        public Boolean Delete();
        public User MakeReturnable(User user);
    }
}
