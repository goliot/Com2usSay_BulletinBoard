using System;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UI_PostItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _autherName;
    [SerializeField] private TextMeshProUGUI _timeInfo;
    [SerializeField] private TextMeshProUGUI _content;
    [SerializeField] private TextMeshProUGUI _likeAndComment;
    [SerializeField] private GameObject _editOpenPanel;

    [SerializeField] private GameObject _fullHeartImage;
    public event Action OnChanged;

    private Post _post;

    public async void Initialize(Post post)
    {
        _post = post;

        _autherName.text = await AccountManager.Instance.GetUserNicknameWithEmail(post.AuthorId);
        _timeInfo.text = post.CreatedAt.ToDateTime().ToString("yyyy년 M월 d일 tt HH:mm", new System.Globalization.CultureInfo("ko-KR"));
        _content.text = post.Content;
        if (post.LikeCount == 0 && post.CommentCount == 0)
        {
            _likeAndComment.text = "진짜 없어서 그럼";
        }
        else if (post.LikeCount == 0)
        {
            _likeAndComment.text = $"댓글 {post.CommentCount}개";
        }
        else if (post.CommentCount == 0)
        {
            _likeAndComment.text = $"좋아요 {post.LikeCount}개";
        }
        else
        {
            _likeAndComment.text = $"좋아요 {post.LikeCount}개, 댓글 {post.CommentCount}개";
        }

        _fullHeartImage.SetActive(LikeManager.Instance.IsLikedByMe(_post));
    }

    public void OnEditOpenPanelClicked()
    {
        _editOpenPanel.SetActive(true);
    }

    public void OnItemClicked()
    {
        if (_post == null)
        {
            Debug.LogWarning("Post 정보가 없습니다.");
            return;
        }

        UIManager.Instance.OpenPostPanel(_post);
    }


    public async void OnDeleteButtonClicked()
    {
        UIManager.Instance.ShowLoading(true);
        await PostManager.Instance.DeletePost(_post.PostId);
        UIManager.Instance.ShowLoading(false);
        OnChanged?.Invoke();
    }

    public async void OnLikeButtonClicked()
    {
        //UIManager.Instance.ShowLoading(true);
        //await LikeManager.Instance.ToggleLike(_post);
        //UIManager.Instance.ShowLoading(false);
        //OnChanged?.Invoke();

        _fullHeartImage.SetActive(await LikeManager.Instance.ToggleLike(_post));
    }

    public void OnEditButtonClicked()
    {
        if (_post.AuthorId != AccountManager.Instance.MyAccount.Email)
        {
            Debug.LogWarning("작성자만 수정할 수 있습니다.");
            return;
        }
        PostManager.Instance.SetCurrentPost(_post);
        UIManager.Instance.OpenPanel(EUIPanelType.EditPost);


    }
}