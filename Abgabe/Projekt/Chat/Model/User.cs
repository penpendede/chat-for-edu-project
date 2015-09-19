using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public delegate void UserOnConversationAdd(User user, Conversation conversation);

    /// <summary>
    /// General user class - child classes are UserRemote and UserLocal
    /// </summary>
    public class User
    {
        public int Id;
        public string Name;
        public string IP;
        public int Port;

        private List<Conversation> _conversations;

        // getter/setter for different access
        public ReadOnlyCollection<Conversation> Conversations
        {
            private set;
            get;
        }

        public UserOnConversationAdd ConversationAdd;

        /// <summary>
        /// Constructor
        /// </summary>
        public User()
        {
            _conversations = new List<Conversation>();
            Conversations = _conversations.AsReadOnly();
        }

        /// <summary>
        /// Add a conversation to the user
        /// </summary>
        /// <param name="conversation">the conversation to be added</param>
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
