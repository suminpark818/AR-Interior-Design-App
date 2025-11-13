using UnityEngine;
using UnityEngine.EventSystems;

public class TapToSelect : MonoBehaviour
{
    [SerializeField] private ColorApplicator colorApplicator;

    void Awake()
    {
        if (!colorApplicator) colorApplicator = FindObjectOfType<ColorApplicator>();
    }

    void Update()
    {
        // UI 위 클릭은 무시
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount == 0) return;
        var t = Input.GetTouch(0);
        if (t.phase != TouchPhase.Began) return;

        var ray = Camera.main.ScreenPointToRay(t.position);
#else
        if (!Input.GetMouseButtonDown(0)) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif
        if (Physics.Raycast(ray, out var hit))
        {
            // 배치된 프리팹에 공통 태그/레이어가 있으면 더 정확히 필터링
            var go = hit.collider.transform.root.gameObject;

            // 선택 알림 & 색상 타깃 갱신
            PlacementEvents.OnObjectSelectedChanged?.Invoke(go);
            if (colorApplicator) colorApplicator.SetTarget(go);
        }
    }
}
