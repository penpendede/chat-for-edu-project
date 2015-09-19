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

        /// <summary>
        /// Create new Messenger Controller
        /// </summary>
        public MessengerController()
        {
            // setting the standard port
            _standardPort = 4711;

            // initialization
            _conversationControllers = new List<ConversationController>();
            _databaseController = new DatabaseController();

            _mainWindow = new MessengerMainWindowForm();
            _mainWindow.Hide();
            _mainWindow.FormClosing += _mainWindowOnClosing;

            TabControl = new ConversationTabControl();
            TabControl.TabClose += _conversationTabOnClose;
            _mainWindow.AddConversationTabControl(TabControl);

            List<string> usedUsernames;
			usedUsernames = _databaseController.UserLocalRepo.GetAllUserNames();

            _newUserForm = new NewUserForm(usedUsernames, _standardPort);
            _newUserForm.NewUser += _newUserFormOnNewUser;
            _newUserForm.FormClosing += _onLoginFormClosing;

            // peerManager gets initialized after login

            _keepMainWindow = false;

			if (usedUsernames.Count != 0)
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

        /// <summary>
        /// Get the active conversation
        /// </summary>
        /// <returns>the active conversation</returns>
        public Conversation GetActiveConversationController()
        {
            // returns the conversation whichs tab is selected
            // lambda maps a conversation controller to the truth value of
            // "conversation controller's tab page is tab control's selected tab"
            return _conversationControllers.First(cC => cC.TabPage == TabControl.SelectedTab).Conversation;
        }

        /// <summary>
        /// Get conversation for a given (remote) sender
        /// </summary>
        /// <param name="partner">the sender (a remote user)</param>
        /// <returns>the dialog for the sender</returns>
        public Conversation GetDialog(UserRemote partner)
        {
            // find a conversation with partner, that is not closed and not a group chat
            Conversation conv = _userLocal.Conversations.FirstOrDefault(c => c.Buddies.Contains(partner) && !c.Closed && c.Buddies.Count == 1);
            
            if (conv == null) 
            {
                // create this conversation if it doesn't exist
                conv = new Conversation() { UserLocal = _userLocal };
                conv.AddBuddy(partner);
                _userLocal.AddConversation(conv);
            }

            return conv;
        }

        /// <summary>
        /// Obtain conversation controller for a given conversation
        /// </summary>
        /// <param name="conv">the conversation for which the controller is searched</param>
        /// <returns>conversation's controller</returns>
        public ConversationController GetConversationController(Conversation conv)
        {
            // find the conversation controller for conv (if it exists)
            return _conversationControllers.FirstOrDefault(cC => cC.Conversation == conv);
        }

        #region login process

        /// <summary>
        /// Handler for "login form closing"
        /// </summary>
        /// <param name="sender">unused</param>
        /// <param name="e">unused</param>
        private void _onLoginFormClosing(object sender, EventArgs e)
        {
            if (!_keepMainWindow)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Handler for "new user" (occurring in login form)
        /// </summary>
        private void _loginFormOnNewUser()
        {
            _loginForm.Hide();
            _newUserForm.ShowDialog();
        }

        /// <summary>
        /// Initialize main window
        /// </summary>
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

        /// <summary>
        /// Handler for "submit (credentials)" (occurring in login form)
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password"></param>
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

        /// <summary>
        /// Handler for "(add) new user" (occurring in new user form)
        /// </summary>
        /// <param name="userName">name of user to be added</param>
        /// <param name="port">user's port</param>
        /// <param name="password">user's password</param>
        /// <param name="passwordRepetition">user's password, repeated</param>
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

        /// <summary>
        /// activate conversation
        /// </summary>
        /// <param name="conv">conversation</param>
        /// <returns>controller for the provided conversation</returns>
        private ConversationController _activateConversation(Conversation conv)
        {

            ConversationController convController = GetConversationController(conv);

            if (convController == null)
            {
                 convController = new ConversationController(_userLocal, conv, TabControl, _peerManager);
                _conversationControllers.Add(convController);
            }

            TabControl.ChangeActiveTab(convController.TabPage);

            return convController;
        }

        /// <summary>
        /// deactivate conversation
        /// </summary>
        /// <param name="conv">conversation to be deacitvated</param>
        private void _deactivateConversation(Conversation conv)
        {
            ConversationController convController = GetConversationController(conv);

            if (convController != null)
            {
                _conversationControllers.Remove(convController);
                convController.Dispose();
            }
        }

        /// <summary>
        /// handler for "toggle activity state of conversation"
        /// </summary>
        /// <param name="conv">conversation</param>
        /// <param name="active">conversation's current activity status</param>
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

        /// <summary>
        /// Handler for "add conversation"
        /// </summary>
        /// <param name="user"></param>
        /// <param name="conversation"></param>
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

        /// <summary>
        /// handler for "peer manager connection occurs"
        /// </summary>
        /// <param name="peer">the peer representing the connection</param>
        private void _onPeerManagerConnection(TcpPeer peer)
        {
            bool resolved = false;

            // this lambda expression waits for the first incoming message
            // then it decides to which conversation this tcpPeer belongs
            // after that it doesn't do anything anymore
            peer.MessageReceive += s =>
            {
                if (!resolved)
                {
                    // deserialize the message
                    Dictionary<string, string> messageDict = NetworkMessageInterpreter.Deserialize(s);

                    // get the sender of the message
                    UserRemote sender = NetworkMessageInterpreter.GetSender(messageDict, _userLocal);

                    // if the sender is not a buddy ask the user if the buddy should be added
                    if (!_userLocal.Buddies.Contains(sender))
                    {
                        if (_mainWindow.AskForBuddyAdd(sender.Name, sender.IP, sender.Port))
                        {
                            _userLocal.AddBuddy(sender);
                        }
                    }

                    Conversation conv = GetDialog(sender); // TODO: support group chats

                    //get the networkCommunicationController which handles the communication with the sender in the given conversation
                    ConversationController convContr = _activateConversation(conv);
                    NetworkCommunicationController netContr = convContr.GetNetworkCommunicationController(sender);

                    // give the peer to this networkconversation controller
                    netContr.Peer = peer;
                    // handle the first message
                    netContr.OnMessageReceive(s);

                    resolved = true;
                }
            };

            // start listening after the delegate got registered
            peer.StartListening();
        }

        #endregion

        #region closing windows

        /// <summary>
        /// HAndler for "main window closing"
        /// </summary>
        /// <param name="o">unused</param>
        /// <param name="e">unused</param>
        private void _mainWindowOnClosing(object o, EventArgs e)
        {
            _peerManager.Dispose();
        }

        /// <summary>
        /// handler for "conversation tab closing"
        /// </summary>
        /// <param name="tabPage"></param>
        /// <param name="closeConversation"></param>
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
