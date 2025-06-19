using UnityEngine;
using System;
using System.Threading.Tasks;

public class PostManager : Singleton<PostManager>
{
    private readonly PostRepository _postRepository = new PostRepository();

    public async Task CreatePost(string title, string content)
    {
        string postId = Guid.NewGuid().ToString();
        string authorId = AccountManager.Instance.MyAccount.Email;
        Post newpost = new Post(postId, title, content, authorId);

        await _postRepository.AddPost(newpost);
        Debug.Log($"Post created: {newpost.PostId}, Title: {newpost.Title}, Author: {newpost.AuthorId}");
    }

    public async Task UpdatePost(string postId, string content)
    {
        if (string.IsNullOrEmpty(postId))
        {
            throw new Exception("Post ID cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(content))
        {
            throw new Exception("Content cannot be null or empty.");
        }

        Post targetPost = await _postRepository.GetPost(postId);
        string requesterId = AccountManager.Instance.MyAccount.Email;
        if ( targetPost.AuthorId != requesterId)
        {
            throw new Exception("You do not have permission to update this post.");
        }

        await _postRepository.UpdatePost(postId, content);
        Debug.Log($"Post updated: {postId}, New Content: {content}");
    }

    public async Task DeletePost(string postId)
    {
        if (string.IsNullOrEmpty(postId))
        {
            throw new Exception("Post ID cannot be null or empty.");
        }
        Post targetPost = await _postRepository.GetPost(postId);
        string requesterId = AccountManager.Instance.MyAccount.Email;
        if (targetPost.AuthorId != requesterId)
        {
            throw new Exception("You do not have permission to delete this post.");
        }
        await _postRepository.DeletePost(postId);
        Debug.Log($"Post deleted: {postId}");
    }
}
