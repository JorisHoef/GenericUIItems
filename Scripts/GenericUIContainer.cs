using System.Collections.Generic;
using UnityEngine;

namespace JorisHoef.GenericItems
{
    /// <summary>
    /// Represents a container for generic UI items of type ISettableItem
    /// </summary>
    /// <typeparam name="T">The type of data each item holds.</typeparam>
    public class GenericUIContainer<T> : IGenericUIContainer<T> where T : class
    {
        private readonly GenericItemManager<T> _genericItemManager;

        public RectTransform Parent { get; private set; }

        public RectTransform NestingChild { get; private set; }

        public GenericItemManager<T> GenericItemManager => _genericItemManager;
        
        public GenericUIContainer(RectTransform parent)
        {
            this.Parent = parent;
            this._genericItemManager = new GenericItemManager<T>();
        }
        
        public void SetNestingChild(RectTransform childTransform)
        {
            this.NestingChild = childTransform;
        }

        public List<ISettableItem<T>> GetItems()
        {
            return _genericItemManager.GetItems();
        }
        
        public ISettableItem<T> GetItem(T dataObject)
        {
            return _genericItemManager.GetItem(dataObject);
        }
        
        public void AddItems(List<T> items, GameObject itemPrefab, bool shouldClearFirst = false, bool addToNestingChild = false)
        {
            _genericItemManager.CreateItems(items, itemPrefab, addToNestingChild && NestingChild ? NestingChild : Parent, shouldClearFirst);
        }
        
        public void AddItem(T item, GameObject itemPrefab, bool addToNestingChild = false)
        {
            _genericItemManager.CreateItem(item, itemPrefab, addToNestingChild && NestingChild ? NestingChild : Parent, out _);
        }

        public void AddItem(T item, GameObject itemPrefab, out GameObject createdObject, bool addToNestingChild = false)
        {
            _genericItemManager.CreateItem(item, itemPrefab, addToNestingChild && NestingChild ? NestingChild : Parent, out createdObject);
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