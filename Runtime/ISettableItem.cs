namespace Deucarian.UIBinding
{
    /// <summary>
    /// Implement this on the root component of a UI item prefab to receive data from a generic UI container.
    /// </summary>
    /// <typeparam name="T">The data type displayed by the UI item.</typeparam>
    public interface ISettableItem<in T>
    {
        void SetData(T data);
    }
}
