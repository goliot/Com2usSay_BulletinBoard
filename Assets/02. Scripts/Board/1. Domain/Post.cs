using Firebase.Firestore;
using System;
using System.Collections.Generic;

public class Post
{
    public string PostId { get; set; }  // ¹®¼­ ID
    public string Title { get; set; }
    public string Content { get; set; }
    public string AuthorId { get; set; }
    public readonly Timestamp CreatedAt;

    public List<Comment> CommentList { get; private set; }

    public Post(string postId, string title, string content, string authorId)
    {
        PostId = postId;
        Title = title;
        Content = content;
        AuthorId = authorId;

        CreatedAt = Timestamp.GetCurrentTimestamp();
        CommentList = new List<Comment>();
    }
}