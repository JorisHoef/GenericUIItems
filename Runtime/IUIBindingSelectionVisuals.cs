namespace Deucarian.UIBinding
{
    /// <summary>
    /// Optional container contract for applying item visual state by explicit keys.
    /// </summary>
    /// <typeparam name="TKey">The stable item key type.</typeparam>
    /// <typeparam name="T">The data type displayed by the UI item.</typeparam>
    public interface IUIBindingSelectionVisuals<TKey, T>
    {
        bool HasItemVisual { get; }
        bool HasSelectedKey { get; }
        TKey SelectedKey { get; }
        bool HasHoveredKey { get; }
        TKey HoveredKey { get; }

        void SetItemVisual(IUIBindingItemVisual<TKey, T> itemVisual);
        void SetSelectedKey(TKey selectedKey);
        void ClearSelectedKey();
        void SetHoveredKey(TKey hoveredKey);
        void ClearHoveredKey();
        void RefreshItemVisuals();
    }
}
