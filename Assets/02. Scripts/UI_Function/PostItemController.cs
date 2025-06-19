using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PostItemController : MonoBehaviour
{
    private TextMeshProUGUI _authorNameText;
    private TextMeshProUGUI _timeInfoText;
    private TextMeshProUGUI _contentText;
    private TextMeshProUGUI _likeCommentCountText;
    private Button _commentButton;
    private Button _editPanelButton;
    private Button _toPostButton;
    private bool _inited;

    private void InitIfNeeded()
    {
        if (_inited) return;
        var root = transform;
        _authorNameText = root.Find("Root_Base/Text_AuthorName").GetComponent<TextMeshProUGUI>();
        _timeInfoText = root.Find("Root_Base/Text_TimeInfo").GetComponent<TextMeshProUGUI>();
        _contentText = root.Find("Panel_BodyContainer/Text_Content").GetComponent<TextMeshProUGUI>();
        _likeCommentCountText = root.Find("Panel_BodyContainer/Text_LikeAndCommentCount").GetComponent<TextMeshProUGUI>();

        _commentButton = root.Find("Panel_BodyContainer/Panel_LikeAndComment/Button_Comment").GetComponent<Button>();
        _editPanelButton = root.Find("Root_Base/Button_EditPanelOpen").GetComponent<Button>();
        _toPostButton = root.Find("Root_Base/Button_ToPostPanel").GetComponent<Button>();

        _inited = true;
    }

    /// <summary>
    /// 전달된 데이터를 텍스트에 바인딩하고, 버튼 리스너를 연결합니다.
    /// </summary>
    public void Setup(PostData data)
    {
        InitIfNeeded();

        _authorNameText.text = data.AuthorName;
        _timeInfoText.text = data.TimeInfo;
        _contentText.text = data.Content;
        _likeCommentCountText.text = $"{data.LikeCount}♥  {data.CommentCount}💬";

        // 기존 리스너 클리어
        _commentButton.onClick.RemoveAllListeners();
        _editPanelButton.onClick.RemoveAllListeners();
        _toPostButton.onClick.RemoveAllListeners();

        // 댓글 버튼 → 상세 Post 패널 오픈 (PostId 전달)
        _commentButton.onClick.AddListener(() =>
          UIManager.Instance.OpenPanel("Panel_Post", data.PostId));

        // 편집 팝업 토글
        _editPanelButton.onClick.AddListener(() =>
        {
            var popup = transform.Find("Root_Base/Panel_EditPanelPopUP");
            if (popup != null)
                popup.gameObject.SetActive(!popup.gameObject.activeSelf);
        });

        // 본문으로 이동
        _toPostButton.onClick.AddListener(() =>
          UIManager.Instance.OpenPanel("Panel_Post", data.PostId));
    }
}
