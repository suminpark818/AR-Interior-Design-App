using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FurnitureSelectionManager : MonoBehaviour
{
    public static GameObject CurrentFurniturePrefab;
    [SerializeField] private List<Button> furnitureButtons;

    public void SelectFurniture(GameObject prefab)
    {
        CurrentFurniturePrefab = prefab;
        Debug.Log("선택된 가구: " + prefab.name);

        // 모든 버튼의 Outline 끄기
        foreach (var btn in furnitureButtons)
        {
            var outline = btn.GetComponent<Outline>();
            if (outline != null) outline.enabled = false;
        }

        // 현재 클릭된 버튼 찾기
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        if (clickedButton != null)
        {
            var clickedOutline = clickedButton.GetComponent<Outline>();
            if (clickedOutline != null) clickedOutline.enabled = true;
        }
    }
}
