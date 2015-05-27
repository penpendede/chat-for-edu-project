using Chat.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class ConversationRepository : IRepository<Conversation>
    {
        private DatabaseController _dbController;

        private List<Conversation> _loaded;

        public ConversationRepository(DatabaseController dbController)
        {
            _dbController = dbController;
            _loaded = new List<Conversation>();
        }

        // check if conversation is contained in database
        public bool Contains(Conversation obj)
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
            if (_loaded.Where(o => o.Id == obj.Id).Count() > 0)
            {
                return true;
            }
            // ID found -> contained
            if (_dbController.Database.ExecuteSQLQuery("SELECT COUNT(id) FROM conversation WHERE id = " + obj.Id + ";")[0][0] != "0")
            {
                return true;
            }
            // default: not contained
            return false;
        }

        //List<Conversation> GetByParticipant(User user)
        //{
        //    List<Conversation> conversations = new List<Conversation>();

        //    if (user != null)
        //    {
        //        List<string[]> result = dbController.Database.ExecuteSQLQuery("SELECT conversationid FROM conversation_has_user WHERE userid = " + user.Id + ";");
        //        foreach (string[] row in result)
        //        {
        //            conversations.Add(GetById(Int32.Parse(row[0])));
        //        }
        //    }
        //    return conversations;
        //}

        // get a single Conversation by ID
        public Conversation GetById(int id)
        {
            // if already fetched serve from memory
            if (_loaded.Where(c => c.Id == id).Count() > 0)
            {
                return _loaded.Where(c => c.Id == id).First();
            }

            List<string[]> result = _dbController.Database.ExecuteSQLQuery("SELECT active, ownerid FROM conversation WHERE id = " + id + ";");

            Conversation conversation = new Conversation()
            {
                Id = id,
                Owner = _dbController.UserLocalRepo.GetById(Int32.Parse(result[0][1]))
            };

            conversation.SetActive(Boolean.Parse(result[0][0]));
            conversation.Owner.AddConversation(conversation);

            // store reference
            _loaded.Add(conversation);

            List<string[]> resultBuddies = _dbController.Database.ExecuteSQLQuery("SELECT userid FROM conversation_has_user WHERE conversationid = " + id + ";");

            foreach (string[] row in resultBuddies)
            {
                conversation.AddBuddy(_dbController.UserRemoteRepo.GetById(Int32.Parse(row[0])));
            }

            List<string[]> resultMessages = _dbController.Database.ExecuteSQLQuery("SELECT id FROM message WHERE conversationid = " + id + ";");

            foreach (string[] row in resultMessages)
            {
                conversation.AddMessage(_dbController.MessageRepo.GetById(Int32.Parse(row[0])));
            }

            // store future changes
            _bindAutoSaveDelegates(conversation);

            return conversation;
        }

        // Insert conversation into database
        public void Insert(Conversation obj)
        {
            // Add conversation to conversation table
            _dbController.Database.ExecuteSQLQuery("INSERT INTO conversation (active, ownerid) VALUES (" + (obj.Active? 1:0) + ", " + obj.Owner.Id + ");");

            // get id
            obj.Id = Int32.Parse(_dbController.Database.LastInsertedId("conversation"));

            // add all conversationId/userID pairs to conversation_has_user
            foreach (UserRemote buddy in obj.Buddies)
            {
                _dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES (" + obj.Id + ", " + buddy.Id + ");");
            }

            //_dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES (" + obj.Id + ", " + obj.Owner.Id + ");");

            // store
            _loaded.Add(obj);

            _bindAutoSaveDelegates(obj);
        }

        // Remove conversation: Obtain conversation's ID, then remove by ID
        public void Remove(Conversation obj)
        {
            if (obj != null)
            {
                RemoveById(obj.Id);
                _loaded.Remove(obj);
            }
        }

        // Remove conversation by ID
        public void RemoveById(int id)
        {
            // remove from conversation_has_user
            _dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE conversationid = " + id + ";");
            // remove conversation itself
            _dbController.Database.ExecuteSQLQuery("DELETE FROM conversation WHERE id = " + id + ";");
        }

        // update a single conversation
        public void Update(Conversation obj)
        {
            if (obj != null)
            {
                // update activity 
                _dbController.Database.ExecuteSQLQuery("UPDATE conversation SET active = " + (obj.Active? 1: 0) + " WHERE id = " + obj.Id + ";");

                // rebuild buddy list ()
                _dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE conversationid = " + obj.Id + ";");
                foreach (UserRemote buddy in obj.Buddies)
                {
                    _dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES (" + obj.Id + ", " + buddy.Id + ");");
                }
            }
        }

        // insert new conversation or update existing one
        public void InsertOrUpdate(Conversation obj)
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

        private void _bindAutoSaveDelegates(Conversation obj)
        {
            obj.BuddyAdd += (conv, bud) => Update(conv);
            obj.BuddyRemove += (conv, bud) => Update(conv);
            obj.ChangeActive += (conv, act) => Update(conv);
            obj.MessageAdd += (conv, mes) => _dbController.MessageRepo.Insert(mes);
        }
    }
}
