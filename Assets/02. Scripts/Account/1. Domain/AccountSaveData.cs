[System.Serializable]
public class AccountSaveData
{
    public string Email;
    public string Nickname;
    public string Password;

    public AccountSaveData(AccountDTO accountDto)
    {
        Email = accountDto.Email;
        Nickname = accountDto.Nickname;
        Password = accountDto.Password;
    }
}