using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public delegate void UserOnConversationAdd(User user, Conversation conversation);

    public class User
    {
        public int Id;
        public string Name;

        private List<Conversation> _conversations;

        public ReadOnlyCollection<Conversation> Conversations
        {
            private set;
            get;
        }

        public UserOnConversationAdd ConversationAdd;

        public User()
        {
            _conversations = new List<Conversation>();
            Conversations = _conversations.AsReadOnly();
        }

        public void AddConversation(Conversation conversation)
        {
            _conversations.Add(conversation);

            if (ConversationAdd != null)
            {
                ConversationAdd(this, conversation);
            }
        }
    }
}
