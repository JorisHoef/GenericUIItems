using System.Collections.Generic;
using UnityEngine;

namespace Deucarian.UIBinding.Samples.BasicUsage
{
    public sealed class BasicUsageExample : MonoBehaviour
    {
        [SerializeField] private BasicUsageSampleLayout layout;
        [SerializeField] private RectTransform parent;
        [SerializeField] private GameObject itemPrefab;

        private UIBindingContainer<ExampleItemData, string> _container;
        private int _nextId = 3;

        private void Awake()
        {
            EnsureSetup();

            _container = new UIBindingContainer<ExampleItemData, string>(
                parent,
                itemPrefab,
                item => item.Id,
                new GraphicTintUIBindingItemVisual<string, ExampleItemData>(
                    new Color(0.12f, 0.16f, 0.20f, 0.9f),
                    new Color(0.16f, 0.42f, 0.95f, 1f),
                    new Color(0.20f, 0.24f, 0.28f, 1f)));

            _container.SetItems(new List<ExampleItemData>
            {
                new ExampleItemData("1", "First item"),
                new ExampleItemData("2", "Second item")
            });
            _container.SetSelectedKey("1");
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

        public void SelectFirst()
        {
            _container.SetSelectedKey("1");
        }

        public void SelectSecond()
        {
            _container.SetSelectedKey("2");
        }

        public void ClearSelectedItem()
        {
            _container.ClearSelectedKey();
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
            _container.ClearSelectedKey();
        }

        private void EnsureSetup()
        {
            if (layout == null)
            {
                layout = BasicUsageSampleLayout.GetOrCreateShared();
            }

            if (parent == null)
            {
                parent = layout.GetFlatItemsParent();
            }

            if (itemPrefab == null)
            {
                itemPrefab = layout.GetFlatItemPrefab(transform);
            }
        }
    }
}
