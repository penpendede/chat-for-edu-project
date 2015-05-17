using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    /// \todo implement OnConversationAdd
    public delegate void OnConversationAdd(User user, Conversation conversation);

    public class User
    {
        public int Id;
        public String Name;
        public List<Conversation> Conversations;

        User()
        {
            /// \todo default init for User
        }

        public void AddConversation(Conversation conversation)
        {
            /// \todo implement User.AddConversation
        }
    }
}
