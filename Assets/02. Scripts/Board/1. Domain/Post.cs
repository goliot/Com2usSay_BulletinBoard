using Firebase.Firestore;
using System;
using System.Collections.Generic;

public class Post
{
    public string PostId { get; set; }  // 문서 ID
    public string Title { get; set; }
    public string Content { get; set; }
    public string AuthorId { get; set; }
    public readonly Timestamp CreatedAt;

    public List<Comment> CommentList { get; private set; }

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
        PostId = postId;
        Title = title;
        Content = content;
        AuthorId = authorId;

        CreatedAt = Timestamp.GetCurrentTimestamp();
        CommentList = new List<Comment>();
    }
}