using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JorisHoef.GenericItems
{
    public class GenericItemManager<T>
    {
        /// <summary>
        /// Save UI items and their Connected dataObjects in a dictionary
        /// </summary>
        private readonly Dictionary<T, ISettableItem<T>> _itemDictionary = new Dictionary<T, ISettableItem<T>>();

        public List<ISettableItem<T>> GetItems()
        {
            return _itemDictionary.Values.ToList();
        }
        
        public ISettableItem<T> GetItem(T dataObject) 
        {
            _itemDictionary.TryGetValue(dataObject, out var item);
            return item;
        }
        
        public void CreateItems(List<T> dataObjects, GameObject itemPrefab, Transform parent, bool shouldClearFirst = false)
        {
            if (shouldClearFirst)
            {
                RemoveItems();
            }
            
            foreach (T dataObject in dataObjects)
            {
                CreateItem(dataObject, itemPrefab, parent);
            }
        }

        public void CreateItem(T dataObject, GameObject itemPrefab, Transform parent)
        {
            CreateItem(dataObject, itemPrefab, parent, out _);
        }

        public void CreateItem(T dataObject, GameObject itemPrefab, Transform parent, out GameObject createdObject)
        {
            GameObject itemGameObject = UnityEngine.Object.Instantiate(itemPrefab, parent);
            ISettableItem<T> settableItem = itemGameObject.GetComponent<ISettableItem<T>>();
            if (settableItem != null)
            {
                settableItem.SetData(dataObject);
                _itemDictionary[dataObject] = settableItem;
            }
            createdObject = itemGameObject;
        }

        public void UpdateItem(T dataObject)
        {
            if (_itemDictionary.TryGetValue(dataObject, out ISettableItem<T> settableItem))
            {
                settableItem.SetData(dataObject);
            }
        }
        
        public void RemoveItem(T dataObject)
        {
            if (_itemDictionary.Remove(dataObject, out ISettableItem<T> settableItem))
            {
                UnityEngine.Object.Destroy(settableItem.GetGameObject());
            }
        }

        public void RemoveItems()
        {
            foreach (var settableItem in _itemDictionary.Values)
            {
                Object.Destroy(settableItem.GetGameObject());
            }

            _itemDictionary.Clear();
        }
    }
}