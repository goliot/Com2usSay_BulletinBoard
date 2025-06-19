using Firebase.Firestore;

[FirestoreData]
public class CommentDTO
{
    private readonly string _authorId;
    [FirestoreProperty] public string AuthorId => _authorId;

    private readonly string _content;
    [FirestoreProperty] public string Content => _content;

    private readonly Timestamp _createdAt;
    [FirestoreProperty] public Timestamp CreatedAt => _createdAt;
}