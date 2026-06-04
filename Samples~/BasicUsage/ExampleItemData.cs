using System;

namespace JorisHoef.GenericUIItems.Samples.BasicUsage
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
