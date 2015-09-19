using Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Chat.Controller
{
    public class NetworkCommunicationController
    {
        // Model

        // getter and setter for having different access restrictions
        public Conversation Conversation
        {
            private set;
            get;
        }

        // getter and setter for having different access restrictions
        public UserRemote UserRemote
        {
            private set;
            get;
        }

        private UserLocal _userLocal;

        private TcpPeer _peer;

        // getter and setter actually perform tasks besides assigning values
        public TcpPeer Peer
        {
            get
            {
                if (_peer == null || !_peer.IsConnected()) // create if not yet existing or not connected
                {
                    _peer = _peerManager.GetPeer(UserRemote.IP, UserRemote.Port);
                    _peer.MessageReceive += OnMessageReceive;
                    _peer.StartListening();
                }
                return _peer;
            }
            set
            {
                _peer = value;
                _peer.MessageReceive += OnMessageReceive;
                _peer.StartListening();

                // if peer received messages before being added to a conversation, handle them
                //while (Peer.UnhandledMessages.Count > 0)
                //{
                //    _onMessageReceive(Peer.UnhandledMessages.Dequeue());
                //}
            }
        }


        private TcpPeerManager _peerManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peerManager">peer manager to use</param>
        /// <param name="userLocal">local user to use</param>
        /// <param name="conv">conversation to use</param>
        /// <param name="userRemote">remote user to use</param>
        public NetworkCommunicationController(TcpPeerManager peerManager, UserLocal userLocal, Conversation conv, UserRemote userRemote)
        {
            _peerManager = peerManager;

            _userLocal = userLocal;
            Conversation = conv;
            UserRemote = userRemote;
            Conversation.MessageAdd += _conversationOnMessageAdd;
        }

        /// <summary>
        /// Handler for "add message to conversation"
        /// </summary>
        /// <param name="conv">conversation to which message is to be added</param>
        /// <param name="message">message to be added</param>
        private void _conversationOnMessageAdd(Conversation conv, Message message)
        {
            if (message.Sender == _userLocal)
            {
                string serialized = NetworkMessageInterpreter.Serialize(NetworkMessageInterpreter.SerializeTextMessage(message, _userLocal));
                if (Peer.SendMessage(serialized) == TcpPeerStatus.NOT_CONNECTED)
                {
                    Conversation.AddMessage(new Message() { Sender = UserRemote.SystemUser, Text = "Empfänger ist nicht verbunden" });
                }
            }
        }

        /// <summary>
        /// Handler for "message received"
        /// </summary>
        /// <param name="msg">the message that is received</param>
        public void OnMessageReceive(string msg)
        {
            Dictionary<string, string> messageDict = NetworkMessageInterpreter.Deserialize(msg);
            switch (NetworkMessageInterpreter.GetType(messageDict)) {
                case MessageType.MSG:
                    Conversation.AddMessage(NetworkMessageInterpreter.DeserializeTextMessage(messageDict, _userLocal));
                    break;
                case MessageType.CLOSE:
                    UserRemote buddy;
                    string quitMessage = NetworkMessageInterpreter.DeserializeQuitMessage(messageDict, _userLocal, out buddy);
                    Conversation.AddMessage(new Message() { Sender = UserRemote.SystemUser, Text = string.Format("Benutzer {0} hat den Chat verlassen: {1}", buddy.Name, quitMessage) });
                    break;
                case MessageType.BINARY:
                    throw new NotImplementedException();
                    // no break here as it would be unreachable code
            }
            
        }

        /// <summary>
        /// Dispose network communication controller: just don't use _conversationOnMessageAdd anymore
        /// </summary>
        public void Dispose()
        {
            Conversation.MessageAdd -= _conversationOnMessageAdd;
        }
    }
}
