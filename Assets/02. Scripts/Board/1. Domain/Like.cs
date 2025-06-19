using Firebase.Firestore;
using System.Collections.Generic;

[FirestoreData]
public class Like
{
    [FirestoreProperty] private List<string> _likedUserIds { get; set; }
    public HashSet<string> LikedUserIds { get; private set; }
    public int LikeCount => _likedUserIds.Count;

    public Like() { }

    public Like(List<string> likedUserIds)
    {
        _likedUserIds = likedUserIds;
        LikedUserIds = new HashSet<string>(_likedUserIds);
    }

    public void AddLike(string userId)
    {
        if(IsLikedBy(userId))
        {
            return;
        }

        _likedUserIds.Add(userId);
        LikedUserIds.Add(userId);
    }

    public bool IsLikedBy(string userId)
    {
        return LikedUserIds.Contains(userId);
    }

    /// <summary>
    /// 얘는 나중에 UI쪽으로 옮긴다
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
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
