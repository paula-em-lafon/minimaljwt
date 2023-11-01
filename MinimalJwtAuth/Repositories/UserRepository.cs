using Microsoft.AspNetCore.Identity;
using MinimalJwtAuth.Models;

namespace MinimalJwtAuth.Repositories
{
    public class UserRepository
    {
        public static List<User> Users = new()
        {
            new() { Id=1, Username = "luke_admin", EmailAddress = "luke.admin@email.com", Password = "MyPass_w0rd", GivenName = "Luke", Surname = "Rogers", Role = "Administrator"},
            new() { Id=2, Username = "lydia_standard", EmailAddress = "lycia.standard@email.com", Password = "MyPass_w0rd", GivenName = "Elyse", Surname = "Burton", Role = "Standard"},
        };
    }
}
