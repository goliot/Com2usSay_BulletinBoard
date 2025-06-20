using UnityEngine;

public class UI_Login : MonoBehaviour
{
    public async void OnLoginButtonClicked(string email, string password)
    {
        UIManager.Instance.ShowLoading(true);

        AccountResult result = await AccountManager.Instance.LoginAsync(email, password);

        UIManager.Instance.ShowLoading(false);

        if (result.Success)
        {
            UIManager.Instance.OpenPanel(EUIPanelType.BulletinBoard); // 성공 시 메인 UI로 전환
        }
        else
        {
            UIManager.Instance.ShowError(result.ErrorMessage);
            UIManager.Instance.OpenPanel(EUIPanelType.Login);
        }
        // 실패 시는 onFail 콜백에서 처리되므로 else 생략 가능
    }
}
