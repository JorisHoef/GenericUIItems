using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.GenericUIItems.Tests
{
    public sealed class GenericUIContainerTests
    {
        private RectTransform _parent;
        private GameObject _prefab;

        [SetUp]
        public void SetUp()
        {
            _parent = new GameObject("Parent", typeof(RectTransform)).GetComponent<RectTransform>();
            _prefab = new GameObject("ItemPrefab", typeof(RectTransform), typeof(TestItem));
        }

        [TearDown]
        public void TearDown()
        {
            if (_parent != null)
            {
                UnityEngine.Object.DestroyImmediate(_parent.gameObject);
            }

            if (_prefab != null)
            {
                UnityEngine.Object.DestroyImmediate(_prefab);
            }
        }

        [Test]
        public void SetItems_CreatesItems()
        {
            GenericUIContainer<TestData, string> container = CreateContainer();

            container.SetItems(new[]
            {
                new TestData("one", "First"),
                new TestData("two", "Second")
            });

            Assert.That(container.Count, Is.EqualTo(2));
            Assert.That(_parent.childCount, Is.EqualTo(2));
            Assert.That(container.GetItems().Cast<TestItem>().Select(item => item.Data.Label), Is.EqualTo(new[] { "First", "Second" }));
        }

        [Test]
        public void ReplaceAll_UpdatesAddsRemovesAndPreservesExistingItemIdentity()
        {
            GenericUIContainer<TestData, string> container = CreateContainer();
            container.SetItems(new[]
            {
                new TestData("one", "First"),
                new TestData("two", "Second")
            });
            container.TryGetItem("one", out ISettableItem<TestData> originalOne);

            container.ReplaceAll(new[]
            {
                new TestData("one", "First updated"),
                new TestData("three", "Third")
            });

            Assert.That(container.Count, Is.EqualTo(2));
            Assert.That(_parent.childCount, Is.EqualTo(2));
            Assert.That(container.TryGetItem("two", out _), Is.False);
            Assert.That(container.TryGetItem("one", out ISettableItem<TestData> updatedOne), Is.True);
            Assert.That(updatedOne, Is.SameAs(originalOne));
            Assert.That(((TestItem)updatedOne).Data.Label, Is.EqualTo("First updated"));
            Assert.That(container.GetItems().Cast<TestItem>().Select(item => item.Data.Id), Is.EqualTo(new[] { "one", "three" }));
        }

        [Test]
        public void Add_CreatesSingleItem()
        {
            GenericUIContainer<TestData, string> container = CreateContainer();

            ISettableItem<TestData> addedItem = container.Add(new TestData("one", "First"));

            Assert.That(container.Count, Is.EqualTo(1));
            Assert.That(_parent.childCount, Is.EqualTo(1));
            Assert.That(((TestItem)addedItem).Data.Label, Is.EqualTo("First"));
        }

        [Test]
        public void Add_WithDuplicateKeyThrows()
        {
            GenericUIContainer<TestData, string> container = CreateContainer();
            container.Add(new TestData("one", "First"));

            Assert.Throws<ArgumentException>(() => container.Add(new TestData("one", "Duplicate")));
        }

        [Test]
        public void Update_UsesExplicitKeySelector()
        {
            GenericUIContainer<TestData, string> container = CreateContainer();
            container.Add(new TestData("one", "First"));

            bool updated = container.Update(new TestData("one", "First updated"));

            Assert.That(updated, Is.True);
            Assert.That(container.TryGetItem("one", out ISettableItem<TestData> item), Is.True);
            Assert.That(((TestItem)item).Data.Label, Is.EqualTo("First updated"));
            Assert.That(_parent.childCount, Is.EqualTo(1));
        }

        [Test]
        public void Update_WhenKeyDoesNotExistReturnsFalse()
        {
            GenericUIContainer<TestData, string> container = CreateContainer();

            bool updated = container.Update(new TestData("missing", "Missing"));

            Assert.That(updated, Is.False);
            Assert.That(container.Count, Is.EqualTo(0));
        }

        [Test]
        public void Remove_RemovesItemByKey()
        {
            GenericUIContainer<TestData, string> container = CreateContainer();
            container.SetItems(new[]
            {
                new TestData("one", "First"),
                new TestData("two", "Second")
            });

            bool removed = container.Remove("one");

            Assert.That(removed, Is.True);
            Assert.That(container.Count, Is.EqualTo(1));
            Assert.That(_parent.childCount, Is.EqualTo(1));
            Assert.That(container.TryGetItem("one", out _), Is.False);
        }

        [Test]
        public void Clear_RemovesAllItems()
        {
            GenericUIContainer<TestData, string> container = CreateContainer();
            container.SetItems(new[]
            {
                new TestData("one", "First"),
                new TestData("two", "Second")
            });

            container.Clear();

            Assert.That(container.Count, Is.EqualTo(0));
            Assert.That(_parent.childCount, Is.EqualTo(0));
        }

        [Test]
        public void Containers_DoNotShareStaticCaches()
        {
            GenericUIContainer<TestData, string> first = CreateContainer();
            GenericUIContainer<TestData, string> second = CreateContainer();

            first.Add(new TestData("one", "First"));
            second.Add(new TestData("two", "Second"));

            Assert.That(first, Is.Not.SameAs(second));
            Assert.That(first.Count, Is.EqualTo(1));
            Assert.That(second.Count, Is.EqualTo(1));
            Assert.That(first.TryGetItem("two", out _), Is.False);
            Assert.That(second.TryGetItem("one", out _), Is.False);
        }

        [Test]
        public void SetItems_WithDuplicateKeysThrows()
        {
            GenericUIContainer<TestData, string> container = CreateContainer();

            Assert.Throws<ArgumentException>(() => container.SetItems(new[]
            {
                new TestData("one", "First"),
                new TestData("one", "Duplicate")
            }));
        }

        [Test]
        public void Add_WhenKeySelectorReturnsNullThrows()
        {
            GenericUIContainer<TestData, string> container = new GenericUIContainer<TestData, string>(
                _parent,
                _prefab,
                _ => null);

            Assert.Throws<InvalidOperationException>(() => container.Add(new TestData("one", "First")));
        }

        [Test]
        public void GenericScrollView_UsesScrollRectContent()
        {
            GameObject scrollObject = new GameObject("ScrollRect", typeof(RectTransform), typeof(ScrollRect));
            RectTransform content = new GameObject("Content", typeof(RectTransform)).GetComponent<RectTransform>();
            content.SetParent(scrollObject.transform, false);

            try
            {
                ScrollRect scrollRect = scrollObject.GetComponent<ScrollRect>();
                scrollRect.content = content;
                GenericScrollView<TestData, string> scrollView = new GenericScrollView<TestData, string>(
                    scrollRect,
                    _prefab,
                    data => data.Id);

                scrollView.SetItems(new[]
                {
                    new TestData("one", "First"),
                    new TestData("two", "Second")
                });

                Assert.That(scrollView.Count, Is.EqualTo(2));
                Assert.That(content.childCount, Is.EqualTo(2));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(scrollObject);
            }
        }

        private GenericUIContainer<TestData, string> CreateContainer()
        {
            return new GenericUIContainer<TestData, string>(_parent, _prefab, data => data.Id);
        }

        private sealed class TestData
        {
            public TestData(string id, string label)
            {
                Id = id;
                Label = label;
            }

            public string Id { get; }
            public string Label { get; }
        }

        private sealed class TestItem : MonoBehaviour, ISettableItem<TestData>
        {
            public TestData Data { get; private set; }

            public void SetData(TestData data)
            {
                Data = data;
            }
        }
    }
}
