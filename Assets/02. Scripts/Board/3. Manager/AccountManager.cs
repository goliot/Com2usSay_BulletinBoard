using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;
using System.Collections.Generic;
using System;


public class AccountManager : Singleton<AccountManager>
{
    public event Action OnLoginSuccess;

    private Account _myAccount;
    public AccountDTO MyAccount => _myAccount.ToDto();

    private FirebaseAuth _auth => FirebaseInitialize.Auth;

    public void Register(string email, string nickname, string password)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError($"ȸ�����Կ� �����߽��ϴ�. : {task.Exception.Message}");
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.Log($"ȸ�����Կ� �����߽��ϴ�.: {result.User.DisplayName} ({result.User.UserId})");

            InitialNicknameChange(result.User, nickname);
        });
    }

    public void ChangeMyNickname(string newNickname)
    {
        FirebaseUser user = _auth.CurrentUser;

        if (user == null)
        {
            Debug.LogWarning("�α��ε� ������ �����ϴ�.");
            return;
        }
        if(user.Email != _myAccount.Email)
        {
            throw new Exception("���� ������ �ٸ��ϴ�");
        }

        UserProfile profile = new UserProfile
        {
            DisplayName = newNickname,
        };

        user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("�г��� ���濡 �����߽��ϴ�.: " + task.Exception?.Flatten().Message);
                return;
            }

            Debug.Log($"�г����� '{newNickname}'(��)�� ����Ǿ����ϴ�.");

            // UI�� ����� ���� �ִٸ� ���⼭ ����
            _myAccount.SetNickname(newNickname, out string message);
            // ��: UIManager.Instance.UpdateNickname(newNickname);
        });
    }

    private void InitialNicknameChange(FirebaseUser user, string nickname)
    {
        if (user == null)
        {
            return;
        }

        UserProfile profile = new UserProfile
        {
            DisplayName = nickname,
        };
        user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("�г��� ���濡 �����߽��ϴ�.: " + task.Exception);
                return;
            }

            Debug.Log("�г��� ���濡 �����߽��ϴ�.");
        });
    }

    public void Login(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError($"�α��ο� �����߽��ϴ�.: {task.Exception.Message}");
                return;
            }
            
            AuthResult result = task.Result;
            SetMyAccount(result.User);
            Debug.Log($"�α��� ���� : {result.User.DisplayName} ({result.User.UserId})");

            OnLoginSuccess?.Invoke();
        });
    }

    private void SetMyAccount(FirebaseUser user)
    {
        _myAccount = new Account(user.Email, user.DisplayName);
    }
}