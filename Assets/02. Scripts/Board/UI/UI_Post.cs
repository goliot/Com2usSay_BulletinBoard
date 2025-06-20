using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Post : UI_PopUp
{
    private PostDTO _currentPost;
    public PostDTO CurrentPost => _currentPost;

    [Header("# Comment")]
    [SerializeField] private Transform CommentParent;
    [SerializeField] private GameObject UI_CommentItem;

    [Header("# Post")]
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI AuthorIdText;
    [SerializeField] private TextMeshProUGUI CreatedAtText;
    [SerializeField] private TextMeshProUGUI ContentText;
    [SerializeField] private TextMeshProUGUI LikeCountText;

    [Header("# New Comment")]
    [SerializeField] private TMP_InputField CommentInputField; // 작성하는곳

    [Header("# Like")]
    [SerializeField] private GameObject _fullHeartImage;

    private List<UI_CommentItem> _commentItems = new List<UI_CommentItem>();

    private void OnEnable()
    {
        if(_currentPost == null)
        {
            //Post post = new Post("1", TitleText.text, ContentText.text, AuthorIdText.text);
            //_currentPost = post.ToDto();

            return;
        }
        //SetPost(_currentPost);
        //InitComments();
        //InitLike();
    }

    public override void Show() { base.Show(); }
    public override void Hide() { base.Hide(); }

    private async void InitComments()
    {
        RefreshComments();
        List<CommentDTO> comments = await CommentManager.Instance.GetComments(_currentPost);
        foreach(var comment in comments)
        {
            AddCommentItemUI(comment);
        }
    }

    private void InitLike()
    {
        bool flag = LikeManager.Instance.IsLikedByMe(_currentPost.ToEntity());
        _fullHeartImage.SetActive(flag);
    }

    public void OnClickToBoardButton()
    {
        UIManager.Instance.OpenPanel(EUIPanelType.BulletinBoard);
    }

    public async void OnClickLikeButton()
    {
        bool flag = await LikeManager.Instance.ToggleLike(_currentPost);

        _fullHeartImage.SetActive(flag);

        LikeCountText.text = _currentPost.LikeCount.ToString();
    }

    public async void OnClickAddCommentButton()
    {
        Comment comment = new Comment(AccountManager.Instance.MyAccount.Email, CommentInputField.text);
        await CommentManager.Instance.AddComment(_currentPost.ToEntity(), comment.ToDto());
        CommentInputField.text = string.Empty;

        AddCommentItemUI(comment.ToDto());
    }

    public void SetPost(PostDTO post)
    {
        _currentPost = post;
        TitleText.text = post.Title;
        AuthorIdText.text = post.AuthorId;
        CreatedAtText.text = post.CreatedAt.ToDateTime().ToString("yyyy년 M월 d일 tt HH:mm", new System.Globalization.CultureInfo("ko-KR"));
        ContentText.text = post.Content;
        LikeCountText.text = post.LikeCount.ToString();
        InitComments();
        InitLike();
        //ShowComments();
    }

    private void ShowComments()
    {
        List<Comment> comments = _currentPost.CommentList;

        foreach(var comment in comments)
        {
            UI_CommentItem item = Instantiate(UI_CommentItem, CommentParent).GetComponent<UI_CommentItem>();
            item.SetComment(comment.ToDto());
            _commentItems.Add(item);
        }
    }

    private void AddCommentItemUI(CommentDTO comment)
    {
        UI_CommentItem item = Instantiate(UI_CommentItem, CommentParent).GetComponent<UI_CommentItem>();
        item.SetComment(comment);
        _commentItems.Add(item);
    }

    public void DeleteComment(CommentDTO comment)
    {
        string targetId = comment.CommentId;

        for (int i = 0; i < _commentItems.Count; i++)
        {
            if (_commentItems[i].CurrentComment.CommentId == targetId)
            {
                Destroy(_commentItems[i].gameObject);
                _commentItems.RemoveAt(i); // ✅ 리스트에서도 제거
                break;
            }
        }
    }

    private void RefreshComments()
    {
        foreach(var item in _commentItems)
        {
            Destroy(item.gameObject);
        }
        _commentItems.Clear();
    }
}