using DG.Tweening;
using UnityEngine;

/// <summary>
/// 팝업형 UI의 공통 트위닝 기능을 제공하는 베이스 클래스입니다.
/// </summary>
public abstract class UI_PopUp : MonoBehaviour
{
    [SerializeField] protected float _showDuration = 0.3f;
    [SerializeField] protected float _hideDuration = 0.2f;

    protected virtual void Awake()
    {
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// 팝업을 열 때 트위닝으로 보여줍니다.
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, _showDuration).SetEase(Ease.OutBack);
    }

    /// <summary>
    /// 팝업을 닫을 때 트위닝으로 숨깁니다.
    /// </summary>
    public virtual void Hide()
    {
        transform.DOScale(Vector3.zero, _hideDuration)
                 .SetEase(Ease.InBack)
                 .OnComplete(() => gameObject.SetActive(false));
    }
}
