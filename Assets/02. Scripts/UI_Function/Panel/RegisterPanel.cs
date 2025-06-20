using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 회원가입 화면의 UI 기능을 담당합니다.
/// 입력 필드 검증 후 완료 시 로그인 화면 또는 기본 화면으로 전환.
/// </summary>
public class RegisterPanel : BasePanel
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField nicknameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField passwordConfirmField;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button cancelButton;

    private void Awake()
    {
        registerButton.onClick.AddListener(OnRegisterClicked);
        cancelButton.onClick.AddListener(OnCancelClicked);
    }

    public override void OnShow(object parameter = null)
    {
        base.OnShow(parameter);
        emailField.text = string.Empty;
        nicknameField.text = string.Empty;
        passwordField.text = string.Empty;
        passwordConfirmField.text = string.Empty;
        registerButton.interactable = true;
    }

    private async void OnRegisterClicked()
    {
        string email = emailField.text;
        string nickname = nicknameField.text;
        string pwd = passwordField.text;
        string pwdConfirm = passwordConfirmField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(pwdConfirm))
        {
            Debug.LogWarning("All fields are required.");
            return;
        }
        if (pwd != pwdConfirm)
        {
            Debug.LogWarning("Passwords do not match.");
            return;
        }

        Debug.Log($"Register attempt: {email}");
        registerButton.interactable = false;

        AccountResult registerResult = await AccountManager.Instance.RegisterAsync(email, nickname, pwd);

        if (registerResult.Success)
        {
            // 회원가입 성공 시
            UIManager.Instance.ClosePanel();            // 현재 Register 패널 닫고
            UIManager.Instance.OpenPanel("Login");   // Login 화면으로 이동
        }
        else
        {
            registerButton.interactable = true;
            Debug.LogError(registerResult.ErrorMessage);
            return;
        }

    }

    private void OnCancelClicked()
    {
        UIManager.Instance.ClosePanel();
    }
}