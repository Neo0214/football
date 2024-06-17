namespace football.Models
{
    public class LoginDTO
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class LoginResponseDTO
    {
        public string message { get; set; }
        public int id { get; set; }
        public string token { get; set; }
        public LoginResponseDTO(string message, int id, string token)
        {
            this.message = message;
            this.id = id;
            this.token = token;
        }
    }
}
