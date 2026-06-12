using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Deucarian.UIBinding.Samples.BasicUsage
{
    public sealed class NestedCategoryItem : GenericItem<NestedCategoryData>
    {
        [SerializeField] private Text title;
        [SerializeField] private RectTransform childrenParent;
        [SerializeField] private GameObject childItemPrefab;

        private UIBindingContainer<NestedItemData, string> _children;

        public int ChildCount => _children != null ? _children.Count : 0;

        public void Configure(Text titleText, RectTransform childParent, GameObject childPrefab)
        {
            title = titleText;
            childrenParent = childParent;
            childItemPrefab = childPrefab;
        }

        public override void SetData(NestedCategoryData data)
        {
            base.SetData(data);
            EnsureReferences();

            if (title != null)
            {
                title.text = data != null ? data.Title : string.Empty;
            }

            EnsureChildContainer();
            _children.SetItems(data != null ? data.Items : new List<NestedItemData>());
        }

        public IReadOnlyList<ISettableItem<NestedItemData>> GetChildren()
        {
            EnsureChildContainer();
            return _children.GetItems();
        }

        public void SetChildren(IEnumerable<NestedItemData> items)
        {
            EnsureChildContainer();
            _children.SetItems(items);
        }

        public void ReplaceChildren(IEnumerable<NestedItemData> items)
        {
            EnsureChildContainer();
            _children.ReplaceAll(items);
        }

        public ISettableItem<NestedItemData> AddChild(NestedItemData item)
        {
            EnsureChildContainer();
            return _children.Add(item);
        }

        public bool UpdateChild(NestedItemData item)
        {
            EnsureChildContainer();
            return _children.Update(item);
        }

        public bool RemoveChild(string id)
        {
            EnsureChildContainer();
            return _children.Remove(id);
        }

        public void ClearChildren()
        {
            EnsureChildContainer();
            _children.Clear();
        }

        private void EnsureReferences()
        {
            if (title == null)
            {
                title = GetComponentInChildren<Text>();
            }

            if (childrenParent == null)
            {
                Transform childTransform = transform.Find("Children");
                childrenParent = childTransform != null
                    ? childTransform.GetComponent<RectTransform>()
                    : null;
            }
        }

        private void EnsureChildContainer()
        {
            EnsureReferences();

            if (_children != null)
            {
                return;
            }

            _children = new UIBindingContainer<NestedItemData, string>(
                childrenParent,
                childItemPrefab,
                item => item.Id);
        }
    }
}
