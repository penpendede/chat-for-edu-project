using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    interface IBaseRepository<T>
    {
        void Create();
        List<T> Get();
        T GetById(int id);
        void Insert(T obj);
        void Remove(T obj);
        void RemoveById(int id);
        void Update(T obj);
        void InsertOrUpdate(T obj);
    }

    interface IConversationRepository : IBaseRepository<Conversation>
    {
        List<Conversation> GetByParticipant(User user);
    }

    interface IMessageRepository : IBaseRepository<Message>
    {
        List<Message> GetBySender(User user);
    }

    interface IUserLocalRepository : IBaseRepository<UserLocal>
    {
        UserLocal GetByName(String userName);
    }

    interface IUserRemoteRepository : IBaseRepository<UserRemote>
    {
        List<UserRemote> GetParticipants(Conversation conversation);
    }
}
