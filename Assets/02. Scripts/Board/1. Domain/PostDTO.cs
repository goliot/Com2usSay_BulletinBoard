using System;

public class PostDTO
{
    public readonly string PostId;
    public readonly string AuthorId;
    public readonly string Title;
    public readonly string Content;
    public readonly DateTime CreatedAt;

    public PostDTO(Post post)
    {
        PostId = post.PostId;
        AuthorId = post.AuthorId;
        Title = post.Title;
        Content = post.Content;
        CreatedAt = post.CreatedAt.ToDateTime();
    }
}
