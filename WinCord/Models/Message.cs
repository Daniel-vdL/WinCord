using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCord.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int? UserId { get; set; } = null;
        public string? Username { get; set; } = null; 

        public HorizontalAlignment UserAlignment => UserId == null
            ? HorizontalAlignment.Center 
            : HorizontalAlignment.Right;

        public string UserNameFormatted => UserId == null
            ? "[SYSTEM]"
            : Username + ":";

    }
}
