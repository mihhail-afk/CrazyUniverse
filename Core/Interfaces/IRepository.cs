using System;
using System.Collections.Generic;
using Core.Models;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : BaseAnimal
    {
        void Add(T item);
        void Remove(T item);
        IEnumerable<T> GetAll();
        T? Find(Func<T, bool> predicate);
    }
}