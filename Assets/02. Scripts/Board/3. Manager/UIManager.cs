using UnityEngine;
using System;
using System.Collections.Generic;

public class UIManger : Singleton<UIManger>
{
    [SerializeField] GameObject Panel_Login;          // 로그인
    [SerializeField] GameObject Panel_Register;       // 회원가입
    [SerializeField] GameObject Panel_BulletinBoard;  // 전체 글 목록
    [SerializeField] GameObject Panel_Post;           // 글 상세 보기
    [SerializeField] GameObject Panel_WritePost;      // 글 쓰기
    [SerializeField] GameObject Panel_EditPost;       // 글 수정

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
            { EUIPanelType.Login, Panel_Login },
            { EUIPanelType.Register, Panel_Register },
            { EUIPanelType.BulletinBoard, Panel_BulletinBoard },
            { EUIPanelType.Post, Panel_Post },
            { EUIPanelType.WritePost, Panel_WritePost },
            { EUIPanelType.EditPost, Panel_EditPost }
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
}