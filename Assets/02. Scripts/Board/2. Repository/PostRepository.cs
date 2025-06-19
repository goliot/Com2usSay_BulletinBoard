using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PostRepository
{
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;
    public async Task AddPost(Post post) 
    {
        DocumentReference docRef = _db.Collection("Posts").Document(post.PostId);
        await docRef.SetAsync(post);
        Debug.Log($"Post Uploaded: {post.PostId}, Title: {post.Title}, Author: {post.AuthorId}");
    }

    public async Task<List<Post>> GetPosts(int start, int limit)
    {
        Query query = _db.Collection("Posts")
                         .OrderByDescending("CreatedAt")
                         .Limit(limit);

        if (start > 0)
        {
            QuerySnapshot tempSnapshot = await _db.Collection("Posts")
                                                  .OrderByDescending("CreatedAt")
                                                  .Limit(start)
                                                  .GetSnapshotAsync();
            var cursor = tempSnapshot.Documents.LastOrDefault();
            if (cursor != null)
            {
                query = query.StartAfter(cursor);
            }
        }

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        List<Post> postList = new List<Post>();
        foreach (var doc in snapshot.Documents)
        {
            postList.Add(doc.ConvertTo<Post>());
        }

        return postList;
    }


    public async Task<Post> GetPost(string postId)
    {
        DocumentReference docRef = _db.Collection("Posts").Document(postId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            Post post = snapshot.ConvertTo<Post>();
            return post;
        }
        else
        {
            Debug.Log($"Document {postId} does not exist!");
            return null;
        }
    }

    public async Task UpdatePost(string postId, string content) 
    {
        DocumentReference docRef = _db.Collection("Posts").Document(postId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "Content", content }
        };
        await docRef.UpdateAsync(updates);
        Debug.Log($"Post Updated: {postId}");
    }
    public async Task DeletePost(string postId) 
    {
        DocumentReference docRef = _db.Collection("posts").Document(postId);

        // 하위 컬렉션 'comments' 제거
        var comments = await docRef.Collection("comments").GetSnapshotAsync();
        foreach (var comment in comments.Documents)
            await comment.Reference.DeleteAsync();

        await docRef.DeleteAsync();

        Debug.Log($"Post '{postId}' 및 하위 comment deleted");
    }
}