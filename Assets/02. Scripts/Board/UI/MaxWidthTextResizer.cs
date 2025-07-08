using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MaxWidthTextResizer : MonoBehaviour
{
    public float maxWidth = 500f; // 원하는 최대 너비

    private TextMeshProUGUI tmpText;
    private RectTransform rectTransform;
    private LayoutElement layoutElement;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        // LayoutElement 없으면 자동 추가
        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null)
            layoutElement = gameObject.AddComponent<LayoutElement>();
    }

    void Update()
    {
        // 텍스트가 갱신된 뒤 Layout이 반영되도록
        float preferredWidth = tmpText.preferredWidth;

        if (preferredWidth > maxWidth)
        {
            layoutElement.preferredWidth = maxWidth;
        }
        else
        {
            layoutElement.preferredWidth = -1; // 자동 크기
        }
    }
}
