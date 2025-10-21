# AR Interior Design App

## Week 1 Progress

### What I Did
- Created new AR scene
- Configured **AR Foundation + ARKit XR Plugin**
- Set up **AR Session Origin**, **AR Plane Manager**, and **AR Camera**
- Connected **PlaneVisualizer.prefab** to visualize detected planes
- Added initial **touch-to-place cube** logic (Anchor placement test)

### Troubleshooting Summary
- **Build Error**: Xcode linker issue  
  → Fixed by upgrading Unity/Xcode environment (see Issue #1).
- **Plane Visualization**: Connected AR Plane Manager → Plane Prefab with PlaneVisualizer.  
  → Planes highlight correctly.
- **Touch Input**: Not detected initially.  
  → Fixed by setting `Active Input Handling` → **Both**.  
  → Cube placement confirmed on iPhone.

## Week 2 Progress: Object Manipulation (Move, Rotate, Scale)

### What I Did
- Implemented full object manipulation system:
  - Tap on plane → place object
  - Tap on object → select with outline
  - Drag → move object
  - Pinch → scale object
  - Two-finger rotation → rotate around Y-axis
- Integrated QuickOutline asset for visual feedback
  - Added tag-based selection system ("Furniture") to separate placement vs manipulation modes

### Troubleshooting Summary
- Continuous spawning on drag:
  → Added isPlacing flag + tag-based selection → fixed.
- Gesture control not applied:
  → Implemented ARRaycastManager.Raycast() based touch projection.
- Outline not showing:
  → Installed and integrated QuickOutline → working correctly.

### Result
- Objects can now be placed, selected, moved, scaled, and rotated intuitively.
- Verified smooth gesture response and accurate placement on iPhone build.

---

## Week 3 Progress: Furniture Selection & Placement Integration

### What I Did

- Refactored **furniture placement system**:
  -  Moved all placement logic to **`ObjectPlacementAndManipulation`**
  -  Implemented **ScrollView button-based placement** — object spawns only when selected from UI, not by tapping screen
  -  Ensured **plane detection** via `ARRaycastManager` for grounded placement
- Updated **Selectable.cs** and **ObjectSelector.cs**:
  -Rebuilt **highlight toggle system** with `SetHighlight()` / `ToggleHighlight()` methods
  - Integrated **QuickOutline** for visual feedback on selection
  - Fixed missing method references (`SetHighlight`, `ToggleHighlight`) and script link issues
- Connected **FurnitureSelectionManager → ObjectPlacementAndManipulation** reference
  - Added null check & auto-link fallback
  - Resolved `[FurnitureSelectionManager] placementSystem이 연결되지 않았습니다!` error

### Troubleshooting Summary

- **Problem:** Furniture spawned randomly on drag
  → Fixed by removing touch-based placement; placement now triggered only via ScrollView button.
- **Problem:** Missing highlight references (`SetHighlight`, `ToggleHighlight`)
  → Reimplemented methods in `Selectable.cs`.
- **Problem:** placementSystem not linked
  → Added runtime fallback `FindObjectOfType<ObjectPlacementAndManipulation>()`.
- **Problem:** Lost local edits after stash
  → Restored using `git stash list` + `git stash pop stash@{0}`.

### Result

- Furniture placement now works **only through ScrollView UI** — no unintended spawning.
- Selected objects can be **moved, rotated, scaled, and highlighted correctly**.
- All scripts compile and link without error.
- Verified clean operation on iPhone build with proper plane alignment and stable selection system.

---
