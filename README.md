# Unity UI Layout & Splash Screen Flow

This repository contains a Unity 6.2 project demonstrating a clean and minimal
approach to portrait UI layout and splash screen initialization.

The project focuses on:
- adaptive portrait UI layout
- simple splash screen flow
- dependency injection via VContainer
- centralized scene management via a catalog (no hardcoded scene strings)

---

## How to run
1. Open the project in **Unity 6.2**
2. Open the scene: Assets/Scenes/SplashScreen.unity
3. Press **Play**

---

## What is implemented

### Global application scope
- A single `GlobalLifetimeScope` acts as the composition root
- It persists between scenes (`DontDestroyOnLoad`)
- Registers global services (scene navigation, scene catalog)

### Centralized scene catalog
- A single `SceneCatalog` ScriptableObject is used as the source of truth for scenes
- It is stored in `Resources/SceneCatalog.asset` and loaded automatically
- Scenes are addressed by **keys** (e.g. `Menu`) instead of string literals
- Entries can be enabled/disabled
- A tool button can synchronize the catalog with Unity Build Settings
- Inspector UI is implemented using **Odin Inspector**

### Splash screen flow
- Splash screen has its own LifetimeScope
- `SplashScreenManager` is initialized via DI
- Current behavior: wait N seconds → load next scene (by key)
- Next scene is selected via an Odin dropdown using keys from `SceneCatalog`
- No manual scene name strings are used in gameplay/configuration code

---

## Architecture overview

GlobalLifetimeScope
├─ SceneCatalog
└─ SceneNavigator

SplashScreenLifetimeScope
└─ SplashScreenManager
   └─ Delay → Load scene by key

This structure is intentionally minimal and can be extended later with:
- authorization
- resource preloading
- remote config
without changing scene setup approach.

---

## Dependencies
- **VContainer** — dependency injection
- **Odin Inspector** — inspector UI and editor tooling

---

## Notes
- Scene configuration is centralized and editor-friendly
- The runtime uses scene keys, not hardcoded scene names
- The repository is kept minimal to match the test requirements
