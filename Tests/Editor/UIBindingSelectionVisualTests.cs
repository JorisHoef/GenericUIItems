using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Deucarian.UIBinding.Tests
{
    public sealed class UIBindingSelectionVisualTests
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
                Object.DestroyImmediate(_parent.gameObject);
            }

            if (_prefab != null)
            {
                Object.DestroyImmediate(_prefab);
            }
        }

        [Test]
        public void SelectedKeyAppliesSelectedVisualToCorrectItem()
        {
            RecordingVisual visual = new RecordingVisual();
            UIBindingContainer<TestData, string> container = CreateContainer(visual);
            SetDefaultItems(container);

            visual.Clear();
            container.SetSelectedKey("two");

            Assert.That(visual.Calls.Select(call => call.State), Is.EqualTo(new[] { "selected" }));
            Assert.That(visual.Calls[0].Key, Is.EqualTo("two"));
        }

        [Test]
        public void PreviousSelectedItemReturnsToNormal()
        {
            RecordingVisual visual = new RecordingVisual();
            UIBindingContainer<TestData, string> container = CreateContainer(visual);
            SetDefaultItems(container);
            container.SetSelectedKey("one");

            visual.Clear();
            container.SetSelectedKey("two");

            Assert.That(visual.Calls.Select(call => call.State), Is.EqualTo(new[] { "normal", "selected" }));
            Assert.That(visual.Calls.Select(call => call.Key), Is.EqualTo(new[] { "one", "two" }));
        }

        [Test]
        public void ClearingSelectedKeyNormalizesItems()
        {
            RecordingVisual visual = new RecordingVisual();
            UIBindingContainer<TestData, string> container = CreateContainer(visual);
            SetDefaultItems(container);
            container.SetSelectedKey("one");

            visual.Clear();
            container.ClearSelectedKey();

            Assert.That(visual.Calls.Select(call => call.State), Is.EqualTo(new[] { "normal", "normal" }));
            Assert.That(visual.Calls.Select(call => call.Key), Is.EqualTo(new[] { "one", "two" }));
        }

        [Test]
        public void SameSelectedKeyIsIdempotent()
        {
            RecordingVisual visual = new RecordingVisual();
            UIBindingContainer<TestData, string> container = CreateContainer(visual);
            SetDefaultItems(container);
            container.SetSelectedKey("one");

            visual.Clear();
            container.SetSelectedKey("one");

            Assert.That(visual.Calls, Is.Empty);
        }

        [Test]
        public void VisualStrategyReceivesCorrectKeyItemAndView()
        {
            RecordingVisual visual = new RecordingVisual();
            UIBindingContainer<TestData, string> container = CreateContainer(visual);
            SetDefaultItems(container);
            container.TryGetItem("one", out ISettableItem<TestData> expectedView);

            visual.Clear();
            container.SetSelectedKey("one");

            VisualCall call = visual.Calls.Single();
            Assert.That(call.Key, Is.EqualTo("one"));
            Assert.That(call.Item.Label, Is.EqualTo("First"));
            Assert.That(call.View, Is.SameAs(expectedView));
        }

        [Test]
        public void RuntimeAssemblyDoesNotReferenceCoreStateOrObjectSelection()
        {
            string[] referencedAssemblyNames = typeof(UIBindingContainer<TestData, string>)
                .Assembly
                .GetReferencedAssemblies()
                .Select(assemblyName => assemblyName.Name)
                .ToArray();

            Assert.That(referencedAssemblyNames.Any(name => name.Contains("Core.State")), Is.False);
            Assert.That(referencedAssemblyNames.Any(name => name.Contains("CoreState")), Is.False);
            Assert.That(referencedAssemblyNames.Any(name => name.Contains("ObjectSelection")), Is.False);
        }

        private UIBindingContainer<TestData, string> CreateContainer(RecordingVisual visual)
        {
            return new UIBindingContainer<TestData, string>(_parent, _prefab, data => data.Id, visual);
        }

        private static void SetDefaultItems(UIBindingContainer<TestData, string> container)
        {
            container.SetItems(new[]
            {
                new TestData("one", "First"),
                new TestData("two", "Second")
            });
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

        private sealed class RecordingVisual : IUIBindingItemVisual<string, TestData>
        {
            public readonly List<VisualCall> Calls = new List<VisualCall>();

            public void ApplyNormal(string key, TestData item, object view)
            {
                Calls.Add(new VisualCall("normal", key, item, view));
            }

            public void ApplySelected(string key, TestData item, object view)
            {
                Calls.Add(new VisualCall("selected", key, item, view));
            }

            public void ApplyHovered(string key, TestData item, object view)
            {
                Calls.Add(new VisualCall("hovered", key, item, view));
            }

            public void Clear()
            {
                Calls.Clear();
            }
        }

        private readonly struct VisualCall
        {
            public VisualCall(string state, string key, TestData item, object view)
            {
                State = state;
                Key = key;
                Item = item;
                View = view;
            }

            public string State { get; }
            public string Key { get; }
            public TestData Item { get; }
            public object View { get; }
        }
    }
}
