using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PostRepository
{
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;
    public async Task AddPost(Post post) 
    {
        DocumentReference docRef = _db.Collection("Post").Document(post.PostId);
        await docRef.SetAsync(post);
    }
//    public async Task<List<PostDTO>> GetPosts(int start, int limit) { ... }
//    public async Task<PostDTO> GetPost(string postId)) { ... }
//    public async Task UpdatePost(PostDTO post) { ... }
//    public async Task DeletePost(string postId) { ... }
}