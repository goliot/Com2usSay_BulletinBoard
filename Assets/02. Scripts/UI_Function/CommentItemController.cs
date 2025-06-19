using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Panel_CommentItem 프리팹에 붙여서,
/// 하위의 Text와 Button을 자동으로 Find 후 바인딩하고,
/// Setup() 호출로 데이터를 채워줍니다.
/// </summary>
public class CommentItemController : MonoBehaviour
{
    // 자동 할당될 하위 컴포넌트들
    private TextMeshProUGUI _authorNameText;
    private TextMeshProUGUI _timeInfoText;
    private TextMeshProUGUI _contentText;
    private Button _replyButton;

    private bool _initialized;

    private void Initialize()
    {
        if (_initialized) return;
        var root = transform;

        // 작성자
        _authorNameText = root.Find("Panel_CommentAuthorInfo/Text_AuthorName")
                              .GetComponent<TextMeshProUGUI>();
        // 시간
        _timeInfoText = root.Find("Panel_CommentAuthorInfo/Text_TimeInfo")
                              .GetComponent<TextMeshProUGUI>();
        // 내용
        _contentText = root.Find("Text_Comment")
                              .GetComponent<TextMeshProUGUI>();
        // (선택) 답글 버튼
        var replyTrans = root.Find("Button_Reply");
        if (replyTrans != null)
            _replyButton = replyTrans.GetComponent<Button>();

        _initialized = true;
    }

    /// <summary>
    /// 외부에서 데이터를 넘겨 주고, 버튼 리스너도 여기서 등록합니다.
    /// </summary>
    public void Setup(CommentData data)
    {
        Initialize();

        // 텍스트 채우기
        _authorNameText.text = data.AuthorName;
        _timeInfoText.text = data.TimeInfo;
        _contentText.text = data.Content;

        // 답글 버튼
        if (_replyButton != null)
        {
            _replyButton.onClick.RemoveAllListeners();
            _replyButton.onClick.AddListener(() =>
            {
                // TODO: 답글 기능 구현
                Debug.Log($"Reply to comment {data.CommentId}");
            });
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Apply Dummy Data")]
    private void ApplyDummy()
    {
        var dummy = new CommentData
        {
            CommentId = "dummy",
            AuthorName = "테스트 유저",
            TimeInfo = "방금 전",
            Content = "이것은 더미 댓글 내용입니다."
        };
        Setup(dummy);
    }
#endif
}

/// <summary>
/// 댓글 하나의 데이터를 담는 DTO.
/// 필요에 따라 필드를 추가하세요.
/// </summary>
public struct CommentData
{
    public string CommentId;
    public string AuthorName;
    public string TimeInfo;
    public string Content;
}
