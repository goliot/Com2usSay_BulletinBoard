using Firebase.Firestore;
using System;

[FirestoreData]
public class Comment
{
    [FirestoreProperty] public string CommentId { get; set; }
    [FirestoreProperty] public string AuthorId { get; set; }
    [FirestoreProperty] public string Content { get; set; }
    [FirestoreProperty] public Timestamp CreatedAt { get; set; }

    // �⺻ ������ �ʿ� (Firestore��)
    public Comment() { }

    public Comment(string authorId, string content)
    {
        if (string.IsNullOrEmpty(authorId)) throw new Exception("�ۼ��� Id�� ������ϴ�.");
        if (string.IsNullOrEmpty(content)) throw new Exception("������ ������ϴ�.");

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
