using Firebase.Firestore;

public class Comment
{
    public string AutherId { get; private set; }
    public string Content { get; private set; }
    public readonly Timestamp CreatedAt;
}