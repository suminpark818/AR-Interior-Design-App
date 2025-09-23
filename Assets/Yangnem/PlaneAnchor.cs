using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaneAnchor : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;

    private ARRaycastManager raycastManager;
    private ARAnchorManager anchorManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();
    }

    void Update()
    {
        Debug.Log("Touch count: " + Input.touchCount);

        if (Input.touchCount > 0)
{
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch detected at: " + touch.position);

                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    Debug.Log("Plane hit at: " + hitPose.position);

                    var spawnedObj = Instantiate(cubePrefab, hitPose.position, hitPose.rotation);

                    if (spawnedObj.GetComponent<ARAnchor>() == null)
                    {
                        spawnedObj.AddComponent<ARAnchor>();
                    }
                }
                else
                {
                    Debug.Log("Raycast did not hit any plane.");
                }
            }
        }

    }
}
