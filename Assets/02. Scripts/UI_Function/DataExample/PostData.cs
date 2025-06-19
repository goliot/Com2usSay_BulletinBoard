using System;

[Serializable]
public class PostData
{
    public string PostId;            // 고유 식별자
    public string AuthorName;
    public string TimeInfo;
    public string Content;
    public int LikeCount;
    public int CommentCount;

    // TODO: 나중에 Firebase DTO 필드가 바뀌면 여기에 추가
    public PostData() { }
}
