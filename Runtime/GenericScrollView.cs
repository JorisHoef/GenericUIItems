using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.GenericUIItems
{
    public sealed class GenericScrollView<T, TKey> : IGenericUIContainer<T, TKey>
    {
        private readonly GenericUIContainer<T, TKey> _container;

        public GenericScrollView(ScrollRect scrollRect, GameObject itemPrefab, Func<T, TKey> keySelector)
        {
            ScrollRect = scrollRect != null
                ? scrollRect
                : throw new ArgumentNullException(nameof(scrollRect));

            if (ScrollRect.content == null)
            {
                throw new ArgumentException("ScrollRect content must be assigned.", nameof(scrollRect));
            }

            _container = new GenericUIContainer<T, TKey>(ScrollRect.content, itemPrefab, keySelector);
        }

        public ScrollRect ScrollRect { get; }
        public RectTransform Parent => _container.Parent;
        public int Count => _container.Count;

        public IReadOnlyList<ISettableItem<T>> GetItems()
        {
            return _container.GetItems();
        }

        public bool TryGetItem(TKey key, out ISettableItem<T> item)
        {
            return _container.TryGetItem(key, out item);
        }

        public void SetItems(IEnumerable<T> items)
        {
            _container.SetItems(items);
        }

        public void ReplaceAll(IEnumerable<T> items)
        {
            _container.ReplaceAll(items);
        }

        public ISettableItem<T> Add(T item)
        {
            return _container.Add(item);
        }

        public bool Update(T item)
        {
            return _container.Update(item);
        }

        public bool Remove(TKey key)
        {
            return _container.Remove(key);
        }

        public void Clear()
        {
            _container.Clear();
        }
    }
}
