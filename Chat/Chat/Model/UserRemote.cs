using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public delegate void UserRemoteOnDelete(UserRemote userRemote);

    public class UserRemote : User
    {
        public string IP;
        public UserLocal BuddyOf;
        public bool Deleted { private set; get; }

        public UserRemoteOnDelete OnDelete;

        public UserRemote(bool deleted=false) : base()
        {
            Deleted = deleted;
        }

        public void Delete()
        {
            Deleted = true;
            if (OnDelete != null)
            {
                OnDelete(this);
            }
        }
    }
}
