using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public class PostManager : Singleton<PostManager>
{
    private readonly PostRepository _postRepository = new PostRepository();

    private Post _currentPost;
    public Post CurrentPost => _currentPost;

    public async Task<bool> CreatePost(string title, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            Debug.LogError("Content cannot be null or empty.");
            return false;
        }
        string postId = Guid.NewGuid().ToString();
        string authorId = AccountManager.Instance.MyAccount.Email;
        Post newpost = new Post(postId, title, content, authorId);
        PostDTO newpostDTO = new PostDTO(newpost);

        await _postRepository.AddPost(newpostDTO);
        Debug.Log($"Post created: {newpost.PostId}, Title: {newpost.Title}, Author: {newpost.AuthorId}");
        return true;
    }

    public async Task<PostDTO> GetPost(string postId)
    {
        PostDTO post = await _postRepository.GetPost(postId);

        return post;
    }

    public async Task<bool> UpdatePost(string content)
    {
        if (string.IsNullOrEmpty(CurrentPost.PostId))
        {
            Debug.Log("Post ID cannot be null or empty.");
            return false;
        }
        if (string.IsNullOrEmpty(content))
        {
            Debug.LogError("Content cannot be null or empty.");
            return false;
        }

        string requesterId = AccountManager.Instance.MyAccount.Email;
        if (CurrentPost == null || CurrentPost.AuthorId != requesterId)
        {
            Debug.LogError("You do not have permission to update this post.");
            return false;
        }

        await _postRepository.UpdatePost(CurrentPost.PostId, content);
        Debug.Log($"Post updated: {CurrentPost.PostId}, New Content: {content}");
        return true;
    }

    public async Task DeletePost()
    {
        if (string.IsNullOrEmpty(CurrentPost.PostId))
        {
            throw new Exception("Post ID cannot be null or empty.");
        }

        string requesterId = AccountManager.Instance.MyAccount.Email;
        if (CurrentPost == null || CurrentPost.AuthorId != requesterId)
        {
            throw new Exception("You do not have permission to update this post.");
        }
        await _postRepository.DeletePost(CurrentPost.PostId);
        Debug.Log($"Post deleted: {CurrentPost.PostId}");
    }
    public async Task DeletePost(string postId)
    {
        if (string.IsNullOrEmpty(postId))
        {
            throw new Exception("Post ID cannot be null or empty.");
        }

        string requesterId = AccountManager.Instance.MyAccount.Email;
        PostDTO targetPost = await _postRepository.GetPost(postId);
        if (targetPost == null || targetPost.AuthorId != requesterId)
        {
            throw new Exception("You do not have permission to update this post.");
        }
        await _postRepository.DeletePost(postId);
        Debug.Log($"Post deleted: {postId}");
    }

    public void SetCurrentPost(Post post)
    {
        if (post == null)
        {
            Debug.LogWarning("선택된 포스트가 null입니다.");
            return;
        }

        _currentPost = post;
        Debug.Log($"CurrentPost 설정됨: {post.Title} (ID: {post.PostId})");
    }

    public async Task ToggleLikeOnCurrentPost()
    {
        await LikeManager.Instance.ToggleLike(_currentPost);
    }

    public async Task<List<Post>> GetAllPosts()
    {
        List<Post> posts = await _postRepository.GetPosts();

        return posts;
    }
}
