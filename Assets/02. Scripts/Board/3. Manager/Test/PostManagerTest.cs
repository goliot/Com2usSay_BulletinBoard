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

        string testPostId = "post_" + System.Guid.NewGuid().ToString();
        string title = "likeTest Title";
        string content = "likeTest Content";

        bool registerResult = await AccountManager.Instance.RegisterAsync("nicknameTest@test.com", "liketest", "123456");
        Debug.Log("회원가입 결과: " + registerResult);

        bool loginResult = await AccountManager.Instance.LoginAsync("liketest@test.com", "123456");
        Debug.Log("로그인 결과: " + loginResult);

        // 1. 게시글 생성
        Post newPost = new Post(testPostId, title, content, AccountManager.Instance.MyAccount.Email);
        PostDTO newPostDTO = new PostDTO(newPost);
        await _repository.AddPost(newPostDTO);
        Debug.Log("✅ 게시글 등록 완료");

        // 2. 좋아요 누르기 (가정: 로그인된 계정의 닉네임이 "user123")
        Debug.Log($"얘가 토글할거임 {AccountManager.Instance.MyAccount.Email}");
        await LikeManager.Instance.ToggleLike(newPostDTO); // ← 도메인 Post 전달
        Debug.Log("👍 좋아요 1회 토글 완료");

        // 4. 게시글 조회
        PostDTO fetchedPost = await _repository.GetPost(testPostId);
        if (fetchedPost != null)
        {
            Debug.Log($"📥 게시글 조회 성공 - 제목: {fetchedPost.Title}, 작성자: {fetchedPost.AuthorId}, 좋아요 : {fetchedPost.Like.LikeCount}");
        }

        // 2. 좋아요 누르기 (가정: 로그인된 계정의 닉네임이 "user123")
        Debug.Log($"얘가 토글할거임 {AccountManager.Instance.MyAccount.Email}");
        await LikeManager.Instance.ToggleLike(newPostDTO); // ← 도메인 Post 전달
        Debug.Log("👍 좋아요 1회 토글 완료");

        // 4. 게시글 조회
        fetchedPost = await _repository.GetPost(testPostId);
        if (fetchedPost != null)
        {
            Debug.Log($"📥 게시글 조회 성공 - 제목: {fetchedPost.Title}, 작성자: {fetchedPost.AuthorId}, 좋아요 : {fetchedPost.Like.LikeCount}");
        }

        // 5. 게시글 수정
        string updatedContent = "This is updated content.";
        await _repository.UpdatePost(testPostId, updatedContent);
        Debug.Log("✏️ 게시글 수정 완료");

        // 6. 수정 확인
        PostDTO updatedPost = await _repository.GetPost(testPostId);
        if (updatedPost != null)
        {
            Debug.Log($"🔄 수정된 게시글 내용: {updatedPost.Content}");
        }

        // 7. 전체 게시글 목록
        List<PostDTO> postList = await _repository.GetPosts(0, 10);
        Debug.Log($"📃 전체 게시글 수: {postList.Count}");

        await TestAddAndFetchComments(newPostDTO);

        /*// 6. 게시글 삭제
        await _repository.DeletePost(testPostId);
        Debug.Log("🗑 게시글 삭제 완료");

        // 7. 삭제 후 확인
        Post deletedPost = await _repository.GetPost(testPostId);
        Debug.Log(deletedPost == null ? "❌ 게시글이 성공적으로 삭제됨" : "⚠ 게시글 삭제 실패");*/
    }

    private async Task TestAddAndFetchComments(PostDTO post)
    {
        var commentManager = CommentManager.Instance;

        // 댓글 생성
        string commentId = System.Guid.NewGuid().ToString();
        string commentContent = "이것은 테스트 댓글입니다.";
        string authorId = AccountManager.Instance.MyAccount.Email;

        CommentDTO newComment = new Comment
        {
            CommentId = commentId,
            AuthorId = authorId,
            Content = commentContent,
            CreatedAt = Firebase.Firestore.Timestamp.GetCurrentTimestamp()
        }.ToDto();

        await commentManager.AddComment(post, newComment);
        Debug.Log($"✅ 댓글 추가 완료: {commentContent}");

        // 댓글 목록 조회
        List<CommentDTO> comments = await commentManager.GetComments(post);
        Debug.Log($"📥 댓글 조회 성공, 총 댓글 수: {comments.Count}");

        foreach (var comment in comments)
        {
            Debug.Log($" - 댓글: {comment.Content} (작성자: {comment.AuthorId})");
        }
    }
}
