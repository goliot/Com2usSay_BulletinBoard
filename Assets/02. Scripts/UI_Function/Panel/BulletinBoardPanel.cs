// Assets/02. Scripts/UI_Function/Panel/BulletinBoardPanel.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BulletinBoardPanel : BasePanel
{
    [SerializeField] private GameObject postItemPrefab;
    private Transform _postListContent;
    private Button _writeButton;
    private Button _sidePanelButton;
    private readonly List<GameObject> _spawnedItems = new List<GameObject>();
    private bool _isInitialized;

    public override void OnShow(object parameter = null)
    {
        if (!_isInitialized) Initialize();
        base.OnShow(parameter);

        LoadAndDisplayPosts();
    }

    private void Initialize()
    {
        var root = transform;
        _postListContent = root.Find("Panel_PostList/Scroll View/Viewport/Content");
        _writeButton = root.Find("Button_Write").GetComponent<Button>();
        _sidePanelButton = root.Find("Button_SidePanelOpen").GetComponent<Button>();

        _writeButton.onClick.AddListener(() => UIManagerFuck.Instance.OpenPanel("Panel_WritePost"));
        _sidePanelButton.onClick.AddListener(() => UIManagerFuck.Instance.OpenPanel("Panel_SidePanel"));

        _isInitialized = true;
    }

    private void LoadAndDisplayPosts()
    {
        // TODO: 이 부분에 FirebaseService.LoadAllPostsAsync() 호출 등으로 데이터를 가져오기
        List<Post> posts = new List<Post>
        {
          // 임시 더미 데이터
          new Post("1", "제목1", "첫 번째 글입니다.", "Alice"),
          new Post("2", "제목2", "두 번째 글입니다.", "Bob"),
        };

        //  화면 갱신
        foreach (var go in _spawnedItems) Destroy(go);
        _spawnedItems.Clear();

        foreach (var data in posts)
        {
            var go = Instantiate(postItemPrefab, _postListContent);
            var ctrl = go.GetComponent<PostItemController>();
            if (ctrl != null)
                ctrl.Setup(data.ToDto());
            else
                Debug.LogWarning("[BulletinBoardPanel] PostItemController가 없음!");
            _spawnedItems.Add(go);
        }
    }
}
