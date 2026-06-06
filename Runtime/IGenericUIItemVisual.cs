namespace JorisHoef.GenericUIItems
{
    /// <summary>
    /// Defines how a Generic UI item should look for normal, selected, and hovered states.
    /// </summary>
    /// <typeparam name="TKey">The stable item key type.</typeparam>
    /// <typeparam name="T">The data type displayed by the UI item.</typeparam>
    public interface IGenericUIItemVisual<TKey, T>
    {
        /// <summary>
        /// Applies normal visuals to an item view.
        /// </summary>
        /// <param name="key">The item key.</param>
        /// <param name="item">The item data.</param>
        /// <param name="view">The spawned item view, usually the root item component.</param>
        void ApplyNormal(TKey key, T item, object view);

        /// <summary>
        /// Applies selected visuals to an item view.
        /// </summary>
        /// <param name="key">The item key.</param>
        /// <param name="item">The item data.</param>
        /// <param name="view">The spawned item view, usually the root item component.</param>
        void ApplySelected(TKey key, T item, object view);

        /// <summary>
        /// Applies hovered visuals to an item view.
        /// </summary>
        /// <param name="key">The item key.</param>
        /// <param name="item">The item data.</param>
        /// <param name="view">The spawned item view, usually the root item component.</param>
        void ApplyHovered(TKey key, T item, object view);
    }
}
