using Firebase.Firestore;

public class CommentDTO
{
    public readonly string CommentId;
    public readonly string AuthorId;
    public readonly string Content;
    public readonly Timestamp CreatedAt;

    public CommentDTO(Comment comment)
    {
        CommentId = comment.CommentId;
        AuthorId = comment.AuthorId;
        Content = comment.Content;
        CreatedAt = comment.CreatedAt;
    }

    public Comment ToEntity()
    {
        return new Comment(this);
    }
}