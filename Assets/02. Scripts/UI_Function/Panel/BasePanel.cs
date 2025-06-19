using UnityEngine;

/// <summary>
/// 모든 UI 패널이 상속하는 추상 클래스입니다.
/// IPanel 인터페이스의 기본 구현을 제공하며, OnShow/OnHide 콜백을 처리합니다.
/// </summary>
public abstract class BasePanel : MonoBehaviour, IPanel
{
    public virtual void OnShow(object parameter = null)
    {
        gameObject.SetActive(true);
    }

    public virtual void OnHide()
    {
        gameObject.SetActive(false);
    }
}
