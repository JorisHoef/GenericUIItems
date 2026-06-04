using System.Collections.Generic;
using UnityEngine;

namespace JorisHoef.GenericUIItems
{
    public interface IGenericUIContainer<T, in TKey>
    {
        RectTransform Parent { get; }
        int Count { get; }

        IReadOnlyList<ISettableItem<T>> GetItems();
        bool TryGetItem(TKey key, out ISettableItem<T> item);
        void SetItems(IEnumerable<T> items);
        void ReplaceAll(IEnumerable<T> items);
        ISettableItem<T> Add(T item);
        bool Update(T item);
        bool Remove(TKey key);
        void Clear();
    }
}
