using Firebase.Firestore;
using System;

[FirestoreData]
public class Comment
{
    [FirestoreProperty] public string CommentId { get; set; }
    [FirestoreProperty] public string AuthorId { get; set; }
    [FirestoreProperty] public string Content { get; set; }
    [FirestoreProperty] public Timestamp CreatedAt { get; set; }

    // 기본 생성자 필요 (Firestore용)
    public Comment() { }

    public Comment(string authorId, string content)
    {
        CommentSpecification spec = new CommentSpecification();
        if (!spec.IsSatisfiedBy(authorId))
        {
            throw new Exception($"{nameof(authorId)} {spec.ErrorMessage}");
        }
        if (!spec.IsSatisfiedBy(content))
        {
            throw new Exception($"{nameof(content)} {spec.ErrorMessage}");
        }

        AuthorId = authorId;
        Content = content;
        CreatedAt = Timestamp.GetCurrentTimestamp();
    }

    public Comment(CommentDTO dto)
    {
        CommentId = dto.CommentId;
        AuthorId = dto.AuthorId;
        Content = dto.Content;
        CreatedAt = dto.CreatedAt;
    }

    public CommentDTO ToDto()
    {
        return new CommentDTO(this);
    }
}
