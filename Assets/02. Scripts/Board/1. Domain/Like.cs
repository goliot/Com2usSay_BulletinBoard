using Firebase.Firestore;
using System.Collections.Generic;

[FirestoreData]
public class Like
{
    [FirestoreProperty]
    public List<string> LikedUserIds { get; set; } = new List<string>();

    public HashSet<string> LikedUserIdHash => new HashSet<string>(LikedUserIds);

    public int LikeCount => LikedUserIds.Count;

    public Like() { }

    public Like(List<string> likedUserIds)
    {
        LikedUserIds = likedUserIds ?? new List<string>();
    }

    public bool IsLikedBy(string userId)
    {
        return LikedUserIdHash.Contains(userId);
    }

    public bool ToggleLike(string userId)
    {
        if (LikedUserIdHash.Contains(userId))
        {
            LikedUserIds.Remove(userId);
            return false;
        }
        else
        {
            LikedUserIds.Add(userId);
            return true;
        }
    }

    public LikeDTO ToDto()
    {
        return new LikeDTO(this);
    }
}