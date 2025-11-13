//using UnityEngine;
//using UnityEngine.XR.ARFoundation;
//using UnityEngine.XR.ARSubsystems;
//using System.Collections.Generic;
//using UnityEngine.EventSystems; // ?? UI ??????

//public class ObjectPlacer : MonoBehaviour
//{
//    [SerializeField] private ARRaycastManager arRaycastManager;   // AR ???? ??????
//    [SerializeField] private ARAnchorManager anchorManager;       // Anchor ??????
//    [SerializeField] private ColorApplicator colorApplicator;

//    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

//    void Awake()
//    {
//        if (colorApplicator == null)
//            colorApplicator = FindObjectOfType<ColorApplicator>();
//    }

//    void Update()
//    {
//#if UNITY_ANDROID || UNITY_IOS
//        // UI ???????? ???? ????
//        if (Input.touchCount > 0)
//        {
//            // ?? ???? ???? ????
//            Touch touch = Input.GetTouch(0);

//            // UI ???? ?????????? return
//            if (EventSystem.current != null &&
//                EventSystem.current.IsPointerOverGameObject(touch.fingerId))
//                return;

//            if (arRaycastManager != null &&
//                arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
//            {
//                Pose hitPose = hits[0].pose;
//                var plane = hits[0].trackable.GetComponent<ARPlane>();

//                if (plane.alignment == PlaneAlignment.HorizontalUp &&
//                    FurnitureSelectionManager.CurrentFurniturePrefab != null)
//                {
//                    GameObject newObj = Instantiate(
//                        FurnitureSelectionManager.CurrentFurniturePrefab,
//                        hitPose.position, hitPose.rotation);

//                    AttachAnchor(newObj, hits[0]);
//                    PlacementEvents.OnObjectPlaced?.Invoke(newObj);

//                    SetColorTarget(newObj);
//                }
//            }
//        }
//#else
//        // ???????? (?????? ????)
//        if (Input.GetMouseButtonDown(0))
//        {
//            // UI ?? ?????? ????
//            if (EventSystem.current != null &&
//                EventSystem.current.IsPointerOverGameObject())
//                return;

//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//            if (Physics.Raycast(ray, out RaycastHit hit))
//            {
//                if (hit.collider.CompareTag("Ground") &&
//                    FurnitureSelectionManager.CurrentFurniturePrefab != null)
//                {
//                    GameObject newObj = Instantiate(
//                        FurnitureSelectionManager.CurrentFurniturePrefab,
//                        hit.point, Quaternion.identity);

//                    PlacementEvents.OnObjectPlaced?.Invoke(newObj);

//                    SetColorTarget(newObj);
//                }
//            }
//        }
//#endif
//    }

//    private void AttachAnchor(GameObject obj, ARRaycastHit hit)
//    {
//        var plane = hit.trackable as ARPlane;
//        ARAnchor anchor = null;

//        if (plane != null && anchorManager != null)
//        {
//            anchor = anchorManager.AttachAnchor(plane, hit.pose);
//        }

//        if (anchor != null)
//        {
//            obj.transform.SetParent(anchor.transform, worldPositionStays: true);
//        }
//        else
//        {
//            Debug.LogWarning("Anchor ???? ???? ?? ?????? ???? ARAnchor ????");
//            obj.AddComponent<ARAnchor>();
//        }
//    }

//    private void SetColorTarget(GameObject go)
//    {
//        if (colorApplicator == null)
//            colorApplicator = FindObjectOfType<ColorApplicator>();

//        if (colorApplicator != null)
//            colorApplicator.SetTarget(go);
//        else
//            Debug.LogWarning("ColorApplicator?? ???? ????????. ???? ?????????? ??????????.");
//    }
//}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ObjectPlacer : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private Camera arCamera;
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private ARAnchorManager anchorManager;
    [SerializeField] private ColorApplicator colorApplicator;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject selectedObject;
    private Vector2 prevTouchPos1, prevTouchPos2;

    void Awake()
    {
        if (arRaycastManager == null)
            arRaycastManager = GetComponent<ARRaycastManager>();
        if (colorApplicator == null)
            colorApplicator = FindObjectOfType<ColorApplicator>();
    }

    void Update()
    {
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);

        if (IsTouchOverUI(touch.fingerId))
            return;

        // 가구 선택
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Furniture"))
                {
                    SelectObject(hit.collider.gameObject);
                    return;
                }
                else
                {
                    // 빈 공간 클릭 → 선택 해제
                    DeselectObject();
                }
            }
            else
            {
                // 완전 빈 공간 → 선택 해제
                DeselectObject();
            }
        }

        // 선택된 오브젝트 조작
        if (selectedObject != null)
        {
            HandleManipulation();
            return;
        }

        // 새 오브젝트 배치
        if (touch.phase == TouchPhase.Began && FurnitureSelectionManager.CurrentFurniturePrefab != null)
        {
            if (arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                GameObject newObj = Instantiate(FurnitureSelectionManager.CurrentFurniturePrefab, hitPose.position, hitPose.rotation);
                newObj.tag = "Furniture";
                AttachAnchor(newObj, hits[0]);
                PlacementEvents.OnObjectPlaced?.Invoke(newObj);
                Debug.Log($"[AR] Object placed: {newObj.name}");
                SetColorTarget(newObj);
                SelectObject(newObj);
            }
        }
    }

    bool IsTouchOverUI(int fingerId)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.GetTouch(fingerId).position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // UI ??? ?? ????? ?? ?? (?: RightPanel ?)
        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("IgnoreUI") || result.gameObject.name.Contains("RightPanel"))
                continue;
            return true; // ?? UI ??
        }

        return false;
    }

    // -----------------------------
    // ?? / ?? / ??? ??
    // -----------------------------
    void HandleManipulation()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved &&
                arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                selectedObject.transform.position = hits[0].pose.position;
            }
        }
        else if (Input.touchCount == 2)
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
                float prevDist = (prevTouchPos1 - prevTouchPos2).magnitude;
                float currDist = (t0.position - t1.position).magnitude;
                if (!Mathf.Approximately(prevDist, 0))
                {
                    float scaleFactor = currDist / prevDist;
                    selectedObject.transform.localScale *= scaleFactor;
                }

                float prevAngle = Mathf.Atan2(prevTouchPos1.y - prevTouchPos2.y, prevTouchPos1.x - prevTouchPos2.x) * Mathf.Rad2Deg;
                float currAngle = Mathf.Atan2(t0.position.y - t1.position.y, t0.position.x - t1.position.x) * Mathf.Rad2Deg;
                selectedObject.transform.Rotate(Vector3.up, currAngle - prevAngle);

                prevTouchPos1 = t0.position;
                prevTouchPos2 = t1.position;
            }
        }
    }

    // -----------------------------
    // ?? ??
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
        if (outline != null)
            outline.enabled = enable;
    }

    // -----------------------------
    // ?? ??
    // -----------------------------
    void AttachAnchor(GameObject obj, ARRaycastHit hit)
    {
        var plane = hit.trackable as ARPlane;
        ARAnchor anchor = anchorManager != null ? anchorManager.AttachAnchor(plane, hit.pose) : null;

        if (anchor != null)
            obj.transform.SetParent(anchor.transform, true);
        else
        {
            obj.AddComponent<ARAnchor>();
            Debug.LogWarning("Anchor attach ?? ? ?? ARAnchor ???");
        }
    }
    void DeselectObject()
    {
        if (selectedObject != null)
        {
            SetOutline(selectedObject, false);
            selectedObject = null;
            Debug.Log("[AR] Object deselected.");
        }
    }

    void SetColorTarget(GameObject go)
    {
        if (colorApplicator == null)
            colorApplicator = FindObjectOfType<ColorApplicator>();

        if (colorApplicator != null)
            colorApplicator.SetTarget(go);
    }
}
