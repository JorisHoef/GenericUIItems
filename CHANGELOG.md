# Changelog

## Unreleased

- Added optional item visual strategy support for normal, selected, and hovered UI item states.
- Added explicit selected and hovered key APIs on `GenericUIContainer<T, TKey>` and `GenericScrollView<T, TKey>`.
- Added dependency-free built-in UGUI tint and transform scale item visuals.
- Updated the Basic Usage sample to demonstrate selected and normal item visuals.
- Added EditMode tests for selection visual behavior and package dependency boundaries.
- Improved README structure and clarified package branch, samples, API, integrations, and limitations.
- Added nested UI item sample coverage using parent item composition.
- Moved Basic Usage sample UI construction into a sample-only layout helper.
- Added EditMode tests for nested child containers, scoped child keys, and nested cleanup.

## 1.0.0

- Created canonical UPM package layout.
- Added explicit-key `GenericUIContainer<T, TKey>` and `GenericScrollView<T, TKey>`.
- Added `ISettableItem<T>` prefab contract and optional `GenericItem<T>` base class.
- Added EditMode tests, Basic Usage sample, docs, validation tooling, and GitHub workflows.
