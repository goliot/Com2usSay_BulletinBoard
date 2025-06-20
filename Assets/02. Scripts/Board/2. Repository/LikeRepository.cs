using Firebase.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LikeRepository
{
    private FirebaseFirestore _db => FirebaseInitialize.DB;

    public async Task<Like> GetLike(PostDTO post)
    {
        var likeDoc = await _db.Collection("Posts").Document(post.PostId).Collection("Likes").Document("likeDoc").GetSnapshotAsync();
        if (likeDoc.Exists)
            return likeDoc.ConvertTo<Like>();
        else
            return new Like(new List<string>());
    }

    public async Task ToggleLike(PostDTO post, AccountDTO accuont)
    {
        var likeDocRef = _db.Collection("Posts").Document(post.PostId).Collection("Likes").Document("likeDoc");
        var snapshot = await likeDocRef.GetSnapshotAsync();

        Like likeData = snapshot.Exists ? snapshot.ConvertTo<Like>() : new Like(new List<string>());
        likeData.ToggleLike(accuont.Email);
        await likeDocRef.SetAsync(likeData);
    }
}
