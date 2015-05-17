using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class SQLiteMessageRepository : IMessageRepository
    {
        public void Create()
        {
            /// \todo implement SQLiteMessageRepository.Create
        }
        public List<Message> Get()
        {
            /// \todo implement SQLiteMessageRepository.Get
        }
        public Message GetById(int id)
        {
            /// \todo implement SQLiteMessageRepository.GetById
        }
        public void Insert(Message obj)
        {
            /// \todo implement SQLiteMessageRepository.Insert
        }
        public void Remove(Message obj)
        {
            /// \todo implement SQLiteMessageRepository.Remove
        }
        public void RemoveById(int id)
        {
            /// \todo implement SQLiteMessageRepository.RemoveById
        }
        public void Update(Message obj)
        {
            /// \todo implement SQLiteMessageRepository.Update
        }
        public void InsertOrUpdate(Message obj)
        {
            /// \todo implement SQLiteMessageRepository.InsertOrUpdate
        }
        public List<Message> GetBySender(User user)
        {
            /// \todo implement SQLiteMessageRepository.GetBySender
        }
    }
}
