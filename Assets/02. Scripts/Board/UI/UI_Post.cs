using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Post : UI_PopUp
{
    public Post CurrentPost => PostManager.Instance.CurrentPost;

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
        SetPost();
    }

    public override void Show() { base.Show(); }
    public override void Hide() { base.Hide(); }

    private async void InitComments()
    {
        RefreshComments();
        List<CommentDTO> comments = await CommentManager.Instance.GetComments(CurrentPost.ToDto());
        foreach(var comment in comments)
        {
            AddCommentItemUI(comment);
        }
    }

    private async void InitLike()
    {
        var likeDto = await LikeManager.Instance.LoadLikeData(CurrentPost.ToDto());
        CurrentPost.SetLike(likeDto.ToEntity()); // 🔥 최신 데이터로 반영

        bool isLiked = likeDto.LikedUserIds.Contains(AccountManager.Instance.MyAccount.Email);
        _fullHeartImage.SetActive(isLiked);

        LikeCountText.text = likeDto.LikeCount.ToString(); // 초기 진입 시 카운트 반영
    }



    public void OnClickToBoardButton()
    {
        UIManager.Instance.OpenPanel(EUIPanelType.BulletinBoard);
    }

    public async void OnClickLikeButton()
    {
        bool flag = await LikeManager.Instance.ToggleLike(CurrentPost);

        _fullHeartImage.SetActive(flag);
        LikeCountText.text = CurrentPost.LikeCount.ToString(); // 최신 LikeCount 반영
    }


    public async void OnClickAddCommentButton()
    {
        Comment comment = new Comment(AccountManager.Instance.MyAccount.Email, CommentInputField.text);
        await CommentManager.Instance.AddComment(CurrentPost, comment.ToDto());
        CommentInputField.text = string.Empty;

        AddCommentItemUI(comment.ToDto());
    }

    public void SetPost()
    {
        TitleText.text = CurrentPost.Title;
        AuthorIdText.text = CurrentPost.AuthorId;
        CreatedAtText.text = CurrentPost.CreatedAt.ToDateTime().ToString("yyyy년 M월 d일 tt HH:mm", new System.Globalization.CultureInfo("ko-KR"));
        ContentText.text = CurrentPost.Content;
        LikeCountText.text = CurrentPost.LikeCount.ToString();
        InitComments();
        InitLike();
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