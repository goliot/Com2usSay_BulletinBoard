using Firebase.Firestore;
using System;
using System.Collections.Generic;

public class PostDTO
{
    public readonly string PostId;
    public readonly string AuthorId;
    public readonly string Title;
    public readonly string Content;
    public readonly Timestamp CreatedAt;
    public readonly List<Comment> CommentList;
    public int CommentCount => CommentList.Count;
    public readonly Like Like;
    public int LikeCount => Like.LikeCount;

    public PostDTO(Post post)
    {
        PostId = post.PostId;
        AuthorId = post.AuthorId;
        Title = post.Title;
        Content = post.Content;
        CreatedAt = post.CreatedAt;
        CommentList = post.CommentList;
        Like = post.Like;
    }
    
    public Post ToEntity()
    {
        return new Post(this);
    }
}
