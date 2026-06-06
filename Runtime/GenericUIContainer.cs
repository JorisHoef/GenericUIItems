using System;
using System.Collections.Generic;
using UnityEngine;

namespace JorisHoef.GenericUIItems
{
    public sealed class GenericUIContainer<T, TKey> :
        IGenericUIContainer<T, TKey>,
        IGenericUISelectionVisuals<TKey, T>
    {
        private readonly GenericItemManager<T, TKey> _itemManager;
        private IGenericUIItemVisual<TKey, T> _itemVisual;
        private bool _hasSelectedKey;
        private TKey _selectedKey;
        private bool _hasHoveredKey;
        private TKey _hoveredKey;

        public GenericUIContainer(RectTransform parent, GameObject itemPrefab, Func<T, TKey> keySelector)
            : this(parent, itemPrefab, keySelector, null)
        {
        }

        public GenericUIContainer(
            RectTransform parent,
            GameObject itemPrefab,
            Func<T, TKey> keySelector,
            IGenericUIItemVisual<TKey, T> itemVisual)
        {
            Parent = parent != null
                ? parent
                : throw new ArgumentNullException(nameof(parent));
            _itemManager = new GenericItemManager<T, TKey>(itemPrefab, keySelector);
            _itemVisual = itemVisual;
        }

        public RectTransform Parent { get; }
        public RectTransform NestingChild { get; private set; }
        public int Count => _itemManager.Count;
        public bool HasItemVisual => _itemVisual != null;
        public bool HasSelectedKey => _hasSelectedKey;
        public TKey SelectedKey => _hasSelectedKey ? _selectedKey : default;
        public bool HasHoveredKey => _hasHoveredKey;
        public TKey HoveredKey => _hasHoveredKey ? _hoveredKey : default;

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

        public void SetItemVisual(IGenericUIItemVisual<TKey, T> itemVisual)
        {
            if (!ReferenceEquals(_itemVisual, itemVisual))
            {
                ApplyNormalToAll(_itemVisual);
            }

            _itemVisual = itemVisual;
            RefreshItemVisuals();
        }

        public void SetSelectedKey(TKey selectedKey)
        {
            if (IsNullKey(selectedKey))
            {
                ClearSelectedKey();
                return;
            }

            if (_hasSelectedKey && EqualityComparer<TKey>.Default.Equals(_selectedKey, selectedKey))
            {
                return;
            }

            bool hadPreviousSelection = _hasSelectedKey;
            TKey previousKey = _selectedKey;

            _selectedKey = selectedKey;
            _hasSelectedKey = true;

            if (hadPreviousSelection)
            {
                ApplyVisualForKey(previousKey);
            }

            ApplyVisualForKey(_selectedKey);
        }

        public void ClearSelectedKey()
        {
            if (!_hasSelectedKey)
            {
                return;
            }

            _selectedKey = default;
            _hasSelectedKey = false;
            RefreshItemVisuals();
        }

        public void SetHoveredKey(TKey hoveredKey)
        {
            if (IsNullKey(hoveredKey))
            {
                ClearHoveredKey();
                return;
            }

            if (_hasHoveredKey && EqualityComparer<TKey>.Default.Equals(_hoveredKey, hoveredKey))
            {
                return;
            }

            bool hadPreviousHover = _hasHoveredKey;
            TKey previousKey = _hoveredKey;

            _hoveredKey = hoveredKey;
            _hasHoveredKey = true;

            if (hadPreviousHover)
            {
                ApplyVisualForKey(previousKey);
            }

            ApplyVisualForKey(_hoveredKey);
        }

        public void ClearHoveredKey()
        {
            if (!_hasHoveredKey)
            {
                return;
            }

            TKey previousKey = _hoveredKey;
            _hoveredKey = default;
            _hasHoveredKey = false;
            ApplyVisualForKey(previousKey);
        }

        public void RefreshItemVisuals()
        {
            if (_itemVisual == null)
            {
                return;
            }

            foreach (GenericItemManager<T, TKey>.ManagedItemSnapshot item in _itemManager.GetItemSnapshots())
            {
                ApplyVisual(item);
            }
        }

        public void SetItems(IEnumerable<T> items)
        {
            ReplaceAll(items);
        }

        public void ReplaceAll(IEnumerable<T> items)
        {
            _itemManager.ReplaceAll(items, GetItemParent());
            RefreshItemVisuals();
        }

        public ISettableItem<T> Add(T item)
        {
            ISettableItem<T> addedItem = _itemManager.Add(item, GetItemParent());
            ApplyVisualForKey(_itemManager.GetKeyForItem(item));
            return addedItem;
        }

        public bool Update(T item)
        {
            TKey key = _itemManager.GetKeyForItem(item);
            bool updated = _itemManager.Update(item);
            if (updated)
            {
                ApplyVisualForKey(key);
            }

            return updated;
        }

        public bool Remove(TKey key)
        {
            return _itemManager.Remove(key);
        }

        public void Clear()
        {
            _itemManager.Clear();
        }

        private void ApplyVisualForKey(TKey key)
        {
            if (_itemVisual == null)
            {
                return;
            }

            GenericItemManager<T, TKey>.ManagedItemSnapshot item;
            if (_itemManager.TryGetItemSnapshot(key, out item))
            {
                ApplyVisual(item);
            }
        }

        private void ApplyVisual(GenericItemManager<T, TKey>.ManagedItemSnapshot item)
        {
            if (_hasSelectedKey && EqualityComparer<TKey>.Default.Equals(item.Key, _selectedKey))
            {
                _itemVisual.ApplySelected(item.Key, item.Data, item.SettableItem);
                return;
            }

            if (_hasHoveredKey && EqualityComparer<TKey>.Default.Equals(item.Key, _hoveredKey))
            {
                _itemVisual.ApplyHovered(item.Key, item.Data, item.SettableItem);
                return;
            }

            _itemVisual.ApplyNormal(item.Key, item.Data, item.SettableItem);
        }

        private void ApplyNormalToAll(IGenericUIItemVisual<TKey, T> itemVisual)
        {
            if (itemVisual == null)
            {
                return;
            }

            foreach (GenericItemManager<T, TKey>.ManagedItemSnapshot item in _itemManager.GetItemSnapshots())
            {
                itemVisual.ApplyNormal(item.Key, item.Data, item.SettableItem);
            }
        }

        private Transform GetItemParent()
        {
            return NestingChild != null ? NestingChild : Parent;
        }

        private static bool IsNullKey(TKey key)
        {
            return (object)key == null;
        }
    }
}
