using TMPro;
using UnityEngine;

public class UI_EditPost : MonoBehaviour
{
    [SerializeField] private TMP_InputField contentText;

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
