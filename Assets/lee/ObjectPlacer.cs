using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private ARRaycastManager arRaycastManager;   // AR 평면 감지용
    [SerializeField] private ARAnchorManager anchorManager;       // Anchor 생성용
    [SerializeField] private GameObject cubePrefab;               // 배치할 프리팹

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        // 모바일 
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (arRaycastManager != null &&
                arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                var plane = hits[0].trackable.GetComponent<ARPlane>();

                if (plane.alignment == PlaneAlignment.HorizontalUp) 
                {
                    GameObject newObj = Instantiate(cubePrefab, hitPose.position, hitPose.rotation);
                    AttachAnchor(newObj, hits[0]); 
                }
            }
        }
#else
        // Unity Editor
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Ground")) 
                {
                    Instantiate(cubePrefab, hit.point, Quaternion.identity);
                }
            }
        }
#endif
    }

    private void AttachAnchor(GameObject obj, ARRaycastHit hit)
    {
        var plane = hit.trackable as ARPlane;
        ARAnchor anchor = null;

        if (plane != null && anchorManager != null)
        {
            // 평면에 Anchor 붙이기
            anchor = anchorManager.AttachAnchor(plane, hit.pose);
        }

        if (anchor != null)
        {
            obj.transform.SetParent(anchor.transform, worldPositionStays: true);
        }
        else
        {
            Debug.LogWarning("Anchor 생성 실패 객체에 직접 ARAnchor 추가");
            // AnchorManager를 못 쓸 때는 오브젝트에 직접 ARAnchor 컴포넌트 추가
            obj.AddComponent<ARAnchor>();
        }
    }

}
