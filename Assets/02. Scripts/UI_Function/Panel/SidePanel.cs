using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SidePanel : BasePanel
{
    [SerializeField] private Image _profileImage;
    [SerializeField] private TextMeshProUGUI _profileText;
    [SerializeField] private Button _logOutButton;
    [SerializeField] private Button _recentPostButton;
    [SerializeField] private Button _unregisterButton;
    [SerializeField] private Button _backButton;

    private bool _isInitialized;
    public override void OnShow(object parameter = null)
    {
        if(!_isInitialized) Initialize();
        base.OnShow(parameter);
        // TODO : _profileText.text = string.받아온 이름; 으로 설정하기

    }

    private void Initialize()
    {
        _backButton.onClick.AddListener(() => UIManager.Instance.ClosePanel());
        _logOutButton.onClick.AddListener(() => UIManager.Instance.OpenPanel("Login"));
        _recentPostButton.onClick.AddListener(() => UIManager.Instance.OpenPanel("RecentPost"));
    }

    private void OnUnregisterClicked()
    {
        // 회원탈퇴기능
    }
}
