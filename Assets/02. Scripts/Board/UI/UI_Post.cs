using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Post : MonoBehaviour
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

    private List<UI_CommentItem> _commentItems = new List<UI_CommentItem>();

    public async void OnClickLikeButton()
    {
        await LikeManager.Instance.ToggleLike(_currentPost);
    }

    public async void OnClickAddCommentButton()
    {
        Comment comment = new Comment(AccountManager.Instance.MyAccount.Email, CommentInputField.text);
        await CommentManager.Instance.AddComment(_currentPost.ToEntity(), comment.ToDto());
        CommentInputField.text = string.Empty;

        AddComment(comment.ToDto());
    }

    public void SetPost(PostDTO post)
    {
        _currentPost = post;
        TitleText.text = post.Title;
        AuthorIdText.text = post.AuthorId;
        CreatedAtText.text = post.CreatedAt.ToString();
        ContentText.text = post.Content;
        LikeCountText.text = post.LikeCount.ToString();

        ShowComments();
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

    private void AddComment(CommentDTO comment)
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

}