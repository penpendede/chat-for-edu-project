using Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Controller
{
    /// <summary>
    /// Possible outcomes for password verification: okay, wrong password, user name not found
    /// </summary>
    public enum StatusVerifyPassword
    {
        OK,
        USER_NAME_NOT_FOUND,
        WRONG_PASSWORD
    }

    /// <summary>
    /// Possible outcomes of password change: okay, wrong (old) password, (new) password and its retyped version do not match,
    /// user name not found
    /// </summary>
    public enum StatusChangePassword {
        OK,
        USER_NAME_NOT_FOUND,
        WRONG_PASSWORD,
        PASSWORDS_NOT_MATCHING
    }

    /// <summary>
    /// Possible outcomes of user creation: okay, user name already present
    /// </summary>
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

        /// <summary>
        /// If necessary construct tables using CREATE TABLE
        /// </summary>
        public void CreateTablesIfNotExistant() {
            // messages
            string createTableMessage = "CREATE TABLE IF NOT EXISTS message ("
                + "id integer primary key auto_increment, "
                + "senderid integer references user(id), "
                + "conversationid integer references conversation(id), "
                + "text varchar(2046), "
                + "time datetime);";
            // conversations
            string createTableConversation = "CREATE TABLE IF NOT EXISTS conversation ("
                + "id integer primary key auto_increment, "
                + "ownerid integer references user(id), "
                + "active boolean, " 
                + "closed boolean)";
            // users
            string createTableUser = "CREATE TABLE IF NOT EXISTS user ("
                + "id integer primary key auto_increment, "
                + "name varchar(32), "
                + "islocal tinyint(1), "
                + "passwordsaltedhash varchar(42), "
                + "ip varchar(40), " 
                + "port integer, "
                + "deleted boolean);";
            // "user has buddy" relations
            string createTableUserHasBuddy = "CREATE TABLE IF NOT EXISTS user_has_buddy ("
                + "userid integer references user(id), "
                + "buddyid integer references user(id));";
            // "conversation has user" relations
            string createTableConversationHasUser = "CREATE TABLE IF NOT EXISTS conversation_has_user ("
                + "conversationid integer references conversation(id), "
                + "userid integer references user(id));";

            Database.ExecuteSQLQuery(createTableUser);
            Database.ExecuteSQLQuery(createTableUserHasBuddy);
            Database.ExecuteSQLQuery(createTableConversation);
            Database.ExecuteSQLQuery(createTableConversationHasUser);
            Database.ExecuteSQLQuery(createTableMessage);
        }

        /// <summary>
        /// Verify user's password
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">user's password</param>
        /// <param name="out_userLocal">out: local user for the given user name</param>
        /// <returns>verification result</returns>
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

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userName">the new user's name</param>
        /// <param name="password">the new user's password</param>
        /// <param name="out_userLocal">out: local user for the given user name</param>
        /// <returns></returns>
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

        /// <summary>
        /// Change password of an existing user
        /// </summary>
        /// <param name="userName">user's name</param>
        /// <param name="oldPassword">old password</param>
        /// <param name="newPassword">new password</param>
        /// <returns></returns>
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
