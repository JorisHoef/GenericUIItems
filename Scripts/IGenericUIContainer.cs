using System.Collections.Generic;
using UnityEngine;

namespace JorisHoef.GenericItems
{
    /// <summary>
    /// Represents a generic UI container that can hold multiple items of type T.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <remarks>Implement for RectTransforms that require container behaviour filled with UI element based on data type T</remarks>
    public interface IGenericUIContainer<T> where T : class
    {
        public RectTransform Parent { get; }
        public GenericItemManager<T> GenericItemManager { get; }
        
        void AddItems(List<T> items, GameObject itemPrefab, bool shouldClearFirst = false, bool addToNestingChild = false);
        void AddItem(T item, GameObject itemPrefab, bool addToNestingChild = false);
        void UpdateItem(T item);
        void RemoveItem(T dataObject);
        void RemoveItems();
    }
}