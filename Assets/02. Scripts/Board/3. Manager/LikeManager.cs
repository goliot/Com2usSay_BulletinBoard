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

    public async Task<bool> ToggleLike(Post post)
    {
        var account = AccountManager.Instance.MyAccount;

        // 서버에 토글 요청
        bool flag = await _repository.ToggleLike(post, account);

        // 최신 Like 정보를 다시 받아와서 동기화
        var updatedLike = await _repository.GetLike(post.ToDto());
        post.SetLike(updatedLike);  // 🔥 반드시 반영 필요!

        return flag;
    }


    public bool IsLikedByMe(Post post)
    {
        return post.Like?.IsLikedBy(AccountManager.Instance.MyAccount.Email) ?? false;
    }
}
