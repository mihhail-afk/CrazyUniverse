using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Models;

namespace Core.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseAnimal
    {
        private readonly List<T> _items = new();

        public event Action<T, T>? OnItemAdded;
        public event Action<T, T>? OnItemRemoved;
        public event Action<T, T>? OnCrazyActionTriggered;

        public void Add(T item)
        {
            var existingItems = _items.ToList();
            _items.Add(item);

            foreach (var neighbor in existingItems)
            {
                OnItemAdded?.Invoke(item, neighbor);
            }
        }

        public void Remove(T item)
        {
            if (_items.Remove(item))
            {
                var remainingItems = _items.ToList();
                foreach (var neighbor in remainingItems)
                {
                    OnItemRemoved?.Invoke(item, neighbor);
                }
            }
        }

        public IEnumerable<T> GetAll() => new List<T>(_items);

        public T? Find(Func<T, bool> predicate) => _items.FirstOrDefault(predicate);

        public void PerformCrazyAction(T item)
        {
            if (!_items.Contains(item)) return;

            item.CrazyAction();

            var neighbors = _items.Where(x => x != item).ToList();
            foreach (var neighbor in neighbors)
            {
                OnCrazyActionTriggered?.Invoke(item, neighbor);
            }
        }
    }
}