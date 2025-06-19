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
        if (string.IsNullOrEmpty(authorId)) throw new Exception("작성자 Id가 비었습니다.");
        if (string.IsNullOrEmpty(content)) throw new Exception("내용이 비었습니다.");

        AuthorId = authorId;
        Content = content;
        CreatedAt = Timestamp.GetCurrentTimestamp();
    }

    public CommentDTO ToDto()
    {
        return new CommentDTO(this);
    }
}
