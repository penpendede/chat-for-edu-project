using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    /// \todo implement OnBuddyAdd
    public delegate void OnBuddyAdd(UserLocal userLocal, UserRemote userRemote);

    /// \todo implement OnBuddyRemove
    public delegate void OnBuddyRemove(UserLocal userLocal, UserRemote userRemote);

    public class UserLocal : User
    {
        private String PasswordSaltedHash;

        public List<UserRemote> Buddies;

        public OnBuddyAdd BuddyAdd;
        public OnBuddyRemove BuddyRemove;

        public void AddBuddy(User buddy)
        {
            /// \todo implement UserLocal.AddBuddy
        }

        public void RemoveBuddy(User buddy)
        {
            /// \todo implement UserLocal.RemoveBuddy
        }

        public Boolean VerifyPassword(String Password)
        {
            /// \todo implement UserLocal.VerifyPassword
            return false;
        }

        public Boolean SetPassword(String oldPassword, String newPassword)
        {
            /// \todo implement UserLocal.SetPassword
            return false;
        }
         
    }
}
