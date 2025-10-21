using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class PlaneLayerFixer : MonoBehaviour
{
    void OnEnable()
    {
        var planeManager = GetComponent<ARPlaneManager>();
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        var planeManager = GetComponent<ARPlaneManager>();
        planeManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            // ✅ 새로 생성된 Plane의 Layer를 IgnoreRaycast로 변경
            plane.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
}
