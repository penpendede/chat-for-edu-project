using Chat.Model;
using Chat.View;
using System.Collections.Generic;

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

            Conversation.MessageAdd += _conversationAddMessage;
            Conversation.BuddyAdd += _conversationOnBuddyAdd;
            //Conversation.BuddyRemove += _onConversationOnBuddyRemove;

            TabPage = new ConversationTabPage(_tabControl.MeasureLabelText);
            TabPage.OnTextSubmit += _tabPageOnTextSubmit;

            _tabControl.AddTab(TabPage);

            foreach (UserRemote buddy in conversation.Buddies)
            {
                _conversationOnBuddyAdd(Conversation, buddy);
            }

            foreach (Model.Message message in conversation.Messages)
            {
                _conversationAddMessage(Conversation, message); 
            }

            if (Conversation.Closed)
            {
                _conversationOnClose(Conversation);
            }
        }

        private void _conversationAddMessage(Conversation conv, Model.Message message)
        {
            TabPage.AddMessage(message.Sender.Name, message.Text, message.Time);
        }

        private void _conversationOnBuddyAdd(Conversation conv, UserRemote buddy)
        {
            _networkConnectionControllers.Add(new NetworkConnectionController(_userLocal, conv, buddy));
            TabPage.AddUser(buddy.Name);
        }

        //private void _ConversationOnBuddyRemove(Conversation conv, UserRemote buddy)
        //{
        //    NetworkConnectionController networkConnectionController = _networkConnectionControllers.Find(n => n.UserRemote == buddy);
        //    networkConnectionController.Dispose();
        //    _networkConnectionControllers.Remove(networkConnectionController);
        //    TabPage.RemoveUser(buddy.Name);
        //}

        private void _conversationOnClose(Conversation conv)
        {
            TabPage.Disable();
        }

        private void _tabPageOnTextSubmit(string messageText)
        {
            Conversation.AddMessage(new Model.Message() { Text = messageText, Sender = this._userLocal });
        }

        public void Dispose()
        {
            _tabControl.RemoveTab(TabPage);
            Conversation.MessageAdd -= _conversationAddMessage;
            Conversation.BuddyAdd -= _conversationOnBuddyAdd;
            //Conversation.BuddyRemove -= _onConversationOnBuddyRemove;
            TabPage.OnTextSubmit -= _tabPageOnTextSubmit;
        }
    }
}
