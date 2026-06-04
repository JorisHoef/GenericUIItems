using System.Collections.Generic;
using UnityEngine;

namespace JorisHoef.GenericUIItems.Samples.BasicUsage
{
    public sealed class NestedCategoriesExample : MonoBehaviour
    {
        [SerializeField] private BasicUsageSampleLayout layout;
        [SerializeField] private RectTransform categoriesParent;
        [SerializeField] private GameObject categoryPrefab;

        private GenericUIContainer<NestedCategoryData, string> _categories;
        private int _nextCategoryId = 3;
        private int _nextChildId = 4;

        private void Awake()
        {
            EnsureSetup();

            _categories = new GenericUIContainer<NestedCategoryData, string>(
                categoriesParent,
                categoryPrefab,
                category => category.Id);

            _categories.SetItems(CreateStartingCategories());
        }

        public void AddCategory()
        {
            string id = _nextCategoryId.ToString();
            _nextCategoryId++;

            _categories.Add(new NestedCategoryData(
                id,
                $"Added category {id}",
                new[]
                {
                    new NestedItemData("1", $"Child for category {id}")
                }));
        }

        public void UpdateWeapons()
        {
            _categories.Update(new NestedCategoryData(
                "weapons",
                "Weapons updated",
                new[]
                {
                    new NestedItemData("sword", "Longsword"),
                    new NestedItemData("bow", "Bow")
                }));
        }

        public void RemoveArmor()
        {
            _categories.Remove("armor");
        }

        public void AddWeaponChild()
        {
            if (!TryGetCategory("weapons", out NestedCategoryItem category))
            {
                return;
            }

            string id = $"extra-{_nextChildId}";
            _nextChildId++;
            category.AddChild(new NestedItemData(id, $"Added child {id}"));
        }

        public void UpdateSword()
        {
            if (TryGetCategory("weapons", out NestedCategoryItem category))
            {
                category.UpdateChild(new NestedItemData("sword", "Sword updated in child container"));
            }
        }

        public void RemoveShield()
        {
            if (TryGetCategory("armor", out NestedCategoryItem category))
            {
                category.RemoveChild("shield");
            }
        }

        public void ReplaceWeaponChildren()
        {
            if (TryGetCategory("weapons", out NestedCategoryItem category))
            {
                category.ReplaceChildren(new[]
                {
                    new NestedItemData("dagger", "Dagger"),
                    new NestedItemData("staff", "Staff")
                });
            }
        }

        public void Clear()
        {
            _categories.Clear();
        }

        private void EnsureSetup()
        {
            if (layout == null)
            {
                layout = BasicUsageSampleLayout.GetOrCreateShared();
            }

            if (categoriesParent == null)
            {
                categoriesParent = layout.GetNestedCategoriesParent();
            }

            if (categoryPrefab == null)
            {
                categoryPrefab = layout.GetNestedCategoryPrefab(transform);
            }
        }

        private bool TryGetCategory(string id, out NestedCategoryItem category)
        {
            category = null;
            return _categories.TryGetItem(id, out ISettableItem<NestedCategoryData> item)
                && (category = item as NestedCategoryItem) != null;
        }

        private static List<NestedCategoryData> CreateStartingCategories()
        {
            return new List<NestedCategoryData>
            {
                new NestedCategoryData(
                    "weapons",
                    "Weapons",
                    new[]
                    {
                        new NestedItemData("sword", "Sword"),
                        new NestedItemData("bow", "Bow")
                    }),
                new NestedCategoryData(
                    "armor",
                    "Armor",
                    new[]
                    {
                        new NestedItemData("shield", "Shield"),
                        new NestedItemData("helmet", "Helmet")
                    })
            };
        }
    }
}
