using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Model;
using Chat.View;
using System.Windows.Forms;

namespace Chat.Controller
{
    public class BuddyListController
    {
        // Model
        private UserLocal _userLocal;

        // View
        public BuddyListGroupBox BuddyListGroupBox;
        private BuddyAddForm _buddyAddForm;

        // Controller
        private MessengerController _messengerController;
        
        public BuddyListController(UserLocal userLocal, MessengerController messengerController)
        {
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

        private UserRemote _getBuddyById(int id)
        {
            return _userLocal.Buddies.First(b => b.Id == id);
        }

        // Model delegates
        private void _onUserLocalBuddyAdd(UserLocal userLocal, UserRemote buddy)
        {
            BuddyListGroupBox.AddBuddy(buddy.Id, buddy.Name);
        }

        private void _onUserLocalBuddyRemove(UserLocal userLocal, UserRemote buddy)
        {
            BuddyListGroupBox.RemoveBuddy(buddy.Id);
        }

        // View delegates
        private void _onOpenChatAction(int id)
        {
            UserRemote buddy = _getBuddyById(id);
            ConversationController convContr =
                _messengerController.GetDialogController(buddy);
            if (convContr != null) // check if dialog already exists
            {
                convContr.Conversation.SetActive(true);
                _messengerController.TabControl.ChangeActiveTab(convContr.TabPage);
            }
            else
            {
                Conversation conv = new Conversation() {UserLocal = _userLocal};
                conv.SetActive(true);
                conv.AddBuddy(buddy);
                _userLocal.AddConversation(conv);
            }
        }

        private void _onAddToChatAction(int id)
        {
            _messengerController.GetActiveConversation().AddBuddy(_getBuddyById(id));
        }

        //private void _onRemoveFromChatAction(int id)
        //{
        //    _messengerController.GetActiveConversation().RemoveBuddy(_getBuddyById(id));
        //}

        private void _onBuddyRemoveAction(int id)
        {
            UserRemote buddy = _getBuddyById(id);
            if (BuddyListGroupBox.AskForBuddyRemove(buddy.Name))
            {
                _userLocal.RemoveBuddy(buddy);
            }
        }

        private void _onBuddyAddAction()
        {
            _buddyAddForm = new BuddyAddForm();
            _buddyAddForm.BuddyAddSubmit += _onBuddyAddSubmit;
            _buddyAddForm.ShowDialog();
        }

        private void _onBuddyAddSubmit(string userName, string IP)
        {
            _userLocal.AddBuddy(new UserRemote() { Name = userName, IP = IP, BuddyOf = _userLocal });
            _buddyAddForm.Close();
        }

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
