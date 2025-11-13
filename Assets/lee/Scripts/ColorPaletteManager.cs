using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
// UI Outline 별칭
using UIOutline = UnityEngine.UI.Outline;

public class ColorPaletteManager : MonoBehaviour
{
    [SerializeField] private List<Button> colorButtons;
    [SerializeField] private ColorApplicator applicator;

    private Button selectedButton;

    public void SelectColorFromButton(Button button)
    {
        // 색 적용
        var img = button.GetComponent<Image>();
        if (img) applicator?.ApplyColor(img.color);

        // 이전 버튼 Outline 끄기
        if (selectedButton && selectedButton != button)
        {
            var prev = selectedButton.GetComponent<UIOutline>();
            if (prev) prev.enabled = false;
        }

        // 현재 버튼 Outline 켜기 (없으면 추가)
        selectedButton = button;
        var ol = button.GetComponent<UIOutline>();
        if (!ol) ol = button.gameObject.AddComponent<UIOutline>();
        ol.enabled = true;
    }
}
