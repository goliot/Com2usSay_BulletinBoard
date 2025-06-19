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
        var newCommentRef = commentsRef.Document(); // �ڵ� ID ����
        await newCommentRef.SetAsync(comment);
    }
}