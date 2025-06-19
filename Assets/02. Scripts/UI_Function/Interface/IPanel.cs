using UnityEngine;

/// <summary>
/// UI 패널의 생명주기 콜백을 정의하는 인터페이스.
/// 모든 UI 패널은 이 인터페이스를 구현해야 합니다.
/// </summary>
public interface IPanel
{
    /// <summary>
    /// 패널이 화면에 보여질 때 호출됩니다.
    /// </summary>
    /// <param name="parameter">
    /// 화면 전환 시 전달할 데이터가 있으면 여기로 받고, 없으면 null로 호출하세요.
    /// </param>
    void OnShow(object parameter = null);

    /// <summary>
    /// 패널이 화면에서 사라지기 직전에 호출됩니다.
    /// </summary>
    void OnHide();
}