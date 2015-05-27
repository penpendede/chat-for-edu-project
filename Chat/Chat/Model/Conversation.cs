using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public delegate void ConversationOnBuddyAdd(Conversation conversation, UserRemote buddy);

    public delegate void ConversationOnBuddyRemove(Conversation conversation, UserRemote buddy);

    public delegate void ConversationOnMessageAdd(Conversation conversation, Message message);

    public delegate void ConversationOnChangeActive(Conversation conversation, Boolean active);

    public class Conversation
    {
        public int Id;
        public UserLocal Owner;
        private List<User> _buddies;
        private List<Message> _messages;

        public ReadOnlyCollection<User> Buddies
        {
            private set;
            get;
        }
        public ReadOnlyCollection<Message> Messages
        {
            private set;
            get;
        }

        public bool Active
        {
            private set;
            get;
        }

        public ConversationOnBuddyAdd BuddyAdd;
        public ConversationOnBuddyRemove BuddyRemove;
        public ConversationOnMessageAdd MessageAdd;
        public ConversationOnChangeActive ChangeActive;

        

        public Conversation()
        {
            _buddies = new List<User>();
            _messages = new List<Message>();
            Buddies = _buddies.AsReadOnly();
            Messages = _messages.AsReadOnly();
            Active = true;
        }

        public void AddBuddy(UserRemote buddy)
        {
            if (!_buddies.Contains(buddy))
            {
                _buddies.Add(buddy);
                if (BuddyAdd != null)
                {
                    BuddyAdd(this, buddy);
                }
            }
        }

        public void RemoveBuddy(UserRemote buddy)
        {
            _buddies.Remove(buddy);
            if (BuddyRemove != null)
            {
                BuddyRemove(this, buddy);
            }
        }

        public void AddMessage(Message message)
        {
            _messages.Add(message);
            message.Conversation = this;

            if (MessageAdd != null)
            {
                MessageAdd(this, message);
            }
        }

        public void SetActive(Boolean active)
        {
            Active = active;
            if (ChangeActive != null)
            {
                ChangeActive(this, active);
            }
        }
    }
}
