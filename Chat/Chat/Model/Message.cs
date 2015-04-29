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
        public String Text;
        public DateTime Time;

        Message()
        {
            /// \todo default init for Message
        }

        // Message requires no functions as all values are public
    }
}
