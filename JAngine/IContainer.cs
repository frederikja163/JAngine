using System;
using System.Collections.Generic;

namespace JAngine
{
    public interface IContainer<T>
        where T : IDisposable
    {
        protected List<T> Items { get; }

        internal void Add(T item)
        {
            Items.Add(item);
        }

        internal void Remove(T item)
        {
            Items.Remove(item);
        }

        public void DisposeItems()
        {
            foreach (T item in Items)
            {
                item.Dispose();
            }
        }
    }
}