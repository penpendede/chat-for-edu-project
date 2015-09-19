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

        public MessageRepository(DatabaseController dbController)
        {
            _dbController = dbController;
            _loaded = new List<Message>();
        }

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

        public void Insert(Message obj)
        {
            _dbController.Database.ExecuteSQLQuery("INSERT INTO message (senderid, conversationid, text, time) VALUES (" +
                obj.Sender.Id + ", " + obj.Conversation.Id + ", '" + _dbController.Database.Escape(obj.Text) + "', '" + _dbController.Database.FormatDateTime(obj.Time) + "');");

            // set id
            obj.Id = Int32.Parse(_dbController.Database.LastInsertedId("message"));

            // store
            _loaded.Add(obj);
        }

        public void Remove(Message obj)
        {
            if (obj != null)
            {
                RemoveById(obj.Id);
                _loaded.Remove(obj);
            }
        }

        public void RemoveById(int id)
        {
            _dbController.Database.ExecuteSQLQuery("DELETE FROM message WHERE id = " + id + ";");
        }

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
