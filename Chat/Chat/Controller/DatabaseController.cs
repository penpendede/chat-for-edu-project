using Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Controller
{
    public class UserNameNotFoundException : Exception
    {
        public string UserName;

        public UserNameNotFoundException(string userName) 
            :base(string.Format("Username {0} not found!", userName)) 
        {
            UserName = userName;
        }
    }

    public class WrongPasswordException : Exception
    {
        public string UserName;

        public WrongPasswordException(string userName)
            : base(string.Format("Wrong password for username {0}!", userName)) 
        {
            UserName = userName;
        }
    }

    public class NewUserNameAlreadyExists : Exception
    {
        public string UserName;

        public NewUserNameAlreadyExists(string userName)
            : base(string.Format("The username {0} already exists!", userName))
        {
            UserName = userName;
        }
    }

    public class DatabaseController
    {

        public void CreateDatabase() {
            string createTableMessage = "CREATE TABLE message ("
                + "id integer unsigned primary_key, "
                + "senderid integer unsigned foreign_key(User), "
                + "conversationid integer unsigned foreign_key(conversation), "
                + "text varchar(2046), "
                + "time datetime);";
            string createTableConversation = "CREATE TABLE conversation ("
                + "id integer unsigned primary_key, "
                + "active boolean)";
            string createTableUser = "CREATE TABLE user ("
                + "id integer unsigned primary_key, "
                + "name varchar(32), "
                + "islocal boolean, "
                + "passwordsaltedhash varchar(42), "
                + "ip varchar(40));";
            string createTableUserHasBuddy = "CREATE TABLE user_has_buddy ("
                + "userid integer unsigned foreign_key(user), "
                + "buddyid integer unsigned foreign_key(user));";
            string createTableConversationHasUser = "CREATE TABLE conversation_has_user ("
                + "conversationid integer unsigned foreign_key(conversation), "
                + "userid integer unsigned foreign_key(user));";

            //this.db.ExecuteSQLQuery(createTableUser);
            //this.db.ExecuteSQLQuery(createTableUserHasBuddy);
            //this.db.ExecuteSQLQuery(createTableConversation);
            //this.db.ExecuteSQLQuery(createTableConversationHasUser);
            //this.db.ExecuteSQLQuery(createTableMessage);
        }

        public List<string> GetLocalUserNames()
        {
            #warning GetLocalUserNames is not implemented yet
            return new List<string>() { "UserA", "UserB" };
        }

        public UserLocal LoadModelFor(string userName, string password)
        {
            #warning LoadModelFor is using dummydata
            if (userName == "UserA")
            {
                throw new UserNameNotFoundException(userName);
            }
            if (userName == "UserB")
            {
                throw new WrongPasswordException(userName);
            }
            return DummyData.CreateUserLocalTest1();
        }

        public UserLocal CreateNewUser(string userName, string password)
        {
            if (GetLocalUserNames().Contains(userName))
            {
                throw new NewUserNameAlreadyExists(userName);
            }
            #warning New user is not written to database
           
            UserLocal newUser = new UserLocal() { Name = userName };
            
            return newUser;
        }
    }

}
