using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Panel_WritePost 화면의 UI 로직을 담당합니다.
/// 모든 UI 컴포넌트는 인스펙터에서 수동 할당하세요.
/// </summary>
public class WritePostPanel : BasePanel
{
    [Header("Top Bar (Assign in Inspector)")]
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _commitButton;

    [Header("Content Input (Assign in Inspector)")]
    [SerializeField] private ScrollRect _scrollView;       // 스크롤 뷰 (optional)
    [SerializeField] private TMP_InputField _postContentInput; // InputField_PostContent

    [Header("Bottom Bar (Assign in Inspector)")]
    [SerializeField] private Button _imageInsertButton;

    private bool _isInitialized;

    public override void OnShow(object parameter = null)
    {
        if (!_isInitialized) Initialize();
        base.OnShow(parameter);

        _postContentInput.text = string.Empty; // 초기 텍스트
        _scrollView.verticalNormalizedPosition = 1f;
    }

    private void Initialize()
    {
        // 인스펙터 할당 검증
        if (_backButton == null) Debug.LogError("[EditPostPanel] _backButton 누락");
        if (_commitButton == null) Debug.LogError("[EditPostPanel] _commitButton 누락");
        if (_postContentInput == null) Debug.LogError("[EditPostPanel] _postContentInput 누락");
        if (_imageInsertButton == null) Debug.LogError("[EditPostPanel] _imageInsertButton 누락");

        _backButton.onClick.AddListener(() => { UIManagerFuck.Instance.ClosePanel(); });

        _commitButton.onClick.AddListener(OnCommitClicked);

        _imageInsertButton.onClick.AddListener(() => { Debug.Log("Image Insert clicked"); });

        _isInitialized = true;
    }

    private void OnCommitClicked()
    {
        var content = _postContentInput.text.Trim();
        if (string.IsNullOrEmpty(content))
        {
            Debug.LogWarning("[EditPostPanel] 내용이 비어 있습니다.");
            return;
        }

        // TODO: 게시글 저장 API 호출 (Firebase 등)
        Debug.Log($"Saving post: {content}");

        // 저장 후 기본 게시판으로 돌아가기
        UIManagerFuck.Instance.ShowDefaultPanel();
    }
}
