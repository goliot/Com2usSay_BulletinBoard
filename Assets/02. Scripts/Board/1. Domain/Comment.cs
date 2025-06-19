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
            throw new Exception("������ ������ϴ�.");
        }
        if (string.IsNullOrEmpty(authorId))
        {
            throw new Exception("�ۼ��� Id�� ������ϴ�.");
        }
        AuthorId = authorId;
        Content = content;
        CreatedAt = Timestamp.GetCurrentTimestamp();
    }
}