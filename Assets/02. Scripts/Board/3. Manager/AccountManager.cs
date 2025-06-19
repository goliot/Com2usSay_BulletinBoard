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
                Debug.LogError($"회원가입에 실패했습니다. : {task.Exception.Message}");
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.Log($"회원가입에 성공했습니다.: {result.User.DisplayName} ({result.User.UserId})");

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
                Debug.LogError("닉네임 변경에 실패했습니다.: " + task.Exception);
                return;
            }

            Debug.Log("닉네임 변경에 성공했습니다.");
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
                Debug.LogError("닉네임 변경에 실패했습니다.: " + task.Exception);
                return;
            }

            Debug.Log("닉네임 변경에 성공했습니다.");
        });
    }

    public void Login(Account account)
    {
        _auth.SignInWithEmailAndPasswordAsync(account.Email, account.Password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError($"로그인에 실패했습니다.: {task.Exception.Message}");
                return;
            }

            AuthResult result = task.Result;
            Debug.Log($"로그인 성공 : {result.User.DisplayName} ({result.User.UserId})");
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