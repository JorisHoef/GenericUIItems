using System.Collections.Generic;
using UnityEngine;

namespace JorisHoef.GenericUIItems.Samples.BasicUsage
{
    public sealed class BasicUsageExample : MonoBehaviour
    {
        [SerializeField] private RectTransform parent;
        [SerializeField] private GameObject itemPrefab;

        private GenericUIContainer<ExampleItemData, string> _container;
        private int _nextId = 3;

        private void Awake()
        {
            _container = new GenericUIContainer<ExampleItemData, string>(
                parent,
                itemPrefab,
                item => item.Id);

            _container.SetItems(new List<ExampleItemData>
            {
                new ExampleItemData("1", "First item"),
                new ExampleItemData("2", "Second item")
            });
        }

        public void Add()
        {
            string id = _nextId.ToString();
            _nextId++;
            _container.Add(new ExampleItemData(id, $"Added item {id}"));
        }

        public void UpdateFirst()
        {
            _container.Update(new ExampleItemData("1", "First item updated"));
        }

        public void RemoveFirst()
        {
            _container.Remove("1");
        }

        public void ReplaceAll()
        {
            _container.ReplaceAll(new[]
            {
                new ExampleItemData("1", "First item replaced"),
                new ExampleItemData("3", "Third item")
            });
        }

        public void Clear()
        {
            _container.Clear();
        }
    }
}
