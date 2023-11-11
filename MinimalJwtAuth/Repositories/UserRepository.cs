using Microsoft.AspNetCore.Identity;
using MinimalJwtAuth.Models;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;

namespace MinimalJwtAuth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string CS;

        public UserRepository(string connectionString)
        {
            CS = connectionString;
        }

        public User Login(UserLogin userLogin)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                var user = new User();
                var cmd = new SqlCommand("spLogin", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", userLogin.Username);
                cmd.Parameters.AddWithValue("@Password", userLogin.Password);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                {
                    return null;
                }
                while (rdr.Read())
                {
                    user.Id = Convert.ToInt32(rdr["Id"]);
                    user.Username = rdr["Username"].ToString();
                    user.EmailAddress = rdr["EmailAddress"].ToString();
                    user.Password = rdr["Password"].ToString();
                    user.GivenName = rdr["GivenName"].ToString();
                    user.Surname = rdr["Surname"].ToString();
                    user.Role = rdr["Role"].ToString();
                }
                return user;
            }
        }
        public NewRefreshToken CreateRefreshToken(int id, NewRefreshToken newRefreshToken)
        {
            var returnedRefreshToken = new NewRefreshToken();
            using (SqlConnection con = new SqlConnection(CS))
            {
                var cmd = new SqlCommand("spCreateRefreshToken", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@RefreshToken", newRefreshToken.RefreshToken);
                cmd.Parameters.AddWithValue("@RefreshTokenExpiryTime", newRefreshToken.RefreshTokenExpiryTime);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    returnedRefreshToken.RefreshToken = rdr["RefreshToken"].ToString(); ;
                    returnedRefreshToken.RefreshTokenExpiryTime = rdr.GetDateTime(rdr.GetOrdinal("RefreshTokenExpiryTime")); ;
                }
                return returnedRefreshToken;
            };

        }
        public User CreateNewUser(User user)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                var cmd = new SqlCommand("spAddNewUser", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@GivenName", user.GivenName);
                cmd.Parameters.AddWithValue("@Surname", user.Surname);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    user.Id = Convert.ToInt32(rdr["Id"]);
                    user.Username = rdr["Username"].ToString();
                    user.EmailAddress = rdr["EmailAddress"].ToString();
                    user.Password = rdr["Password"].ToString();
                    user.GivenName = rdr["GivenName"].ToString();
                    user.Surname = rdr["Surname"].ToString();
                    user.Role = rdr["Role"].ToString();
                }
                return user;
            }
        }
        public User GetCurrentUserById(int id)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                var user = new User();
                var cmd = new SqlCommand("spGetUserById", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                {
                    return null;
                }
                while (rdr.Read())
                {
                    user.Id = Convert.ToInt32(rdr["Id"]);
                    user.Username = rdr["Username"].ToString();
                    user.EmailAddress = rdr["EmailAddress"].ToString();
                    user.Password = rdr["Password"].ToString();
                    user.GivenName = rdr["GivenName"].ToString();
                    user.Surname = rdr["Surname"].ToString();
                    user.Role = rdr["Role"].ToString();
                    user.RefreshToken = rdr["RefreshToken"].ToString(); ;
                    user.RefreshTokenExpiryTime = rdr.GetDateTime(rdr.GetOrdinal("RefreshTokenExpiryTime"));

                }
                return user;
            }
        }

        public User UpdateUser(User oldUser)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                var newUser = new User();
                var cmd = new SqlCommand("spUpdateUser", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", oldUser.Id);
                cmd.Parameters.AddWithValue("@Username", oldUser.Username);
                cmd.Parameters.AddWithValue("@EmailAddress", oldUser.EmailAddress);
                cmd.Parameters.AddWithValue("@Password", oldUser.Password);
                cmd.Parameters.AddWithValue("@GivenName", oldUser.GivenName);
                cmd.Parameters.AddWithValue("@Surname", oldUser.Surname);
                cmd.Parameters.AddWithValue("@Role", oldUser.Role);
                if (oldUser.Password == null || oldUser.Password.Length == 0)
                {
                    cmd.Parameters["@Password"].Value = DBNull.Value;
                }
                SqlDataReader rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                {
                    return null;
                }
                while (rdr.Read())
                {
                    newUser.Id = Convert.ToInt32(rdr["Id"]);
                    newUser.Username = rdr["Username"].ToString();
                    newUser.EmailAddress = rdr["EmailAddress"].ToString();
                    newUser.Password = rdr["Password"].ToString();
                    newUser.GivenName = rdr["GivenName"].ToString();
                    newUser.Surname = rdr["Surname"].ToString();
                    newUser.Role = rdr["Role"].ToString();
                    newUser.RefreshToken = rdr["RefreshToken"].ToString(); ;
                    newUser.RefreshTokenExpiryTime = rdr.GetDateTime(rdr.GetOrdinal("RefreshTokenExpiryTime"));

                }
                return newUser;
            }
        }

        public bool DeleteUser(User user)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                var cmd = new SqlCommand("spDeleteUser", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", user.Id);
                cmd.ExecuteNonQuery();
                return true;
            }
        }


        public static List<User> Users = new()
        {
            new() { Id=1, Username = "luke_admin", EmailAddress = "luke.admin@email.com", Password = "MyPass_w0rd", GivenName = "Luke", Surname = "Rogers", Role = "Administrator"},
            new() { Id=2, Username = "lydia_standard", EmailAddress = "lycia.standard@email.com", Password = "MyPass_w0rd", GivenName = "Elyse", Surname = "Burton", Role = "Standard"},
        };
    }
}
