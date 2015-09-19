using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public delegate void UserLocalOnBuddyAdd(UserLocal userLocal, UserRemote userRemote);

    public delegate void UserLocalOnBuddyRemove(UserLocal userLocal, UserRemote userRemote);

    public class UserLocal : User
    {
        private List<UserRemote> _buddies;

        public ReadOnlyCollection<UserRemote> Buddies
        {
            private set;
            get;
        }

        public UserLocalOnBuddyAdd BuddyAdd;
        public UserLocalOnBuddyRemove BuddyRemove;

        /// <summary>
        /// Constructor is derived from the base class's constructor
        /// </summary>
        public UserLocal() 
            :base() 
        { 
            _buddies = new List<UserRemote>();
            Buddies = _buddies.AsReadOnly();
        }

        /// <summary>
        /// Add a buddy to the local user
        /// </summary>
        /// <param name="buddy">buddy to be added</param>
        public void AddBuddy(UserRemote buddy)
        {
            _buddies.Add(buddy);
            buddy.BuddyOf = this;
            if (BuddyAdd != null)
            {
                BuddyAdd(this, buddy);
            }
        }

        /// <summary>
        /// Remove a buddy from the local user
        /// </summary>
        /// <param name="buddy">buddy to be removed</param>
        public void RemoveBuddy(UserRemote buddy)
        {
            _buddies.Remove(buddy);
            if (BuddyRemove != null)
            {
                BuddyRemove(this, buddy);
            }
        }
    }
}
