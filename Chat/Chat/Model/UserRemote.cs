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
        /// Mark remote user as (not) deleted
        /// </summary>
        /// <param name="deleted">if true remote user is marked as deleted, otherwise as not deleted</param>
        public UserRemote(bool deleted=false) : base()
        {
            Deleted = deleted;
        }

        /// <summary>
        /// Delete remote user: mark as deleted, if method is available also call OnDelete for this remote user
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
        /// This represents the System itself as a user. It is used to display informative messages like 'user is not online' etc.
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
