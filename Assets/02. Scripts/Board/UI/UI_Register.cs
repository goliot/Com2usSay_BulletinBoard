using TMPro;
using UnityEngine;

public class UI_Register : MonoBehaviour
{
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _nicknameInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordCheckInputField;

    public void OnClickRegisterPanelExit()
    {
        UIManager.Instance.OpenPanel(EUIPanelType.Login);
    }

    public async void OnRegisterButtonClicked()
    {
        string email = _emailInputField.text;
        string nickname = _nicknameInputField.text;
        string password = _passwordInputField.text;
        string passwordCheck = _passwordCheckInputField.text;

        if (string.IsNullOrWhiteSpace(email))
        {
            UIManager.Instance.ShowError("이메일를 입력해주세요.");
            return;
        }
        else if (string.IsNullOrWhiteSpace(nickname))
        {
            UIManager.Instance.ShowError("닉네임을 입력해주세요.");
            return;
        }
        else if (string.IsNullOrWhiteSpace(password))
        {
            UIManager.Instance.ShowError("비밀번호를 입력해주세요.");
            return;
        }
        else if (string.IsNullOrWhiteSpace(passwordCheck))
        {
            UIManager.Instance.ShowError("비밀번호 확인을 입력해주세요.");
            return;
        }
        else if (password != passwordCheck)
        {
            UIManager.Instance.ShowError("비밀번호와 확인이 일치하지 않습니다.");
            return;
        }

        string message;
        Account account;
        if(!Account.TryCreate(email, nickname, out account, out message))
        {
            UIManager.Instance.ShowError(message);
            return;
        }
        
        AccountResult result = await AccountManager.Instance.RegisterAsync(email, nickname, password);
        if (result.Success)
        {
            UIManager.Instance.OpenPanel(EUIPanelType.Login);
        }
        else
        {
            UIManager.Instance.ShowError(result.ErrorMessage);
        }
    }
}