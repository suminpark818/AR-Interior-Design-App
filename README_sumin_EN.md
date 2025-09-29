# AR Interior Design App

## Week 1 Progress

### ✅ What I Did
- Created new AR scene
- Configured **AR Foundation + ARKit XR Plugin**
- Set up **AR Session Origin**, **AR Plane Manager**, and **AR Camera**
- Connected **PlaneVisualizer.prefab** to visualize detected planes
- Added initial **touch-to-place cube** logic (Anchor placement test)

### 🚧 Troubleshooting Summary
- **Build Error**: Xcode linker issue  
  → Fixed by upgrading Unity/Xcode environment (see Issue #1).
- **Plane Visualization**: Connected AR Plane Manager → Plane Prefab with PlaneVisualizer.  
  → Planes highlight correctly.
- **Touch Input**: Not detected initially.  
  → Fixed by setting `Active Input Handling` → **Both**.  
  → Cube placement confirmed on iPhone.
