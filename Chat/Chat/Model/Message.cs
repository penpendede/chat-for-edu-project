using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    /// <summary>
    /// properties are public; the class's only method is the constructor
    /// </summary>
    public class Message
    {
        public int Id;
        public User Sender;
        public string Text;
        public DateTime Time;

        public Conversation Conversation;

        public Message()
        {
            Time = DateTime.Now;
        }
    }
}
