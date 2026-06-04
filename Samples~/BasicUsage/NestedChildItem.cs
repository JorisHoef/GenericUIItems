using UnityEngine;
using UnityEngine.UI;

namespace JorisHoef.GenericUIItems.Samples.BasicUsage
{
    public sealed class NestedChildItem : GenericItem<NestedItemData>
    {
        [SerializeField] private Text label;

        public override void SetData(NestedItemData data)
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
