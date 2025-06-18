using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[Serializable]
public struct AccountInputFields
{
    public TMP_InputField EmailInputField;
    public TMP_InputField NicknameInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PasswordCheckInputField;
    public Button ConfirmButton;

    public List<TMP_InputField> Fields { get; private set; }

    public void InitFields()
    {
        Fields = new List<TMP_InputField>();

        if (EmailInputField != null)
        {
            Fields.Add(EmailInputField);
        }

        if (NicknameInputField != null)
        {
            Fields.Add(NicknameInputField);
        }

        if (PasswordInputField != null)
        {
            Fields.Add(PasswordInputField);
        }

        if (PasswordCheckInputField != null)
        {
            Fields.Add(PasswordCheckInputField);
        }
    }

    public void Clear()
    {
        foreach(var input in Fields)
        {
            input.text = string.Empty;
        }
    }
}

public class UI_LoginScene : MonoBehaviour
{
    [SerializeField] private string _nextSceneName = "MainScene";

    [Header ("# Panel")]
    [SerializeField] private GameObject _loginPanel;
    [SerializeField] private GameObject _registerPanel;
    [SerializeField] private TextMeshProUGUI _resultText;
    private Vector2 _resultOriginAnchoredPos;
    private AccountInputFields _nowPanel;

    [Header("# Login")]
    [SerializeField] private AccountInputFields _loginFields;

    [Header("# Register")]
    [SerializeField] private AccountInputFields _registerFields;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        _loginFields.InitFields();
        _registerFields.InitFields();
        _loginPanel.SetActive(true);
        _registerPanel.SetActive(false);
        _nowPanel = _loginFields;
        _resultOriginAnchoredPos = _resultText.rectTransform.anchoredPosition;
        SetResultText("");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < _nowPanel.Fields.Count; i++)
            {
                if (_nowPanel.Fields[i].isFocused)
                {
                    int nextIndex = (i + 1) % _nowPanel.Fields.Count;
                    TMP_InputField nextField = _nowPanel.Fields[nextIndex];

                    // 강제로 다음 필드 선택
                    EventSystem.current.SetSelectedGameObject(nextField.gameObject);

                    // 선택 후에 강제로 포커스 설정
                    nextField.OnPointerClick(new PointerEventData(EventSystem.current));

                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _nowPanel.ConfirmButton.onClick?.Invoke();
            if(_nowPanel.ConfirmButton.TryGetComponent<UI_TouchBounce>(out var bounce)) 
            {
                bounce.Bounce();
            }
        }
    }

    public void OnClickGoToLoginButton()
    {
        _loginPanel.SetActive(true);
        _registerPanel.SetActive(false);
        _nowPanel = _loginFields;
        _nowPanel.Clear();
    }

    public void OnClickGoToRegisterButton()
    {
        _loginPanel.SetActive(false);
        _registerPanel.SetActive(true);
        _nowPanel = _registerFields;
        _nowPanel.Clear();
    }

    public void Register()
    {
        string email = _registerFields.EmailInputField.text;
        string nickname = _registerFields.NicknameInputField.text;
        string pw = _registerFields.PasswordInputField.text;
        string pwCheck = _registerFields.PasswordCheckInputField.text;

        if (string.IsNullOrWhiteSpace(email))
        {
            SetResultText("이메일를 입력해주세요.", false);
            return;
        }
        else if (string.IsNullOrWhiteSpace(nickname))
        {
            SetResultText("닉네임을 입력해주세요.", false);
            return;
        }
        else if (string.IsNullOrWhiteSpace(pw))
        {
            SetResultText("비밀번호를 입력해주세요.", false);
            return;
        }
        else if (string.IsNullOrWhiteSpace(pwCheck))
        {
            SetResultText("비밀번호 확인을 입력해주세요.", false);
            return;
        }
        else if (pw != pwCheck)
        {
            SetResultText("비밀번호와 확인이 일치하지 않습니다.", false);
            return;
        }

        if (AccountManager.Instance.TryRegister(email, nickname, pw, out string errorMessage))
        {
            SetResultText(errorMessage, true);
            OnClickGoToLoginButton();
            _loginFields.EmailInputField.text = email;
            _loginFields.PasswordInputField.text = string.Empty;
        }
        else
        {
            SetResultText(errorMessage, false);
        }
    }

    public void Login()
    {
        string email = _loginFields.EmailInputField.text;
        string pw = _loginFields.PasswordInputField.text;

        if (string.IsNullOrWhiteSpace(email))
        {
            SetResultText("이메일를 입력해주세요.", false);
            return;
        }
        else if (string.IsNullOrWhiteSpace(pw))
        {
            SetResultText("비밀번호를 입력해주세요.", false);
            return;
        }

        if(AccountManager.Instance.TryLogin(email, pw, out string message))
        {
            SetResultText(message, true);
            SceneManager.LoadScene(_nextSceneName);
            //SceneLoader.LoadSceneWithLoading(_nextSceneName);
        }
        else
        {
            SetResultText(message, false);
        }
    }

    private void SetResultText(string s, bool isSuccess = false)
    {
        _resultText.text = s;
        _resultText.color = isSuccess ? Color.green : Color.red;
        _resultText.DOKill();
        _resultText.rectTransform.anchoredPosition = _resultOriginAnchoredPos;
        if (!isSuccess)
        {
            _resultText.rectTransform.DOShakeAnchorPos(0.3f, 30, 100).OnComplete(() =>
                _resultText.rectTransform.anchoredPosition = _resultOriginAnchoredPos);
        }
        else
        {
            Vector3 originalScale = _resultText.rectTransform.localScale;
            _resultText.rectTransform.DOShakeScale(0.3f).OnComplete(() =>
                _resultText.rectTransform.localScale = originalScale);
        }
    }
}