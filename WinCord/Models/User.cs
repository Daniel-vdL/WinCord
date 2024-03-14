using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCord.Models
{
    public class User
    {
        public static User CurrentLoggedInUser { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
