using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 로그인 화면의 UI 기능을 담당합니다.
/// 데이터 검증 및 로그인 성공 시 기본 패널 전환 호출 예시 포함.
/// </summary>
public class LoginPanel : BasePanel
{
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;

    private void Awake()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);
    }

    public override void OnShow(object parameter = null)
    {
        base.OnShow(parameter);
        emailField.text = string.Empty;
        passwordField.text = string.Empty;
        loginButton.interactable = true;
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    private void OnLoginClicked()
    {
        // TODO: 실제 로그인 로직 연동
        string email = emailField.text;
        string password = passwordField.text;
        Debug.Log($"Login attempt: {email}");

        // 임시: 이메일/비번이 비어있지 않으면 성공 처리
        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            loginButton.interactable = false;
            UIManager.Instance.ShowDefaultPanel();
        }
        else
        {
            Debug.LogWarning("Email or Password is empty.");
        }
    }

    private void OnRegisterClicked()
    {
        UIManager.Instance.OpenPanel("Register");
    }
}
