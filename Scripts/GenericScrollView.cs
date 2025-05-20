using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.GenericItems
{
    /// <summary>
    /// Allows ISettables to be added to our ScrollRects and allow creation of their UI items
    /// </summary>
    /// <typeparam name="T">Where T is the type of Model (Data) we want to create a UI item for</typeparam>
    public class GenericScrollView<T> : IGenericUIContainer<T> where T : class
    {
        private readonly GenericItemManager<T> _genericItemManager;
        private readonly ScrollRect _scrollRect;
        
        public RectTransform Parent => _scrollRect.content;
        public GenericItemManager<T> GenericItemManager => _genericItemManager;
        
        public GenericScrollView(ScrollRect scrollRect)
        {
            this._scrollRect = scrollRect;
            this._genericItemManager = new GenericItemManager<T>();
        }
        
        public void AddItems(List<T> items, GameObject itemPrefab, bool shouldClearFirst = false, bool addToNestingChild = false)
        {
            _genericItemManager.CreateItems(items, itemPrefab, Parent, shouldClearFirst);
        }

        public void AddItem(T item, GameObject itemPrefab, bool addToNestingChild = false)
        {
            _genericItemManager.CreateItem(item, itemPrefab, Parent);
        }

        public void UpdateItem(T item)
        {
            _genericItemManager.UpdateItem(item);
        }
        
        public void RemoveItem(T dataObject)
        {
            _genericItemManager.RemoveItem(dataObject);
        }

        public void RemoveItems()
        {
            _genericItemManager.RemoveItems();
        }
    }
}