namespace MinimalJwtAuth.Models
{
    public class NewRefreshToken
    {

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
