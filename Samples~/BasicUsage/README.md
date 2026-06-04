# Generic UI Items Basic Usage

This sample contains a small runtime example for creating and synchronizing UI item prefabs from a data collection.

Open `BasicUsage.unity` and enter Play Mode to see the default scene create a list from data. You can also use `BasicUsageItem` on the root GameObject of an item prefab and assign a `Text` component to its label field. Use `BasicUsageExample` on any scene object, then assign a parent `RectTransform` and that item prefab.

The sample methods show:

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
