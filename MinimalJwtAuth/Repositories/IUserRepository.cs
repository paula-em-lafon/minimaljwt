using MinimalJwtAuth.Models;


namespace MinimalJwtAuth.Repositories
{
    public interface IUserRepository
    {
        User Login(UserLogin userLogin);
        NewRefreshToken CreateRefreshToken(int id, NewRefreshToken refreshToken);
        User GetCurrentUserById(int id);
        User CreateNewUser(User user);
        User UpdateUser(User user);
        bool DeleteUser(User user);
    }
}
