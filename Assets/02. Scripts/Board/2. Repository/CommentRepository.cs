using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CommentRepository
{
    private FirebaseFirestore _db = FirebaseInitialize.DB;

    public async Task AddComment(string postId, Comment comment)
    {
        var commentsRef = _db.Collection("Posts").Document(postId).Collection("Comments");
        var newCommentRef = commentsRef.Document();
        await newCommentRef.SetAsync(comment);
    }

    public async Task<List<Comment>> GetComments(string postId)
    {
        var commentsSnapshot = await _db.Collection("Posts").Document(postId).Collection("Comments")
                                        .OrderBy("CreatedAt").GetSnapshotAsync();

        List<Comment> comments = new List<Comment>();
        foreach (var doc in commentsSnapshot.Documents)
            comments.Add(doc.ConvertTo<Comment>());

        return comments;
    }

    public async Task DeleteComment(string postId, string commentId)
    {
        await _db.Collection("Posts").Document(postId).Collection("Comments").Document(commentId).DeleteAsync();
    }
}
