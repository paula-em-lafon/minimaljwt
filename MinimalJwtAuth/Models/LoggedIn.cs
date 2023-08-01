namespace MinimalJwtAuth.Models
{
    public class LoggedIn
    {
        public string? Token { get; set; }
        public DateTime? TokenExpiry { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public string? GivenName { get; set; }
        public string? Surname { get; set; }    
    }
}
