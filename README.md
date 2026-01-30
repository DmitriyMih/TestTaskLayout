# Unity UI Layout & Animation

This repository contains a small Unity project demonstrating:
- adaptive UI layout (portrait mode)
- simple, modular UI animation logic based on Animator

The focus of the project is **clean Animator control without complex Animator transitions**.

---

## How to run
1. Open the project in **Unity 2022 and the future**
2. Open the scene: Assets/Scenes/SplashScreen.unity
3. Press **Play**

---

## What is implemented

### Adaptive UI layout
- Portrait-oriented UI
- Canvas Scaler with reference resolution
- Safe Area handling for different screen sizes

### Animator-driven UI animations
UI animations are built using small, independent components instead of large
Animator graphs with many transitions.

Each component solves a single problem:
- delaying animation start
- detecting animation completion
- switching Animator Controllers
- starting specific Animator states
- optional animation desynchronization

---

## Core animation flow

A typical UI element follows this sequence:

Delay
→ Fade-in animation (UIShow)
→ Animator Controller switch
→ Explicit state start (UIScale / UIRotation / etc)


This approach allows:
- reuse of the same Animator Controllers across multiple UI elements
- different animation behavior per object
- predictable animation order

---

## Components overview

All animation-related scripts are located in: Assets/Scripts/Presentation/Animations


### AnimatorDelayActivator
Disables the Animator on start, waits for a fixed or random delay,
then enables the Animator to start the appearance animation.

### AnimatorAutoSwitchOnComplete
Monitors completion of a specified Animator state and switches to another
Animator Controller when the state finishes.

### AnimatorStateRouter
Explicitly starts a configured Animator state.
Used when a controller does not automatically start a state (Entry → Empty).

### AnimatorRandomStart
Optional component used to desynchronize repeated UI animations
by randomizing delay, speed, and animation phase.

---

## Example setup (single UI element)

Components on one UI object:

Animator
AnimatorDelayActivator
AnimatorAutoSwitchOnComplete
AnimatorStateRouter


Flow:
1. AnimatorDelayActivator waits before enabling the Animator
2. UIShow animation plays
3. AnimatorAutoSwitchOnComplete switches the Animator Controller
4. AnimatorStateRouter starts the required state in the new controller

Component interaction is configured via **UnityEvent in the Inspector**.

---

## Dependencies
- **Odin Inspector** — used only for improved Inspector UI and conditional fields

---

## Notes
- Animation Events inside clips are intentionally avoided
- Animation logic is handled in code, not Animator transition graphs
- Components are safe to use on multiple objects sharing the same Animator Controller
