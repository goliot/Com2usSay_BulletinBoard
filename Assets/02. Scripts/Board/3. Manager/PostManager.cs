using UnityEngine;
using System;
using System.Threading.Tasks;

public class PostManager 
{
    private readonly PostRepository _postRepository;

    public async Task CreatePost(string title, string content, string authorId)
    {
        string postId = Guid.NewGuid().ToString();
        Post newpost = new Post(postId, title, content, authorId);

        await _postRepository.AddPost(newpost);
        Debug.Log($"Post created: {newpost.PostId}, Title: {newpost.Title}, Author: {newpost.AuthorId}");
    }
}
