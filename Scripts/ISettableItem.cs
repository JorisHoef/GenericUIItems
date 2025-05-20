using UnityEngine;

namespace JorisHoef.GenericItems
{
    /// <summary>
    /// Implement ISettableItem for any UI items you want to fill with Data
    /// </summary>
    /// <typeparam name="T">Where T is our dataType</typeparam>
    public interface ISettableItem<T>
    {
        T GetData();
        void SetData(T data);
        GameObject GetGameObject();
    }
}