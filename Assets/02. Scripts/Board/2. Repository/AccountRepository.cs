using Firebase.Auth;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class AccountRepository
{
    private FirebaseAuth _auth => FirebaseInitialize.Auth;

    private Account _myAccount;
    public AccountDTO MyAccount
    {
        get
        {
            if (_myAccount == null)
            {
                throw new InvalidOperationException("현재 로그인된 계정이 없습니다.");
            }
            return _myAccount.ToDto();
        }
    }

    #region Register
    /// <summary>
    /// 회원가입
    /// </summary>
    public async Task<(bool isSuccess, string errorMessage)> RegisterAsync(string email, string nickname, string password)
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        try
        {
            var result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
            await SetInitialNicknameAsync(result.User, nickname);
            Debug.Log($"회원가입 성공 : {result.User.UserId}");
            return (true, null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"회원가입 실패: {ex.Message}");
            return (false, ex.Message);
        }
    }


    /// <summary>
    /// 회원가입 시 최초 1회 서버의 DisplayName변경용
    /// </summary>
    private async Task SetInitialNicknameAsync(FirebaseUser user, string nickname)
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        if (user == null) return;

        var profile = new UserProfile { DisplayName = nickname };
        await user.UpdateUserProfileAsync(profile);
    }
    #endregion

    #region 로그인/로그아웃
    /// <summary>
    /// 로그인
    /// </summary>
    public async Task<(bool isSuccess, string errorMessage)> LoginAsync(string email, string password)
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        try
        {
            var result = await _auth.SignInWithEmailAndPasswordAsync(email, password);
            SetMyAccount(result.User);
            Debug.Log($"로그인 성공 : {result.User.DisplayName} ({result.User.UserId})");
            return (true, null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"로그인 실패: {ex.Message}");
            return (false, ex.Message);
        }
    }


    /// <summary>
    /// 동기로 로그아웃이 더 안정적일 듯 함
    /// </summary>
    public void Logout()
    {
        if (_auth.CurrentUser == null)
        {
            Debug.LogWarning("이미 로그아웃된 상태입니다.");
            return;
        }

        _auth.SignOut();
        SetMyAccount(null);

        Debug.Log("로그아웃 되었습니다.");
    }
    #endregion

    #region Nickname
    /// <summary>
    /// 로그인 후, 자신의 닉네임 변경 용
    /// </summary>
    public async Task<(bool isSuccess, string errorMessage)> ChangeMyNicknameAsync(string newNickname)
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        var user = _auth.CurrentUser;

        if (user == null)
        {
            string msg = "로그인된 유저가 없습니다.";
            Debug.LogWarning(msg);
            return (false, msg);
        }

        if (user.Email != _myAccount.Email)
        {
            string msg = "유저 정보가 다릅니다";
            Debug.LogError(msg);
            return (false, msg);
        }

        var profile = new UserProfile { DisplayName = newNickname };

        try
        {
            await user.UpdateUserProfileAsync(profile);
            _myAccount.SetNickname(newNickname, out string _);
            Debug.Log($"닉네임이 '{newNickname}'(으)로 변경되었습니다.");
            return (true, null);
        }
        catch (Exception ex)
        {
            Debug.LogError("닉네임 변경 실패: " + ex.Message);
            return (false, ex.Message);
        }
    }
    #endregion

    /// <summary>
    /// 내부적으로 가지고 있는 Account 객체 유지용
    /// </summary>
    private void SetMyAccount(FirebaseUser user)
    {
        if (user != null)
        {
            _myAccount = new Account(user.Email, user.DisplayName);
        }
        else
        {
            _myAccount = null;
        }
    }
}
