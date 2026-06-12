using System;
using UnityEngine;

namespace Deucarian.UIBinding
{
    public static class RectTransformExtensions
    {
        public static UIBindingContainer<T, TKey> CreateUIBindingContainer<T, TKey>(
            this RectTransform parent,
            GameObject itemPrefab,
            Func<T, TKey> keySelector)
        {
            return new UIBindingContainer<T, TKey>(parent, itemPrefab, keySelector);
        }
    }
}
