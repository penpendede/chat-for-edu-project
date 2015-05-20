using System;
using System.Collections.Generic;
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

        //private List<ConversationController> conversationControllers; // NOTE: needed
        //private ConversationTabControl TabControl;

        public MessengerController()
        {
            mainWindow = new MessengerMainWindowForm();
            mainWindow.Hide();
            mainWindow.FormClosing += this.mainWindowOnClosing;

            loginForm = new LoginForm();
            loginForm.OnLoginSubmit += this.LoginFormOnSubmit;
            loginForm.ShowDialog();

            Application.Run(mainWindow);
        }

        private void LoginFormOnSubmit(string userName, string password)
        {

            // verify user
            #warning Not loading user from database

            userLocal = DummyData.CreateUserLocalTest1();

            // TODO: bind delegate OnUserConversationAdd to UserLocal

            mainWindow = new MessengerMainWindowForm();

            foreach (User buddy in userLocal.Buddies)
            {
                mainWindow.BuddyListGroupBox.AddBuddy(buddy.Id, buddy.Name);
            }

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
            //conversationControllers.Add(new ConversationController(conversation)) // NOTE:
            mainWindow.ConversationTabControl.AddTab(convController.TabPage);
        }

        private void mainWindowOnClosing(object o, EventArgs e)
        {
            // TODO: implement
        }
    }
}
