using UnityEngine;
using System.Threading.Tasks;

public class AccountManagerTest : MonoBehaviour
{
    private async void Start()
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        // 회원가입 테스트
        bool registerResult = await AccountManager.Instance.RegisterAsync("test@test.com", "testNick", "123456");
        Debug.Log("회원가입 결과: " + registerResult);

        // 로그인 테스트
        bool loginResult = await AccountManager.Instance.LoginAsync("test@test.com", "123456");
        Debug.Log("로그인 결과: " + loginResult);

        if (loginResult)
        {
            // 닉네임 변경 테스트
            bool changeNickResult = await AccountManager.Instance.ChangeMyNicknameAsync("newNick");
            Debug.Log("닉네임 변경 결과: " + changeNickResult);

            // 로그아웃 테스트
            AccountManager.Instance.Logout();
            Debug.Log("로그아웃 완료");
        }
    }
}
