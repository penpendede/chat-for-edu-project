using Chat.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class MessageRepository : IRepository<Message>
    {
        private DatabaseController _dbController;

        private List<Message> _loaded;

<<<<<<< Updated upstream
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbController">database controller to be used</param>
=======
>>>>>>> Stashed changes
        public MessageRepository(DatabaseController dbController)
        {
            _dbController = dbController;
            _loaded = new List<Message>();
        }

<<<<<<< Updated upstream
        /// <summary>
        /// Check if message is contained in database
        /// </summary>
        /// <param name="obj">message</param>
        /// <returns>truth value of "message is contained in database"</returns>
=======
>>>>>>> Stashed changes
        public bool Contains(Message obj)
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
            if (_dbController.Database.ExecuteSQLQuery("SELECT COUNT(Id) FROM message WHERE id = " + obj.Id + ";")[0][0] != "0")
            {
                return true;
            }
            // default: not contained
            return false;
        }

        //public List<Message> GetBySender(User user)
        //{
        //    List<Message> messages = new List<Message>();

        //    if (user != null)
        //    {
        //        List<string[]> result = dbController.Database.ExecuteSQLQuery("SELECT id FROM message WHERE senderid = " + user.Id + ";");
        //        foreach (string[] row in result)
        //        {
        //            messages.Add(GetById(Int32.Parse(row[0])));
        //        }
        //    }
        //    return messages;
        //}

<<<<<<< Updated upstream
        /// <summary>
        /// Get message given its ID
        /// </summary>
        /// <param name="id">The ID to seek</param>
        /// <returns>message that has been seeked</returns>
=======
>>>>>>> Stashed changes
        public Message GetById(int id)
        {
            // if already fetched serve from memory
            if (_loaded.Any(c => c.Id == id))
            {
                return _loaded.First(c => c.Id == id);
            }

            List<string[]> resultMessage = _dbController.Database.ExecuteSQLQuery("SELECT senderid, conversationid, text, time FROM message WHERE id = " + id + ";");

            int senderId = Int32.Parse(resultMessage[0][0]);
            int conversationId = Int32.Parse(resultMessage[0][1]);

            Message message = new Message()
            {
                Id = id,
                Text = resultMessage[0][2],
                Time = DateTime.Parse(resultMessage[0][3])
            };

            // store reference
            _loaded.Add(message);

            if (_dbController.UserLocalRepo.IsLocalById(senderId))
            {
                message.Sender = _dbController.UserLocalRepo.GetById(senderId);
            }
            else
            {
                message.Sender = _dbController.UserRemoteRepo.GetById(senderId);
            }

            message.Conversation = _dbController.ConversationRepo.GetById(conversationId);

            

            return message;
        }

<<<<<<< Updated upstream
        /// <summary>
        /// Insert a message
        /// </summary>
        /// <param name="obj">message to be inserted into database</param>
=======
>>>>>>> Stashed changes
        public void Insert(Message obj)
        {
            _dbController.Database.ExecuteSQLQuery("INSERT INTO message (senderid, conversationid, text, time) VALUES (" +
                obj.Sender.Id + ", " + obj.Conversation.Id + ", '" + _dbController.Database.Escape(obj.Text) + "', '" + _dbController.Database.FormatDateTime(obj.Time) + "');");

            // set id
            obj.Id = Int32.Parse(_dbController.Database.LastInsertedId("message"));

            // store
            _loaded.Add(obj);
        }

<<<<<<< Updated upstream
        /// <summary>
        /// Remove a message
        /// </summary>
        /// <param name="obj">message to be removed</param>
=======
>>>>>>> Stashed changes
        public void Remove(Message obj)
        {
            if (obj != null)
            {
                RemoveById(obj.Id);
                _loaded.Remove(obj);
            }
        }

<<<<<<< Updated upstream
        /// <summary>
        /// Remove message of given ID
        /// </summary>
        /// <param name="id">ID of the message to be removed</param>
=======
>>>>>>> Stashed changes
        public void RemoveById(int id)
        {
            _dbController.Database.ExecuteSQLQuery("DELETE FROM message WHERE id = " + id + ";");
        }

<<<<<<< Updated upstream
        /// <summary>
        /// Update message
        /// </summary>
        /// <param name="obj">the message to be updated</param>
=======
>>>>>>> Stashed changes
        public void Update(Message obj)
        {
            if (obj != null)
            {
                // update activity 
                _dbController.Database.ExecuteSQLQuery("UPDATE message"+
                    " SET senderid = " + obj.Sender.Id +
                    "SET conversationid = " +  obj.Conversation.Id +
                    " SET text = '" + _dbController.Database.Escape(obj.Text) +
                    "', SET time = '" +  _dbController.Database.FormatDateTime(obj.Time) + 
                    "' WHERE conversationid = " + obj.Id + ";");
            }
        }

<<<<<<< Updated upstream
        /// <summary>
        /// Insert or update message
        /// </summary>
        /// <param name="obj">message to be added or, if it already exists, to be updated</param>
=======
>>>>>>> Stashed changes
        public void InsertOrUpdate(Message obj)
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
