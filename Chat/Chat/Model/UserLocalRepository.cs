using Chat.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class UserLocalRepository : IRepository<UserLocal>
    {
        private DatabaseController dbController;

        private List<UserLocal> loaded;

        public void Create()
        {
        }
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
            if (loaded.Where(o => o.Id == obj.Id).Count() > 0)
            {
                return true;
            }
            // ID found -> contained
            if (dbController.Database.ExecuteSQLQuery("SELECT COUNT(id) FROM user WHERE id = " + obj.Id + ";")[0][0] != "0")
            {
                return true;
            }
            // default: not contained
            return false;
        }
        public UserLocal GetByName(String userName)
        {
            if (userName != null)
            {
                List<string[]> result = dbController.Database.ExecuteSQLQuery("SELECT id FROM user WHERE name = '" + userName + "';");
                return GetById(Int32.Parse(result[0][0]));
            }
            return null;
        }
        public UserLocal GetById(int id)
        {
            // if already fetched serve from memory
            if (loaded.Where(c => c.Id == id).Count() > 0)
            {
                return loaded.Where(c => c.Id == id).First();
            }

            List<string[]> resultLocalUser = dbController.Database.ExecuteSQLQuery("SELECT name, passwordsaltedhash FROM message WHERE id = " + id + " AND islocal = true;");

            UserLocal userLocal = new UserLocal()
            {
                Id = id,
                Name = resultLocalUser[0][0] //,
                //PasswordSaltedHash = resultLocalUser[0][2] cannot be set like this
            };

            List<string[]> buddyIds = dbController.Database.ExecuteSQLQuery("SELECT buddyid FROM user_has_buddies WHERE userid = " + id + ";");

            foreach (string[] row in buddyIds)
            {
                userLocal.AddBuddy(dbController.UserRemoteRepo.GetById(Int32.Parse(row[0])));
            }

            List<string[]> conversationIds = dbController.Database.ExecuteSQLQuery("SELECT conversationid from conversation_has_user WHERE userid = " + id + ";");

            foreach (string[] row in conversationIds) {
                userLocal.AddConversation(dbController.ConversationRepo.GetById(Int32.Parse(row[0])));
            }

            // store future changes

            userLocal.BuddyAdd += (user, bud) => dbController.UserRemoteRepo.Insert(bud);
            userLocal.BuddyRemove += (user, bud) => dbController.UserRemoteRepo.Remove(bud);
            userLocal.ConversationAdd += (user, conv) => dbController.ConversationRepo.Insert(conv);

            // store
            loaded.Add(userLocal);

            return userLocal;
        }
        public void Insert(UserLocal obj)
        {
            dbController.Database.ExecuteSQLQuery("INSERT INTO user (name, islocal, passwordsaltedhash) VALUES " +
                "(" + obj.Name + ", TRUE, '');");

            // todo: set id
            // todo: store

            // add all conversationId/userID pairs to conversation_has_user
            foreach (UserRemote buddy in obj.Buddies)
            {
                dbController.Database.ExecuteSQLQuery("INSERT INTO user_has_buddy (userid, buddyid) VALUES (" + obj.Id + ", " + buddy.Id + ");");
            }

            foreach (Conversation conversation in obj.Conversations)
            {
                dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES (" + conversation.Id + ", " + obj.Id + ");");
            }
        }
        public void Remove(UserLocal obj)
        {
            RemoveById(obj.Id);
        }
        public void RemoveById(int id)
        {
            dbController.Database.ExecuteSQLQuery("DELETE FROM user_has_buddy WHERE userid = " + id + ";");
            //dbController.Database.ExecuteSQLQuery("DELETE FROM user_has_buddy WHERE buddyid = " + id + ";"); // no - buddies are always userRemote
            dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE userid = " + id + ";");
            dbController.Database.ExecuteSQLQuery("DELETE FROM user where id = " + id + ";");
        }
        public void Update(UserLocal obj)
        {
            if (obj != null) // find ich nicht so gut, das ist eher Fehlerunterdrückung, als wirklich hilfreich, oder?
            {
                // update activity 
                dbController.Database.ExecuteSQLQuery("UPDATE user " + 
                    "SET name = '" + obj.Name + "', SET passwordsaltedhash = '' WHERE id = " + obj.Id + ";");

                // rebuild buddy list ()
                dbController.Database.ExecuteSQLQuery("DELETE FROM user_has_buddy WHERE userid = " + obj.Id + ";");
                foreach (UserRemote buddy in obj.Buddies)
                {
                    dbController.Database.ExecuteSQLQuery("INSERT INTO user_has_budy (userid, buddyid) VALUES (" + obj.Id + ", " + buddy.Id + ");");
                }

                dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE userid = " + obj.Id + ";");
                foreach (Conversation conversation in obj.Conversations)
                {
                    dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES " +
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

    }
}
