using UnityEngine;
using System.Threading.Tasks;

public class AccountManagerTest : MonoBehaviour
{
    private async void Start()
    {
        await FirebaseInitialize.WaitForInitializationAsync();

        // ȸ������ �׽�Ʈ
        bool registerResult = await AccountManager.Instance.RegisterAsync("test@test.com", "testNick", "123456");
        Debug.Log("ȸ������ ���: " + registerResult);

        // �α��� �׽�Ʈ
        bool loginResult = await AccountManager.Instance.LoginAsync("test@test.com", "123456");
        Debug.Log("�α��� ���: " + loginResult);

        if (loginResult)
        {
            // �г��� ���� �׽�Ʈ
            bool changeNickResult = await AccountManager.Instance.ChangeMyNicknameAsync("newNick");
            Debug.Log("�г��� ���� ���: " + changeNickResult);

            // �α׾ƿ� �׽�Ʈ
            AccountManager.Instance.Logout();
            Debug.Log("�α׾ƿ� �Ϸ�");
        }
    }
}
