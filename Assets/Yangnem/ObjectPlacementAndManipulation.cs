using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

[RequireComponent(typeof(ARRaycastManager))]
public class ObjectPlacementAndManipulation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera arCamera;
    [SerializeField] private ColorApplicator colorApplicator;

    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new();
    private GameObject selectedObject;

    private Vector2 prevTouchPos1, prevTouchPos2;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        if (arCamera == null)
            arCamera = Camera.main;
    }

    void Update()
    {
        // 선택된 오브젝트 조작만 처리
        if (Input.touchCount > 0 && selectedObject != null)
            HandleManipulation();
    }

    // ✅ 스크롤뷰 버튼에서 호출할 함수
    public void PlaceObject(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("No prefab assigned!");
            return;
        }

        Pose placePose;
        bool foundPlane = false;

        // 화면 중앙 기준으로 평면 탐색
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        if (raycastManager != null && raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            placePose = hits[0].pose;
            foundPlane = true;
        }
        else
        {
            // 평면 없으면 카메라 앞 1m에 임시 배치
            placePose = new Pose(
                arCamera.transform.position + arCamera.transform.forward * 1.0f,
                Quaternion.LookRotation(arCamera.transform.forward)
            );
            foundPlane = false;
        }

        GameObject newObj = Instantiate(prefab, placePose.position, placePose.rotation);
        newObj.tag = "Furniture";

        if (newObj.GetComponent<Collider>() == null)
            newObj.AddComponent<BoxCollider>();
        if (newObj.GetComponent<Selectable>() == null)
            newObj.AddComponent<Selectable>();

        SelectObject(newObj);

        if (foundPlane)
            Debug.Log($"[AR] {prefab.name} placed on plane at {placePose.position}");
        else
            Debug.LogWarning($"[AR] No plane found — placed {prefab.name} in front of camera");
    }

    private void HandleManipulation()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved &&
                raycastManager.Raycast(t.position, hits, TrackableType.PlaneWithinPolygon))
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

            float prevDist = (prevTouchPos1 - prevTouchPos2).magnitude;
            float currDist = (t0.position - t1.position).magnitude;
            float scaleFactor = currDist / prevDist;
            selectedObject.transform.localScale *= scaleFactor;

            float prevAngle = Mathf.Atan2(prevTouchPos1.y - prevTouchPos2.y, prevTouchPos1.x - prevTouchPos2.x) * Mathf.Rad2Deg;
            float currAngle = Mathf.Atan2(t0.position.y - t1.position.y, t0.position.x - t1.position.x) * Mathf.Rad2Deg;
            selectedObject.transform.Rotate(Vector3.up, currAngle - prevAngle);

            prevTouchPos1 = t0.position;
            prevTouchPos2 = t1.position;
        }
    }

    private void SelectObject(GameObject obj)
    {
        // 기존 선택 해제
        if (selectedObject != null && selectedObject != obj)
        {
            var prevSel = selectedObject.GetComponent<Selectable>();
            if (prevSel != null)
                prevSel.SetHighlight(false);
        }

        selectedObject = obj;

        // Outline 표시
        var selectable = obj.GetComponent<Selectable>();
        if (selectable != null)
            selectable.SetHighlight(true);

        colorApplicator?.SetTarget(selectedObject);
    }
}
