using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Model;
using Chat.View;

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
                BuddyListGroupBox.AddBuddy(buddy.Id, buddy.Name);
            }

            BuddyListGroupBox.OpenChatAction += this.onOpenChatAction;
            BuddyListGroupBox.AddToChatAction += this.onAddToChatAction;
            BuddyListGroupBox.BuddyRemoveAction += this.onBuddyRemoveAction;
            BuddyListGroupBox.BuddyAddAction += this.onBuddyAddAction;
        }

        private UserRemote getBuddyById(int id)
        {
            return userLocal.Buddies.Where(b => b.Id == id).First();
        }

        private void onOpenChatAction(int id)
        {
            this.userLocal.AddConversation(new Conversation() { Users = new List<User>() { getBuddyById(id) } });
        }

        private void onAddToChatAction(int id)
        {
            this.messengerController.GetActiveConversation().AddUser(getBuddyById(id));
        }

        private void onBuddyRemoveAction(int id)
        {
            this.userLocal.RemoveBuddy(getBuddyById(id));
        }

        private void onBuddyAddAction()
        {
            buddyAddForm = new BuddyAddForm();
            buddyAddForm.BuddyAddSubmit += this.onBuddyAddSubmit;
            buddyAddForm.ShowDialog();
        }

        private void onBuddyAddSubmit(string userName, string IP)
        {
            userLocal.AddBuddy(new UserRemote() { Name = userName, IP = IP });
            buddyAddForm.Close();
        }
    }
}
