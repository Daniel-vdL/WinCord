namespace WinCordApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; } 

        public ICollection<Message> Messages { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
    }


}
