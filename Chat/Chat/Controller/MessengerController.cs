using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Model;
using Chat.View;
using System.Windows.Forms;

namespace Chat.Controller
{
    public class MessengerController
    {
        private UserLocal userLocal;
        private LoginForm loginForm;
        private MessengerMainWindowForm mainWindow;

        private List<ConversationController> conversationControllers;
        private BuddyListController buddyListController;
        //private ConversationTabControl TabControl;

        public MessengerController()
        {
            conversationControllers = new List<ConversationController>();

            mainWindow = new MessengerMainWindowForm();
            mainWindow.Hide();
            mainWindow.FormClosing += this.mainWindowOnClosing;

            loginForm = new LoginForm();
            loginForm.OnLoginSubmit += this.loginFormOnSubmit;
            loginForm.ShowDialog();

            Application.Run(mainWindow);
        }

        // NOTE: Is this the right place for this functionality?
        public Conversation GetActiveConversation()
        {
            return this.conversationControllers.Where(c => c.TabPage == this.mainWindow.ConversationTabControl.SelectedTab).First().Conversation;
        }

        private void loginFormOnSubmit(string userName, string password)
        {

            // verify user
            #warning Not loading user from database

            userLocal = DummyData.CreateUserLocalTest1();

            // TODO: bind delegate OnUserConversationAdd to UserLocal

            buddyListController = new BuddyListController(userLocal, this);

            mainWindow.AddBuddyListGroupBox(buddyListController.BuddyListGroupBox);

            foreach (Conversation conversation in userLocal.Conversations)
            {
                userOnConversationAdd(conversation);
            }

            loginForm.Close(); // NOTE: dispose
            mainWindow.Show();
        }

        private void userOnConversationAdd(Conversation conversation)
        {
            ConversationController convController = new ConversationController(userLocal, conversation);
            conversationControllers.Add(convController);
            mainWindow.ConversationTabControl.AddTab(convController.TabPage);
        }

        private void mainWindowOnClosing(object o, EventArgs e)
        {
            // TODO: implement
        }
    }
}
