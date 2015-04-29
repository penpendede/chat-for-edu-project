using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    public class SQLiteUserRemoteRepository : IUserRemoteRepository
    {
        public void Create()
        {
            /// \todo implement SQLiteUserRemoteRepository.Create
        }
        public List<UserRemote> Get()
        {
            /// \todo implement SQLiteUserRemoteRepository.Get
        }
        public UserRemote GetById(int id)
        {
            /// \todo implement SQLiteUserRemoteRepository.GetById
        }
        public void Insert(UserRemote obj)
        {
            /// \todo implement SQLiteUserRemoteRepository.Insert
        }
        public void Remove(UserRemote obj)
        {
            /// \todo implement SQLiteUserRemoteRepository.Remove
        }
        public void RemoveById(int id)
        {
            /// \todo implement SQLiteUserRemoteRepository.RemoveById
        }
        public void Update(UserRemote obj)
        {
            /// \todo implement SQLiteUserRemoteRepository.Update
        }
        public void InsertOrUpdate(UserRemote obj)
        {
            /// \todo implement SQLiteUserRemoteRepository.InsertOrUpdate
        }
        public List<UserRemote> GetParticipants(Conversation conversation)
        {
            /// \todo implement SQLiteUserRemoteRepository.GetParticipants
        }
    }
}
