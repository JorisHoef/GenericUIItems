using System;
using System.Collections.Generic;
using UnityEngine;

namespace JorisHoef.GenericUIItems
{
    public sealed class GenericUIContainer<T, TKey> : IGenericUIContainer<T, TKey>
    {
        private readonly GenericItemManager<T, TKey> _itemManager;

        public GenericUIContainer(RectTransform parent, GameObject itemPrefab, Func<T, TKey> keySelector)
        {
            Parent = parent != null
                ? parent
                : throw new ArgumentNullException(nameof(parent));
            _itemManager = new GenericItemManager<T, TKey>(itemPrefab, keySelector);
        }

        public RectTransform Parent { get; }
        public RectTransform NestingChild { get; private set; }
        public int Count => _itemManager.Count;
        public GenericItemManager<T, TKey> ItemManager => _itemManager;

        public void SetNestingChild(RectTransform childTransform)
        {
            NestingChild = childTransform;
        }

        public IReadOnlyList<ISettableItem<T>> GetItems()
        {
            return _itemManager.GetItems();
        }

        public bool TryGetItem(TKey key, out ISettableItem<T> item)
        {
            return _itemManager.TryGetItem(key, out item);
        }

        public void SetItems(IEnumerable<T> items)
        {
            _itemManager.SetItems(items, GetItemParent());
        }

        public void ReplaceAll(IEnumerable<T> items)
        {
            _itemManager.ReplaceAll(items, GetItemParent());
        }

        public ISettableItem<T> Add(T item)
        {
            return _itemManager.Add(item, GetItemParent());
        }

        public bool Update(T item)
        {
            return _itemManager.Update(item);
        }

        public bool Remove(TKey key)
        {
            return _itemManager.Remove(key);
        }

        public void Clear()
        {
            _itemManager.Clear();
        }

        private Transform GetItemParent()
        {
            return NestingChild != null ? NestingChild : Parent;
        }
    }
}
