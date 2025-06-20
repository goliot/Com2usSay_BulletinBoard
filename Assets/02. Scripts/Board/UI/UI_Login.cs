using UnityEngine;

public class UI_Login : MonoBehaviour
{
    public async void OnLoginButtonClicked(string email, string password)
    {
        UIManager.Instance.ShowLoading(true);

        bool success = await AccountManager.Instance.LoginAsync(email, password, errorMessage =>
        {
            UIManager.Instance.ShowError(errorMessage); // 실패 시 에러 메시지 출력
        });

        UIManager.Instance.ShowLoading(false);

        if (success)
        {
            UIManager.Instance.OpenPanel(EUIPanelType.BulletinBoard); // 성공 시 메인 UI로 전환
        }
        // 실패 시는 onFail 콜백에서 처리되므로 else 생략 가능
    }
}
