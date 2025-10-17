using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private ARRaycastManager arRaycastManager;   // AR 평면 감지용
    [SerializeField] private ARAnchorManager anchorManager;       // Anchor 생성용

    // Inspector에 넣어도 되고, 비워두면 자동으로 찾아서 씀
    [SerializeField] private ColorApplicator colorApplicator;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        // 비워져 있으면 한 번 찾아서 캐싱
        if (colorApplicator == null) colorApplicator = FindObjectOfType<ColorApplicator>();
    }

    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        // 모바일 터치 입력
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (arRaycastManager != null &&
                arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                var plane = hits[0].trackable.GetComponent<ARPlane>();

                // 평면이 수평(바닥)이고, 선택된 프리팹이 있을 때만 배치
                if (plane.alignment == PlaneAlignment.HorizontalUp &&
                    FurnitureSelectionManager.CurrentFurniturePrefab != null)
                {
                    GameObject newObj = Instantiate(
                        FurnitureSelectionManager.CurrentFurniturePrefab,
                        hitPose.position, hitPose.rotation);

                    AttachAnchor(newObj, hits[0]);

                    // 방금 배치한 오브젝트를 색상 타겟으로 지정
                    SetColorTarget(newObj);
                }
            }
        }
#else
        // 에디터
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Ground") &&
                    FurnitureSelectionManager.CurrentFurniturePrefab != null)
                {
                    GameObject newObj = Instantiate(
                        FurnitureSelectionManager.CurrentFurniturePrefab,
                        hit.point, Quaternion.identity);

                    // 에디터에서도 동일하게 타겟 지정
                    SetColorTarget(newObj);
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
            Debug.LogWarning("Anchor 생성 실패 → 객체에 직접 ARAnchor 추가");
            obj.AddComponent<ARAnchor>();
        }
    }

    // 타겟 지정 헬퍼
    private void SetColorTarget(GameObject go)
    {
        if (colorApplicator == null)
            colorApplicator = FindObjectOfType<ColorApplicator>();

        if (colorApplicator != null)
            colorApplicator.SetTarget(go);
        else
            Debug.LogWarning("ColorApplicator를 찾지 못했어요. 씬에 추가했는지 확인하세요.");
    }
}
