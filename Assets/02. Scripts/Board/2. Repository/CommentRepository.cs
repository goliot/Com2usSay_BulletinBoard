using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CommentRepository
{
    private FirebaseFirestore _db = FirebaseInitialize.DB;

    public async Task AddComment(PostDTO post, CommentDTO comment)
    {
        var commentsRef = _db.Collection("Posts").Document(post.PostId).Collection("Comments");
        var newCommentRef = commentsRef.Document();
        await newCommentRef.SetAsync(comment);
    }

    public async Task<List<CommentDTO>> GetComments(PostDTO post)
    {
        var commentsSnapshot = await _db.Collection("Posts").Document(post.PostId).Collection("Comments")
                                        .OrderBy("CreatedAt").GetSnapshotAsync();

        List<CommentDTO> comments = new List<CommentDTO>();
        foreach (var doc in commentsSnapshot.Documents)
            comments.Add(doc.ConvertTo<Comment>().ToDto());

        return comments;
    }

    public async Task DeleteComment(PostDTO post, CommentDTO comment)
    {
        await _db.Collection("Posts").Document(post.PostId).Collection("Comments").Document(comment.CommentId).DeleteAsync();
    }
}
