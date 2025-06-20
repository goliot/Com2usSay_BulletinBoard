using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecentPostPanel : BasePanel
{
    [Header("Top Bar (Assign in Inspector)")]
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _orderButton;              // 정렬 버튼

    [Header("Recent Posts (Assign in Inspector)")]
    [SerializeField] private Transform _contentParent;        // Scroll View/Content
    [SerializeField] private GameObject _recentPostPrefab;     // RecentPostItemController가 붙은 프리팹

    private readonly List<GameObject> _spawnedItems = new List<GameObject>();
    private bool _isInitialized;

    public override void OnShow(object parameter = null)
    {
        if (!_isInitialized) Initialize();
        base.OnShow(parameter);
        RefreshList();
    }

    private void Initialize()
    {
        if (_backButton == null) Debug.LogError("[RecentPostPanel] _backButton 누락");
        if (_orderButton == null) Debug.LogError("[RecentPostPanel] _orderButton 누락");
        if (_contentParent == null) Debug.LogError("[RecentPostPanel] _contentParent 누락");
        if (_recentPostPrefab == null) Debug.LogError("[RecentPostPanel] _recentPostPrefab 누락");

        _backButton.onClick.AddListener(() => UIManagerFuck.Instance.ClosePanel());
        _orderButton.onClick.AddListener(OnOrderClicked);

        _isInitialized = true;
    }

    private void RefreshList()
    {
        // 기존 아이템 정리
        foreach (var go in _spawnedItems) Destroy(go);
        _spawnedItems.Clear();

        // TODO: 실제 데이터 소스로 대체
        // 임시 더미 리스트
        var dummy = new List<string> { "최근글 A", "최근글 B", "최근글 C" };

        foreach (var text in dummy)
        {
            var go = Instantiate(_recentPostPrefab, _contentParent);
            var ctrl = go.GetComponent<RecentPostItemController>();
            if (ctrl != null)
            {
                ctrl.Setup(text, () =>
                {
                    // Edit 버튼 누르면 해당 게시글 수정으로 이동
                    UIManagerFuck.Instance.OpenPanel("Panel_EditPost", /* postId or text */ text);
                });
            }
            _spawnedItems.Add(go);
        }
    }

    private void OnOrderClicked()
    {
        // TODO: 정렬 로직 (예: _spawnedItems 순서 변경 후 레이아웃 재빌드)
        Debug.Log("[RecentPostPanel] 정렬 버튼 클릭");
    }
}
