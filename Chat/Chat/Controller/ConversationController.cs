using Chat.Model;
using Chat.View;
using System.Collections.Generic;
using System.Linq;

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
        private List<NetworkCommunicationController> _networkConnectionControllers;
        private TcpPeerManager _peerManager; // only for pass-through

        public ConversationController(UserLocal userLocal, Conversation conversation, ConversationTabControl tabControl, TcpPeerManager peerManager)
        {
            _peerManager = peerManager;

            _userLocal = userLocal;
            Conversation = conversation;
            _networkConnectionControllers = new List<NetworkCommunicationController>();
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

        public NetworkCommunicationController GetNetworkCommunicationController(UserRemote remoteUser)
        {
            // returns the communication controller which is responsible for the communication with the given user
            // returns null if it doesn't exist
            return _networkConnectionControllers.FirstOrDefault(c => c.UserRemote == remoteUser);
        }

        private void _conversationAddMessage(Conversation conv, Model.Message message)
        {
            TabPage.AddMessage(message.Sender.Name, message.Text, message.Time);
        }

        private void _conversationOnBuddyAdd(Conversation conv, UserRemote buddy)
        {
            TabPage.AddUser(buddy.Name);
            _networkConnectionControllers.Add(new NetworkCommunicationController(_peerManager, _userLocal, conv, buddy));
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
