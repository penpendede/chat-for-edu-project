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
        private BuddyListController _senderListController;
        private DatabaseController _databaseController;
        private List<ConversationController> _conversationControllers;
        private TcpPeerManager _peerManager;
        

        // Other 
        private bool _keepMainWindow;
        private int _standardPort;

        public MessengerController()
        {
            _standardPort = 4711;

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
            _newUserForm = new NewUserForm(usernames, _standardPort);
            _newUserForm.NewUser += _newUserFormOnNewUser;
            _newUserForm.FormClosing += _onLoginFormClosing;

            // peerManager gets initialized after login

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

        public Conversation GetActiveConversationController()
        {
            return _conversationControllers.First(cC => cC.TabPage == TabControl.SelectedTab).Conversation;
        }

        public Conversation GetDialog(UserRemote sender)
        {
            Conversation conv = _userLocal.Conversations.FirstOrDefault(c => c.Buddies.Contains(sender) && !c.Closed && c.Buddies.Count == 1);
            
            if (conv == null) 
            {
                conv = new Conversation() { UserLocal = _userLocal };
                conv.AddBuddy(sender);
                _userLocal.AddConversation(conv);
            }

            return conv;
        }

        public ConversationController GetConversationController(Conversation conv)
        {
            return _conversationControllers.FirstOrDefault(cC => cC.Conversation == conv);
        }

        #region login process

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

        private void _initMainWindow()
        {
            _keepMainWindow = true;

            _mainWindow.SetUserName(_userLocal.Name);

            _peerManager = new TcpPeerManager(_userLocal.Port);

            _peerManager.OnConnect += _onPeerManagerConnection;

            _userLocal.IP = _peerManager.GetLocalIpAddress();

            _userLocal.ConversationAdd += _userOnConversationAdd;
            _senderListController = new BuddyListController(_userLocal, this, _standardPort);
            _mainWindow.AddBuddyListGroupBox(_senderListController.BuddyListGroupBox);

            foreach (Conversation conversation in _userLocal.Conversations)
            {
                _userOnConversationAdd(_userLocal, conversation);
            }

            _mainWindow.Show();
        }

        private void _loginFormOnSubmit(string userName, string password)
        {

            StatusVerifyPassword status = _databaseController.GetUserLocal(userName, password, out _userLocal);

            if (status == StatusVerifyPassword.OK)
            {
                _initMainWindow();

                _loginForm.Close();
                _loginForm.Dispose();

            }
            else if (status == StatusVerifyPassword.USER_NAME_NOT_FOUND)
            {
                _loginForm.UsernameUnknownMessage(userName);
            }
            else if (status == StatusVerifyPassword.WRONG_PASSWORD)
            {
                _loginForm.WrongPasswordMessage(userName);
            }
        }

        private void _newUserFormOnNewUser(string userName, int port, string password, string passwordRepetition)
        {
            bool isValid = true;

            if (userName.Length == 0)
            {
                _newUserForm.UsernameIsMissingMessage();
                isValid = false;
            }
            else if (password.Length == 0)
            {
                _newUserForm.PasswordIsMissingMessage();   
                isValid = false;
            }
            else if (passwordRepetition.Length == 0)
            {
                _newUserForm.PasswordRepeatIsMissingMessage();
                isValid = false;
            }
            else if (password != passwordRepetition)
            {
                _newUserForm.PasswordsMismatchMessage();
                isValid = false;
            }

            if (isValid)
            {
                if (_databaseController.UserLocalRepo.IsUserNameTaken(userName))
                {
                    _newUserForm.UsernameExistsMessage(userName);
                }
                else 
                {
                    _userLocal = new UserLocal { Name = userName, Port = port };

                    _databaseController.UserLocalRepo.Insert(_userLocal);

                    _databaseController.UserLocalRepo.SetNewPassword(userName, password);

                    _initMainWindow();

                    _newUserForm.Close();
                    _newUserForm.Dispose();
                }
            }
        }

        #endregion

        #region managing conversations

        private ConversationController _activateConversation(Conversation conv)
        {
            ConversationController convController =
                _conversationControllers.FirstOrDefault(cC => cC.Conversation == conv);
            if (convController == null)
            {
                 convController = new ConversationController(_userLocal, conv, TabControl, _peerManager);
                _conversationControllers.Add(convController);
            }
            TabControl.ChangeActiveTab(convController.TabPage);

            return convController;
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

        #endregion

        #region network

        private void _onPeerManagerConnection(TcpPeer peer)
        {
            bool resolved = false;

            peer.MessageReceive += s =>
            {
                if (!resolved)
                {
                    Dictionary<string, string> messageDict = NetworkMessageInterpreter.Deserialize(s);

                    UserRemote sender = NetworkMessageInterpreter.GetSender(messageDict, _userLocal);

                    if (!_userLocal.Buddies.Contains(sender))
                    {
                        if (_mainWindow.AskForBuddyAdd(sender.Name, sender.IP, sender.Port))
                        {
                            _userLocal.AddBuddy(sender);
                        }
                    }

                    Conversation conv = GetDialog(sender); // TODO: support group chats

                    //get the networkCommunicationController which handles the communication with the sender in the given conversation
                    ConversationController convContr = GetConversationController(conv);
                    if (convContr == null) {
                        convContr = _activateConversation(conv);
                    }

                    NetworkCommunicationController netContr = convContr.GetNetworkCommunicationController(sender);
                    netContr.Peer = peer;
                    netContr.OnMessageReceive(s);

                    resolved = true;
                }
            };

            peer.StartListening();
        }

        #endregion

        #region closing windows

        private void _mainWindowOnClosing(object o, EventArgs e)
        {
            _peerManager.Dispose();
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

        #endregion
    }
}
