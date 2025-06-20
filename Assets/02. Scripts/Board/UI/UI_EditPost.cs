using TMPro;
using UnityEngine;

public class UI_EditPost : UI_PopUp
{
    [SerializeField] private TMP_InputField contentText;
    private Post _post;

    public override void Show() { base.Show(); }
    public override void Hide() { base.Hide(); }

    private void OnEnable()
    {
        Initialize();
    }
    private void Initialize()
    {
        _post = PostManager.Instance.CurrentPost;
        contentText.text = _post.Content;
    }
    public async void OnCommitButtonClicked()
    {
        UIManager.Instance.ShowLoading(true);
        bool result = await PostManager.Instance.UpdatePost(contentText.text);
        UIManager.Instance.ShowLoading(false);
        if (result)
        {
            ClearText();
            UIManager.Instance.OpenPanel(EUIPanelType.BulletinBoard);
        }
        else
        {
            UIManager.Instance.ShowError("글 수정에 실패했습니다.");
        }
    }
    public void OnBackButtonClicked()
    {
        ClearText();
        UIManager.Instance.OpenPanel(EUIPanelType.BulletinBoard);
    }

    private void ClearText()
    {
        contentText.text = string.Empty;
    }
}
