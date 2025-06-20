using TMPro;
using UnityEngine;
using System;

public class UI_WritePost : UI_PopUp
{
    [SerializeField] private TMP_InputField contentText;

    public override void Show() { base.Show(); }
    public override void Hide() { base.Hide(); }

    public async void OnCommitButtonClicked()
    {
        UIManager.Instance.ShowLoading(true);
        bool result = await PostManager.Instance.CreatePost(Guid.NewGuid().ToString(),contentText.text);
        UIManager.Instance.ShowLoading(false);
        if (result)
        {
            ClearText();
            UIManager.Instance.OpenPanel(EUIPanelType.BulletinBoard);
        }
        else
        {
            UIManager.Instance.ShowError("글 생성에 실패했습니다.");
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
