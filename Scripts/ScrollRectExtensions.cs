using System.Collections.Generic;
using UnityEngine.UI;

namespace JorisHoef.GenericItems
{
    public static class ScrollRectExtensions
    {
        private static readonly Dictionary<ScrollRect, object> _customScrollViews = new Dictionary<ScrollRect, object>();

        /// <summary>
        /// Get CustomScrollView to generically be able to add different types of ISettables
        /// </summary>
        /// <param name="scrollRect"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static GenericScrollView<T> GenericScrollView<T>(this ScrollRect scrollRect) where T : class
        {
            if (_customScrollViews.TryGetValue(scrollRect, out object existingScrollView))
            {
                return existingScrollView as GenericScrollView<T>;
            }
            
            GenericScrollView<T> genericScrollView = new GenericScrollView<T>(scrollRect);
            _customScrollViews.Add(scrollRect, genericScrollView);
            return genericScrollView;
        }
    }
}