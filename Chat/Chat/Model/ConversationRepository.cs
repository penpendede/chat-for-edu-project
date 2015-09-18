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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbController">database controller to be used</param>
        public ConversationRepository(DatabaseController dbController)
        {
            _dbController = dbController;
            _loaded = new List<Conversation>();
        }

        /// <summary>
        /// check if conversation is contained in database
        /// </summary>
        /// <param name="obj">the conversation to seek</param>
        /// <returns>truth value for "conversation is in database"</returns>
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
            if (_loaded.Any(o => o.Id == obj.Id))
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

        /// <summary>
        /// get a single Conversation by ID
        /// </summary>
        /// <param name="id">conversation's ID</param>
        /// <returns>the conversation you've been looking for</returns>
        public Conversation GetById(int id)
        {
            // if already fetched serve from memory
            if (_loaded.Any(c => c.Id == id))
            {
                return _loaded.First(c => c.Id == id);
            }

            List<string[]> result = _dbController.Database.ExecuteSQLQuery("SELECT ownerid, active, closed FROM conversation WHERE id = " + id + ";");

            Conversation conversation = new Conversation()
            {
                Id = id,
                UserLocal = _dbController.UserLocalRepo.GetById(Int32.Parse(result[0][0]))
            };

            if (Boolean.Parse(result[0][2]))
            {
                conversation.Close();
            }

            conversation.SetActive(Boolean.Parse(result[0][1]));
            conversation.UserLocal.AddConversation(conversation);

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

        /// <summary>
        /// Insert conversation into database
        /// </summary>
        /// <param name="obj">Conversation to be inserted into database</param>
        public void Insert(Conversation obj)
        {
            // Add conversation to conversation table
            _dbController.Database.ExecuteSQLQuery("INSERT INTO conversation (active, ownerid, closed) VALUES (" + (obj.Active? 1:0) + ", " + obj.UserLocal.Id + ", " + (obj.Closed? 1: 0) + ");");

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

        /// <summary>
        /// Remove conversation: Obtain conversation's ID, then remove by ID
        /// </summary>
        /// <param name="obj">Conversation to be removed</param>
        public void Remove(Conversation obj)
        {
            if (obj != null)
            {
                RemoveById(obj.Id);
                _loaded.Remove(obj);
            }
        }

        /// <summary>
        /// Remove conversation by ID
        /// </summary>
        /// <param name="id">ID of the conversation to be removed</param>
        public void RemoveById(int id)
        {
            // remove from conversation_has_user
            _dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE conversationid = " + id + ";");
            // remove conversation itself
            _dbController.Database.ExecuteSQLQuery("DELETE FROM conversation WHERE id = " + id + ";");
        }

        /// <summary>
        /// update (as in SQL's UPDATE) a single conversation
        /// </summary>
        /// <param name="obj">the Conversation to be removed</param>
        public void Update(Conversation obj)
        {
            if (obj != null)
            {
                // update activity 
                _dbController.Database.ExecuteSQLQuery("UPDATE conversation SET active = " + (obj.Active? 1: 0) + ", closed = " + (obj.Closed? 1: 0) + " WHERE id = " + obj.Id + ";");

                // rebuild buddy list ()
                _dbController.Database.ExecuteSQLQuery("DELETE FROM conversation_has_user WHERE conversationid = " + obj.Id + ";");
                foreach (UserRemote buddy in obj.Buddies)
                {
                    _dbController.Database.ExecuteSQLQuery("INSERT INTO conversation_has_user (conversationid, userid) VALUES (" + obj.Id + ", " + buddy.Id + ");");
                }
            }
        }

        /// <summary>
        /// insert new conversation or update existing one
        /// </summary>
        /// <param name="obj">the conversation to be either inserted or updated</param>
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

        /// <summary>
        /// Bind the auto-save delegates to be used by the conversation
        /// </summary>
        /// <param name="obj">Conversation to which auto-save delegates are to be added</param>
        private void _bindAutoSaveDelegates(Conversation obj)
        {
            // update conversation when a buddy gets added to it
            obj.BuddyAdd += (conv, bud) => Update(conv);
            //obj.BuddyRemove += (conv, bud) => Update(conv);
            // update a conversations active status
            obj.ChangeActive += (conv, act) => Update(conv);
            // save new messages that gets added to the conversation 
            obj.MessageAdd += (conv, mes) => _dbController.MessageRepo.Insert(mes);
            // update a conversations open status
            obj.OnClose += conv => Update(conv);
        }
    }
}
