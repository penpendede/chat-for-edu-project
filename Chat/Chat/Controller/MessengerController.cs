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
        private NewUserForm newUserForm;
        private MessengerMainWindowForm mainWindow;

        private List<ConversationController> conversationControllers;
        private BuddyListController buddyListController;
        //private ConversationTabControl TabControl;

        private DatabaseController databaseController;

        private bool _keepMainWindow;

        public MessengerController()
        {
            List<string> usernames;
            conversationControllers = new List<ConversationController>();
            databaseController = new DatabaseController();

            mainWindow = new MessengerMainWindowForm();
            mainWindow.Hide();
            mainWindow.FormClosing += this.mainWindowOnClosing;
            mainWindow.ConversationTabControl.TabClose += _onConversationTabClose;

            usernames = databaseController.UserLocalRepo.GetAllUserNames();
            
            newUserForm = new NewUserForm(usernames);
            newUserForm.NewUser += this.newUserFormOnNewUser;
            newUserForm.FormClosing += this.onLoginFormClosing;

            _keepMainWindow = false;

            if (usernames.Count != 0)
            {
                loginForm = new LoginForm(databaseController.UserLocalRepo.GetAllUserNames());
                loginForm.LoginSubmit += this.loginFormOnSubmit;
                loginForm.NewUser += this.loginFormOnNewUser;
                loginForm.FormClosing += this.onLoginFormClosing;
                loginForm.ShowDialog();
            }
            else
            {
                newUserForm.ShowDialog();
            }

            Application.Run(mainWindow);
        }

        // NOTE: Is this the right place for this functionality?
        public Conversation GetActiveConversation()
        {
            return this.conversationControllers.Where(c => c.TabPage == this.mainWindow.ConversationTabControl.SelectedTab).First().Conversation;
        }

        private void onLoginFormClosing(object sender, EventArgs e)
        {
            if (!_keepMainWindow)
            {
                Application.Exit();
            }
        }

        private void loginFormOnNewUser()
        {
            loginForm.Hide();
            newUserForm.ShowDialog();
        }

        private void loginFormOnSubmit(string userName, string password)
        {
            try
            {
                databaseController.UserLocalRepo.VerifyPassword(userName, password);

                // only reached if verifyPassword does not throw an error

                userLocal = databaseController.UserLocalRepo.GetByName(userName);

                _keepMainWindow = true;

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
                MessageBox.Show(string.Format("Der Benutzername {0} ist nicht bekannt.", e.UserName), mainWindow.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (WrongPasswordException e)
            {
                MessageBox.Show(string.Format("Der Kennwort für User {0} ist falsch.", e.UserName), mainWindow.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void newUserFormOnNewUser(string userName, string password, string passwordRepetition)
        {
            try
            {
                bool isValid = true;

                if (userName.Length == 0)
                {
                    MessageBox.Show("Benutzername fehlt", "Benutzername fehlt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isValid = false;
                }
                else if (password.Length == 0)
                {
                    MessageBox.Show("Kennwort fehlt", "Kennwort fehlt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isValid = false;
                }
                else if (passwordRepetition.Length == 0)
                {
                    MessageBox.Show("Kennwortwiederholung fehlt", "Kennwortwiederholung fehlt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isValid = false;
                }
                else if (password != passwordRepetition)
                {
                    MessageBox.Show("Kennwort und Wiederholung sind verschieden", "Kennwort und Wiederholung verschieden", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isValid = false;
                }

                if (isValid)
                {
                    userLocal = new UserLocal() { Name = userName };

                    databaseController.UserLocalRepo.Insert(userLocal);

                    databaseController.UserLocalRepo.SetNewPassword(userName, "", password);

                    mainWindow.SetUserName(userName);

                    _keepMainWindow = true;

                    userLocal.ConversationAdd += userOnConversationAdd;

                    buddyListController = new BuddyListController(userLocal, this);

                    mainWindow.AddBuddyListGroupBox(buddyListController.BuddyListGroupBox);

                    newUserForm.Close();
                    //loginForm.Close();
                }
            }
            catch (NewUserNameAlreadyExists e)
            {
                MessageBox.Show(string.Format("Der Benutzername {0} existiert bereits.", e.UserName), mainWindow.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
