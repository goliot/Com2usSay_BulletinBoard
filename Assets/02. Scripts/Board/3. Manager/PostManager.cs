using UnityEngine;
using System;
using System.Threading.Tasks;

public class PostManager : Singleton<PostManager>
{
    private readonly PostRepository _postRepository = new PostRepository();
    public PostDTO CurrentPost { get; private set; }

    public async Task CreatePost(string title, string content)
    {
        string postId = Guid.NewGuid().ToString();
        string authorId = AccountManager.Instance.MyAccount.Email;
        Post newpost = new Post(postId, title, content, authorId);
        PostDTO newpostDTO = new PostDTO(newpost);

        await _postRepository.AddPost(newpostDTO);
        Debug.Log($"Post created: {newpost.PostId}, Title: {newpost.Title}, Author: {newpost.AuthorId}");
    }

    public async Task UpdatePost(string content)
    {
        if (string.IsNullOrEmpty(CurrentPost.PostId))
        {
            throw new Exception("Post ID cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(content))
        {
            throw new Exception("Content cannot be null or empty.");
        }

        string requesterId = AccountManager.Instance.MyAccount.Email;
        if (CurrentPost == null || CurrentPost.AuthorId != requesterId)
        {
            throw new Exception("You do not have permission to update this post.");
        }

        await _postRepository.UpdatePost(CurrentPost.PostId, content);
        Debug.Log($"Post updated: {CurrentPost.PostId}, New Content: {content}");
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

    public void SetCurrentPost(PostDTO post)
    {
        if (post == null)
        {
            Debug.LogWarning("선택된 포스트가 null입니다.");
            return;
        }

        CurrentPost = post;
        Debug.Log($"CurrentPost 설정됨: {post.Title} (ID: {post.PostId})");
    }
}
