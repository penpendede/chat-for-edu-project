using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chat.Model
{
    /// <summary>
    /// Repository interface
    /// </summary>
    /// <typeparam name="T">Type for which the repository is created</typeparam>
    public interface IRepository<T>
    {
        //List<T> Get();
        T GetById(int id);
        //void Create();
        bool Contains(T obj);
        void Insert(T obj);
        void Remove(T obj);
        void RemoveById(int id);
        void Update(T obj);
        void InsertOrUpdate(T obj);
    }
}
