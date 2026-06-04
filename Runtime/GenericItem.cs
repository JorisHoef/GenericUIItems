using UnityEngine;

namespace JorisHoef.GenericUIItems
{
    /// <summary>
    /// Optional base class for UI item prefabs that want to keep the latest assigned data.
    /// </summary>
    /// <typeparam name="T">The data type displayed by the UI item.</typeparam>
    public abstract class GenericItem<T> : MonoBehaviour, ISettableItem<T>
    {
        public T Data { get; private set; }

        public virtual void SetData(T data)
        {
            Data = data;
        }
    }
}
