namespace WinCordApi.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public User User { get; set; }
        public int? UserId { get; set; } = null;
        public string? Username { get; set; } = null;
    }

    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? UserId { get; set; } = null;
        public string? Username { get; set; } = null;
    }
}
