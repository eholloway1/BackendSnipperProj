namespace Snipper.server.Models
{
    public class Snippet
    {
        public long id {  get; set; }

        public required string language { get; set; }

        public required string code { get; set; }
    }
}
