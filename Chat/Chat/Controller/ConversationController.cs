using Chat.Model;
using Chat.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chat.Controller
{
    public class ConversationController
    {
        private UserLocal userLocal;
        private Conversation conversation;
        public ConversationTabPage TabPage;

        public ConversationController(UserLocal userLocal, Conversation conversation)
        {
            this.userLocal = userLocal;
            this.conversation = conversation;

            // TODO: bind addMessage to model
            // TODO: bind conversationOnUserAdd to model
            // TODO: bind conversationOnUserRemove to model

            TabPage = new ConversationTabPage();

            foreach (User user in conversation.Users)
            {
                conversationOnUserAdd(user);
            }

            foreach (Model.Message message in conversation.Messages)
            {
                addMessage(message); 
                // NOTE: It seems to be right to shorten most delegate names like this if they are doing generic tasks like this
                // conversation.OnMessageAdd += this.addMessage; // seems quite understandable for me, too
            }
        }

        private void addMessage(Model.Message message)
        {
            TabPage.AddMessage(message.Sender.Name, message.Text, message.Time);
        }

        private void conversationOnUserAdd(User user)
        {
            TabPage.AddUser(user.Name);
        }

        private void conversationOnUserRemove(User user)
        {
            TabPage.RemoveUser(user.Name);
        }

        private void tabPageOnTextSubmit(string messageText)
        {
            // NOTE: Where does the id come to the model? Maybe the momment it is written into the database? Or should we ask the databaseController (repository) to create a new object?
            conversation.AddMessage(new Model.Message() { Text = messageText, Sender = this.userLocal, Time = DateTime.Now });
        }
    }
}
