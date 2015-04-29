using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class SQLiteConversationRepository : IConversationRepository
    {
        public void Create()
        {
            /// \todo implement SQLiteConversationRepository.Create
        }
        public List<Conversation> Get()
        {
            /// \todo implement SQLiteConversationRepository.Get
        }
        public Conversation GetById(int id)
        {
            /// \todo implement SQLiteConversationRepository.GetById
        }
        public void Insert(Conversation obj)
        {
            /// \todo implement SQLiteConversationRepository.Insert
        }
        public void Remove(Conversation obj)
        {
            /// \todo implement SQLiteConversationRepository.Remove
        }
        public void RemoveById(int id)
        {
            /// \todo implement SQLiteConversationRepository.RemoveById
        }
        public void Update(Conversation obj)
        {
            /// \todo implement SQLiteConversationRepository.Udate
        }
        public void InsertOrUpdate(Conversation obj)
        {
            /// \todo implement SQLiteConversationRepository.InsertOrUpdate
        }
        public List<Conversation> GetByParticipant(User user)
        {
            /// \todo implement SQLiteConversationRepository.GetByParticipant
        }
    }
}
