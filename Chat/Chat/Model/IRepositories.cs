using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    interface IBaseRepository<T>
    {
        public void Create();
        public List<T> Get();
        public T GetById(int id);
        public void Insert(T obj);
        public void Remove(T obj);
        public void RemoveById(int id);
        public void Update(T obj);
        public void InsertOrUpdate(T obj);
    }

    interface IConversationRepository : IBaseRepository<Conversation>
    {
        public List<Conversation> GetByParticipant(User user);
    }

    interface IMessageRepository : IBaseRepository<Message>
    {
        public List<Message> GetBySender(User user);
    }

    interface IUserLocalRepository : IBaseRepository<UserLocal>
    {
        public UserLocal GetByName(String userName);
    }

    interface IUserRemoteRepository : IBaseRepository<UserRemote>
    {
        public List<UserRemote> GetParticipants(Conversation conversation);
    }
}
