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
        // Model
        private UserLocal _userLocal;

        // View
        private LoginForm _loginForm;
		private NewUserForm _newUserForm;
        private MessengerMainWindowForm _mainWindow;
        public ConversationTabControl TabControl;

        // Other controllers
        private BuddyListController _buddyListController;
        private DatabaseController _databaseController;
        private List<ConversationController> _conversationControllers;

        // Other 
        private bool _keepMainWindow;

        public MessengerController()
        {
			List<string> usernames;
            _conversationControllers = new List<ConversationController>();
            _databaseController = new DatabaseController();

            _mainWindow = new MessengerMainWindowForm();
            _mainWindow.Hide();
            _mainWindow.FormClosing += _mainWindowOnClosing;

            TabControl = new ConversationTabControl();
            TabControl.TabClose += _conversationTabOnClose;
            _mainWindow.AddConversationTabControl(TabControl);

			usernames = _databaseController.UserLocalRepo.GetAllUserNames();
            _newUserForm = new NewUserForm(usernames);
            _newUserForm.NewUser += this._newUserFormOnNewUser;
            _newUserForm.FormClosing += _onLoginFormClosing;

            _keepMainWindow = false;

			if (usernames.Count != 0)
            {
	            _loginForm = new LoginForm(_databaseController.UserLocalRepo.GetAllUserNames());
	            _loginForm.LoginSubmit += _loginFormOnSubmit;
	            _loginForm.NewUser += _loginFormOnNewUser;
	            _loginForm.FormClosing += _onLoginFormClosing;
	            _loginForm.ShowDialog();
			}
			else
            {
                _newUserForm.ShowDialog();
            }

            Application.Run(_mainWindow);
        }

        public Conversation GetActiveConversation()
        {
            return _conversationControllers.First(cC => cC.TabPage == TabControl.SelectedTab).Conversation;
        }

        public ConversationController GetDialogController(UserRemote buddy)
        {
            return _conversationControllers.FirstOrDefault(cC => cC.Conversation.Buddies.Contains(buddy) && !cC.Conversation.Closed && cC.Conversation.Buddies.Count == 1);
        }

        private void _onLoginFormClosing(object sender, EventArgs e)
        {
            if (!_keepMainWindow)
            {
                Application.Exit();
            }
        }

        private void _loginFormOnNewUser()
        {
            _loginForm.Hide();
            _newUserForm.ShowDialog();
        }

        private void _loginFormOnSubmit(string userName, string password)
        {
            try
            {
                _databaseController.UserLocalRepo.VerifyPassword(userName, password);

                // only reached if verifyPassword does not throw an error

                _userLocal = _databaseController.UserLocalRepo.GetByName(userName);

                _keepMainWindow = true;

                _userLocal.ConversationAdd += _userOnConversationAdd;

                _buddyListController = new BuddyListController(_userLocal, this);

                _mainWindow.AddBuddyListGroupBox(_buddyListController.BuddyListGroupBox);

                foreach (Conversation conversation in _userLocal.Conversations)
                {
                    _userOnConversationAdd(_userLocal, conversation);
                }

                _loginForm.Close(); // NOTE: dispose?
                _mainWindow.Show();
            }
            catch (UserNameNotFoundException e)
            {
                MessageBox.Show(string.Format("Der Username {0} ist nicht bekannt.", e.UserName), _mainWindow.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (WrongPasswordException e)
            {
                MessageBox.Show(string.Format("Der Passwort für User {0} ist falsch.", e.UserName), _mainWindow.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _newUserFormOnNewUser(string userName, string password, string passwordRepetition)
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
                    _userLocal = new UserLocal() { Name = userName };

                    _databaseController.UserLocalRepo.Insert(_userLocal);

                    _databaseController.UserLocalRepo.SetNewPassword(userName, "", password);

                    _mainWindow.SetUserName(userName);

                    _keepMainWindow = true;

                    _userLocal.ConversationAdd += _userOnConversationAdd;

                    _buddyListController = new BuddyListController(_userLocal, this);

                    _mainWindow.AddBuddyListGroupBox(_buddyListController.BuddyListGroupBox);

                    _newUserForm.Close();
                    //loginForm.Close();
                }
            }
            catch (NewUserNameAlreadyExists e)
            {
                MessageBox.Show(string.Format("Der Benutzername {0} existiert bereits.", e.UserName), _mainWindow.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _activateConversation(Conversation conv)
        {
            ConversationController convController =
                _conversationControllers.FirstOrDefault(cC => cC.Conversation == conv);
            if (convController == null)
            {
                 convController = new ConversationController(_userLocal, conv, TabControl);
                _conversationControllers.Add(convController);
            }
            TabControl.ChangeActiveTab(convController.TabPage);
        }

        private void _deactivateConversation(Conversation conv)
        {
            ConversationController convController = _conversationControllers.Find(c => c.Conversation == conv);
            if (convController != null)
            {
                _conversationControllers.Remove(convController);
                convController.Dispose();
            }
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

        private void _userOnConversationAdd(User user, Conversation conversation)
        {
            if (conversation.Active)
            {
                _activateConversation(conversation);
            }
            conversation.ChangeActive += _onConverationChangeActive;
        }

        private void _mainWindowOnClosing(object o, EventArgs e)
        {
            // TODO: implement
        }

        private void _conversationTabOnClose(TabPage tabPage, bool closeConversation)
        {
            ConversationController convController = _conversationControllers.Find(c => (c.TabPage as TabPage) == tabPage);
            convController.Conversation.SetActive(false);
            if ( closeConversation )
            {
                convController.Conversation.Close();    
            }
        }
    }
}
