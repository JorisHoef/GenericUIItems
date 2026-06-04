using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JorisHoef.GenericUIItems
{
    internal sealed class GenericItemManager<T, TKey>
    {
        private readonly Dictionary<TKey, ManagedItem> _itemsByKey = new Dictionary<TKey, ManagedItem>();
        private readonly GameObject _itemPrefab;
        private readonly Func<T, TKey> _keySelector;

        internal GenericItemManager(GameObject itemPrefab, Func<T, TKey> keySelector)
        {
            _itemPrefab = itemPrefab != null
                ? itemPrefab
                : throw new ArgumentNullException(nameof(itemPrefab));
            _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        }

        public int Count => _itemsByKey.Count;

        public IReadOnlyList<ISettableItem<T>> GetItems()
        {
            return _itemsByKey.Values
                .OrderBy(item => item.GameObject.transform.GetSiblingIndex())
                .Select(item => item.SettableItem)
                .ToList();
        }

        public bool TryGetItem(TKey key, out ISettableItem<T> item)
        {
            if (IsNullKey(key))
            {
                item = default;
                return false;
            }

            if (_itemsByKey.TryGetValue(key, out ManagedItem managedItem))
            {
                item = managedItem.SettableItem;
                return true;
            }

            item = default;
            return false;
        }

        public void SetItems(IEnumerable<T> items, Transform parent)
        {
            ReplaceAll(items, parent);
        }

        public void ReplaceAll(IEnumerable<T> items, Transform parent)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            List<IncomingItem> incomingItems = MaterializeIncomingItems(items);
            HashSet<TKey> incomingKeys = new HashSet<TKey>(incomingItems.Select(item => item.Key));
            List<TKey> keysToRemove = _itemsByKey.Keys
                .Where(key => !incomingKeys.Contains(key))
                .ToList();

            foreach (TKey key in keysToRemove)
            {
                Remove(key);
            }

            for (int index = 0; index < incomingItems.Count; index++)
            {
                IncomingItem incomingItem = incomingItems[index];

                if (_itemsByKey.TryGetValue(incomingItem.Key, out ManagedItem existingItem))
                {
                    existingItem.Data = incomingItem.Data;
                    existingItem.SettableItem.SetData(incomingItem.Data);
                    existingItem.GameObject.transform.SetParent(parent, false);
                    existingItem.GameObject.transform.SetSiblingIndex(index);
                    continue;
                }

                ManagedItem createdItem = CreateManagedItem(incomingItem.Data, parent);
                _itemsByKey.Add(incomingItem.Key, createdItem);
                createdItem.GameObject.transform.SetSiblingIndex(index);
            }
        }

        public ISettableItem<T> Add(T item, Transform parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            TKey key = GetKey(item);
            if (_itemsByKey.ContainsKey(key))
            {
                throw new ArgumentException("An item with the same key already exists.", nameof(item));
            }

            ManagedItem managedItem = CreateManagedItem(item, parent);
            _itemsByKey.Add(key, managedItem);
            return managedItem.SettableItem;
        }

        public bool Update(T item)
        {
            TKey key = GetKey(item);
            if (!_itemsByKey.TryGetValue(key, out ManagedItem managedItem))
            {
                return false;
            }

            managedItem.Data = item;
            managedItem.SettableItem.SetData(item);
            return true;
        }

        public bool Remove(TKey key)
        {
            if (IsNullKey(key) || !_itemsByKey.Remove(key, out ManagedItem managedItem))
            {
                return false;
            }

            DestroyItem(managedItem.GameObject);
            return true;
        }

        public void Clear()
        {
            foreach (ManagedItem managedItem in _itemsByKey.Values)
            {
                DestroyItem(managedItem.GameObject);
            }

            _itemsByKey.Clear();
        }

        private List<IncomingItem> MaterializeIncomingItems(IEnumerable<T> items)
        {
            List<IncomingItem> incomingItems = new List<IncomingItem>();
            HashSet<TKey> keys = new HashSet<TKey>();

            foreach (T item in items)
            {
                TKey key = GetKey(item);
                if (!keys.Add(key))
                {
                    throw new ArgumentException("The item collection contains duplicate keys.", nameof(items));
                }

                incomingItems.Add(new IncomingItem(item, key));
            }

            return incomingItems;
        }

        private ManagedItem CreateManagedItem(T data, Transform parent)
        {
            GameObject itemGameObject = Object.Instantiate(_itemPrefab, parent);
            ISettableItem<T> settableItem = itemGameObject
                .GetComponents<Component>()
                .OfType<ISettableItem<T>>()
                .FirstOrDefault();

            if (settableItem == null)
            {
                DestroyItem(itemGameObject);
                throw new InvalidOperationException(
                    $"Prefab '{_itemPrefab.name}' must have a component implementing ISettableItem<{typeof(T).Name}> on its root GameObject.");
            }

            settableItem.SetData(data);
            return new ManagedItem(data, settableItem, itemGameObject);
        }

        private TKey GetKey(T item)
        {
            if ((object)item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            TKey key = _keySelector(item);
            if (IsNullKey(key))
            {
                throw new InvalidOperationException("The key selector returned null.");
            }

            return key;
        }

        private static bool IsNullKey(TKey key)
        {
            return (object)key == null;
        }

        private static void DestroyItem(GameObject itemGameObject)
        {
            if (itemGameObject == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Object.Destroy(itemGameObject);
                return;
            }

            Object.DestroyImmediate(itemGameObject);
        }

        private sealed class ManagedItem
        {
            public ManagedItem(T data, ISettableItem<T> settableItem, GameObject gameObject)
            {
                Data = data;
                SettableItem = settableItem;
                GameObject = gameObject;
            }

            public T Data { get; set; }
            public ISettableItem<T> SettableItem { get; }
            public GameObject GameObject { get; }
        }

        private readonly struct IncomingItem
        {
            public IncomingItem(T data, TKey key)
            {
                Data = data;
                Key = key;
            }

            public T Data { get; }
            public TKey Key { get; }
        }
    }
}
