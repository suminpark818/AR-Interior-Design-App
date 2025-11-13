using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectListUIManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform content;     // ScrollView/Viewport/Content
    [SerializeField] private GameObject itemPrefab; // ObjectListItem ÇÁ¸®ÆÕ
    [SerializeField] private Button undoBtn;
    [SerializeField] private Button redoBtn;
    [SerializeField] private Button deleteSelectedBtn;

    private readonly Dictionary<GameObject, ObjectListItemUI> map = new();
    private GameObject currentSelected;

    void OnEnable()
    {
        PlacementEvents.OnObjectPlaced += AddObject;
        PlacementEvents.OnObjectDeleted += RemoveObject;
        PlacementEvents.OnObjectSelectedChanged += OnExternalSelection;
    }
    void OnDisable()
    {
        PlacementEvents.OnObjectPlaced -= AddObject;
        PlacementEvents.OnObjectDeleted -= RemoveObject;
        PlacementEvents.OnObjectSelectedChanged -= OnExternalSelection;
    }
    void Start()
    {
        if (undoBtn) undoBtn.onClick.AddListener(() => PlacementEvents.OnUndoRequested?.Invoke());
        if (redoBtn) redoBtn.onClick.AddListener(() => PlacementEvents.OnRedoRequested?.Invoke());
        if (deleteSelectedBtn) deleteSelectedBtn.onClick.AddListener(() => PlacementEvents.OnDeleteSelectedRequested?.Invoke());
    }

    public void AddObject(GameObject obj)
    {
        if (!obj || map.ContainsKey(obj)) return;
        var go = Instantiate(itemPrefab, content);
        var item = go.GetComponent<ObjectListItemUI>();
        item.Initialize(obj, BuildName(obj), this);
        map[obj] = item;
    }

    public void RemoveObject(GameObject obj)
    {
        if (!obj) return;
        if (map.TryGetValue(obj, out var ui))
        {
            Destroy(ui.gameObject);
            map.Remove(obj);
            if (currentSelected == obj) currentSelected = null;
        }
    }

    public void SelectFromUI(GameObject obj)
    {
        currentSelected = obj;
        PlacementEvents.OnObjectSelectedChanged?.Invoke(obj);
        foreach (var kv in map) kv.Value.SetSelected(kv.Key == obj);
    }

    private void OnExternalSelection(GameObject obj)
    {
        currentSelected = obj;
        foreach (var kv in map) kv.Value.SetSelected(kv.Key == obj);
    }

    private string BuildName(GameObject obj)
    {
        string baseName = obj.name.Replace("(Clone)", "").Trim();

        int dup = 0;
        foreach (var k in map.Keys)
            if (k && (k.name.Replace("(Clone)", "").Trim() == baseName)) dup++;

        return dup > 0 ? $"{baseName} #{dup + 1}" : baseName;
    }

}
