using UnityEngine;

public class Selectable : MonoBehaviour
{
    private Outline outline;

    void Awake()
    {
        outline = gameObject.GetComponent<Outline>();
        if (outline == null)
            outline = gameObject.AddComponent<Outline>();

        outline.enabled = false;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
    }

    public void SetHighlight(bool enable)
    {
        if (outline != null)
            outline.enabled = enable;
    }

    // ✅ ObjectSelector.cs 호환용 추가
    public void ToggleHighlight()
    {
        if (outline != null)
            outline.enabled = !outline.enabled;
    }
}
