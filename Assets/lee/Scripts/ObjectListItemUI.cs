using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ObjectListItemUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Button selectBtn;
    [SerializeField] private Button deleteBtn;
    [SerializeField] private Image highlight;

    [Header("Label Color Settings")]
    [SerializeField] private Color normalLabelColor = Color.white;
    [SerializeField] private Color selectedLabelColor = new(1f, 0.95f, 0.5f);
    [Range(0f, 0.5f)]
    [SerializeField] private float colorFadeDuration = 0.15f;

    private GameObject target;
    private ObjectListUIManager owner;
    private Coroutine colorRoutine;

    public void Initialize(GameObject obj, string display, ObjectListUIManager owner)
    {
        this.target = obj;
        this.owner = owner;

        if (label)
        {
            label.text = display;
            label.color = normalLabelColor; // 초기 색상
        }

        if (selectBtn) selectBtn.onClick.AddListener(() =>
        {
            if (target) owner.SelectFromUI(target);
        });

        if (deleteBtn) deleteBtn.onClick.AddListener(() =>
        {
            if (target)
            {
                owner.SelectFromUI(target); // 먼저 선택 고정
                PlacementEvents.OnDeleteSelectedRequested?.Invoke();
            }
        });
    }

    public void SetSelected(bool on)
    {
        if (highlight) highlight.enabled = on;

        if (label)
        {
            // 부드러운 색 전환
            if (colorRoutine != null) StopCoroutine(colorRoutine);
            colorRoutine = StartCoroutine(LerpLabelColor(
                label,
                on ? selectedLabelColor : normalLabelColor,
                colorFadeDuration
            ));

            // (옵션) 볼드 효과를 주고 싶다면 주석 해제
            // label.fontStyle = on ? FontStyles.Bold : FontStyles.Normal;
        }
    }

    private IEnumerator LerpLabelColor(TextMeshProUGUI text, Color targetColor, float duration)
    {
        Color startColor = text.color;
        if (duration <= 0f)
        {
            text.color = targetColor;
            yield break;
        }

        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            text.color = Color.Lerp(startColor, targetColor, time / duration);
            yield return null;
        }

        text.color = targetColor;
    }
}
