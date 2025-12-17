namespace CidConnectada.Tests.Models
{
    public class LoginResponseDto
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string roles { get; set; }
        public object permissions { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string _issued { get; set; }
        public string _expires { get; set; }
    }
}