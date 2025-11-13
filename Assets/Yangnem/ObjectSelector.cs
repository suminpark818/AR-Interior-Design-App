using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private Camera arCamera;

    void Start()
    {
        arCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = arCamera.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var selectable = hit.transform.GetComponent<Selectable>();
                if (selectable != null)
                {
                    selectable.ToggleHighlight();
                }
            }
        }
    }
}
