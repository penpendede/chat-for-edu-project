using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    /// \todo implement OnUserAdd
    public delegate void OnUserAdd(Conversation conversation, User user);

    /// \todo implement OnUserRemove
    public delegate void OnUserRemove(Conversation conversation, User user);

    /// \todo implement OnMessageAdd
    public delegate void OnMessageAdd(Conversation conversation, User user);

    /// \todo implement OnChangeActive
    public delegate void OnChangeActive(Conversation conversation, Boolean active);

    public class Conversation
    {
        public int Id;
        public bool Active;
        public UserLocal Owner;
        public List<UserRemote> Buddies;
        public List<Message> Messages;

        public OnUserAdd UserAdd;
        public OnUserRemove UserRemove;
        public OnMessageAdd MessageAdd;
        public OnChangeActive ChangeActive;

        public Conversation()
        {
            /// \todo default init for Conversation
        }

        public void AddUser(User user)
        {
            /// \todo implement Conversation.AddUser
        }

        public void RemoveUser(User user)
        {
            /// \todo implement Conversation.RemoveUser
        }

        public void AddMessage(Message message)
        {
            /// \todo implement Conversation.AddMessage
        }

        public void SetActive(Boolean active)
        {
            /// \todo implement Conversation.SetActive
        }
    }
}
