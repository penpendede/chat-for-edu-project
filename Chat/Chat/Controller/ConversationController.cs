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
        // Model
        private UserLocal _userLocal;
        public Conversation Conversation
        {
            private set;
            get;
        }

        // View
        private ConversationTabControl _tabControl;
        public ConversationTabPage TabPage
        {
            private set;
            get;
        }

        // Other controllers
        private List<NetworkConnectionController> _networkConnectionControllers;
        

        public ConversationController(UserLocal userLocal, Conversation conversation, ConversationTabControl tabControl)
        {
            _userLocal = userLocal;
            Conversation = conversation;
            _networkConnectionControllers = new List<NetworkConnectionController>();
            _tabControl = tabControl;

            Conversation.MessageAdd += _onConversationAddMessage;
            Conversation.BuddyAdd += _onConversationOnBuddyAdd;
            Conversation.BuddyRemove += _onConversationOnBuddyRemove;

            TabPage = new ConversationTabPage(_tabControl.MeasureLabelText);
            TabPage.OnTextSubmit += _tabPageOnTextSubmit;

            _tabControl.AddTab(TabPage);

            foreach (UserRemote buddy in conversation.Buddies)
            {
                _onConversationOnBuddyAdd(Conversation, buddy);
            }

            foreach (Model.Message message in conversation.Messages)
            {
                _onConversationAddMessage(Conversation, message); 
            }
        }


        private void _onConversationAddMessage(Conversation conv, Model.Message message)
        {
            TabPage.AddMessage(message.Sender.Name, message.Text, message.Time);
        }

        private void _onConversationOnBuddyAdd(Conversation conv, UserRemote buddy)
        {
            _networkConnectionControllers.Add(new NetworkConnectionController(_userLocal, conv, buddy));
            TabPage.AddUser(buddy.Name);
        }

        private void _onConversationOnBuddyRemove(Conversation conv, UserRemote buddy)
        {
            NetworkConnectionController networkConnectionController = _networkConnectionControllers.Find(n => n.UserRemote == buddy);
            networkConnectionController.Dispose();
            _networkConnectionControllers.Remove(networkConnectionController);
            TabPage.RemoveUser(buddy.Name);
        }

        private void _tabPageOnTextSubmit(string messageText)
        {
            Conversation.AddMessage(new Model.Message() { Text = messageText, Sender = this._userLocal });
        }

        public void Dispose()
        {
            _tabControl.RemoveTab(TabPage);
            Conversation.MessageAdd -= _onConversationAddMessage;
            Conversation.BuddyAdd -= _onConversationOnBuddyAdd;
            Conversation.BuddyRemove -= _onConversationOnBuddyRemove;
            TabPage.OnTextSubmit -= _tabPageOnTextSubmit;
        }
    }
}
