using System;
using UnityEngine;

public static class PlacementEvents
{
    // 배치/삭제 시 UI에 알림
    public static Action<GameObject> OnObjectPlaced;
    public static Action<GameObject> OnObjectDeleted;

    // 선택 변경 알림 (UI↔로직 공용)
    public static Action<GameObject> OnObjectSelectedChanged;

    // UI → 로직(혹은 우리 쪽 선택 삭제 요청)
    public static Action OnDeleteSelectedRequested;

    // UI → 로직(Undo/Redo)
    public static Action OnUndoRequested;
    public static Action OnRedoRequested;
}
