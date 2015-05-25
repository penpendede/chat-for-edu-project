using Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Controller
{
    public class DatabaseController
    {
        public Database Database;
 
        public ConversationRepository ConversationRepo;

        public IBaseRepository<UserLocal> UserLocalRepo;
        public IBaseRepository<UserRemote> UserRemoteRepo;
        public IBaseRepository<Message> MessageRepo;
    }
}
