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
                Debug.LogError($"회원가입에 실패했습니다. : {task.Exception.Message}");
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.Log($"회원가입에 성공했습니다.: {result.User.DisplayName} ({result.User.UserId})");

            InitialNicknameChange(result.User, nickname);
        });
    }

    public void ChangeMyNickname(string newNickname)
    {
        FirebaseUser user = _auth.CurrentUser;

        if (user == null)
        {
            Debug.LogWarning("로그인된 유저가 없습니다.");
            return;
        }
        if(user.Email != _myAccount.Email)
        {
            throw new Exception("유저 정보가 다릅니다");
        }

        UserProfile profile = new UserProfile
        {
            DisplayName = newNickname,
        };

        user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("닉네임 변경에 실패했습니다.: " + task.Exception?.Flatten().Message);
                return;
            }

            Debug.Log($"닉네임이 '{newNickname}'(으)로 변경되었습니다.");

            // UI나 저장된 모델이 있다면 여기서 갱신
            _myAccount.SetNickname(newNickname, out string message);
            // 예: UIManager.Instance.UpdateNickname(newNickname);
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
                Debug.LogError("닉네임 변경에 실패했습니다.: " + task.Exception);
                return;
            }

            Debug.Log("닉네임 변경에 성공했습니다.");
        });
    }

    public void Login(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError($"로그인에 실패했습니다.: {task.Exception.Message}");
                return;
            }
            
            AuthResult result = task.Result;
            SetMyAccount(result.User);
            Debug.Log($"로그인 성공 : {result.User.DisplayName} ({result.User.UserId})");

            OnLoginSuccess?.Invoke();
        });
    }

    private void SetMyAccount(FirebaseUser user)
    {
        _myAccount = new Account(user.Email, user.DisplayName);
    }
}