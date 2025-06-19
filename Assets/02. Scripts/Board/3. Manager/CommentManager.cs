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
            Debug.LogError("PostDTO 또는 CommentDTO가 null입니다.");
            return;
        }

        await _repository.AddComment(post, comment);
        Debug.Log($"댓글 추가 완료 - PostId: {post.PostId}");
        OnCommentChanged?.Invoke();
    }

    public async Task<List<CommentDTO>> GetComments(PostDTO post)
    {
        if (post == null)
        {
            Debug.LogError("PostDTO가 null입니다.");
            return new List<CommentDTO>();
        }

        var comments = await _repository.GetComments(post);
        Debug.Log($"댓글 목록 조회 완료 - Count: {comments.Count}");
        return comments;
    }

    public async Task DeleteComment(Post post, CommentDTO comment)
    {
        if (post == null || comment == null)
        {
            Debug.LogError("PostDTO 또는 CommentDTO가 null입니다.");
            return;
        }

        await _repository.DeleteComment(post, comment);
        Debug.Log($"댓글 삭제 완료 - CommentId: {comment.CommentId}");
        OnCommentChanged?.Invoke();
    }
}
