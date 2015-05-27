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

        public UserLocal() 
            :base() 
        { 
            _buddies = new List<UserRemote>();
            Buddies = _buddies.AsReadOnly();
        }

        public void AddBuddy(UserRemote buddy)
        {
            _buddies.Add(buddy);
            buddy.BuddyOf = this;
            if (BuddyAdd != null)
            {
                BuddyAdd(this, buddy);
            }
        }

        public void RemoveBuddy(UserRemote buddy)
        {
            _buddies.Remove(buddy);
            foreach (Conversation conv in Conversations)
            {
                if (conv.Buddies.Contains(buddy))
                {
                    conv.RemoveBuddy(buddy);
                }
            }
            if (BuddyRemove != null)
            {
                BuddyRemove(this, buddy);
            }
        }
    }
}
