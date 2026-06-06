using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.GenericUIItems
{
    public sealed class GenericScrollView<T, TKey> :
        IGenericUIContainer<T, TKey>,
        IGenericUISelectionVisuals<TKey, T>
    {
        private readonly GenericUIContainer<T, TKey> _container;

        public GenericScrollView(ScrollRect scrollRect, GameObject itemPrefab, Func<T, TKey> keySelector)
            : this(scrollRect, itemPrefab, keySelector, null)
        {
        }

        public GenericScrollView(
            ScrollRect scrollRect,
            GameObject itemPrefab,
            Func<T, TKey> keySelector,
            IGenericUIItemVisual<TKey, T> itemVisual)
        {
            ScrollRect = scrollRect != null
                ? scrollRect
                : throw new ArgumentNullException(nameof(scrollRect));

            if (ScrollRect.content == null)
            {
                throw new ArgumentException("ScrollRect content must be assigned.", nameof(scrollRect));
            }

            _container = new GenericUIContainer<T, TKey>(
                ScrollRect.content,
                itemPrefab,
                keySelector,
                itemVisual);
        }

        public ScrollRect ScrollRect { get; }
        public RectTransform Parent => _container.Parent;
        public int Count => _container.Count;
        public bool HasItemVisual => _container.HasItemVisual;
        public bool HasSelectedKey => _container.HasSelectedKey;
        public TKey SelectedKey => _container.SelectedKey;
        public bool HasHoveredKey => _container.HasHoveredKey;
        public TKey HoveredKey => _container.HoveredKey;

        public IReadOnlyList<ISettableItem<T>> GetItems()
        {
            return _container.GetItems();
        }

        public bool TryGetItem(TKey key, out ISettableItem<T> item)
        {
            return _container.TryGetItem(key, out item);
        }

        public void SetItemVisual(IGenericUIItemVisual<TKey, T> itemVisual)
        {
            _container.SetItemVisual(itemVisual);
        }

        public void SetSelectedKey(TKey selectedKey)
        {
            _container.SetSelectedKey(selectedKey);
        }

        public void ClearSelectedKey()
        {
            _container.ClearSelectedKey();
        }

        public void SetHoveredKey(TKey hoveredKey)
        {
            _container.SetHoveredKey(hoveredKey);
        }

        public void ClearHoveredKey()
        {
            _container.ClearHoveredKey();
        }

        public void RefreshItemVisuals()
        {
            _container.RefreshItemVisuals();
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
