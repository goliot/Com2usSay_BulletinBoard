using System.Threading.Tasks;
using System;

public class AccountManager : Singleton<AccountManager>
{
    public event Action OnLoginSuccess;
    public event Action OnLogout;

    private AccountRepository _repository = new AccountRepository();

    public AccountDTO MyAccount => _repository.MyAccount;

    public async Task<bool> RegisterAsync(string email, string nickname, string password, Action<string> onFail = null)
    {
        var (success, errorMessage) = await _repository.RegisterAsync(email, nickname, password);
        if (!success)
        {
            onFail?.Invoke(errorMessage);
        }
        return success;
    }

    public async Task<bool> LoginAsync(string email, string password, Action<string> onFail = null)
    {
        var (success, errorMessage) = await _repository.LoginAsync(email, password);
        if (success)
        {
            OnLoginSuccess?.Invoke();
        }
        else
        {
            onFail?.Invoke(errorMessage);
        }
        return success;
    }

    public void Logout()
    {
        _repository.Logout();
        OnLogout?.Invoke();
    }

    public async Task<bool> ChangeMyNicknameAsync(string newNickname, Action<string> onFail = null)
    {
        var (success, errorMessage) = await _repository.ChangeMyNicknameAsync(newNickname);
        if (!success)
        {
            onFail?.Invoke(errorMessage);
        }
        return success;
    }
}
