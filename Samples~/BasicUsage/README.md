# Generic UI Items Basic Usage

This sample contains a small runtime example for creating and synchronizing UI item prefabs from a data collection.

Open `BasicUsage.unity` and enter Play Mode to see the default scene create a list from data. You can also use `BasicUsageItem` on the root GameObject of an item prefab and assign a `Text` component to its label field. Use `BasicUsageExample` on any scene object, then assign a parent `RectTransform` and that item prefab.

The flat sample methods show:

- `SetItems`
- `Add`
- `Update`
- `Remove`
- `ReplaceAll`
- `Clear`

The identity selector is explicit:

```csharp
_container = new GenericUIContainer<ExampleItemData, string>(
    parent,
    itemPrefab,
    item => item.Id);
```

## Nested Categories

`NestedCategoriesExample` shows the nested pattern without adding runtime nesting APIs. Each `NestedCategoryItem` owns a child `GenericUIContainer<NestedItemData, string>` that renders under the category item's `Children` transform.

The nested sample methods show parent collection actions plus child actions through the owning parent item:

- `AddCategory`
- `UpdateWeapons`
- `RemoveArmor`
- `AddWeaponChild`
- `UpdateSword`
- `RemoveShield`
- `ReplaceWeaponChildren`
- `Clear`

`BasicUsageSampleLayout` is sample-only. It creates default UGUI scene objects and templates when fields are not assigned, keeping layout construction out of the package runtime container.
