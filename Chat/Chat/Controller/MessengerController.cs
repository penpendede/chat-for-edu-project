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

        private DatabaseController databaseController;

        private bool loginFormClosedKeepMainWindow;

        public MessengerController()
        {
            conversationControllers = new List<ConversationController>();
            databaseController = new DatabaseController();

            mainWindow = new MessengerMainWindowForm();
            mainWindow.Hide();
            mainWindow.FormClosing += this.mainWindowOnClosing;
            mainWindow.ConversationTabControl.TabClose += _onConversationTabClose;

            loginFormClosedKeepMainWindow = false;

            loginForm = new LoginForm(databaseController.GetLocalUserNames());
            loginForm.LoginSubmit += this.loginFormOnSubmit;
            loginForm.NewUser += this.loginFormOnNewUser;
            loginForm.FormClosing += this.onLoginFormClosing;
            loginForm.ShowDialog();

            Application.Run(mainWindow);
        }

        // NOTE: Is this the right place for this functionality?
        public Conversation GetActiveConversation()
        {
            return this.conversationControllers.Where(c => c.TabPage == this.mainWindow.ConversationTabControl.SelectedTab).First().Conversation;
        }

        private void onLoginFormClosing(object sender, EventArgs e)
        {
            if (!loginFormClosedKeepMainWindow)
            {
                Application.Exit();
            }
        }

        private void loginFormOnNewUser(string userName, string password)
        {
            try
            {
                userLocal = databaseController.CreateNewUser(userName, password);

                loginFormClosedKeepMainWindow = true;

                userLocal.ConversationAdd += userOnConversationAdd;

                buddyListController = new BuddyListController(userLocal, this);

                mainWindow.AddBuddyListGroupBox(buddyListController.BuddyListGroupBox);

                loginForm.Close(); // NOTE: dispose?
                mainWindow.Show();
            }
            catch (NewUserNameAlreadyExists e)
            {
                MessageBox.Show(string.Format("Der Username {0} existiert bereits.", e.UserName), mainWindow.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loginFormOnSubmit(string userName, string password)
        {
            try
            {
                userLocal = databaseController.LoadModelFor(userName, password);

                loginFormClosedKeepMainWindow = true;

                userLocal.ConversationAdd += userOnConversationAdd;

                buddyListController = new BuddyListController(userLocal, this);

                mainWindow.AddBuddyListGroupBox(buddyListController.BuddyListGroupBox);

                foreach (Conversation conversation in userLocal.Conversations)
                {
                    userOnConversationAdd(userLocal, conversation);
                }

                loginForm.Close(); // NOTE: dispose?
                mainWindow.Show();
            }
            catch (UserNameNotFoundException e)
            {
                MessageBox.Show(string.Format("Der Username {0} ist nicht bekannt.", e.UserName), mainWindow.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (WrongPasswordException e)
            {
                MessageBox.Show(string.Format("Der Passwort für User {0} ist falsch.", e.UserName), mainWindow.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _activateConversation(Conversation conv)
        {
            ConversationController convController = new ConversationController(userLocal, conv, mainWindow.ConversationTabControl);
            conversationControllers.Add(convController);
        }

        private void _deactivateConversation(Conversation conv)
        {
            ConversationController convController = conversationControllers.Find(c => c.Conversation == conv);
            conversationControllers.Remove(convController);
            convController.Dispose();
        }

        private void _onConverationChangeActive(Conversation conv, bool active)
        {
            if (active)
            {
                _activateConversation(conv);
            }
            else
            {
                _deactivateConversation(conv);
            }
        }

        private void userOnConversationAdd(User user, Conversation conversation)
        {
            if (conversation.Active)
            {
                _activateConversation(conversation);
            }
            conversation.ChangeActive += _onConverationChangeActive;
        }

        private void mainWindowOnClosing(object o, EventArgs e)
        {
            // TODO: implement
        }

        private void _onConversationTabClose(TabPage tabPage)
        {
            ConversationController convController = conversationControllers.Find(c => (c.TabPage as TabPage) == tabPage);
            convController.Conversation.SetActive(false);
        }
    }
}
