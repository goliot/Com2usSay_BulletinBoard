using TMPro;
using UnityEngine;

public class UI_WritePost : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentText;

    public async void OnCommitButtonClicked()
    {
        UIManager.Instance.ShowLoading(true);
        bool result = await PostManager.Instance.CreatePost("",contentText.text);
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
