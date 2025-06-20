using TMPro;
using UnityEngine;

public class UI_CommentItem : MonoBehaviour
{
    private CommentDTO _currentComment;
    public CommentDTO CurrentComment => _currentComment;

    [SerializeField] private TextMeshProUGUI AuthorIdText;
    [SerializeField] private TextMeshProUGUI CreatedAtText;
    [SerializeField] private TextMeshProUGUI ContentText;

    public void SetComment(CommentDTO comment)
    {
        AuthorIdText.text = comment.AuthorId;
        CreatedAtText.text = comment.CreatedAt.ToDateTime().ToString("yyyy년 M월 d일 tt HH:mm", new System.Globalization.CultureInfo("ko-KR"));
        ContentText.text = comment.Content;

        _currentComment = comment;
    }
}