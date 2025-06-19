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

    public async Task ToggleLike(Post post)
    {
        var account = AccountManager.Instance.MyAccount;

        // Firebase 저장은 DTO로
        await _repository.ToggleLike(post, account);

        // 로컬 상태 반영은 도메인 객체로
        post.ToggleLikeBy(account.Nickname);
    }

    public bool IsLikedByMe(Post post)
    {
        return post.Like?.IsLikedBy(AccountManager.Instance.MyAccount.Nickname) ?? false;
    }
}
