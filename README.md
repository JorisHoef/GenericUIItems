# Deucarian UI Binding

## Overview

Deucarian UI Binding is a Unity UGUI runtime package for presenting data collections as UI item prefabs.

The package keeps the workflow explicit: provide a parent `RectTransform`, an item prefab whose root component implements `ISettableItem<T>`, and a key selector. The container creates, updates, removes, clears, and synchronizes item instances without static caches or project-specific UI architecture.

Package ID: `com.deucarian.ui-binding`

## Installation

Install the package through Unity Package Manager with a Git URL:

```json
{
  "dependencies": {
    "com.deucarian.ui-binding": "https://github.com/Deucarian/UI-Binding.git#develop"
  }
}
```

The current package-ready branch in this repo is `develop`. The package requires Unity `2021.3` or newer and depends on `com.unity.ugui`.

For local development, reference the package by file path from a separate Unity test project:

```json
"com.deucarian.ui-binding": "file:C:/Repositories/UIBinding"
```

## Core Concepts

`ISettableItem<T>` is the prefab contract. The root component of each item prefab must implement `SetData(T data)`.

`UIBindingContainer<T, TKey>` owns a parent transform, item prefab, and `Func<T, TKey>` key selector. Keys define identity for add, update, remove, and replacement.

`ReplaceAll` is the synchronization operation. It updates existing keyed items, adds new keyed items, removes missing keyed items, and reorders transforms to match the input collection.

Visual strategies are optional. `UIBinding` decides which item view belongs to which data key; a caller supplies the selected or hovered key explicitly, and an `IUIBindingItemVisual<TKey, T>` decides how normal, selected, and hovered states look. Selection state and visual representation stay separate, so CoreState bridges, ObjectSelection bridges, DOTween visuals, custom tween packages, outline renderers, shader effects, Animator states, or VFX can be added without changing the container architecture.

Nested UI support is composition. A parent item can own its own child `UIBindingContainer<TChild, TChildKey>` under one of its child transforms. Child keys are scoped to the parent item that owns the child container.

## Public API

- `ISettableItem<T>`: item prefab contract.
- `GenericItem<T>`: optional `MonoBehaviour` base class that stores the latest `Data`.
- `IUIBindingContainer<T, TKey>`: common container operations.
- `UIBindingContainer<T, TKey>`: container for item prefabs under a `RectTransform`.
- `GenericScrollView<T, TKey>`: container wrapper around `ScrollRect.content`.
- `IUIBindingItemVisual<TKey, T>`: optional item visual strategy for normal, selected, and hovered states.
- `IUIBindingSelectionVisuals<TKey, T>`: optional container contract for applying item visual state by explicit selected or hovered keys.
- `GraphicTintUIBindingItemVisual<TKey, T>`: UGUI tint visual strategy for item root graphics.
- `TransformScaleUIBindingItemVisual<TKey, T>`: transform scale visual strategy for item views.
- `RectTransformExtensions.CreateUIBindingContainer`: convenience constructor.
- `ScrollRectExtensions.CreateGenericScrollView`: convenience constructor.

Flat list workflow:

```csharp
using Deucarian.UIBinding;
using UnityEngine;

public sealed class CharacterItem : GenericItem<CharacterData>
{
    public override void SetData(CharacterData data)
    {
        base.SetData(data);
        // Update labels, icons, and other UI state here.
    }
}

var container = new UIBindingContainer<CharacterData, string>(
    parentRectTransform,
    itemPrefab,
    character => character.Id,
    new GraphicTintUIBindingItemVisual<string, CharacterData>(
        normalColor,
        selectedColor,
        hoveredColor));

container.SetItems(characters);
container.SetSelectedKey(selectedCharacterId);
container.Add(newCharacter);
container.Update(updatedCharacter);
container.Remove(characterId);
container.ReplaceAll(nextCharacters);
container.ClearSelectedKey();
container.Clear();
```

Nested item workflow:

```csharp
public sealed class CategoryItem : GenericItem<CategoryData>
{
    [SerializeField] private RectTransform childrenParent;
    [SerializeField] private GameObject childItemPrefab;

    private UIBindingContainer<ItemData, string> children;

    public override void SetData(CategoryData data)
    {
        base.SetData(data);

        if (children == null)
        {
            children = new UIBindingContainer<ItemData, string>(
                childrenParent,
                childItemPrefab,
                item => item.Id);
        }

        children.SetItems(data.Items);
    }
}
```

## Samples

The package contains one sample entry:

- `Basic Usage`: `Samples~/BasicUsage/BasicUsage.unity`

The sample scene includes two workflows:

- `BasicUsageExample`: flat list operations for `SetItems`, `Add`, `Update`, `Remove`, explicit selection visuals, `ReplaceAll`, and `Clear`.
- `NestedCategoriesExample`: nested parent categories where each `NestedCategoryItem` owns a child `UIBindingContainer<NestedItemData, string>`.

`BasicUsageSampleLayout` is sample-only. It creates default UGUI scene objects and item templates when fields are not assigned, keeping layout setup out of the runtime container.

## Integrations

UI Binding has no compiled integration assembly and does not reference Core State, API, Session, or the Package Installer.

It can be composed with Core State in project code by using repository items as the data source for a `UIBindingContainer<T, TKey>`, but this package does not include a Core State adapter.

## Versioning

Current package version: `1.0.0`.

Branch strategy:

- `develop`: current package-ready development branch.
- `main`: repository default branch, but it does not currently contain this UPM package layout.

Use a commit hash or release tag for immutable installs when the repository publishes one.

## Limitations

- The package is UGUI-focused and depends on `com.unity.ugui`.
- Item prefabs must expose `ISettableItem<T>` on a root component.
- Keys must be non-null and unique within each container.
- The package does not provide MVVM, data persistence, app state management, hidden selection synchronization, pooling, virtualization, or async loading.
- Nested UI is built by composing containers in item components; there is no separate nested-container framework.
