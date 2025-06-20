using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class UIManger : Singleton<UIManger>
{
    [SerializeField] private TextMeshProUGUI _warningMessageText;
    [SerializeField] private GameObject _panel_Login;          // 로그인
    [SerializeField] private GameObject _panel_Register;       // 회원가입
    [SerializeField] private GameObject _panel_BulletinBoard;  // 전체 글 목록
    [SerializeField] private GameObject _panel_Post;           // 글 상세 보기
    [SerializeField] private GameObject _panel_WritePost;      // 글 쓰기
    [SerializeField] private GameObject _panel_EditPost;       // 글 수정

    private Dictionary<EUIPanelType, GameObject> _panels;

    public enum EUIPanelType
    {
        Login,
        Register,
        BulletinBoard,
        Post,
        WritePost,
        EditPost
    }

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

    /// <summary>
    /// 지정한 패널만 열고 나머지는 닫습니다.
    /// </summary>
    public void OpenPanel(EUIPanelType panelType)
    {
        CloseAllPanels();
        foreach (var pair in _panels)
        {
            pair.Value.SetActive(pair.Key == panelType);
        }
    }

    /// <summary>
    /// 모든 패널을 닫습니다.
    /// </summary>
    public void CloseAllPanels()
    {
        foreach (var panel in _panels.Values)
        {
            panel.SetActive(false);
        }
    }

    public void SetWarningMessage(string message)
    {
        _warningMessageText.text = message;
    }
}