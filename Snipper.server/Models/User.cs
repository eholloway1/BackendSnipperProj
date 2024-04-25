namespace Snipper.server.Models
{
    public class User
    {
        public long id {  get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
