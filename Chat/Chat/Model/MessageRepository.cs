//using Chat.Controller;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Chat.Model
//{
//    public class MessageRepository : IBaseRepository<Message>
//    {
//        private List<Message> loaded;
//        private DatabaseController dbController;

//        public MessageRepository(DatabaseController dbController)
//        {
//            this.dbController = dbController;
//            loaded = new List<Message>();
//        }

//        public Message GetById(int id)
//        {
//            if (loaded.Where(m => m.Id == id).Count() > 0)
//            {
//                return loaded.Where(m => m.Id == id).First();
//            }

//            string sqlQuery = string.Format("SELECT id, senderid, text, time FROM message WHERE id={0};", id);

//            /// \todo implement MessageRepository.GetById

//            return new Message();
//        }

//        public void Insert(Message message)
//        {
//            string sqlQuery = string.Format("INSERT INTO message(senderid, text, time) VALUES ({0}, {1}, {2});", message.Sender.Id, message.Text, message.Time.ToString("YYYY-MM-DD HH:MM:SS")); // NOTE: using SQLite Syntax

//            string sqlQuery2 = string.Format("");
//            // TODO: implement MessageRepository.Insert
//        }

//        public void Remove(Message message)
//        {
//            RemoveById(message.Id);
//        }

//        public void RemoveById(int id)
//        {
//            string sqlQuery = string.Format("DELETE FROM message WHERE id={0};", id);
//            /// \todo implement MessageRepository.RemoveById
//        }
//        public void Update(Message message)
//        {
//            string sqlQuery = string.Format("UPDATE message SET senderid={0}, text={1}, time={2} WHERE id={3};", message.Sender.Id, message.Text, message.Time.ToString("YYYY-MM-DD HH:MM:SS"), message.Id);
//            /// \todo implement MessageRepository.Update
//        }
//        public void InsertOrUpdate(Message message)
//        {
//            if (Contains(message))
//            {
//                Update(message);
//            }
//            else
//            {
//                Insert(message);
//            }
//        }

//        public bool Contains(Message message)
//        {
//            if (message.Id == null)
//            {
//                return false;
//            }
//            string sqlString = string.Format("SELECT COUNT(id) FROM message WHERE id={0};", message.Id);
//            return true;
//        }
        
//    }
//}
