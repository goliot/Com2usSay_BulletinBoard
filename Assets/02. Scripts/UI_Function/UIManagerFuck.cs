using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManagerFuck : MonoBehaviour
{
    public static UIManagerFuck Instance { get; private set; }

    [Header("UI Root (Canvas) - Assign your Canvas Transform here")]
    [Tooltip("Instantiate할 패널의 부모로 사용할 Transform (Canvas)")]
    [SerializeField] private Transform _uiRoot;

    [Serializable]
    public struct PanelInfo
    {
        public string Key;
        public GameObject Prefab;
    }

    [Header("Panel Prefabs (Assign in Inspector)")]
    [SerializeField] private PanelInfo[] _panelInfos;

    [Header("Initial & Default Panels")]
    [SerializeField] private string _initialPanelKey = "Panel_Login";
    [SerializeField] private string _defaultPanelKey = "Panel_BulletinBoard";

    private readonly Dictionary<string, BasePanel> _panelCache = new Dictionary<string, BasePanel>();
    private readonly Stack<string> _panelStack = new Stack<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_uiRoot == null)
        {
            var canvas = FindAnyObjectByType<Canvas>();
            _uiRoot = canvas != null ? canvas.transform : transform;
        }

        // 씬에 배치된 모든 BasePanel 컴포넌트를 캐시에 등록
        var existingPanels = _uiRoot.GetComponentsInChildren<BasePanel>(true);
        foreach (var panel in existingPanels)
        {
            var key = panel.gameObject.name;
            if (!_panelCache.ContainsKey(key))
            {
                panel.gameObject.SetActive(false);
                _panelCache[key] = panel;
            }
        }
    }

    private void Start()
    {
        OpenPanel(_initialPanelKey);
    }

    /// <summary>
    /// 지정된 key의 패널을 연다.
    /// 씬에 있던 패널은 Instantiate 없이 재사용. 없으면 Prefab으로 생성하여 캐시.
    /// </summary>
    public void OpenPanel(string key, object parameter = null)
    {
        // 1) 현재 패널 숨기기
        if (_panelStack.Count > 0)
            _panelCache[_panelStack.Peek()]?.OnHide();

        BasePanel panel;
        if (_panelCache.ContainsKey(key) && _panelCache[key] != null)
        {
            panel = _panelCache[key];
        }
        else
        {
            // 캐시에 없으면 Prefab으로 Instantiate
            var info = _panelInfos.FirstOrDefault(p => p.Key == key);
            if (string.IsNullOrEmpty(info.Key) || info.Prefab == null)
            {
                Debug.LogError($"[UIManager] '{key}'에 해당하는 Prefab이 없거나 할당되지 않음");
                return;
            }

            var go = Instantiate(info.Prefab, _uiRoot);
            panel = go.GetComponent<BasePanel>();
            if (panel == null)
            {
                Debug.LogError($"[UIManager] Prefab '{info.Prefab.name}'에 BasePanel 컴포넌트가 없음");
                Destroy(go);
                return;
            }

            panel.gameObject.SetActive(false);
            _panelCache[key] = panel;
        }

        // 2) 스택에 등록하고 OnShow 호출
        _panelStack.Push(key);
        panel.OnShow(parameter);
    }

    /// <summary>
    /// 최상위 패널을 닫고, 이전 패널을 재노출한다.
    /// </summary>
    public void ClosePanel()
    {
        if (_panelStack.Count == 0)
            return;

        var topKey = _panelStack.Pop();
        _panelCache[topKey]?.OnHide();

        if (_panelStack.Count > 0)
            _panelCache[_panelStack.Peek()]?.OnShow();
    }

    /// <summary>
    /// 모든 패널을 닫고 기본 패널을 연다.
    /// </summary>
    public void ShowDefaultPanel()
    {
        CloseAllPanels();
        OpenPanel(_defaultPanelKey);
    }

    /// <summary>
    /// 열린 모든 패널을 순서대로 닫는다.
    /// </summary>
    public void CloseAllPanels()
    {
        while (_panelStack.Count > 0)
        {
            var key = _panelStack.Pop();
            _panelCache[key]?.OnHide();
        }
    }
}
