using Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Controller
{
    public enum StatusVerifyPassword
    {
        OK,
        USER_NAME_NOT_FOUND,
        WRONG_PASSWORD
    }

    public enum StatusChangePassword {
        OK,
        USER_NAME_NOT_FOUND,
        WRONG_PASSWORD,
        PASSWORDS_NOT_MATCHING
    }

    public enum StatusNewUser
    {
        OK,
        USER_NAME_ALREADY_EXISTS
    }

    public class DatabaseController
    {
        // Model
        public Database Database;
        
        // Repositories
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

            CreateTablesIfNotExistant();
        }


        public void CreateTablesIfNotExistant() {
            string createTableMessage = "CREATE TABLE IF NOT EXISTS message ("
                + "id integer primary key auto_increment, "
                + "senderid integer references user(id), "
                + "conversationid integer references conversation(id), "
                + "text varchar(2046), "
                + "time datetime);";
            string createTableConversation = "CREATE TABLE IF NOT EXISTS conversation ("
                + "id integer primary key auto_increment, "
                + "ownerid integer references user(id), "
                + "active boolean, " 
                + "closed boolean)";
            string createTableUser = "CREATE TABLE IF NOT EXISTS user ("
                + "id integer primary key auto_increment, "
                + "name varchar(32), "
                + "islocal tinyint(1), "
                + "passwordsaltedhash varchar(42), "
                + "ip varchar(40), " 
                + "port integer, "
                + "deleted boolean);";
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

        
        
        public StatusVerifyPassword GetUserLocal(string userName, string password, out UserLocal out_userLocal)
        {
            if (!UserLocalRepo.IsUserNameTaken(userName))
            {
                out_userLocal = null;
                return StatusVerifyPassword.USER_NAME_NOT_FOUND;
            }

            if (UserLocalRepo.VerifyPassword(userName, password))
            {
                out_userLocal = null;
                return StatusVerifyPassword.WRONG_PASSWORD;
            }

            out_userLocal = UserLocalRepo.GetByName(userName);
            return StatusVerifyPassword.OK;
        }

        public StatusNewUser CreateNewUser(string userName, string password, out UserLocal out_userLocal)
        {
            if (UserLocalRepo.IsUserNameTaken(userName))
            {
                out_userLocal = null;
                return StatusNewUser.USER_NAME_ALREADY_EXISTS;
            }
                
            UserLocal newUser = new UserLocal() { Name = userName };
            UserLocalRepo.Insert(newUser);
            UserLocalRepo.SetNewPassword(userName, password);
            out_userLocal = newUser;
            return StatusNewUser.OK;
        }

        public StatusChangePassword ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (!UserLocalRepo.IsUserNameTaken(userName))
            {
                return StatusChangePassword.USER_NAME_NOT_FOUND;
            }

            if (!UserLocalRepo.VerifyPassword(userName, oldPassword))
            {
                return StatusChangePassword.WRONG_PASSWORD;
            }

            if (oldPassword != newPassword)
            {
                return StatusChangePassword.PASSWORDS_NOT_MATCHING;
            }

            UserLocalRepo.SetNewPassword(userName, newPassword);
            return StatusChangePassword.OK;
        }


    }
}
