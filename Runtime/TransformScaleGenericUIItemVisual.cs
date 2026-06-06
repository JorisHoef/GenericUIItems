using System;
using System.Collections.Generic;
using UnityEngine;

namespace JorisHoef.GenericUIItems
{
    /// <summary>
    /// Visual strategy that scales an item view transform for selected or hovered state.
    /// </summary>
    /// <typeparam name="TKey">The stable item key type.</typeparam>
    /// <typeparam name="T">The data type displayed by the UI item.</typeparam>
    public sealed class TransformScaleGenericUIItemVisual<TKey, T> : IGenericUIItemVisual<TKey, T>
    {
        private readonly Dictionary<Transform, Vector3> _originalScales =
            new Dictionary<Transform, Vector3>();
        private readonly Vector3 _selectedScaleMultiplier;
        private readonly Vector3 _hoveredScaleMultiplier;

        /// <summary>
        /// Creates a transform scale visual with subtle selected and hovered multipliers.
        /// </summary>
        public TransformScaleGenericUIItemVisual()
            : this(1.04f, 1.02f)
        {
        }

        /// <summary>
        /// Creates a transform scale visual using uniform selected and hovered multipliers.
        /// </summary>
        public TransformScaleGenericUIItemVisual(float selectedScaleMultiplier, float hoveredScaleMultiplier)
            : this(
                new Vector3(selectedScaleMultiplier, selectedScaleMultiplier, selectedScaleMultiplier),
                new Vector3(hoveredScaleMultiplier, hoveredScaleMultiplier, hoveredScaleMultiplier))
        {
        }

        /// <summary>
        /// Creates a transform scale visual.
        /// </summary>
        public TransformScaleGenericUIItemVisual(
            Vector3 selectedScaleMultiplier,
            Vector3 hoveredScaleMultiplier)
        {
            ValidateMultiplier(selectedScaleMultiplier, nameof(selectedScaleMultiplier));
            ValidateMultiplier(hoveredScaleMultiplier, nameof(hoveredScaleMultiplier));

            _selectedScaleMultiplier = selectedScaleMultiplier;
            _hoveredScaleMultiplier = hoveredScaleMultiplier;
        }

        /// <inheritdoc />
        public void ApplyNormal(TKey key, T item, object view)
        {
            Transform transform = ResolveTransform(view);
            if (transform == null)
            {
                return;
            }

            Vector3 originalScale;
            if (_originalScales.TryGetValue(transform, out originalScale))
            {
                transform.localScale = originalScale;
                _originalScales.Remove(transform);
            }
        }

        /// <inheritdoc />
        public void ApplySelected(TKey key, T item, object view)
        {
            ApplyScale(view, _selectedScaleMultiplier);
        }

        /// <inheritdoc />
        public void ApplyHovered(TKey key, T item, object view)
        {
            ApplyScale(view, _hoveredScaleMultiplier);
        }

        private void ApplyScale(object view, Vector3 scaleMultiplier)
        {
            Transform transform = ResolveTransform(view);
            if (transform == null)
            {
                return;
            }

            if (!_originalScales.ContainsKey(transform))
            {
                _originalScales.Add(transform, transform.localScale);
            }

            Vector3 originalScale = _originalScales[transform];
            transform.localScale = Vector3.Scale(originalScale, scaleMultiplier);
        }

        private static Transform ResolveTransform(object view)
        {
            Transform transform = view as Transform;
            if (transform != null)
            {
                return transform;
            }

            GameObject gameObject = view as GameObject;
            if (gameObject != null)
            {
                return gameObject.transform;
            }

            Component component = view as Component;
            return component != null ? component.transform : null;
        }

        private static void ValidateMultiplier(Vector3 multiplier, string parameterName)
        {
            if (multiplier.x <= 0f || multiplier.y <= 0f || multiplier.z <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    "Scale multipliers must be greater than zero.");
            }
        }
    }
}
