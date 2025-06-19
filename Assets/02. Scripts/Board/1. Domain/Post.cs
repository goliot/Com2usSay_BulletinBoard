using Firebase.Firestore;
using System;
using System.Collections.Generic;

[FirestoreData]
public class Post
{
    private readonly string _postId;
    [FirestoreProperty] public string PostId => _postId; // 문서 ID

    private readonly string _authorId;
    [FirestoreProperty] public string AuthorId => _authorId;

    [FirestoreProperty] public string Title { get; set; }
    [FirestoreProperty] public string Content { get; set; }

    private readonly Timestamp _createdAt;
    [FirestoreProperty] public Timestamp CreatedAt => _createdAt;

    [FirestoreProperty] public List<Comment> CommentList { get; private set; }

    public Post(string postId, string title, string content, string authorId)
    {
        if(string.IsNullOrEmpty(postId))
        {
            throw new Exception("문서 ID가 비었습니다.");
        }
        if(string.IsNullOrEmpty(title))
        {
            throw new Exception("제목이 비었습니다.");
        }
        if(string.IsNullOrEmpty(content))
        {
            throw new Exception("내용이 비었습니다.");
        }
        if(string.IsNullOrEmpty(authorId))
        {
            throw new Exception("작성자 Id가 비었습니다.");
        }
        _postId = postId;
        _authorId = authorId;
        Title = title;
        Content = content;

        _createdAt = Timestamp.GetCurrentTimestamp();
        CommentList = new List<Comment>();
    }
}