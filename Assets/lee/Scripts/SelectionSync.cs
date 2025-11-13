// SelectionSync.cs (¾ÀÀÇ ºó ¿ÀºêÁ§Æ®¿¡ ºÎÂø)
using UnityEngine;

public class SelectionSync : MonoBehaviour
{
    [SerializeField] private ColorApplicator colorApplicator;

    private GameObject current;
    void Awake()
    {
        if (!colorApplicator) colorApplicator = FindObjectOfType<ColorApplicator>();
        PlacementEvents.OnObjectSelectedChanged += OnSel;
    }
    void OnDestroy() => PlacementEvents.OnObjectSelectedChanged -= OnSel;

    void OnSel(GameObject go)
    {
        // 1) ÆÈ·¹Æ® Å¸±ê º¯°æ
        if (colorApplicator) colorApplicator.SetTarget(go);

    }


}
