using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Model;
using Chat.View;
using System.Windows.Forms;

namespace Chat.Controller
{
    /// <summary>
    /// controls the central buddy list
    /// </summary>
    public class BuddyListController
    {
        // Model
        private UserLocal _userLocal;

        // View
        public BuddyListGroupBox BuddyListGroupBox;
        private BuddyAddForm _buddyAddForm;

        // Controller
        private MessengerController _messengerController;

        private int _standardPort;

        // <summary>
        
        // </summary>
        
        
        /// <summary>
        /// Create a controller for the buddy list
        /// </summary>
        /// <param name="userLocal">local user</param>
        /// <param name="messengerController">the global messenger controller</param>
        /// <param name="port">port to be used</param>
        public BuddyListController(UserLocal userLocal, MessengerController messengerController, int port)
        {
            _standardPort = port;

            _userLocal = userLocal;
            _messengerController = messengerController;

            BuddyListGroupBox = new BuddyListGroupBox();

            foreach (UserRemote buddy in _userLocal.Buddies)
            {
                _onUserLocalBuddyAdd(userLocal, buddy);
            }

            // binding model delegates
            _userLocal.BuddyAdd += _onUserLocalBuddyAdd;
            _userLocal.BuddyRemove += _onUserLocalBuddyRemove;

            // binding view delegates
            BuddyListGroupBox.OpenChatAction += _onOpenChatAction;
            BuddyListGroupBox.AddToChatAction += _onAddToChatAction;
            BuddyListGroupBox.BuddyRemoveAction += _onBuddyRemoveAction;
            BuddyListGroupBox.BuddyAddAction += _onBuddyAddAction;
            BuddyListGroupBox.OpenRecentChatsAction += _onBuddyOpenRecentChatsAction;
        }

        /// <summary>
        /// find a buddy given his/her ID
        /// </summary>
        /// <param name="id">the ID in question</param>
        /// <returns>the buddy having the given ID</returns>
        private UserRemote _getBuddyById(int id)
        {
            // find buddy in buddies with id == id
            return _userLocal.Buddies.First(b => b.Id == id);
        }

        // Model delegates

        /// <summary>
        /// Handler for "local user adds buddy"
        /// </summary>
        /// <param name="userLocal">local user</param>
        /// <param name="buddy">buddy to be added</param>
        private void _onUserLocalBuddyAdd(UserLocal userLocal, UserRemote buddy)
        {
            BuddyListGroupBox.AddBuddy(buddy.Id, buddy.Name);
        }

        /// <summary>
        /// Handler for "local user removes buddy"
        /// </summary>
        /// <param name="userLocal">local user</param>
        /// <param name="buddy">buddy to be removed</param>
        private void _onUserLocalBuddyRemove(UserLocal userLocal, UserRemote buddy)
        {
            BuddyListGroupBox.RemoveBuddy(buddy.Id);
        }

        // View delegates

        /// <summary>
        /// Handler for "open chat with user with given id"
        /// </summary>
        /// <param name="id">buddy id</param>
        private void _onOpenChatAction(int id)
        {
            // find the buddy by id
            UserRemote buddy = _getBuddyById(id);

            // get a converstation with buddy
            Conversation conv = _messengerController.GetDialog(buddy);

            // activate conversation
            conv.SetActive(true);
            
            // get the tabPage that displays this conversation
            ConversationTabPage tabPage = _messengerController.GetConversationController(conv).TabPage;

            // change the displayed tab of tabControl to this tabPage
            _messengerController.TabControl.ChangeActiveTab(tabPage);
        }

        /// <summary>
        /// Handler for "add buddy with given id to chat"
        /// </summary>
        /// <param name="id">buddy id</param>
        private void _onAddToChatAction(int id)
        {
            // add the buddy with the given id to the active conversation
            _messengerController.GetActiveConversationController().AddBuddy(_getBuddyById(id));
        }

        //private void _onRemoveFromChatAction(int id)
        //{
        //    _messengerController.GetActiveConversation().RemoveBuddy(_getBuddyById(id));
        //}

        /// <summary>
        /// Handler for "remove buddy with given id"
        /// </summary>
        /// <param name="id">buddy id</param>
        private void _onBuddyRemoveAction(int id)
        {
            UserRemote buddy = _getBuddyById(id);
            if (BuddyListGroupBox.AskForBuddyRemove(buddy.Name))
            {
                _userLocal.RemoveBuddy(buddy);
            }
        }

        /// <summary>
        /// Handler for "add buddy"
        /// </summary>
        private void _onBuddyAddAction()
        {
            _buddyAddForm = new BuddyAddForm(_standardPort);
            _buddyAddForm.BuddyAddSubmit += _onBuddyAddSubmit;
            _buddyAddForm.ShowDialog();
        }

        /// <summary>
        /// Handler for "buddy add (form) is submitted"
        /// </summary>
        /// <param name="userName">the buddy's user name</param>
        /// <param name="IP">the buddy's IP address</param>
        private void _onBuddyAddSubmit(string userName, string IP, int port)
        {
            _userLocal.AddBuddy(new UserRemote() { Name = userName, IP = IP, Port = port, BuddyOf = _userLocal });
            _buddyAddForm.Close();
        }
        /// <summary>
        /// Handler for "(re)open recent chats with a buddy given his/her id"
        /// </summary>
        /// <param name="id">the buddy's id</param>
        private void _onBuddyOpenRecentChatsAction(int id)
        {
            UserRemote buddy = _getBuddyById(id);
            foreach (Conversation conv in _userLocal.Conversations)
            {
                if (conv.Buddies.Contains(buddy))
                {
                    conv.SetActive(true);
                }
            }
        }
    }
}
