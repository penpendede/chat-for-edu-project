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
        public Conversation Conversation
        {
            private set;
            get;
        }

        public UserRemote UserRemote
        {
            private set;
            get;
        }

        private UserLocal _userLocal;

        private TcpPeer _peer;

        public TcpPeer Peer
        {
            get
            {
                if (_peer == null || !_peer.IsConnected())
                {
                    _peer = _peerManager.GetPeer(UserRemote.IP, UserRemote.Port);
                    _peer.MessageReceive += OnMessageReceive;
                }
                return _peer;
            }
            set
            {
                _peer = value;
                _peer.MessageReceive += OnMessageReceive;

                // if peer received messages before being added to a conversation, handle them
                //while (Peer.UnhandledMessages.Count > 0)
                //{
                //    _onMessageReceive(Peer.UnhandledMessages.Dequeue());
                //}
            }
        }


        private TcpPeerManager _peerManager;

        public NetworkCommunicationController(TcpPeerManager peerManager, UserLocal userLocal, Conversation conv, UserRemote userRemote)
        {
            _peerManager = peerManager;

            _userLocal = userLocal;
            Conversation = conv;
            UserRemote = userRemote;
            Conversation.MessageAdd += _conversationOnMessageAdd;
        }

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
                    break;
            }
            
        }

        public void Dispose()
        {
            Conversation.MessageAdd -= _conversationOnMessageAdd;
        }
    }
}
