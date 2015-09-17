using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public delegate void UserRemoteOnDelete(UserRemote userRemote);

    public class UserRemote : User
    {
        public UserLocal BuddyOf;
        public bool Deleted { private set; get; }

        public UserRemoteOnDelete OnDelete;

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="deleted"></param>
        public UserRemote(bool deleted=false) : base()
        {
            Deleted = deleted;
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void Delete()
        {
            Deleted = true;
            if (OnDelete != null)
            {
                OnDelete(this);
            }
        }

        // Instance of UserRemote but actually local

        private static UserRemote _systemUser;

        /// <summary>
        /// TODO
        /// </summary>
        public static UserRemote SystemUser {
            get
            {
                if (_systemUser == null)
                {
                    _systemUser = new UserRemote() { Id = -1, Name = "System" };
                }
                return _systemUser;
            }
        }
    }
}
