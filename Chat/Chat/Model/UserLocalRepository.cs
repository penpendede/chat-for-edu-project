using Chat.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class UserLocalRepository : IRepository<UserLocal>
    {
        private DatabaseController _dbController;

        private List<UserLocal> _loaded;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbController">database controller to be used</param>
        public UserLocalRepository(DatabaseController dbController)
        {
            _dbController = dbController;
            _loaded = new List<UserLocal>();
        }

        /// <summary>
        /// Check if user name is already in use
        /// </summary>
        /// <param name="userName">user name to seek</param>
        /// <returns></returns>
        public bool IsUserNameTaken(string userName)
        {
            return GetAllUserNames().Contains(userName);
        }

        /// <summary>
        /// Verify password
        /// </summary>
        /// <param name="userName">user's name</param>
        /// <param name="password">user's password</param>
        /// <returns>TODO</returns>
        public bool VerifyPassword(string userName, string password)
        {
            // TODO: Funktioniert gerade irgendwie nicht
            return
                Int32.Parse(
                    _dbController.Database.ExecuteSQLQuery(
                        string.Format("SELECT COUNT(*) FROM user WHERE name='{0}' AND passwordsaltedhash='{1}';",
                            userName, password))[0][0]) < 1;
        }

        /// <summary>
        /// Set new password
        /// </summary>
        /// <param name="userName">user's name</param>
        /// <param name="newPassword">user's new password</param>
        public void SetNewPassword(string userName, string newPassword)
        {
            _dbController.Database.ExecuteSQLQuery(string.Format("UPDATE user SET passwordsaltedhash='{0}' WHERE name='{1}';", newPassword, _dbController.Database.Escape(userName)));
        }

        /// <summary>
        /// Check if repository contains object
        /// </summary>
        /// <param name="obj">object to seek (a local user)</param>
        /// <returns></returns>
        public bool Contains(UserLocal obj)
        {
            // empty object -> not contained
            if (obj == null)
            {
                return false;
            }
            // Id not set -> not contained
            if (obj.Id == null)
            {
                return false;
            }
            // ID in loaded obj -> contained
            if (_loaded.Any(o => o.Id == obj.Id))
            {
                return true;
            }
            // ID found -> contained
            if (_dbController.Database.ExecuteSQLQuery("SELECT COUNT(id) FROM user WHERE id = " + obj.Id + ";")[0][0] != "0")
            {
                return true;
            }
            // default: not contained
            return false;
        }

        /// <summary>
        /// obtain a list of all existing user names
        /// </summary>
        /// <returns>list of user names as strings</returns>
        public List<string> GetAllUserNames()
        {
            return _dbController.Database.ExecuteSQLQuery("SELECT name FROM user WHERE islocal = 1;").Select(s => s[0]).ToList();
        }

        /// <summary>
        /// find local user by his/her name
        /// </summary>
        /// <param name="userName">the user name</param>
        /// <returns>the user with the given name</returns>
        public UserLocal GetByName(string userName)
        {
            if (userName != null)
            {
                List<string[]> result = _dbController.Database.ExecuteSQLQuery("SELECT id FROM user WHERE name='" + userName + "';");
                return GetById(Int32.Parse(result[0][0]));
            }
            return null;
        }

        /// <summary>
        /// Is user with the given id a local user?
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>truth value for "user with given id is local user"</returns>
        public bool IsLocalById(int id)
        {
            if (id >= 0)
            {
                return Int32.Parse(_dbController.Database.ExecuteSQLQuery("SELECT islocal FROM user WHERE id=" + id + ";")[0][0]) > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// get local user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserLocal GetById(int id)
        {
            // if already fetched serve from memory
            if (_loaded.Any(c => c.Id == id))
            {
                return _loaded.First(c => c.Id == id);
            }

            List<string[]> resultLocalUser = _dbController.Database.ExecuteSQLQuery("SELECT name, port FROM user WHERE id = " + id + ";");

            UserLocal userLocal = new UserLocal()
            {
                Id = id,
                Name = resultLocalUser[0][0],
                Port = Int32.Parse(resultLocalUser[0][1]) //,
                //PasswordSaltedHash = resultLocalUser[0][2] cannot be set like this
            };

            // store reference
            _loaded.Add(userLocal);

            List<string[]> buddyIds = _dbController.Database.ExecuteSQLQuery("SELECT buddyid FROM user_has_buddy WHERE userid = " + id + ";");

            foreach (string[] row in buddyIds)
            {
                userLocal.AddBuddy(_dbController.UserRemoteRepo.GetById(Int32.Parse(row[0])));
            }

            List<string[]> conversationIds = _dbController.Database.ExecuteSQLQuery("SELECT conversationid from conversation_has_user WHERE userid = " + id + ";");

            foreach (string[] row in conversationIds) {
                userLocal.AddConversation(_dbController.ConversationRepo.GetById(Int32.Parse(row[0])));
            }

            // store future changes
            _bindAutoSaveDelegates(userLocal);
            
            return userLocal;
        }

        public void Insert(UserLocal obj)
        {
            if (GetAllUserNames().Contains(obj.Name))
            {
                throw new Exception("Username already exists.");
            }

            _dbController.Database.ExecuteSQLQuery("INSERT INTO user (name, port, islocal, passwordsaltedhash) VALUES " +
                "('" + _dbController.Database.Escape(obj.Name) + "', " + obj.Port.ToString() + ", 1, '');");

            // set id
            obj.Id = Int32.Parse(_dbController.Database.LastInsertedId("user"));

            // add all conversationId/userID pairs to conversation_has_user
            foreach (UserRemote buddy in obj.Buddies)
            {
                _dbController.Database.ExecuteSQLQuery("INSERT INTO user_has_buddy (userid, buddyid) VALUES (" + obj.Id + ", " + buddy.Id + ");");
            }

            foreach (Conversation conversation in obj.Conversations)
            {
                _dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES (" + conversation.Id + ", " + obj.Id + ");");
            }

            // store
            _loaded.Add(obj);

            _bindAutoSaveDelegates(obj);
        }

        public void Remove(UserLocal obj)
        {
            RemoveById(obj.Id);
            _loaded.Remove(obj);
        }

        public void RemoveById(int id)
        {
            _dbController.Database.ExecuteSQLQuery("DELETE FROM user_has_buddy WHERE userid = " + id + ";");
            //dbController.Database.ExecuteSQLQuery("DELETE FROM user_has_buddy WHERE buddyid = " + id + ";"); // no - buddies are always userRemote
            _dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE userid = " + id + ";");
            _dbController.Database.ExecuteSQLQuery("DELETE FROM user where id = " + id + ";");
        }

        public void Update(UserLocal obj)
        {
            if (obj != null)
            {
                // update activity 
                _dbController.Database.ExecuteSQLQuery("UPDATE user " +
                    "SET name = '" + _dbController.Database.Escape(obj.Name) + "', port = " + obj.Port.ToString() + ", SET passwordsaltedhash = '' WHERE id = " + obj.Id + ";");

                // rebuild buddy list ()
                _dbController.Database.ExecuteSQLQuery("DELETE FROM user_has_buddy WHERE userid = " + obj.Id + ";");
                foreach (UserRemote buddy in obj.Buddies)
                {
                    _dbController.Database.ExecuteSQLQuery("INSERT INTO user_has_budy (userid, buddyid) VALUES (" + obj.Id + ", " + buddy.Id + ");");
                }

                _dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE userid = " + obj.Id + ";");
                foreach (Conversation conversation in obj.Conversations)
                {
                    _dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES " +
                        "(" + conversation.Id + ", " + obj.Id + ");");
                }
            }
        }
        public void InsertOrUpdate(UserLocal obj)
        {
            if (Contains(obj))
            {
                Update(obj);
            }
            else
            {
                Insert(obj);
            }
        }

        private void _bindAutoSaveDelegates(UserLocal obj)
        {
            obj.BuddyAdd += (user, bud) => _dbController.UserRemoteRepo.Insert(bud);
            obj.BuddyRemove += (user, bud) => _dbController.UserRemoteRepo.Remove(bud);
            obj.ConversationAdd += (user, conv) => _dbController.ConversationRepo.Insert(conv);
        }
    }
}