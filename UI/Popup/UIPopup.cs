using UnityEngine;
using UnityEngine.EventSystems;

public class UIPopup : MonoBehaviour, IPointerDownHandler
{
    RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // 팝업이 클릭되었을 때 호출되는 이벤트 핸들러
    public void OnPointerDown(PointerEventData eventData)
    {
        _rectTransform.SetAsLastSibling(); // 팝업이 가장 위에 오도록 설정
    }

    public void ShowPopup()
    {
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();

        _rectTransform.SetAsLastSibling(); // 팝업이 가장 위에 오도록 설정
        gameObject.SetActive(true);
    }

    public void HidePopup()
    {
        gameObject.SetActive(false);
    }
}
