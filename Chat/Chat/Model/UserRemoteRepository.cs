using Chat.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class UserRemoteRepository : IRepository<UserRemote>
    {
        private DatabaseController dbController;

        private List<UserRemote> loaded;

        public bool Contains(UserRemote obj)
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

        //public UserRemote GetByName(string userName)
        //{
        //    if (userName != null)
        //    {
        //        List<string[]> result = dbController.Database.ExecuteSQLQuery("SELECT id FROM user WHERE name = '" + userName + "';");
        //        return GetById(Int32.Parse(result[0][0]));
        //    }
        //    return null;
        //}
        
        public UserRemote GetById(int id)
        {
            // if already fetched serve from memory
            if (loaded.Where(c => c.Id == id).Count() > 0)
            {
                return loaded.Where(c => c.Id == id).First();
            }

            List<string[]> resultLocalUser = dbController.Database.ExecuteSQLQuery("SELECT name, ip FROM message WHERE id = " + id + " AND islocal = true;");

            UserRemote userRemote = new UserRemote()
            {
                Id = id,
                Name = resultLocalUser[0][0],
                IP = resultLocalUser[0][1]
            };


            List<string[]> conversationIds = dbController.Database.ExecuteSQLQuery("SELECT conversationid from conversation_has_user WHERE userid = " + id + ";");

            foreach (string[] row in conversationIds)
            {
                userRemote.Conversations.Add(dbController.ConversationRepo.GetById(Int32.Parse(row[0])));
            }

            // store
            loaded.Add(userRemote);

            return userRemote;

        }
        public void Insert(UserRemote obj)
        {
            dbController.Database.ExecuteSQLQuery("INSERT INTO user (id, name, islocal, ip) VALUES " +
    "(" + obj.Id + ", " + obj.Name + ", FALSE, '', '" + obj.IP + "');");

            foreach (Conversation conversation in obj.Conversations)
            {
                dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES (" + conversation.Id + ", " + obj.Id + ");");
            }

            // todo: set id
            // todo: store
        }

        public void Remove(UserRemote obj)
        {
            RemoveById(obj.Id);
        }

        public void RemoveById(int id)
        {
            dbController.Database.ExecuteSQLQuery("DELETE FROM user_has_buddy WHERE buddyid = " + id + ";");
            dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE userid = " + id + ";");
            dbController.Database.ExecuteSQLQuery("DELETE FROM user where id = " + id + ";");
        }

        public void Update(UserRemote obj)
        {
            if (obj != null)
            {
                // update activity 
                dbController.Database.ExecuteSQLQuery("UPDATE user " +
                    "SET name = '" + obj.Name + "', SET ip = '" + obj.IP + "'" + " WHERE id = " + obj.Id + ";");

                dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE userid = " + obj.Id + ";");
                foreach (Conversation conversation in obj.Conversations)
                {
                    dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES " +
                        "(" + conversation.Id + ", " + obj.Id + ");");
                }
            }
        }

        public void InsertOrUpdate(UserRemote obj)
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