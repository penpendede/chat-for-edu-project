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

        public UserLocalRepository UserLocalRepo;
        public UserRemoteRepository UserRemoteRepo;
        public MessageRepository MessageRepo;

        public DatabaseController()
        {
            Database = new SQLiteDatabase();

            ConversationRepo = new ConversationRepository(this);
            UserLocalRepo = new UserLocalRepository(this);
            UserRemoteRepo = new UserRemoteRepository(this);
            MessageRepo = new MessageRepository(this);

            CreateDatabase();
        }

        public void CreateDatabase() {
            string createTableMessage = "CREATE TABLE IF NOT EXISTS message ("
                + "id integer primary key auto_increment, "
                + "senderid integer references user(id), "
                + "conversationid integer references conversation(id), "
                + "text varchar(2046), "
                + "time datetime);";
            string createTableConversation = "CREATE TABLE IF NOT EXISTS conversation ("
                + "id integer primary key auto_increment, "
                + "ownerid integer references user(id), "
                + "active boolean)";
            string createTableUser = "CREATE TABLE IF NOT EXISTS user ("
                + "id integer primary key auto_increment, "
                + "name varchar(32), "
                + "islocal tinyint(1), "
                + "passwordsaltedhash varchar(42), "
                + "ip varchar(40));";
            string createTableUserHasBuddy = "CREATE TABLE IF NOT EXISTS user_has_buddy ("
                + "userid integer references user(id), "
                + "buddyid integer references user(id));";
            string createTableConversationHasUser = "CREATE TABLE IF NOT EXISTS conversation_has_user ("
                + "conversationid integer references conversation(id), "
                + "userid integer references user(id));";

            Database.ExecuteSQLQuery(createTableUser);
            Database.ExecuteSQLQuery(createTableUserHasBuddy);
            Database.ExecuteSQLQuery(createTableConversation);
            Database.ExecuteSQLQuery(createTableConversationHasUser);
            Database.ExecuteSQLQuery(createTableMessage);
        }
        
    }
}
