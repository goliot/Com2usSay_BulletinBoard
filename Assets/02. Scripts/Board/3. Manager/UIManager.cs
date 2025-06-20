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
    
    // 타입 변경: GameObject -> UI_PopUp
    [SerializeField] private UI_PopUp _loginPanel;
    [SerializeField] private UI_PopUp _registerPanel;
    [SerializeField] private UI_PopUp _bulletinBoardPanel;
    [SerializeField] private UI_PopUp _postPanel;
    [SerializeField] private UI_PopUp _writePostPanel;
    [SerializeField] private UI_PopUp _editPostPanel;
    [SerializeField] private UI_PopUp _loadingPanel;

    [SerializeField] private GameObject _panel_Loading;

    private Dictionary<EUIPanelType, UI_PopUp> _panels;

    protected override void Awake()
    {
        base.Awake();

        _panels = new Dictionary<EUIPanelType, UI_PopUp>
        {
            { EUIPanelType.Login, _loginPanel },
            { EUIPanelType.Register, _registerPanel },
            { EUIPanelType.BulletinBoard, _bulletinBoardPanel },
            { EUIPanelType.Post, _postPanel },
            { EUIPanelType.WritePost, _writePostPanel },
            { EUIPanelType.EditPost, _editPostPanel }
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

            if (pair.Key == panelType) pair.Value.Show();
            else if (pair.Value.gameObject.activeSelf) pair.Value.Hide();
        }
        
        bool isLogin = _loginPanel != null && _loginPanel.gameObject.activeSelf;

        _warningMessageText?.gameObject.SetActive(isLogin);
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
            
            panel.Hide();
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