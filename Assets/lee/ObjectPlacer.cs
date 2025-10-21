using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
//    [SerializeField] private ARRaycastManager arRaycastManager;   // AR ???? ??????
//    [SerializeField] private ARAnchorManager anchorManager;       // Anchor ??????

//    // Inspector?? ?????? ????, ???????? ???????? ?????? ??
//    [SerializeField] private ColorApplicator colorApplicator;

//    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

//    void Awake()
//    {
//        // ?????? ?????? ?? ?? ?????? ????
//        if (colorApplicator == null) colorApplicator = FindObjectOfType<ColorApplicator>();
//    }

//    void Update()
//    {
//#if UNITY_ANDROID || UNITY_IOS
//        // ?????? ???? ????
//        if (Input.touchCount > 0)
//        {
//            Touch touch = Input.GetTouch(0);

//            if (arRaycastManager != null &&
//                arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
//            {
//                Pose hitPose = hits[0].pose;
//                var plane = hits[0].trackable.GetComponent<ARPlane>();

//                // ?????? ????(????)????, ?????? ???????? ???? ???? ????
//                if (plane.alignment == PlaneAlignment.HorizontalUp &&
//                    FurnitureSelectionManager.CurrentFurniturePrefab != null)
//                {
//                    GameObject newObj = Instantiate(
//                        FurnitureSelectionManager.CurrentFurniturePrefab,
//                        hitPose.position, hitPose.rotation);

//                    AttachAnchor(newObj, hits[0]);

//                    // ???? ?????? ?????????? ???? ???????? ????
//                    SetColorTarget(newObj);
//                }
//            }
//        }
//#else
//        // ??????
//        if (Input.GetMouseButtonDown(0))
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//            if (Physics.Raycast(ray, out RaycastHit hit))
//            {
//                if (hit.collider.CompareTag("Ground") &&
//                    FurnitureSelectionManager.CurrentFurniturePrefab != null)
//                {
//                    GameObject newObj = Instantiate(
//                        FurnitureSelectionManager.CurrentFurniturePrefab,
//                        hit.point, Quaternion.identity);

//                    // ???????????? ???????? ???? ????
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
//            // ?????? Anchor ??????
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

//    // ???? ???? ????
//    private void SetColorTarget(GameObject go)
//    {
//        if (colorApplicator == null)
//            colorApplicator = FindObjectOfType<ColorApplicator>();

//        if (colorApplicator != null)
//            colorApplicator.SetTarget(go);
//        else
//            Debug.LogWarning("ColorApplicator?? ???? ????????. ???? ?????????? ??????????.");
//    }
}
