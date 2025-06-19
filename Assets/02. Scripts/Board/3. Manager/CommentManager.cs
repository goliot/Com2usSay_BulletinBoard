using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CommentManager : Singleton<CommentManager>
{
    public event Action OnCommentChanged;

    private CommentRepository _repository = new CommentRepository();

    public async Task AddComment(Post post, CommentDTO comment)
    {
        if (post == null || comment == null)
        {
            Debug.LogError("PostDTO �Ǵ� CommentDTO�� null�Դϴ�.");
            return;
        }

        await _repository.AddComment(post, comment);
        Debug.Log($"��� �߰� �Ϸ� - PostId: {post.PostId}");
        OnCommentChanged?.Invoke();
    }

    public async Task<List<CommentDTO>> GetComments(PostDTO post)
    {
        if (post == null)
        {
            Debug.LogError("PostDTO�� null�Դϴ�.");
            return new List<CommentDTO>();
        }

        var comments = await _repository.GetComments(post);
        Debug.Log($"��� ��� ��ȸ �Ϸ� - Count: {comments.Count}");
        return comments;
    }

    public async Task DeleteComment(Post post, CommentDTO comment)
    {
        if (post == null || comment == null)
        {
            Debug.LogError("PostDTO �Ǵ� CommentDTO�� null�Դϴ�.");
            return;
        }

        await _repository.DeleteComment(post, comment);
        Debug.Log($"��� ���� �Ϸ� - CommentId: {comment.CommentId}");
        OnCommentChanged?.Invoke();
    }
}
