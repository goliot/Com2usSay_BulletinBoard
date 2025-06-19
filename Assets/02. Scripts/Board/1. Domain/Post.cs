using Firebase.Firestore;
using System;
using System.Collections.Generic;

public class Post
{
    public string PostId { get; set; }  // ���� ID
    public string Title { get; set; }
    public string Content { get; set; }
    public string AuthorId { get; set; }
    public readonly Timestamp CreatedAt;

    public List<Comment> CommentList { get; private set; }

    public Post(string postId, string title, string content, string authorId)
    {
        if(string.IsNullOrEmpty(postId))
        {
            throw new Exception("���� ID�� ������ϴ�.");
        }
        if(string.IsNullOrEmpty(title))
        {
            throw new Exception("������ ������ϴ�.");
        }
        if(string.IsNullOrEmpty(content))
        {
            throw new Exception("������ ������ϴ�.");
        }
        if(string.IsNullOrEmpty(authorId))
        {
            throw new Exception("�ۼ��� Id�� ������ϴ�.");
        }
        PostId = postId;
        Title = title;
        Content = content;
        AuthorId = authorId;

        CreatedAt = Timestamp.GetCurrentTimestamp();
        CommentList = new List<Comment>();
    }
}