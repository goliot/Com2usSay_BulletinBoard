using System.Collections.Generic;

public class LikeDTO
{
    public readonly List<string> LikedUserIds;
    public int LikeCount => LikedUserIds.Count;

    public LikeDTO(Like like) 
    {
        LikedUserIds = new List<string>(like.LikedUserIdHash);
    }
}
