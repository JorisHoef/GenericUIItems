using System.Collections.Generic;
using UnityEngine;

namespace JorisHoef.GenericItems
{
    public static class RectTransformExtensions
    {
        private static readonly Dictionary<RectTransform, object> _customUIContainers = new Dictionary<RectTransform, object>();

        /// <summary>
        /// Use this extension methods to invoke our GenericUIContainer and add different types of ISettables
        /// </summary>
        /// <param name="parent"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static GenericUIContainer<T> GenericUIContainer<T>(this RectTransform parent) where T : class
        {
            if (_customUIContainers.TryGetValue(parent, out object existingUIContainer))
            {
                return existingUIContainer as GenericUIContainer<T>;
            }
            
            GenericUIContainer<T> genericUIContainer = new GenericUIContainer<T>(parent);
            _customUIContainers.Add(parent, genericUIContainer);
            return genericUIContainer;
        }
    }
}