using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.GenericUIItems.Samples.BasicUsage
{
    public sealed class BasicUsageItem : GenericItem<ExampleItemData>
    {
        [SerializeField] private Text label;

        public override void SetData(ExampleItemData data)
        {
            base.SetData(data);

            if (label == null)
            {
                label = GetComponentInChildren<Text>();
            }

            if (label != null)
            {
                label.text = data != null ? data.Label : string.Empty;
            }
        }
    }
}
