using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColorPaletteManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform paletteContainer; // 버튼들이 있는 부모(ScrollView라면 Content)
    [SerializeField] private List<Button> colorButtons;  // 미리 만든 색상 버튼들

    [Header("Apply Target")]
    [SerializeField] private ColorApplicator applicator;

    [Header("Highlight")]
    [SerializeField] private Color outlineColor = new(1f, 0.95f, 0.2f);
    [SerializeField] private Vector2 outlineThickness = new(5f, -5f);

    private Button selectedButton;

    private void Start()
    {
        // 초기 상태: 모든 테두리 끔
        foreach (var b in colorButtons)
        {
            var o = b.GetComponent<Outline>();
            if (o) { o.effectColor = outlineColor; o.effectDistance = outlineThickness; o.enabled = false; }
        }
    }

    

        public void SelectColorFromButton(Button button)
        {
            Image image = button.GetComponent<Image>();
            if (image == null) return;

            Color c = image.color;
            applicator?.ApplyColor(c);
            Debug.Log("색상 적용: " + c);

            if (selectedButton != null)
            {
                var oldOutline = selectedButton.GetComponent<Outline>();
                if (oldOutline) oldOutline.enabled = false;
            }

            selectedButton = button;

            var outline = button.GetComponent<Outline>();
            if (outline) outline.enabled = true;
        
            var img = button.GetComponent<Image>();

            if (img != null)
            {
                applicator?.ApplyColor(c);
                Debug.Log("색상 적용: " + c);
            }
        }
   
}
