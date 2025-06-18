using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;
using System.Collections.Generic;
using System;

public class FirebaseTest : MonoBehaviour
{
    private FirebaseApp _app;
    private FirebaseAuth _auth;
    private FirebaseFirestore _db;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("파이어베이스 연결에 성공했습니다.");
                _app = FirebaseApp.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;
                _db = FirebaseFirestore.DefaultInstance;

                //Register();
                Login();
            }
            else
            {
                Debug.LogError(string.Format($"파이어베이스 연결에 실패했습니다. : {dependencyStatus}"));
            }
        });
    }

    private void Register()
    {
        string email = "teemo@teemo.com";
        string password = "123456";

        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError($"회원가입에 실패했습니다. : {task.Exception.Message}");
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.Log($"회원가입에 성공했습니다.: {result.User.DisplayName} ({result.User.UserId})");
        });
    }

    private void Login()
    {
        string email = "teemo@teemo.com";
        string password = "123456";

        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError($"로그인에 실패했습니다.: {task.Exception.Message}");
                return;
            }

            AuthResult result = task.Result;
            Debug.Log($"로그인 성공 : {result.User.DisplayName} ({result.User.UserId})");
            NicknameChange();
        });
    }

    private void NicknameChange()
    {
        FirebaseUser user = _auth.CurrentUser;
        if (user == null)
        {
            return;
        }

        UserProfile profile = new UserProfile
        {
            DisplayName = "Teemo",
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

    private Account GetProfile()
    {
        FirebaseUser user = _auth.CurrentUser;
        if(user == null)
        {
            return null;
        }

        string nickname = user.DisplayName;
        string email = user.Email;

        Account account = new Account(email, nickname, "firebase");

        return account;
    }
}
