using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

[RequireComponent(typeof(ARRaycastManager))]
public class ObjectPlacementAndManipulation : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject defaultPrefab;

    [Header("AR Components")]
    [SerializeField] private Camera arCamera; // ✅ Inspector에 AR Camera 넣기

    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [Header("State")]
    public bool isPlacing = true;
    private GameObject selectedObject;
    private Vector2 prevTouchPos1, prevTouchPos2;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        // 1) 오브젝트 선택 체크
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null && hit.collider.gameObject.CompareTag("Furniture"))
                {
                    SelectObject(hit.collider.gameObject);
                    return; // ✅ 선택됐으면 Placement 중단
                }
            }
        }

        // 2) 선택된 오브젝트가 있으면 Manipulation 실행
        if (selectedObject != null)
        {
            HandleSelectionAndManipulation(); // ✅ 함수 이름 맞춤
            return;
        }

        // 3) 아무것도 선택 안 된 상태에서 Plane 터치 → Placement
        if (touch.phase == TouchPhase.Began)
        {
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                GameObject newObj = Instantiate(defaultPrefab, hitPose.position, hitPose.rotation); // ✅ defaultPrefab 사용
                newObj.tag = "Furniture"; // ✅ 태그 필수
                SelectObject(newObj);     // 생성 즉시 선택
            }
        }
    }

    // -----------------------------
    // 오브젝트 선택 + 조작
    // -----------------------------
    void HandleSelectionAndManipulation()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved && selectedObject != null)
            {
                // 선택된 오브젝트 이동
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    selectedObject.transform.position = hitPose.position;
                }
            }
        }
        else if (Input.touchCount == 2 && selectedObject != null)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
            {
                prevTouchPos1 = t0.position;
                prevTouchPos2 = t1.position;
                return;
            }

            if (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)
            {
                // Scale
                float prevDist = (prevTouchPos1 - prevTouchPos2).magnitude;
                float currDist = (t0.position - t1.position).magnitude;
                if (!Mathf.Approximately(prevDist, 0))
                {
                    float scaleFactor = currDist / prevDist;
                    selectedObject.transform.localScale *= scaleFactor;
                }

                // Rotate
                float prevAngle = Mathf.Atan2(prevTouchPos1.y - prevTouchPos2.y, prevTouchPos1.x - prevTouchPos2.x) * Mathf.Rad2Deg;
                float currAngle = Mathf.Atan2(t0.position.y - t1.position.y, t0.position.x - t1.position.x) * Mathf.Rad2Deg;
                float angleDelta = currAngle - prevAngle;
                selectedObject.transform.Rotate(Vector3.up, angleDelta);

                prevTouchPos1 = t0.position;
                prevTouchPos2 = t1.position;
            }
        }
    }

    // -----------------------------
    // 선택 처리
    // -----------------------------
    void SelectObject(GameObject obj)
    {
        if (selectedObject != null && selectedObject != obj)
            SetOutline(selectedObject, false);

        selectedObject = obj;
        SetOutline(selectedObject, true);
    }

    void SetOutline(GameObject obj, bool enable)
    {
        var outline = obj.GetComponent<Outline>();
        if (outline != null) outline.enabled = enable;
    }

    public void EnablePlacementMode()
    {
        isPlacing = true;
        if (selectedObject != null)
        {
            SetOutline(selectedObject, false);
            selectedObject = null;
        }
    }
}
