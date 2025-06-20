using UnityEngine;
using System.Collections.Generic;

public class UI_BulletinBoard : UI_PopUp
{
    [SerializeField] private GameObject _sidePanel;
    [SerializeField] private GameObject _postItemPrefab;
    [SerializeField] private Transform _postListContainer;

    public override void Show() { base.Show(); }
    public override void Hide() { base.Hide(); }

    private void OnEnable()
    {
        Refresh();
    }
    public void OnOrderButtonClick()
    {

    }

    public void OnWriteButtonClick()
    {
        Debug.Log("작성시작");
        UIManager.Instance.OpenPanel(EUIPanelType.WritePost);
    }

    public void OnSideButtonClick()
    {
        _sidePanel.SetActive(true);
    }

    public async void Refresh()
    {
        UIManager.Instance.ShowLoading(true);
        List<Post> posts = await PostManager.Instance.GetAllPosts();

        foreach (Transform child in _postListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var post in posts)
        {
            GameObject postItemGO = Instantiate(_postItemPrefab, _postListContainer);
            UI_PostItem postItemUI = postItemGO.GetComponent<UI_PostItem>();
            postItemUI.Initialize(post); 
            postItemUI.OnChanged += () => Refresh();
        }

        UIManager.Instance.ShowLoading(false);
    }
}
