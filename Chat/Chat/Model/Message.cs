using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class Message
    {
        public int Id;
        public User Sender;
        public string Text;
        public DateTime Time;

        public Message()
        {
            Time = DateTime.Now;
        }
    }
}
