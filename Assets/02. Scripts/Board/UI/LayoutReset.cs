using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class LayoutReset : MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
    }

    private void OnTransformChildrenChanged()
    {
        StartCoroutine(DelayedRebuild());
    }

    private IEnumerator DelayedRebuild()
    {
        yield return new WaitForEndOfFrame();

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);

        foreach (Transform child in _rectTransform)
        {
            if (child is RectTransform crt) LayoutRebuilder.ForceRebuildLayoutImmediate(crt);
        }
    }
}
