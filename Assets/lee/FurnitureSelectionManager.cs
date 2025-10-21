using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FurnitureButton
{
    public Button button;
    public GameObject prefab;
}

public class FurnitureSelectionManager : MonoBehaviour
{
    [Header("?? ??? ?? (XR Origin ???)")]
    public ObjectPlacementAndManipulation placementSystem;

    [Header("??? ??? ??")]
    public FurnitureButton[] furnitureButtons;

    private void Start()
    {
        foreach (var fb in furnitureButtons)
        {
            if (fb.button != null && fb.prefab != null)
                fb.button.onClick.AddListener(() => OnFurnitureButtonClicked(fb.prefab));
        }
    }

    private void OnFurnitureButtonClicked(GameObject prefab)
    {
        if (placementSystem == null)
        {
            Debug.LogError("[FurnitureSelectionManager] placementSystem? ???? ?????!");
            return;
        }

        if (prefab == null)
        {
            Debug.LogError("[FurnitureSelectionManager] ??? ???? ????!");
            return;
        }

        Debug.Log($"[UI] {prefab.name} ??? ? ?? ??");
        placementSystem.PlaceObject(prefab);
    }
}
