using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;


public class AccountManager : Singleton<AccountManager>
{
    public event Action OnLoginSuccess;
    public event Action OnLogout;

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
    public async Task<bool> RegisterAsync(string email, string nickname, string password)
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        try
        {
            var result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
            await SetInitialNicknameAsync(result.User, nickname);
            Debug.Log($"ȸ������ ���� : {result.User.UserId}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"ȸ������ ����: {ex.Message}");
            return false;
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
    public async Task<bool> LoginAsync(string email, string password)
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        try
        {
            var result = await _auth.SignInWithEmailAndPasswordAsync(email, password);
            SetMyAccount(result.User);
            Debug.Log($"�α��� ���� : {result.User.DisplayName} ({result.User.UserId})");

            OnLoginSuccess?.Invoke();
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"�α��� ����: {ex.Message}");
            return false;
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
        OnLogout?.Invoke();  // UI ���� ���� �ļ�ó��
    }
    #endregion

    #region Nickname
    /// <summary>
    /// �α��� ��, �ڽ��� �г��� ���� ��
    /// </summary>
    public async Task<bool> ChangeMyNicknameAsync(string newNickname)
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        var user = _auth.CurrentUser;

        if (user == null)
        {
            Debug.LogWarning("�α��ε� ������ �����ϴ�.");
            return false;
        }

        if (user.Email != _myAccount.Email)
        {
            throw new Exception("���� ������ �ٸ��ϴ�");
        }

        var profile = new UserProfile
        {
            DisplayName = newNickname,
        };

        try
        {
            await user.UpdateUserProfileAsync(profile);
            _myAccount.SetNickname(newNickname, out string message);
            Debug.Log($"�г����� '{newNickname}'(��)�� ����Ǿ����ϴ�.");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("�г��� ���� ����: " + ex.Message);
            return false;
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