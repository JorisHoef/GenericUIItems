using UnityEngine;
using UnityEngine.UI;

namespace Deucarian.UIBinding.Samples.BasicUsage
{
    public sealed class BasicUsageSampleLayout : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform flatItemsParent;
        [SerializeField] private RectTransform nestedCategoriesParent;
        [SerializeField] private GameObject flatItemPrefab;
        [SerializeField] private GameObject nestedCategoryPrefab;
        [SerializeField] private GameObject nestedChildItemPrefab;

        private static BasicUsageSampleLayout _sharedLayout;

        public static BasicUsageSampleLayout GetOrCreateShared()
        {
            if (_sharedLayout != null)
            {
                return _sharedLayout;
            }

#if UNITY_2023_1_OR_NEWER
            _sharedLayout = FindFirstObjectByType<BasicUsageSampleLayout>();
#else
            _sharedLayout = FindObjectOfType<BasicUsageSampleLayout>();
#endif
            if (_sharedLayout != null)
            {
                return _sharedLayout;
            }

            GameObject layoutObject = new GameObject("Basic Usage Sample Layout");
            _sharedLayout = layoutObject.AddComponent<BasicUsageSampleLayout>();
            return _sharedLayout;
        }

        public RectTransform GetFlatItemsParent()
        {
            if (flatItemsParent == null)
            {
                flatItemsParent = CreateSection("Flat Items", new Vector2(-260f, 0f));
            }

            return flatItemsParent;
        }

        public RectTransform GetNestedCategoriesParent()
        {
            if (nestedCategoriesParent == null)
            {
                nestedCategoriesParent = CreateSection("Nested Categories", new Vector2(260f, 0f));
            }

            return nestedCategoriesParent;
        }

        public GameObject GetFlatItemPrefab(Transform templateParent)
        {
            if (flatItemPrefab == null)
            {
                flatItemPrefab = CreateFlatItemPrefab(GetTemplateParent(templateParent));
            }

            return flatItemPrefab;
        }

        public GameObject GetNestedCategoryPrefab(Transform templateParent)
        {
            if (nestedCategoryPrefab == null)
            {
                nestedCategoryPrefab = CreateNestedCategoryPrefab(GetTemplateParent(templateParent));
            }

            return nestedCategoryPrefab;
        }

        public GameObject GetNestedChildItemPrefab(Transform templateParent)
        {
            if (nestedChildItemPrefab == null)
            {
                nestedChildItemPrefab = CreateNestedChildItemPrefab(GetTemplateParent(templateParent));
            }

            return nestedChildItemPrefab;
        }

        private Transform GetTemplateParent(Transform templateParent)
        {
            return templateParent != null ? templateParent : transform;
        }

        private RectTransform CreateSection(string title, Vector2 anchoredPosition)
        {
            EnsureCanvas();

            GameObject sectionObject = new GameObject(
                title,
                typeof(RectTransform),
                typeof(Image),
                typeof(VerticalLayoutGroup));
            sectionObject.transform.SetParent(canvas.transform, false);

            RectTransform section = sectionObject.GetComponent<RectTransform>();
            section.anchorMin = new Vector2(0.5f, 0.5f);
            section.anchorMax = new Vector2(0.5f, 0.5f);
            section.pivot = new Vector2(0.5f, 0.5f);
            section.anchoredPosition = anchoredPosition;
            section.sizeDelta = new Vector2(440f, 360f);

            Image image = sectionObject.GetComponent<Image>();
            image.color = new Color(0.07f, 0.09f, 0.12f, 0.82f);

            VerticalLayoutGroup sectionLayout = sectionObject.GetComponent<VerticalLayoutGroup>();
            sectionLayout.padding = new RectOffset(12, 12, 12, 12);
            sectionLayout.spacing = 10f;
            sectionLayout.childAlignment = TextAnchor.UpperCenter;
            sectionLayout.childControlHeight = true;
            sectionLayout.childControlWidth = true;

            Text titleText = CreateText("Title", sectionObject.transform, title, 18, FontStyle.Bold);
            titleText.alignment = TextAnchor.MiddleLeft;
            titleText.color = new Color(0.9f, 0.94f, 1f, 1f);
            titleText.gameObject.AddComponent<LayoutElement>().preferredHeight = 28f;

            GameObject contentObject = new GameObject(
                "Content",
                typeof(RectTransform),
                typeof(VerticalLayoutGroup),
                typeof(ContentSizeFitter));
            contentObject.transform.SetParent(sectionObject.transform, false);

            RectTransform content = contentObject.GetComponent<RectTransform>();
            content.sizeDelta = new Vector2(416f, 300f);

            VerticalLayoutGroup contentLayout = contentObject.GetComponent<VerticalLayoutGroup>();
            contentLayout.spacing = 8f;
            contentLayout.childControlHeight = true;
            contentLayout.childControlWidth = true;

            ContentSizeFitter fitter = contentObject.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            return content;
        }

        private void EnsureCanvas()
        {
            if (canvas != null)
            {
                return;
            }

            GameObject canvasObject = new GameObject(
                "UI Binding Sample Canvas",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);
        }

        private GameObject CreateFlatItemPrefab(Transform templateParent)
        {
            GameObject root = CreateItemRoot(
                "BasicUsageItemTemplate",
                templateParent,
                new Color(0.12f, 0.16f, 0.20f, 0.9f),
                36f);
            root.AddComponent<BasicUsageItem>();
            CreateFillLabel(root.transform, "Label", 16, FontStyle.Normal, new Vector2(12f, 4f), new Vector2(-12f, -4f));
            return root;
        }

        private GameObject CreateNestedChildItemPrefab(Transform templateParent)
        {
            GameObject root = CreateItemRoot(
                "NestedChildItemTemplate",
                templateParent,
                new Color(0.10f, 0.15f, 0.16f, 0.9f),
                30f);
            root.AddComponent<NestedChildItem>();
            CreateFillLabel(root.transform, "Label", 14, FontStyle.Normal, new Vector2(14f, 3f), new Vector2(-10f, -3f));
            return root;
        }

        private GameObject CreateNestedCategoryPrefab(Transform templateParent)
        {
            GameObject childItemPrefab = GetNestedChildItemPrefab(templateParent);
            GameObject root = new GameObject(
                "NestedCategoryItemTemplate",
                typeof(RectTransform),
                typeof(Image),
                typeof(LayoutElement),
                typeof(VerticalLayoutGroup),
                typeof(ContentSizeFitter),
                typeof(NestedCategoryItem));
            root.transform.SetParent(templateParent, false);

            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.sizeDelta = new Vector2(416f, 86f);

            Image background = root.GetComponent<Image>();
            background.color = new Color(0.15f, 0.13f, 0.21f, 0.92f);

            LayoutElement layoutElement = root.GetComponent<LayoutElement>();
            layoutElement.minHeight = 64f;

            VerticalLayoutGroup layout = root.GetComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(10, 10, 8, 8);
            layout.spacing = 6f;
            layout.childControlHeight = true;
            layout.childControlWidth = true;

            ContentSizeFitter fitter = root.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Text title = CreateText("Title", root.transform, string.Empty, 16, FontStyle.Bold);
            title.alignment = TextAnchor.MiddleLeft;
            title.color = new Color(1f, 0.96f, 0.82f, 1f);
            title.gameObject.AddComponent<LayoutElement>().preferredHeight = 24f;

            GameObject childrenObject = new GameObject(
                "Children",
                typeof(RectTransform),
                typeof(VerticalLayoutGroup),
                typeof(ContentSizeFitter));
            childrenObject.transform.SetParent(root.transform, false);

            RectTransform childrenParent = childrenObject.GetComponent<RectTransform>();
            VerticalLayoutGroup childrenLayout = childrenObject.GetComponent<VerticalLayoutGroup>();
            childrenLayout.spacing = 4f;
            childrenLayout.childControlHeight = true;
            childrenLayout.childControlWidth = true;

            ContentSizeFitter childrenFitter = childrenObject.GetComponent<ContentSizeFitter>();
            childrenFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            NestedCategoryItem categoryItem = root.GetComponent<NestedCategoryItem>();
            categoryItem.Configure(title, childrenParent, childItemPrefab);
            return root;
        }

        private static GameObject CreateItemRoot(string name, Transform parent, Color color, float preferredHeight)
        {
            GameObject root = new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(LayoutElement));
            root.transform.SetParent(parent, false);

            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.sizeDelta = new Vector2(416f, preferredHeight);

            Image background = root.GetComponent<Image>();
            background.color = color;

            LayoutElement layoutElement = root.GetComponent<LayoutElement>();
            layoutElement.preferredHeight = preferredHeight;

            return root;
        }

        private static Text CreateFillLabel(
            Transform parent,
            string name,
            int fontSize,
            FontStyle fontStyle,
            Vector2 offsetMin,
            Vector2 offsetMax)
        {
            GameObject labelObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            labelObject.transform.SetParent(parent, false);

            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = offsetMin;
            labelRect.offsetMax = offsetMax;

            Text text = labelObject.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleLeft;
            text.color = Color.white;
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            return text;
        }

        private static Text CreateText(Transform parent, string label, int fontSize, FontStyle fontStyle)
        {
            return CreateText("Text", parent, label, fontSize, fontStyle);
        }

        private static Text CreateText(string name, Transform parent, string label, int fontSize, FontStyle fontStyle)
        {
            GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);

            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 0.5f);
            rect.anchorMax = new Vector2(1f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(0f, 24f);

            Text text = textObject.GetComponent<Text>();
            text.text = label;
            text.alignment = TextAnchor.MiddleLeft;
            text.color = Color.white;
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            return text;
        }

        private void OnDestroy()
        {
            if (_sharedLayout == this)
            {
                _sharedLayout = null;
            }
        }
    }
}
