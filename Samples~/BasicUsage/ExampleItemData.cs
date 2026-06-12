using System;

namespace Deucarian.UIBinding.Samples.BasicUsage
{
    [Serializable]
    public sealed class ExampleItemData
    {
        public ExampleItemData(string id, string label)
        {
            Id = id;
            Label = label;
        }

        public string Id;
        public string Label;
    }
}
