using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PostRepository
{
    private FirebaseFirestore _db = FirebaseInitialize.DB;

    public async Task AddPost(Post post)
    {
        DocumentReference docRef = _db.Collection("Posts").Document(post.PostId);

        // 1. Post 본문 저장
        await docRef.SetAsync(post);
        Debug.Log($"Post Uploaded: {post.PostId}, Title: {post.Title}, Author: {post.AuthorId}");

        // 2. Comments 서브컬렉션 초기화 (빈 상태라면 생략 가능)
        // 빈 서브컬렉션은 Firestore에 따로 저장할 필요 없음

        // 3. Likes 서브컬렉션 또는 Like 도큐먼트 초기화
        // 예를 들어 Like 도큐먼트 초기화
        var likeDoc = docRef.Collection("Likes").Document("likeDoc"); // 임의 ID
        var likeData = new Like(new List<string>());
        await likeDoc.SetAsync(likeData);
    }

    public async Task<List<PostDTO>> GetPosts(int start, int limit)
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

        return postList.ConvertAll((item) => item.ToDto());
    }


    public async Task<PostDTO> GetPost(string postId)
    {
        DocumentReference docRef = _db.Collection("Posts").Document(postId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            Post post = snapshot.ConvertTo<Post>();

            // post.setcomment
            var commentsSnapshot = await docRef.Collection("Comments")
                                      .OrderBy("CreatedAt")
                                      .GetSnapshotAsync();

            List<Comment> comments = new List<Comment>();
            foreach (var commentDoc in commentsSnapshot.Documents)
            {
                comments.Add(commentDoc.ConvertTo<Comment>());
            }
            post.SetComment(comments);

            // post.setlike
            DocumentSnapshot likeDocSnapshot = await docRef.Collection("Likes").Document("likeDoc").GetSnapshotAsync();

            if (likeDocSnapshot.Exists)
            {
                Like likeData = likeDocSnapshot.ConvertTo<Like>();
                post.SetLike(likeData);
            }
            else
            {
                //post.Like = new Like(new List<string>());
            }

            return post.ToDto();
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
        DocumentReference docRef = _db.Collection("Posts").Document(postId);

        // 1. 하위 컬렉션 'Comments' 제거
        var comments = await docRef.Collection("Comments").GetSnapshotAsync();
        foreach (var comment in comments.Documents)
            await comment.Reference.DeleteAsync();

        // 2. 하위 컬렉션 'Likes' 제거
        var likes = await docRef.Collection("Likes").GetSnapshotAsync();
        foreach (var like in likes.Documents)
            await like.Reference.DeleteAsync();

        // 3. 게시글 문서 자체 제거
        await docRef.DeleteAsync();

        Debug.Log($"Post '{postId}' 및 하위 Comments, Likes 삭제 완료");
    }

}