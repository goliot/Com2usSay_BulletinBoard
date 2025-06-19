using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Panel_RecentPostItem(=프리팹)에 붙여서,
/// 하위의 Text, Button 등을 자동으로 Find 후 바인딩하고,
/// Setup() 호출로 데이터를 넣어 줍니다.
/// </summary>
public class RecentPostItemController : MonoBehaviour
{
    private TextMeshProUGUI _subText;
    private Button _editButton;

    private bool _inited;

    private void Initialize()
    {
        if (_inited) return;
        var root = transform;

        _subText = root.Find("Panel_Content/Panel_Sub/Text_Sub")?.GetComponent<TextMeshProUGUI>();
        _editButton = root.Find("Panel_Content/Button_Edit")?.GetComponent<Button>();

        _inited = true;
    }

    /// <summary>
    /// 데이터와 함께 호출하세요.
    /// </summary>
    public void Setup(string subText, System.Action onEdit)
    {
        Initialize();

        if (_subText != null)
            _subText.text = subText;

        if (_editButton != null)
        {
            _editButton.onClick.RemoveAllListeners();
            _editButton.onClick.AddListener(() => onEdit?.Invoke());
        }
    }
}
