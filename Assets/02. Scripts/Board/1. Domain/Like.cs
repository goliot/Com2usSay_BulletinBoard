using Firebase.Firestore;
using System.Collections.Generic;

[FirestoreData]
public class Like
{
    [FirestoreProperty]
    private List<string> _likedUserIds { get; set; } = new List<string>();

    public HashSet<string> LikedUserIds { get; private set; } = new HashSet<string>();

    public int LikeCount => _likedUserIds.Count;

    public Like() { }

    public Like(List<string> likedUserIds)
    {
        _likedUserIds = likedUserIds ?? new List<string>();
        SyncHashSet();
    }

    // Firestore 역직렬화 이후, 호출해서 해시셋 동기화
    public void SyncHashSet()
    {
        LikedUserIds = new HashSet<string>(_likedUserIds);
    }

    public void AddLike(string userId)
    {
        if (IsLikedBy(userId)) return;

        _likedUserIds.Add(userId);
        LikedUserIds.Add(userId);
    }

    public bool IsLikedBy(string userId)
    {
        return LikedUserIds.Contains(userId);
    }

    public bool ToggleLike(string userId)
    {
        if (LikedUserIds.Contains(userId))
        {
            _likedUserIds.Remove(userId);
            LikedUserIds.Remove(userId);
            return false;
        }
        else
        {
            _likedUserIds.Add(userId);
            LikedUserIds.Add(userId);
            return true;
        }
    }
}