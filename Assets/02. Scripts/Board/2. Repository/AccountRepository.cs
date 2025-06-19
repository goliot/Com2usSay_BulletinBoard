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
                throw new InvalidOperationException("���� �α��ε� ������ �����ϴ�.");
            }
            return _myAccount.ToDto();
        }
    }

    #region Register
    /// <summary>
    /// ȸ������
    /// </summary>
    public async Task<(bool isSuccess, string errorMessage)> RegisterAsync(string email, string nickname, string password)
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        try
        {
            var result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
            await SetInitialNicknameAsync(result.User, nickname);
            Debug.Log($"ȸ������ ���� : {result.User.UserId}");
            return (true, null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"ȸ������ ����: {ex.Message}");
            return (false, ex.Message);
        }
    }


    /// <summary>
    /// ȸ������ �� ���� 1ȸ ������ DisplayName�����
    /// </summary>
    private async Task SetInitialNicknameAsync(FirebaseUser user, string nickname)
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        if (user == null) return;

        var profile = new UserProfile { DisplayName = nickname };
        await user.UpdateUserProfileAsync(profile);
    }
    #endregion

    #region �α���/�α׾ƿ�
    /// <summary>
    /// �α���
    /// </summary>
    public async Task<(bool isSuccess, string errorMessage)> LoginAsync(string email, string password)
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        try
        {
            var result = await _auth.SignInWithEmailAndPasswordAsync(email, password);
            SetMyAccount(result.User);
            Debug.Log($"�α��� ���� : {result.User.DisplayName} ({result.User.UserId})");
            return (true, null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"�α��� ����: {ex.Message}");
            return (false, ex.Message);
        }
    }


    /// <summary>
    /// ����� �α׾ƿ��� �� �������� �� ��
    /// </summary>
    public void Logout()
    {
        if (_auth.CurrentUser == null)
        {
            Debug.LogWarning("�̹� �α׾ƿ��� �����Դϴ�.");
            return;
        }

        _auth.SignOut();
        SetMyAccount(null);

        Debug.Log("�α׾ƿ� �Ǿ����ϴ�.");
    }
    #endregion

    #region Nickname
    /// <summary>
    /// �α��� ��, �ڽ��� �г��� ���� ��
    /// </summary>
    public async Task<(bool isSuccess, string errorMessage)> ChangeMyNicknameAsync(string newNickname)
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        var user = _auth.CurrentUser;

        if (user == null)
        {
            string msg = "�α��ε� ������ �����ϴ�.";
            Debug.LogWarning(msg);
            return (false, msg);
        }

        if (user.Email != _myAccount.Email)
        {
            string msg = "���� ������ �ٸ��ϴ�";
            Debug.LogError(msg);
            return (false, msg);
        }

        var profile = new UserProfile { DisplayName = newNickname };

        try
        {
            await user.UpdateUserProfileAsync(profile);
            _myAccount.SetNickname(newNickname, out string _);
            Debug.Log($"�г����� '{newNickname}'(��)�� ����Ǿ����ϴ�.");
            return (true, null);
        }
        catch (Exception ex)
        {
            Debug.LogError("�г��� ���� ����: " + ex.Message);
            return (false, ex.Message);
        }
    }
    #endregion

    /// <summary>
    /// ���������� ������ �ִ� Account ��ü ������
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
