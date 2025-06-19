using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;
using System.Collections.Generic;
using System;


public class AccountManager : Singleton<AccountManager>
{
    private FirebaseAuth _auth => FirebaseInitialize.Auth;

    public void Register(Account account)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(account.Email, account.Password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError($"ȸ�����Կ� �����߽��ϴ�. : {task.Exception.Message}");
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.Log($"ȸ�����Կ� �����߽��ϴ�.: {result.User.DisplayName} ({result.User.UserId})");

            InitialNicknameChange(result.User, account.Nickname);
        });
    }

    /*private void ChangeMyNickname(Account account, string newNickname)
    {
        if(account.Email != GetMyProfile().Email)
        {
            return;
        }
        FirebaseUser user = _auth.CurrentUser;

        UserProfile profile = new UserProfile
        {
            DisplayName = newNickname,
        };
        user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("�г��� ���濡 �����߽��ϴ�.: " + task.Exception);
                return;
            }

            Debug.Log("�г��� ���濡 �����߽��ϴ�.");
        });
    }*/

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

    public void Login(Account account)
    {
        _auth.SignInWithEmailAndPasswordAsync(account.Email, account.Password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError($"�α��ο� �����߽��ϴ�.: {task.Exception.Message}");
                return;
            }

            AuthResult result = task.Result;
            Debug.Log($"�α��� ���� : {result.User.DisplayName} ({result.User.UserId})");
        });
    }

    public Account GetMyProfile()
    {
        FirebaseUser user = _auth.CurrentUser;
        if (user == null)
        {
            return null;
        }

        string nickname = user.DisplayName;
        string email = user.Email;

        Account account = new Account(email, nickname, "confidential");

        return account;
    }
}