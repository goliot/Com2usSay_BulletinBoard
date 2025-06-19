using Firebase.Firestore;
using System;

[FirestoreData]
public class Comment
{
    private readonly string _authorId;
    [FirestoreProperty] public string AuthorId => _authorId;
    [FirestoreProperty] public string Content { get; private set; }

    private readonly Timestamp _createdAt;
    [FirestoreProperty] public Timestamp CreatedAt => _createdAt;

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
        _authorId = authorId;
        Content = content;
        _createdAt = Timestamp.GetCurrentTimestamp();
    }
}