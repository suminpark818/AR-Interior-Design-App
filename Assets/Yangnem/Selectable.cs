using UnityEngine;

public class Selectable : MonoBehaviour
{
    private Outline outline;

    void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.enabled = false;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
    }

    public void ToggleHighlight()
    {
        outline.enabled = !outline.enabled;
    }
}
