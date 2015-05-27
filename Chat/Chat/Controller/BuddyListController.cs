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
        public BuddyListGroupBox BuddyListGroupBox;

        private UserLocal userLocal;
        private MessengerController messengerController;

        private BuddyAddForm buddyAddForm;

        public BuddyListController(UserLocal userLocal, MessengerController messengerController)
        {
            this.userLocal = userLocal;
            this.messengerController = messengerController;

            BuddyListGroupBox = new BuddyListGroupBox();

            foreach (UserRemote buddy in this.userLocal.Buddies)
            {
                onUserLocalBuddyAdd(userLocal, buddy);
            }

            this.userLocal.BuddyAdd += onUserLocalBuddyAdd;
            this.userLocal.BuddyRemove += onUserLocalBuddyRemove;

            BuddyListGroupBox.OpenChatAction += this.onOpenChatAction;
            BuddyListGroupBox.AddToChatAction += this.onAddToChatAction;
            BuddyListGroupBox.BuddyRemoveAction += this.onBuddyRemoveAction;
            BuddyListGroupBox.BuddyAddAction += this.onBuddyAddAction;
            BuddyListGroupBox.RemoveFromChatAction += this._onRemoveFromChatAction;
            BuddyListGroupBox.OpenRecentChatsAction += _onBuddyOpenRecentChatsAction;
        }

        private void onUserLocalBuddyAdd(UserLocal userLocal, UserRemote buddy)
        {
            BuddyListGroupBox.AddBuddy(buddy.Id, buddy.Name);
        }

        private void onUserLocalBuddyRemove(UserLocal userLocal, UserRemote buddy)
        {
            BuddyListGroupBox.RemoveBuddy(buddy.Id);
        }

        private UserRemote getBuddyById(int id)
        {
            return userLocal.Buddies.Where(b => b.Id == id).First();
        }

        private void onOpenChatAction(int id)
        {
            Conversation conv = new Conversation() { Owner = userLocal };
            conv.SetActive(true);
            conv.AddBuddy(getBuddyById(id));
            this.userLocal.AddConversation(conv);
        }

        private void onAddToChatAction(int id)
        {
            this.messengerController.GetActiveConversation().AddBuddy(getBuddyById(id));
        }

        private void _onRemoveFromChatAction(int id)
        {
            this.messengerController.GetActiveConversation().RemoveBuddy(getBuddyById(id));
        }

        private void onBuddyRemoveAction(int id)
        {
            if (MessageBox.Show("Möchten Sie diesen Buddy wirklich von Ihrer Freundesliste entfernen? Der Nachrichtenverlauf wird gelöscht!", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.userLocal.RemoveBuddy(getBuddyById(id));
            }
        }

        private void onBuddyAddAction()
        {
            buddyAddForm = new BuddyAddForm();
            buddyAddForm.BuddyAddSubmit += this.onBuddyAddSubmit;
            buddyAddForm.ShowDialog();
        }

        private void onBuddyAddSubmit(string userName, string IP)
        {
            userLocal.AddBuddy(new UserRemote() { Name = userName, IP = IP, BuddyOf = userLocal });
            buddyAddForm.Close();
        }

        private void _onBuddyOpenRecentChatsAction(int id)
        {
            UserRemote buddy = getBuddyById(id);
            foreach (Conversation conv in userLocal.Conversations)
            {
                if (conv.Buddies.Contains(buddy))
                {
                    conv.SetActive(true);
                }
            }
        }
    }
}
