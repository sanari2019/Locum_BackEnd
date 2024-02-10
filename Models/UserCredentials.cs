namespace Locum_Backend.Models
{
    public class UserCredentials
    {
        public int Id { get; set; }
        public string? Email { get; set; }

        public string? Username { get; set; }
        public required string Password { get; set; }
        public bool Active { get; set; }


    }
}