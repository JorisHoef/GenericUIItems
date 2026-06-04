# Generic UI Items

Generic UI Items is a lightweight Unity UGUI package for presenting data collections as UI item prefabs.

The package is intentionally small: provide a parent `RectTransform`, an item prefab implementing `ISettableItem<T>`, and an explicit key selector. The container creates, updates, removes, clears, and synchronizes item instances without static caches.

## Installation

Add the package to a Unity project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.jorishoef.generic-ui-items": "file:C:/Repositories/GenericUIItems"
  }
}
```

## Basic Usage

```csharp
using JorisHoef.GenericUIItems;
using UnityEngine;

public sealed class CharacterItem : GenericItem<CharacterData>
{
    public override void SetData(CharacterData data)
    {
        base.SetData(data);
        // Update labels, icons, and other UI state here.
    }
}

var container = new GenericUIContainer<CharacterData, string>(
    parentRectTransform,
    itemPrefab,
    character => character.Id);

container.SetItems(characters);
container.Add(newCharacter);
container.Update(updatedCharacter);
container.Remove(characterId);
container.ReplaceAll(nextCharacters);
container.Clear();
```

## Runtime API

- `ISettableItem<T>` is the prefab contract. Implement `SetData(T data)` on the root component of each UI item prefab.
- `GenericItem<T>` is an optional `MonoBehaviour` base class that stores the latest `Data` value.
- `GenericUIContainer<T, TKey>` manages item prefabs under a `RectTransform`.
- `GenericScrollView<T, TKey>` composes a `GenericUIContainer<T, TKey>` over a `ScrollRect.content`.
- `RectTransformExtensions.CreateGenericUIContainer` and `ScrollRectExtensions.CreateGenericScrollView` are convenience constructors only; they do not cache instances.

## Identity And Synchronization

Identity is always explicit through `Func<T, TKey> keySelector`. `ReplaceAll` updates existing keyed items, adds new keyed items, removes missing keyed items, and reorders transforms to match the input collection.

Duplicate keys throw `ArgumentException`. Null data throws `ArgumentNullException`. Null keys throw `InvalidOperationException`.

## Development Test Project

Use a separate Unity project when developing the package:

```text
GenericUIItems
  -> file: package
GenericUIItems-TestProject
```

The test project should consume:

```json
"com.jorishoef.generic-ui-items": "file:C:/Repositories/GenericUIItems"
```

Edit package code in `C:/Repositories/GenericUIItems`, not in the test project copy.
