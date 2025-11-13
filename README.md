# AR Interior Design App

## Week 1 Progress

### What  I Did
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
## Week 4 Progress – Stabilizing Object Placement & Interaction System

### What I Did
- Refactored ObjectPlacer to unify with ObjectPlacementAndManipulation structure
  - Added UI touch filtering (IsTouchOverUI) to prevent unintended placement when touching UI panels
  - Integrated selection / deselection system
    - Tap on furniture → highlight via QuickOutline
    - Tap empty space → deselect and disable outline
  - Added placement-state logic to prevent repeated movement after placement
- Implemented safe null-checks and debug logs for placement flow
  - Guarded FurnitureSelectionManager.CurrentFurniturePrefab against null to avoid ArgumentException
  - Added [AR] prefixed debug messages for runtime tracking
- Adjusted anchor attachment: automatic fallback to ARAnchor if AttachAnchor() fails
- Updated deprecated API calls (FindObjectOfType → FindFirstObjectByType) for Unity 6000.1+

### Troubleshooting Summary
- Issue: Continuous selection without release → object stayed highlighted
  → Added DeselectObject() method + empty-space tap detection
- Issue: IsTouchOverUI missing symbol error
  → Implemented dedicated helper function with PointerEventData filter
- Issue: Prefab null reference on instantiate
  → Verified FurnitureSelectionManager UI binding + added null-protection
- Issue: Model not standing upright after spawn
  → Checked prefab pivot/orientation; adjusted rotation handling in hit pose
- Warnings: Deprecated FindObjectOfType usage
  → Replaced with FindFirstObjectByType for future compatibility
### Result
- Furniture placement and selection now operate consistently through UI interaction only
- Objects can be placed once, highlighted, and locked in position after confirmation
- No more unintended spawning or persistent highlights
- Verified stable build on iPhone (ARKit 6.2.0 / Unity 6000.1.17f1)

# 주차별 진행 요약 
## 1주차
- 새로운 AR 씬 생성 및 AR Foundation + ARKit XR Plugin 설정
- PlaneVisualizer를 활용해 평면 시각화 기능 구현
- 터치 기반 오브젝트 배치(Anchor 테스트) 완료
- 입력 감지 문제 해결 (Active Input Handling → Both)
✅ 아이폰에서 큐브 정상 배치 확인

## 2주차
- 오브젝트 조작 시스템 완성 (이동, 회전, 확대/축소)
- 오브젝트 선택 시 QuickOutline 하이라이트 적용
- 터치 이동 중 중복 배치 문제 해결 (isPlacing 플래그 추가)
- ARRaycast 기반 터치 감지 개선
✅ 자연스러운 제스처 조작과 정확한 배치 확인

## 3주차
- UI 기반 가구 배치 시스템 완성
  → 화면 터치로는 배치 불가, ScrollView 버튼 클릭 시만 배치
- Selectable / ObjectSelector 개선으로 하이라이트 토글 복원
- FurnitureSelectionManager 자동 연결 로직 추가 (null 체크 및 FindObjectOfType 대체)
✅ iPhone 빌드에서 올바른 평면 정렬 및 안정적인 선택 시스템 검증

## 4주차
- ObjectPlacer 및 ObjectPlacementAndManipulation 구조 통합
- UI 터치 필터링 추가(IsTouchOverUI) → UI 패널 터치 시 오브젝트 배치 차단
- 선택 / 선택 해제 시스템 구현 (빈 공간 터치 시 선택 해제)
- Anchor 자동 부착 + 실패 시 폴백 처리
- FindObjectOfType → FindFirstObjectByType 변경으로 API 경고 제거
✅ 오브젝트가 한 번만 생성되고, UI 기반 상호작용만으로 안정적으로 제어 가능
✅ iPhone (ARKit 6.2.0 / Unity 6000.1.17f1)에서 정상 작동 확인
<p align="center">
  <img width="642" height="1389" alt="IMG_1511" src="https://github.com/user-attachments/assets/4ef1c1f3-c4db-4bc8-9e9b-c4dcd0adb179" />
  <img width="642" height="1389" alt="IMG_1514" src="https://github.com/user-attachments/assets/ed490d7c-6663-4b13-91d4-48ddaaa7ec8c" />
</p>


