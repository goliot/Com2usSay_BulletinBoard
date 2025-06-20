using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CommentRepository
{
    private FirebaseFirestore _db => FirebaseInitialize.DB;

    public async Task AddComment(Post post, CommentDTO commentDto)
    {
        var commentsRef = _db.Collection("Posts").Document(post.PostId).Collection("Comments");

        // 문서 레퍼런스 먼저 만들고 ID 확보
        var newCommentRef = commentsRef.Document();
        string generatedId = newCommentRef.Id;

        // Comment 엔티티에 ID 할당
        var comment = commentDto.ToEntity();
        comment.CommentId = generatedId;

        await newCommentRef.SetAsync(comment);

        // Post 객체에도 추가
        post.AddComment(comment.ToDto());
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

    public async Task DeleteComment(Post post, CommentDTO comment)
    {
        await _db.Collection("Posts").Document(post.PostId).Collection("Comments").Document(comment.CommentId).DeleteAsync();
        post.DeleteComment(comment);
    }
}
