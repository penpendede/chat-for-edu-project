using Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Controller
{
    public class NetworkConnectionController
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

        public NetworkConnectionController(UserLocal userLocal, Conversation conv, UserRemote userRemote)
        {
            _userLocal = userLocal;
            Conversation = conv;
            UserRemote = userRemote;
            Conversation.MessageAdd += _conversationOnMessageAdd;
        }

        private void _conversationOnMessageAdd(Conversation conv, Message message)
        {
            if (message.Sender == _userLocal)
            {
                Conversation.AddMessage(new Message() { Sender = UserRemote, Text = "What you said: " + message.Text });
            }
        }

        public void Dispose()
        {
            Conversation.MessageAdd -= _conversationOnMessageAdd;
        }
    }
}
