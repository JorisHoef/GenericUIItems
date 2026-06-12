# UI Binding Basic Usage

This sample contains a small runtime example for creating and synchronizing UI item prefabs from a data collection.

Open `BasicUsage.unity` and enter Play Mode to see the default scene create a list from data. You can also use `BasicUsageItem` on the root GameObject of an item prefab and assign a `Text` component to its label field. Use `BasicUsageExample` on any scene object, then assign a parent `RectTransform` and that item prefab.

The flat sample methods show:

- `SetItems`
- `Add`
- `Update`
- `Remove`
- `SelectFirst`
- `SelectSecond`
- `ClearSelectedItem`
- `ReplaceAll`
- `Clear`

The identity selector is explicit:

```csharp
_container = new UIBindingContainer<ExampleItemData, string>(
    parent,
    itemPrefab,
    item => item.Id,
    new GraphicTintUIBindingItemVisual<string, ExampleItemData>(
        normalColor,
        selectedColor,
        hoveredColor));
```

Selection visuals are supplied explicitly by key. `UIBinding` does not know where the selected key came from; a project, bridge, input adapter, or state package can call `SetSelectedKey(key)` or `ClearSelectedKey()` when appropriate.

## Nested Categories

`NestedCategoriesExample` shows the nested pattern without adding runtime nesting APIs. Each `NestedCategoryItem` owns a child `UIBindingContainer<NestedItemData, string>` that renders under the category item's `Children` transform.

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
