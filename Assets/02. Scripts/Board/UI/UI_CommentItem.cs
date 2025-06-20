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
        CreatedAtText.text = comment.CreatedAt.ToString();
        ContentText.text = comment.Content;

        _currentComment = comment;
    }
}