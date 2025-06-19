using Firebase.Firestore;
using System;

[FirestoreData]
public class Account
{
    private readonly string _email;
    [FirestoreProperty] public string Email => _email;

    private readonly string _password;
    [FirestoreProperty] public string Password => _password;

    [FirestoreProperty] public string Nickname { get; }

    public Account(string email, string nickname, string password)
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

        // 패스워드 검증
        var passwordSpecification = new AccountPasswordSpecification();
        if (!passwordSpecification.IsSatisfiedBy(password))
        {
            throw new Exception(passwordSpecification.ErrorMessage);
        }

        _email = email;
        Nickname = nickname;
        _password = password;
    }
}
