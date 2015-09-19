using Chat.Model;
using Chat.View;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Controller
{
    /// <summary>
    /// Controls an individual conversation
    /// </summary>
    public class ConversationController
    {
        // Model
        private UserLocal _userLocal;

        /// <summary>
        /// getter and setter are so that read and write access can have different access control
        /// </summary>
        public Conversation Conversation
        {
            private set;
            get;
        }

        // View
        private ConversationTabControl _tabControl;

        /// <summary>
        /// getter and setter are so that read and write access can have different access control
        /// </summary>
        public ConversationTabPage TabPage
        {
            private set;
            get;
        }

        // Other controllers
        private List<NetworkCommunicationController> _networkConnectionControllers;
        private TcpPeerManager _peerManager; // only for pass-through

        /// <summary>
        /// Create new ConversationController instance
        /// </summary>
        /// <param name="userLocal">Local user</param>
        /// <param name="conversation">conversation the conversation controller is for</param>
        /// <param name="tabControl">Conversation tab control, visual representation of conversation is added to it</param>
        /// <param name="peerManager">manager for the peers</param>
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

        /// <summary>
        /// communication controller which is responsible for the communication with the given user 
        /// </summary>
        /// <param name="remoteUser"></param>
        /// <returns>the corresponding communication controller or null if it doesn't exist</returns>
        public NetworkCommunicationController GetNetworkCommunicationController(UserRemote remoteUser)
        {
            // lambda maps a controller to the truth value of "controller's UserRemote is remoteUser"
            return _networkConnectionControllers.FirstOrDefault(c => c.UserRemote == remoteUser);
        }

        /// Add a message to the conversation (tab page)
        /// </summary>
        /// <param name="conv">unused</param>
        /// <param name="message">message to add</param>
        private void _conversationAddMessage(Conversation conv, Model.Message message)
        {
            TabPage.AddMessage(message.Sender.Name, message.Text, message.Time);
        }

        /// <summary>
        /// handler for "buddy is added"
        /// </summary>
        /// <param name="conv">Conversation to which the buddy is added</param>
        /// <param name="buddy">said buddy</param>
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

        /// <summary>
        /// handler for "conversation is closed"
        /// </summary>
        /// <param name="conv">unused</param>
        private void _conversationOnClose(Conversation conv)
        {
            TabPage.Disable();
        }
        /// <summary>
        /// handler for "text is submitted"
        /// </summary>
        /// <param name="messageText">the text of the message to be sent</param>
        private void _tabPageOnTextSubmit(string messageText)
        {
            Conversation.AddMessage(new Model.Message() { Text = messageText, Sender = this._userLocal });
        }

        /// <summary>
        /// Dispose the conversation controller
        /// </summary>
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
