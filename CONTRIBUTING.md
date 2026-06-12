# Contributing

## Scope

UI Binding is a standalone UPM package. Keep it independent from API and avoid adding framework-level responsibilities such as MVVM, app state management, or general UI architecture.

## Local Validation

Run structural validation from the package root:

```powershell
pwsh ./Tools/Validate-Package.ps1
```

For Unity validation, use a separate test project that references this package by file path:

```json
"com.deucarian.ui-binding": "file:C:/Repositories/UIBinding"
```

Package source should stay in this repository. Do not copy package code into the test project.

## Pull Requests

- Keep runtime changes focused.
- Add or update EditMode tests for behavior changes.
- Keep runtime asmdef free of editor-only references.
- Do not add dependencies beyond what the package actually uses.
