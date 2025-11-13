// SimpleDeleteHandler.cs (씬의 빈 오브젝트에 부착)
using UnityEngine;

public class DeleteHandler : MonoBehaviour
{
    private GameObject selected;

    void OnEnable()
    {
        PlacementEvents.OnObjectSelectedChanged += go => selected = go;
        PlacementEvents.OnDeleteSelectedRequested += OnDelete;
    }
    void OnDisable()
    {
        PlacementEvents.OnObjectSelectedChanged -= go => selected = go;
        PlacementEvents.OnDeleteSelectedRequested -= OnDelete;
    }

    void OnDelete()
    {
        if (!selected) return;
        var toDel = selected;
        selected = null;

        // 1) UI에 먼저 알림
        PlacementEvents.OnObjectDeleted?.Invoke(toDel);

        // 2) 실제 삭제
        Destroy(toDel);
    }
}
