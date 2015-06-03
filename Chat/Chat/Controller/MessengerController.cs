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
        private MessengerMainWindowForm _mainWindow;
        //private ConversationTabControl TabControl;

        // Other controllers
        private BuddyListController _buddyListController;
        private DatabaseController _databaseController;
        private List<ConversationController> _conversationControllers;

        // Other 
        private bool _loginFormClosedKeepMainWindow;

        public MessengerController()
        {
            _conversationControllers = new List<ConversationController>();
            _databaseController = new DatabaseController();

            _mainWindow = new MessengerMainWindowForm();
            _mainWindow.Hide();
            _mainWindow.FormClosing += this._mainWindowOnClosing;
            _mainWindow.ConversationTabControl.TabClose += _onConversationTabClose;

            _loginFormClosedKeepMainWindow = false;

            _loginForm = new LoginForm(_databaseController.UserLocalRepo.GetAllUserNames());
            _loginForm.LoginSubmit += _loginFormOnSubmit;
            _loginForm.NewUser += _loginFormOnNewUser;
            _loginForm.FormClosing += _onLoginFormClosing;
            _loginForm.ShowDialog();

            Application.Run(_mainWindow);
        }

        // NOTE: Is this the right place for this functionality?
        public Conversation GetActiveConversation()
        {
            return _conversationControllers.Where(c => c.TabPage == this._mainWindow.ConversationTabControl.SelectedTab).First().Conversation;
        }

        private void _onLoginFormClosing(object sender, EventArgs e)
        {
            if (!_loginFormClosedKeepMainWindow)
            {
                Application.Exit();
            }
        }

        private void _loginFormOnNewUser(string userName, string password)
        {
            try
            {
                _userLocal = new UserLocal() { Name = userName };

                _databaseController.UserLocalRepo.Insert(_userLocal);

                _databaseController.UserLocalRepo.SetNewPassword(userName, "", password);

                _loginFormClosedKeepMainWindow = true;

                _userLocal.ConversationAdd += _userOnConversationAdd;

                _buddyListController = new BuddyListController(_userLocal, this);

                _mainWindow.AddBuddyListGroupBox(_buddyListController.BuddyListGroupBox);

                _loginForm.Close(); // NOTE: dispose?
                _mainWindow.Show();
            }
            catch (NewUserNameAlreadyExists e)
            {
                MessageBox.Show(string.Format("Der Username {0} existiert bereits.", e.UserName), _mainWindow.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _loginFormOnSubmit(string userName, string password)
        {
            try
            {
                _databaseController.UserLocalRepo.VerifyPassword(userName, password);

                // only reached if verifyPassword does not throw an error

                _userLocal = _databaseController.UserLocalRepo.GetByName(userName);

                _loginFormClosedKeepMainWindow = true;

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

        private void _activateConversation(Conversation conv)
        {
            ConversationController convController = new ConversationController(_userLocal, conv, _mainWindow.ConversationTabControl);
            _conversationControllers.Add(convController);
        }

        private void _deactivateConversation(Conversation conv)
        {
            ConversationController convController = _conversationControllers.Find(c => c.Conversation == conv);
            _conversationControllers.Remove(convController);
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

        private void _onConversationTabClose(TabPage tabPage)
        {
            ConversationController convController = _conversationControllers.Find(c => (c.TabPage as TabPage) == tabPage);
            convController.Conversation.SetActive(false);
        }
    }
}
