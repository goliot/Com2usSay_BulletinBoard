using Firebase.Firestore;

public class Comment
{
    public string AutherId { get; private set; }
    public string Content { get; private set; }
    public readonly Timestamp CreatedAt;

    public Comment(string autherId, string content)
    {
        AutherId = autherId;
        Content = content;
        CreatedAt = Timestamp.GetCurrentTimestamp();
    }
}