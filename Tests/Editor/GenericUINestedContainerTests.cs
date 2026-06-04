using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace JorisHoef.GenericUIItems.Tests
{
    public sealed class GenericUINestedContainerTests
    {
        private RectTransform _parent;
        private GameObject _categoryPrefab;
        private GameObject _childPrefab;

        [SetUp]
        public void SetUp()
        {
            _parent = new GameObject("Parent", typeof(RectTransform)).GetComponent<RectTransform>();
            _childPrefab = new GameObject("ChildPrefab", typeof(RectTransform), typeof(TestChildItem));
            _categoryPrefab = new GameObject("CategoryPrefab", typeof(RectTransform), typeof(TestCategoryItem));
            _categoryPrefab.GetComponent<TestCategoryItem>().Configure(_childPrefab);
        }

        [TearDown]
        public void TearDown()
        {
            if (_parent != null)
            {
                UnityEngine.Object.DestroyImmediate(_parent.gameObject);
            }

            if (_categoryPrefab != null)
            {
                UnityEngine.Object.DestroyImmediate(_categoryPrefab);
            }

            if (_childPrefab != null)
            {
                UnityEngine.Object.DestroyImmediate(_childPrefab);
            }
        }

        [Test]
        public void SetItems_CreatesNestedChildrenOwnedByParentItems()
        {
            GenericUIContainer<CategoryData, string> container = CreateContainer();

            container.SetItems(new[]
            {
                new CategoryData("weapons", "Weapons", Children(("sword", "Sword"), ("bow", "Bow"))),
                new CategoryData("armor", "Armor", Children(("shield", "Shield")))
            });

            TestCategoryItem weapons = GetCategory(container, "weapons");
            TestCategoryItem armor = GetCategory(container, "armor");

            Assert.That(container.Count, Is.EqualTo(2));
            Assert.That(_parent.childCount, Is.EqualTo(2));
            Assert.That(weapons.ChildCount, Is.EqualTo(2));
            Assert.That(armor.ChildCount, Is.EqualTo(1));
            Assert.That(weapons.ChildLabels(), Is.EqualTo(new[] { "Sword", "Bow" }));
            Assert.That(armor.ChildrenParent.parent, Is.SameAs(armor.transform));
        }

        [Test]
        public void ReplaceAll_UpdatesExistingParentAndSynchronizesChildren()
        {
            GenericUIContainer<CategoryData, string> container = CreateContainer();
            container.SetItems(new[]
            {
                new CategoryData("weapons", "Weapons", Children(("sword", "Sword"), ("bow", "Bow")))
            });
            TestCategoryItem originalWeapons = GetCategory(container, "weapons");

            container.ReplaceAll(new[]
            {
                new CategoryData("weapons", "Weapons updated", Children(("bow", "Bow updated"), ("staff", "Staff")))
            });

            TestCategoryItem updatedWeapons = GetCategory(container, "weapons");
            Assert.That(updatedWeapons, Is.SameAs(originalWeapons));
            Assert.That(updatedWeapons.Data.Title, Is.EqualTo("Weapons updated"));
            Assert.That(updatedWeapons.ChildIds(), Is.EqualTo(new[] { "bow", "staff" }));
            Assert.That(updatedWeapons.ChildLabels(), Is.EqualTo(new[] { "Bow updated", "Staff" }));
        }

        [Test]
        public void ChildOperations_AreHandledByOwningParentItem()
        {
            GenericUIContainer<CategoryData, string> container = CreateContainer();
            container.SetItems(new[]
            {
                new CategoryData("weapons", "Weapons", Children(("sword", "Sword")))
            });
            TestCategoryItem weapons = GetCategory(container, "weapons");

            weapons.AddChild(new ChildData("bow", "Bow"));
            bool updated = weapons.UpdateChild(new ChildData("sword", "Sword updated"));
            bool removed = weapons.RemoveChild("bow");

            Assert.That(updated, Is.True);
            Assert.That(removed, Is.True);
            Assert.That(weapons.ChildCount, Is.EqualTo(1));
            Assert.That(weapons.ChildLabels(), Is.EqualTo(new[] { "Sword updated" }));
        }

        [Test]
        public void DuplicateChildKeys_AreScopedToEachParentItem()
        {
            GenericUIContainer<CategoryData, string> container = CreateContainer();
            container.SetItems(new[]
            {
                new CategoryData("weapons", "Weapons", Children(("shared", "Weapon shared"))),
                new CategoryData("armor", "Armor", Children(("shared", "Armor shared")))
            });

            TestCategoryItem weapons = GetCategory(container, "weapons");
            TestCategoryItem armor = GetCategory(container, "armor");

            weapons.UpdateChild(new ChildData("shared", "Weapon updated"));

            Assert.That(weapons.ChildLabels(), Is.EqualTo(new[] { "Weapon updated" }));
            Assert.That(armor.ChildLabels(), Is.EqualTo(new[] { "Armor shared" }));
        }

        [Test]
        public void RemoveParent_DestroysNestedChildHierarchy()
        {
            GenericUIContainer<CategoryData, string> container = CreateContainer();
            container.SetItems(new[]
            {
                new CategoryData("weapons", "Weapons", Children(("sword", "Sword")))
            });
            TestCategoryItem weapons = GetCategory(container, "weapons");
            RectTransform childParent = weapons.ChildrenParent;
            GameObject childObject = weapons.GetChildItems().First().gameObject;

            bool removed = container.Remove("weapons");

            Assert.That(removed, Is.True);
            Assert.That(container.Count, Is.EqualTo(0));
            Assert.That(_parent.childCount, Is.EqualTo(0));
            Assert.That(childParent == null, Is.True);
            Assert.That(childObject == null, Is.True);
        }

        [Test]
        public void ClearParent_DestroysNestedChildHierarchy()
        {
            GenericUIContainer<CategoryData, string> container = CreateContainer();
            container.SetItems(new[]
            {
                new CategoryData("weapons", "Weapons", Children(("sword", "Sword"))),
                new CategoryData("armor", "Armor", Children(("shield", "Shield")))
            });
            TestCategoryItem weapons = GetCategory(container, "weapons");
            GameObject childObject = weapons.GetChildItems().First().gameObject;

            container.Clear();

            Assert.That(container.Count, Is.EqualTo(0));
            Assert.That(_parent.childCount, Is.EqualTo(0));
            Assert.That(childObject == null, Is.True);
        }

        private GenericUIContainer<CategoryData, string> CreateContainer()
        {
            return new GenericUIContainer<CategoryData, string>(_parent, _categoryPrefab, category => category.Id);
        }

        private static TestCategoryItem GetCategory(GenericUIContainer<CategoryData, string> container, string id)
        {
            Assert.That(container.TryGetItem(id, out ISettableItem<CategoryData> item), Is.True);
            return (TestCategoryItem)item;
        }

        private static List<ChildData> Children(params (string Id, string Label)[] items)
        {
            return items.Select(item => new ChildData(item.Id, item.Label)).ToList();
        }

        private sealed class CategoryData
        {
            public CategoryData(string id, string title, IEnumerable<ChildData> children)
            {
                Id = id;
                Title = title;
                Children = children.ToList();
            }

            public string Id { get; }
            public string Title { get; }
            public List<ChildData> Children { get; }
        }

        private sealed class ChildData
        {
            public ChildData(string id, string label)
            {
                Id = id;
                Label = label;
            }

            public string Id { get; }
            public string Label { get; }
        }

        private sealed class TestCategoryItem : GenericItem<CategoryData>
        {
            private RectTransform _childrenParent;
            [SerializeField] private GameObject _childPrefab;
            private GenericUIContainer<ChildData, string> _children;

            public RectTransform ChildrenParent => _childrenParent;
            public int ChildCount => _children != null ? _children.Count : 0;

            public void Configure(GameObject childPrefab)
            {
                _childPrefab = childPrefab;
            }

            public override void SetData(CategoryData data)
            {
                base.SetData(data);
                EnsureChildContainer();
                _children.SetItems(data.Children);
            }

            public IReadOnlyList<TestChildItem> GetChildItems()
            {
                EnsureChildContainer();
                return _children.GetItems().Cast<TestChildItem>().ToList();
            }

            public IEnumerable<string> ChildIds()
            {
                return GetChildItems().Select(item => item.Data.Id);
            }

            public IEnumerable<string> ChildLabels()
            {
                return GetChildItems().Select(item => item.Data.Label);
            }

            public ISettableItem<ChildData> AddChild(ChildData item)
            {
                EnsureChildContainer();
                return _children.Add(item);
            }

            public bool UpdateChild(ChildData item)
            {
                EnsureChildContainer();
                return _children.Update(item);
            }

            public bool RemoveChild(string id)
            {
                EnsureChildContainer();
                return _children.Remove(id);
            }

            private void EnsureChildContainer()
            {
                if (_childrenParent == null)
                {
                    _childrenParent = new GameObject("Children", typeof(RectTransform)).GetComponent<RectTransform>();
                    _childrenParent.SetParent(transform, false);
                }

                if (_children == null)
                {
                    _children = new GenericUIContainer<ChildData, string>(
                        _childrenParent,
                        _childPrefab,
                        child => child.Id);
                }
            }
        }

        private sealed class TestChildItem : GenericItem<ChildData>
        {
        }
    }
}
