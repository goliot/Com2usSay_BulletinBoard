using Firebase.Firestore;
using System;

public class Comment
{
    public string AuthorId { get; private set; }
    public string Content { get; private set; }
    public readonly Timestamp CreatedAt;

    public Comment(string authorId, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            throw new Exception("내용이 비었습니다.");
        }
        if (string.IsNullOrEmpty(authorId))
        {
            throw new Exception("작성자 Id가 비었습니다.");
        }
        AuthorId = authorId;
        Content = content;
        CreatedAt = Timestamp.GetCurrentTimestamp();
    }
}