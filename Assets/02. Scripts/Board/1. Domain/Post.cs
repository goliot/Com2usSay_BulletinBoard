using Firebase.Firestore;
using System;
using System.Collections.Generic;

[FirestoreData]
public class Post
{
    [FirestoreProperty] public string PostId { get; set; }
    [FirestoreProperty] public string AuthorId { get; set; }
    [FirestoreProperty] public string Title { get; set; }
    [FirestoreProperty] public string Content { get; set; }
    [FirestoreProperty] public Timestamp CreatedAt { get; set; }
    public List<Comment> CommentList { get; set; } = new List<Comment>();
    public int CommentCount => CommentList.Count;
    public Like Like { get; set; } //= new Like();
    public int LikeCount => Like.LikeCount;

    public Post()
    {
        CommentList = new List<Comment>();
        Like = new Like(new List<string>());
    }

    public Post(string postId, string title, string content, string authorId)
    {
        if (string.IsNullOrEmpty(postId))
        {
            throw new Exception("문서 ID가 비었습니다.");
        }
        if (string.IsNullOrEmpty(title))
        {
            throw new Exception("제목이 비었습니다.");
        }
        if (string.IsNullOrEmpty(content))
        {
            throw new Exception("내용이 비었습니다.");
        }
        if (string.IsNullOrEmpty(authorId))
        {
            throw new Exception("작성자 Id가 비었습니다.");
        }
        PostId = postId;
        AuthorId = authorId;
        Title = title;
        Content = content;

        CreatedAt = Timestamp.GetCurrentTimestamp();
        CommentList = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        CommentList.Add(comment);
    }

    public void DeleteComment(CommentDTO comment)
    {
        if (comment == null || string.IsNullOrEmpty(comment.CommentId))
            return;

        CommentList.RemoveAll(c => c.CommentId == comment.CommentId);
    }

    public PostDTO ToDto()
    {
        return new PostDTO(this);
    }
}