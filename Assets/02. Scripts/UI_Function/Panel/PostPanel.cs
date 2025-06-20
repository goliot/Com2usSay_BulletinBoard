using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PostPanel : BasePanel
{
    [Header("Post Content (Assign in Inspector)")]
    [SerializeField] private TextMeshProUGUI authorNameText;
    [SerializeField] private TextMeshProUGUI timeInfoText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI likeCountText;
    [SerializeField] private Button likeButton;

    [Header("Top Bar (Assign in Inspector)")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button editButton;

    [Header("Comments (Assign in Inspector)")]
    [SerializeField] private Transform commentListContent;
    [SerializeField] private GameObject commentItemPrefab;

    [Header("Input (Assign in Inspector)")]
    [SerializeField] private TMP_InputField commentInputField;
    [SerializeField] private Button commentSubmitButton;

    private readonly List<GameObject> _spawnedComments = new List<GameObject>();
    private bool _isInitialized;
    private string _currentPostId;

    public override void OnShow(object parameter = null)
    {
        if (!_isInitialized) Initialize();
        base.OnShow(parameter);

        _currentPostId = parameter as string;
        LoadAndDisplayPost(_currentPostId);
    }

    public override void OnHide()
    {
        base.OnHide();
        ClearComments();
    }

    private void Initialize()
    {
        // Inspector 할당 검증
        if (authorNameText == null) Debug.LogError("[PostPanel] authorNameText 누락");
        if (timeInfoText == null) Debug.LogError("[PostPanel] timeInfoText 누락");
        if (contentText == null) Debug.LogError("[PostPanel] contentText 누락");
        if (likeCountText == null) Debug.LogError("[PostPanel] likeCountText 누락");
        if (likeButton == null) Debug.LogError("[PostPanel] likeButton 누락");
        if (backButton == null) Debug.LogError("[PostPanel] _backButton 누락");
        if (editButton == null) Debug.LogError("[PostPanel] editButton 누락");
        if (commentListContent == null) Debug.LogError("[PostPanel] commentListContent 누락");
        if (commentItemPrefab == null) Debug.LogError("[PostPanel] commentItemPrefab 누락");
        if (commentInputField == null) Debug.LogError("[PostPanel] commentInputField 누락");
        if (commentSubmitButton == null) Debug.LogError("[PostPanel] commentSubmitButton 누락");

        // 버튼 리스너
        backButton.onClick.AddListener(() => UIManager.Instance.ClosePanel());
        editButton.onClick.AddListener(() => UIManager.Instance.OpenPanel("Panel_EditPost", _currentPostId));
        likeButton.onClick.AddListener(OnLikeClicked);
        commentSubmitButton.onClick.AddListener(OnCommentSubmitClicked);

        _isInitialized = true;
    }

    private void LoadAndDisplayPost(string postId)
    {
        // TODO: postId로 실제 데이터 로드


        //// 본문 바인딩
        //authorNameText.text = data.AuthorName;
        //timeInfoText.text = data.TimeInfo;
        //contentText.text = data.Content;
        //likeCountText.text = data.LikeCount.ToString();

        // 댓글 리셋
        ClearComments();
        // (실제 로드는 TODO)
    }

    private void OnLikeClicked()
    {
        // TODO: 좋아요 API 호출
        if (int.TryParse(likeCountText.text, out var c))
            likeCountText.text = (++c).ToString();
    }

    private void OnCommentSubmitClicked()
    {
        var txt = commentInputField.text.Trim();
        if (string.IsNullOrEmpty(txt)) return;

        // TODO: 댓글 저장 API 호출

        // 즉시 UI에 추가
        var comment = new CommentData
        {
            CommentId = System.Guid.NewGuid().ToString(),
            AuthorName = "나",
            TimeInfo = "방금 전",
            Content = txt
        };
        AddSingleComment(comment);

        commentInputField.text = "";
    }

    private void AddSingleComment(CommentData data)
    {
        var go = Instantiate(commentItemPrefab, commentListContent);
        var ctrl = go.GetComponent<CommentItemController>();
        if (ctrl != null) ctrl.Setup(data);
        _spawnedComments.Add(go);
    }

    private void ClearComments()
    {
        foreach (var go in _spawnedComments)
            Destroy(go);
        _spawnedComments.Clear();
    }

#if UNITY_EDITOR
    /// <summary>
    /// Inspector에서 우클릭 → Create Dummy Comments를 선택하면
    /// 더미 댓글 5개를 자동으로 생성해 줍니다.
    /// </summary>
    [ContextMenu("Create Dummy Comments")]
    private void CreateDummyComments()
    {
        ClearComments();
        for (int i = 1; i <= 5; i++)
        {
            var dummy = new CommentData
            {
                CommentId = $"dummy{i}",
                AuthorName = $"User{i}",
                TimeInfo = $"{i}분 전",
                Content = $"이것은 더미 댓글 #{i} 입니다."
            };
            AddSingleComment(dummy);
        }
    }
#endif
}
