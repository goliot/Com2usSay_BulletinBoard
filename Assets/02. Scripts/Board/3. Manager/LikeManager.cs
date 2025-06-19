using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class LikeManager : Singleton<LikeManager>
{
    private readonly LikeRepository _repository = new LikeRepository();

    public async Task<LikeDTO> LoadLikeData(PostDTO post)
    {
        var likeData = await _repository.GetLike(post);
        return likeData.ToDto();
    }

    public async Task ToggleLike(PostDTO post)
    {
        var account = AccountManager.Instance.MyAccount;
        await _repository.ToggleLike(post, account);

        // 서버에서 최신 Like 정보 받아와서 동기화
        //var likeData = await _repository.GetLike(post);
        //post.SetLike(likeData);
    }

    public bool IsLikedByMe(Post post)
    {
        return post.Like?.IsLikedBy(AccountManager.Instance.MyAccount.Nickname) ?? false;
    }
}
