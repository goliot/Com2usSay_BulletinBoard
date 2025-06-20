using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public enum EUIPanelType
{
    Login,
    Register,
    BulletinBoard,
    Post,
    WritePost,
    EditPost
}

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI _warningMessageText;
    [SerializeField] private GameObject _panel_Login;          // 로그인
    [SerializeField] private GameObject _panel_Register;       // 회원가입
    [SerializeField] private GameObject _panel_BulletinBoard;  // 전체 글 목록
    [SerializeField] private GameObject _panel_Post;           // 글 상세 보기
    [SerializeField] private GameObject _panel_WritePost;      // 글 쓰기
    [SerializeField] private GameObject _panel_EditPost;       // 글 수정

    [SerializeField] private GameObject _panel_Loading;

    private Dictionary<EUIPanelType, GameObject> _panels;

    protected override void Awake()
    {
        base.Awake();

        _panels = new Dictionary<EUIPanelType, GameObject>
        {
            { EUIPanelType.Login, _panel_Login },
            { EUIPanelType.Register, _panel_Register },
            { EUIPanelType.BulletinBoard, _panel_BulletinBoard },
            { EUIPanelType.Post, _panel_Post },
            { EUIPanelType.WritePost, _panel_WritePost },
            { EUIPanelType.EditPost, _panel_EditPost }
        };
    }

    public void OpenPostPanel(PostDTO post)
    {
        if (_panels.TryGetValue(EUIPanelType.Post, out var panel) && panel != null)
        {
            var uiPost = panel.GetComponent<UI_Post>();
            if (uiPost != null)
            {
                uiPost.SetPost(post);
            }
        }

        OpenPanel(EUIPanelType.Post);
    }


    /// <summary>
    /// 지정한 패널만 열고 나머지는 닫습니다.
    /// </summary>
    public void OpenPanel(EUIPanelType panelType)
    {
        ShowError(string.Empty);
        foreach (var pair in _panels)
        {
            if (pair.Value == null)
                continue;
            pair.Value.SetActive(pair.Key == panelType);
        }

        _warningMessageText.gameObject.SetActive(_panel_Login.activeSelf && _panel_Login.activeSelf);
    }

    /// <summary>
    /// 모든 패널을 닫습니다.
    /// </summary>
    public void CloseAllPanels()
    {
        ShowError(string.Empty);
        foreach (var panel in _panels.Values)
        {
            if (panel == null)
                continue;
            panel.SetActive(false);
        }
    }

    public void ShowError(string message)
    {
        _warningMessageText.text = message;
    }

    public void ShowLoading(bool flag)
    {
        ShowError(string.Empty);
        //_panel_Loading.SetActive(flag);
    }
}