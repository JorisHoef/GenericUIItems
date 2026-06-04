using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            EnsureSetup();

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

        private void EnsureSetup()
        {
            if (parent == null)
            {
                parent = CreateDefaultParent();
            }

            if (itemPrefab == null)
            {
                itemPrefab = CreateDefaultItemPrefab();
            }
        }

        private static RectTransform CreateDefaultParent()
        {
            GameObject canvasObject = new GameObject(
                "Generic UI Items Sample Canvas",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);

            GameObject contentObject = new GameObject(
                "Items",
                typeof(RectTransform),
                typeof(VerticalLayoutGroup),
                typeof(ContentSizeFitter));
            RectTransform content = contentObject.GetComponent<RectTransform>();
            content.SetParent(canvasObject.transform, false);
            content.anchorMin = new Vector2(0.5f, 0.5f);
            content.anchorMax = new Vector2(0.5f, 0.5f);
            content.pivot = new Vector2(0.5f, 0.5f);
            content.sizeDelta = new Vector2(420f, 240f);

            VerticalLayoutGroup layout = contentObject.GetComponent<VerticalLayoutGroup>();
            layout.spacing = 8f;
            layout.childControlHeight = true;
            layout.childControlWidth = true;

            ContentSizeFitter fitter = contentObject.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            return content;
        }

        private GameObject CreateDefaultItemPrefab()
        {
            GameObject root = new GameObject(
                "BasicUsageItemTemplate",
                typeof(RectTransform),
                typeof(Image),
                typeof(LayoutElement),
                typeof(BasicUsageItem));
            root.transform.SetParent(transform, false);

            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.sizeDelta = new Vector2(420f, 36f);

            Image background = root.GetComponent<Image>();
            background.color = new Color(0.12f, 0.16f, 0.20f, 0.9f);

            LayoutElement layoutElement = root.GetComponent<LayoutElement>();
            layoutElement.preferredHeight = 36f;

            GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(root.transform, false);

            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(12f, 4f);
            labelRect.offsetMax = new Vector2(-12f, -4f);

            Text labelText = labelObject.GetComponent<Text>();
            labelText.alignment = TextAnchor.MiddleLeft;
            labelText.color = Color.white;
            labelText.fontSize = 16;
            labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            return root;
        }
    }
}
