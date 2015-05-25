using Chat.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class SQLiteMessageRepository : IMessageRepository
    {
        private DatabaseController dbController;

        private List<Message> loaded;

        public void Create()
        {
        }
        public bool Contains(Message obj)
        {
            // empty object -> not contained
            if (obj == null)
            {
                return false;
            }
            // ID found -> contained
            if (dbController.Database.ExecuteSQLQuery("SELECT COUNT(Id) FROM message WHERE id = " + obj.Id + ";")[0][0] != "0")
            {
                return true;
            }
            // default: not contained
            return false;
        }

        public List<Message> GetBySender(User user)
        {
            List<Message> messages = new List<Message>();

            if (user != null)
            {
                List<string[]> result = dbController.Database.ExecuteSQLQuery("SELECT id FROM message WHERE senderid = " + user.Id + ";");
                foreach (string[] row in result)
                {
                    messages.Add(GetById(Int32.Parse(row[0])));
                }
            }
            return messages;
        }
        public Message GetById(int id)
        {
            // if already fetched serve from memory
            if (loaded.Where(c => c.Id == id).Count() > 0)
            {
                return loaded.Where(c => c.Id == id).First();
            }

            List<string[]> resultMessage = dbController.Database.ExecuteSQLQuery("SELECT senderid, text, time FROM message WHERE id = " + id + ";");

            int senderId = Int32.Parse(resultMessage[0][0]);

            Message message = new Message()
            {
                Id = id,
                Sender = dbController.UserRemoteRepo.GetById(senderId),
                Text = resultMessage[0][1],
                Time = DateTime.Parse(resultMessage[0][2])
            };

            loaded.Add(message);

            return message;

        }

        public void Insert(Message obj)
        {
            dbController.Database.ExecuteSQLQuery("INSERT INTO message (Id, senderid, conversationid, text, time) VALUES (" +
                obj.Id + ", " + obj.Sender.Id /* ISSUE: CONVERSATIONID */ + ", " + obj.Text + ", " + obj.Time + ");");
        }
        public void Remove(Message obj)
        {
            if (obj != null)
            {
                RemoveById(obj.Id);
            }
        }
        public void RemoveById(int id)
        {
            dbController.Database.ExecuteSQLQuery("DELETE FROM message WHERE id = " + id + ";");
        }
        public void Update(Message obj)
        {
            if (obj != null)
            {
                // update activity 
                dbController.Database.ExecuteSQLQuery("UPDATE message"+
                    " SET senderid = " + obj.Sender.Id +
                    // "SET conversationid = " +  // ISSUE: CONVERSATIONID
                    " SET text = '" + obj.Text +
                    "', SET time = '" +  obj.Time + 
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
