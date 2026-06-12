using System;
using System.Collections.Generic;

namespace Deucarian.UIBinding.Samples.BasicUsage
{
    [Serializable]
    public sealed class NestedCategoryData
    {
        public NestedCategoryData(string id, string title, IEnumerable<NestedItemData> items)
        {
            Id = id;
            Title = title;
            Items = new List<NestedItemData>(items ?? Array.Empty<NestedItemData>());
        }

        public string Id;
        public string Title;
        public List<NestedItemData> Items;
    }
}
