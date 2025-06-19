using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class PostManagerTest : MonoBehaviour
{
    private PostRepository _repository;

    private async void Start()
    {
        await FirebaseInitialize.WaitForInitializationAsync();
        _repository = new PostRepository();

        string testPostId = "post_" + System.Guid.NewGuid().ToString(); // 임시 ID
        string title = "Test Title";
        string content = "Test Content";
        string authorId = "user123";

        // 1. 게시글 생성
        Post newPost = new Post(testPostId, title, content, authorId);
        await _repository.AddPost(newPost);
        Debug.Log("✅ 게시글 등록 완료");

        // 2. 게시글 조회 (단일)
        Post fetchedPost = await _repository.GetPost(testPostId);
        if (fetchedPost != null)
        {
            Debug.Log($"📥 게시글 조회 성공 - 제목: {fetchedPost.Title}, 작성자: {fetchedPost.AuthorId}");
        }

        // 3. 게시글 수정
        string updatedContent = "This is updated content.";
        await _repository.UpdatePost(testPostId, updatedContent);
        Debug.Log("✏️ 게시글 수정 완료");

        // 4. 다시 조회해서 수정 내용 확인
        Post updatedPost = await _repository.GetPost(testPostId);
        if (updatedPost != null)
        {
            Debug.Log($"🔄 수정된 게시글 내용: {updatedPost.Content}");
        }

        // 5. 게시글 목록 조회 (최신순)
        List<Post> postList = await _repository.GetPosts(0, 10);
        Debug.Log($"📃 전체 게시글 수: {postList.Count}");

        /*// 6. 게시글 삭제
        await _repository.DeletePost(testPostId);
        Debug.Log("🗑 게시글 삭제 완료");

        // 7. 삭제 후 확인
        Post deletedPost = await _repository.GetPost(testPostId);
        Debug.Log(deletedPost == null ? "❌ 게시글이 성공적으로 삭제됨" : "⚠ 게시글 삭제 실패");*/
    }
}
