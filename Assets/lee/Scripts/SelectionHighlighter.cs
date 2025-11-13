using UnityEngine;

public class SelectionHighlighter : MonoBehaviour
{
    [SerializeField] private Color outlineColor = new(1f, 0.95f, 0.4f);
    [SerializeField, Range(1f, 10f)] private float outlineWidth = 6f;

    private GameObject selected;
    private Outline lastOutline;  // 현재 선택된 Outline 캐싱

    void OnEnable() => PlacementEvents.OnObjectSelectedChanged += OnSelectedChanged;
    void OnDisable() => PlacementEvents.OnObjectSelectedChanged -= OnSelectedChanged;

    private void OnSelectedChanged(GameObject go)
    {
        // 이전 오브젝트 Outline 끄기
        if (lastOutline)
            lastOutline.enabled = false;

        selected = go;

        if (!selected) return;

        // 새 오브젝트의 Outline 가져오기
        Outline outline = selected.GetComponentInChildren<Outline>(true);

        if (outline)
        {
            outline.OutlineColor = outlineColor;
            outline.OutlineWidth = outlineWidth;
            outline.enabled = true;
            lastOutline = outline;
        }
        else
        {
            Debug.LogWarning($"선택된 오브젝트 '{selected.name}'에 Outline 컴포넌트가 없어요!");
        }
    }
}
