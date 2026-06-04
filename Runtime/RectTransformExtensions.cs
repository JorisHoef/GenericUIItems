using System;
using UnityEngine;

namespace JorisHoef.GenericUIItems
{
    public static class RectTransformExtensions
    {
        public static GenericUIContainer<T, TKey> CreateGenericUIContainer<T, TKey>(
            this RectTransform parent,
            GameObject itemPrefab,
            Func<T, TKey> keySelector)
        {
            return new GenericUIContainer<T, TKey>(parent, itemPrefab, keySelector);
        }
    }
}
