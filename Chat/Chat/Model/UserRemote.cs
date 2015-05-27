using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class UserRemote : User
    {
        public String IP;

        public UserLocal BuddyOf;

        public UserRemote() : base() { }
    }
}
