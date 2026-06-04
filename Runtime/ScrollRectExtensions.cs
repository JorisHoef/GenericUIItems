using System;
using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.GenericUIItems
{
    public static class ScrollRectExtensions
    {
        public static GenericScrollView<T, TKey> CreateGenericScrollView<T, TKey>(
            this ScrollRect scrollRect,
            GameObject itemPrefab,
            Func<T, TKey> keySelector)
        {
            return new GenericScrollView<T, TKey>(scrollRect, itemPrefab, keySelector);
        }
    }
}
