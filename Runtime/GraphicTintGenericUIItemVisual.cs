using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.GenericUIItems
{
    /// <summary>
    /// UGUI visual strategy that tints the root Graphic of an item view.
    /// </summary>
    /// <typeparam name="TKey">The stable item key type.</typeparam>
    /// <typeparam name="T">The data type displayed by the UI item.</typeparam>
    public sealed class GraphicTintGenericUIItemVisual<TKey, T> : IGenericUIItemVisual<TKey, T>
    {
        private readonly Dictionary<Graphic, Color> _originalColors = new Dictionary<Graphic, Color>();
        private readonly bool _hasNormalColor;
        private readonly Color _normalColor;
        private readonly Color _selectedColor;
        private readonly Color _hoveredColor;

        /// <summary>
        /// Creates a tint visual that restores the original color for normal state.
        /// </summary>
        /// <param name="selectedColor">The color applied while selected.</param>
        public GraphicTintGenericUIItemVisual(Color selectedColor)
            : this(false, Color.white, selectedColor, selectedColor)
        {
        }

        /// <summary>
        /// Creates a tint visual with explicit colors for every state.
        /// </summary>
        /// <param name="normalColor">The color applied for normal state.</param>
        /// <param name="selectedColor">The color applied while selected.</param>
        /// <param name="hoveredColor">The color applied while hovered.</param>
        public GraphicTintGenericUIItemVisual(Color normalColor, Color selectedColor, Color hoveredColor)
            : this(true, normalColor, selectedColor, hoveredColor)
        {
        }

        private GraphicTintGenericUIItemVisual(
            bool hasNormalColor,
            Color normalColor,
            Color selectedColor,
            Color hoveredColor)
        {
            _hasNormalColor = hasNormalColor;
            _normalColor = normalColor;
            _selectedColor = selectedColor;
            _hoveredColor = hoveredColor;
        }

        /// <inheritdoc />
        public void ApplyNormal(TKey key, T item, object view)
        {
            Graphic graphic = ResolveGraphic(view);
            if (graphic == null)
            {
                return;
            }

            if (_hasNormalColor)
            {
                graphic.color = _normalColor;
                return;
            }

            Color originalColor;
            if (_originalColors.TryGetValue(graphic, out originalColor))
            {
                graphic.color = originalColor;
                _originalColors.Remove(graphic);
            }
        }

        /// <inheritdoc />
        public void ApplySelected(TKey key, T item, object view)
        {
            ApplyColor(view, _selectedColor);
        }

        /// <inheritdoc />
        public void ApplyHovered(TKey key, T item, object view)
        {
            ApplyColor(view, _hoveredColor);
        }

        private void ApplyColor(object view, Color color)
        {
            Graphic graphic = ResolveGraphic(view);
            if (graphic == null)
            {
                return;
            }

            if (!_originalColors.ContainsKey(graphic))
            {
                _originalColors.Add(graphic, graphic.color);
            }

            graphic.color = color;
        }

        private static Graphic ResolveGraphic(object view)
        {
            Graphic graphic = view as Graphic;
            if (graphic != null)
            {
                return graphic;
            }

            GameObject gameObject = view as GameObject;
            if (gameObject != null)
            {
                return gameObject.GetComponent<Graphic>();
            }

            Component component = view as Component;
            return component != null ? component.GetComponent<Graphic>() : null;
        }
    }
}
