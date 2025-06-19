using UnityEngine;
using UnityEngine.UI;

public class RecentPostPanel : BasePanel
{
    [Header("UI Element TOP Bar")]
    [SerializeField]
    private Button _backButton;
    private Button _orderButton;

    [Header("Panel_RecentPost")]
    [SerializeField]
    private GameObject _recentPostPrefab;

    private bool _isInitialized;

    public override void OnShow(object parameter = null)
    {
        if (!_isInitialized) Initialize();
        base.OnShow(parameter);
    }

    private void Initialize()
    {
        _backButton.onClick.AddListener(() => UIManager.Instance.ClosePanel());
    }

    private void OnCommitClicked()
    {
        // 
    }

}
