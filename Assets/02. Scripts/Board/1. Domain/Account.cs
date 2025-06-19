using Firebase.Firestore;
using System;

[FirestoreData]
public class Account
{
    private readonly string _email;
    [FirestoreProperty] public string Email => _email;

    //private readonly string _password;
    //[FirestoreProperty] public string Password => _password;

    [FirestoreProperty] public string Nickname { get; private set; }

    public Account(string email, string nickname)
    {
        // 이메일 검증
        var emailSpecification = new AccountEmailSpecification();
        if (!emailSpecification.IsSatisfiedBy(email))
        {
            throw new Exception(emailSpecification.ErrorMessage);
        }

        // 닉네임 검증
        var nicknameSpecification = new AccountNicknameSpecification();
        if (!nicknameSpecification.IsSatisfiedBy(nickname))
        {
            throw new Exception(nicknameSpecification.ErrorMessage);
        }

        _email = email;
        Nickname = nickname;
        //_password = password;
    }

    public AccountDTO ToDto()
    {
        return new AccountDTO(this);
    }

    public void SetNickname(string newNickname, out string message)
    {
        var nicknameSpecification = new AccountNicknameSpecification();
        if (!nicknameSpecification.IsSatisfiedBy(newNickname))
        {
            message = nicknameSpecification.ErrorMessage;
            return;
        }

        Nickname = newNickname;
        message = "닉네임 변경에 성공했습니다.";
    }
}
