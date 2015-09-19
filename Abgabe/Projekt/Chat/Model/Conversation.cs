using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public delegate void ConversationOnBuddyAdd(Conversation conversation, UserRemote buddy);

    //public delegate void ConversationOnBuddyRemove(Conversation conversation, UserRemote buddy);

    public delegate void ConversationOnMessageAdd(Conversation conversation, Message message);

    public delegate void ConversationOnChangeActive(Conversation conversation, Boolean active);

    public delegate void ConversationOnClose(Conversation conversation);

    public class Conversation
    {
        public int Id;

        public UserLocal UserLocal;
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

        public bool Closed
        {
            private set;
            get;
        }

        public ConversationOnBuddyAdd BuddyAdd;
        //public ConversationOnBuddyRemove BuddyRemove;
        public ConversationOnMessageAdd MessageAdd;
        public ConversationOnChangeActive ChangeActive;
        public ConversationOnClose OnClose;

        /// <summary>
        /// Constructor, nothing sexy here
        /// </summary>
        public Conversation()
        {
            _buddies = new List<User>();
            _messages = new List<Message>();
            Buddies = _buddies.AsReadOnly();
            Messages = _messages.AsReadOnly();
            Active = true;
            Closed = false;
        }

        /// <summary>
        /// Add Buddy (unless he/she already is a buddy that is)
        /// </summary>
        /// <param name="buddy">buddy to be added (a remote user)</param>
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

        //public void RemoveBuddy(UserRemote buddy)
        //{
        //    _buddies.Remove(buddy);
        //    if (BuddyRemove != null)
        //    {
        //        BuddyRemove(this, buddy);
        //    }
        //}

        /// <summary>
        /// Add message to conversation and call MessageAdd if it is defined
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Message message)
        {
            _messages.Add(message);
            message.Conversation = this;

            if (MessageAdd != null)
            {
                MessageAdd(this, message);
            }
        }

        /// <summary>
        /// Make conversation active or inactive (and call ChangeActive if it is defined)
        /// </summary>
        /// <param name="active">if true conversation becomes active, otherwise inactive</param>
        public void SetActive(Boolean active)
        {
            Active = active;
            if (ChangeActive != null)
            {
                ChangeActive(this, active);
            }
        }

        /// <summary>
        /// Close conversation, call handler OnClose if it is defined
        /// </summary>
        public void Close()
        {
            Closed = true;
            if (OnClose != null)
            {
                OnClose(this);
            }
        }
    }
}
