using Firebase.Firestore;
using System.Collections.Generic;

[FirestoreData]
public class Like
{
    [FirestoreProperty]
    public List<string> LikedUserIds { get; set; } = new List<string>();

    public HashSet<string> LikedUserIdHash { get; private set; }

    public int LikeCount => LikedUserIds.Count;

    public Like() 
    {
        SyncHashSet();
    }

    public Like(List<string> likedUserIds)
    {
        LikedUserIds = likedUserIds ?? new List<string>();
        SyncHashSet();
    }

    // Firestore ������ȭ ����, ȣ���ؼ� �ؽü� ����ȭ
    public void SyncHashSet()
    {
        LikedUserIdHash = new HashSet<string>(LikedUserIds);
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
            LikedUserIdHash.Remove(userId);
            return false;
        }
        else
        {
            LikedUserIds.Add(userId);
            LikedUserIdHash.Add(userId);
            return true;
        }
    }

    public LikeDTO ToDto()
    {
        return new LikeDTO(this);
    }
}