using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;

[FirestoreData]
public class Post
{
    [FirestoreProperty] public string PostId { get; set; }
    [FirestoreProperty] public string AuthorId { get; set; }
    [FirestoreProperty] public string Title { get; set; }
    [FirestoreProperty] public string Content { get; set; }
    [FirestoreProperty] public Timestamp CreatedAt { get; set; }
    public List<Comment> CommentList { get; private set; } = new List<Comment>();
    public int CommentCount => CommentList.Count;
    public Like Like { get; private set; } //= new Like();
    public int LikeCount => Like.LikeCount;

    public Post()
    {
        CommentList = new List<Comment>();
        Like = new Like(new List<string>());
    }

    public Post(PostDTO dto)
    {
        PostId = dto.PostId;
        AuthorId = dto.AuthorId;
        Title = dto.Title;
        Content = dto.Content;
        CreatedAt = Timestamp.FromDateTime(dto.CreatedAt);

        // CommentList는 null이 아닌지 확인하고 복사
        CommentList = dto.CommentList != null
            ? new List<Comment>(dto.CommentList)
            : new List<Comment>();

        // Like 객체 생성 (DTO에서 likedUserIds만 가져온다고 가정)
        Like = dto.Like != null
            ? new Like(new List<string>(dto.Like.LikedUserIds))
            : new Like(new List<string>());
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
        Like = new Like();
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

    public void SetComment(List<Comment> comment)
    {
        CommentList = comment;
    }

    public void SetLike(Like like)
    {
        Like = like;
    }

    public PostDTO ToDto()
    {
        return new PostDTO(this);
    }
}