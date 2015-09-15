using Chat.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class UserRemoteRepository : IRepository<UserRemote>
    {
        private DatabaseController _dbController;

        private List<UserRemote> _loaded;

        public UserRemoteRepository(DatabaseController dbController)
        {
            _dbController = dbController;
            _loaded = new List<UserRemote>();
        }

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
            if (id == -1)
            {
                return UserRemote.SystemUser;
            }

            // if already fetched serve from memory
            if (_loaded.Any(c => c.Id == id))
            {
                return _loaded.First(c => c.Id == id);
            }

            List<string[]> result = _dbController.Database.ExecuteSQLQuery("SELECT name, ip, port, deleted FROM user WHERE id = " + id + ";");

            UserRemote userRemote = new UserRemote(bool.Parse(result[0][3]))
            {
                Id = id,
                Name = result[0][0],
                IP = result[0][1],
                Port = Int32.Parse(result[0][2])
            };

            // store reference
            _loaded.Add(userRemote);


            int buddyOfId = Int32.Parse(_dbController.Database.ExecuteSQLQuery("SELECT userid FROM user_has_buddy WHERE buddyid = " + id + ";")[0][0]);
            
            userRemote.BuddyOf = _dbController.UserLocalRepo.GetById(buddyOfId);


            List<string[]> conversationIds = _dbController.Database.ExecuteSQLQuery("SELECT conversationid from conversation_has_user WHERE userid = " + id + ";");

            foreach (string[] row in conversationIds)
            {
                userRemote.AddConversation(_dbController.ConversationRepo.GetById(Int32.Parse(row[0])));
            }
           
            _bindAutoSaveDelegates(userRemote);

            return userRemote;

        }
        public void Insert(UserRemote obj)
        {
            if (obj.Id != -1)
            {
                _dbController.Database.ExecuteSQLQuery("INSERT INTO user (name, islocal, ip, port, deleted) VALUES " + "('" + _dbController.Database.Escape(obj.Name) + "', 0, '" + _dbController.Database.Escape(obj.IP) + "', " + obj.Port.ToString() + ", " + (obj.Deleted ? 1 : 0) + ");");

                // set id
                obj.Id = Int32.Parse(_dbController.Database.LastInsertedId("user"));

                _dbController.Database.ExecuteSQLQuery("INSERT INTO user_has_buddy (userid, buddyid) VALUES " + "(" + obj.BuddyOf.Id + ", " + obj.Id + ");");

                foreach (Conversation conversation in obj.Conversations)
                {
                    _dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES (" + conversation.Id + ", " + obj.Id + ");");
                }

                _bindAutoSaveDelegates(obj);

                // store
                _loaded.Add(obj);
            }
        }

        public void Remove(UserRemote obj)
        {
            if (obj.Id != -1)
            {
                _loaded.Remove(obj);
                RemoveById(obj.Id);
            }
        }

        public void RemoveById(int id)
        {
            if (id >= 0)
            {
                _dbController.Database.ExecuteSQLQuery("DELETE FROM user_has_buddy WHERE buddyid = " + id + ";");
                _dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE userid = " + id + ";");
                _dbController.Database.ExecuteSQLQuery("DELETE FROM user where id = " + id + ";");
            }
        }

        public void Update(UserRemote obj)
        {
            if (obj != null && obj.Id != -1)
            {
                // update activity 
                _dbController.Database.ExecuteSQLQuery("UPDATE user " +
                    "SET name = '" + _dbController.Database.Escape(obj.Name) + "', SET ip = '" + _dbController.Database.Escape(obj.IP) + "', port = " + obj.IP.ToString() + ", deleted = " + (obj.Deleted? 1: 0) + " WHERE id = " + obj.Id + ";");

                _dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE userid = " + obj.Id + ";");
                foreach (Conversation conversation in obj.Conversations)
                {
                    _dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES " +
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

        private void _bindAutoSaveDelegates(UserRemote obj)
        {
            obj.OnDelete += u => Update(obj);
        }
    }
}