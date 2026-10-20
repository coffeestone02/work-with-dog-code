using UnityEngine;
using UnityEngine.EventSystems;

public class UIPopupHeader : MonoBehaviour, IDragHandler
{
    RectTransform _popupRect;
    Canvas _canvas;

    void Start()
    {
        _popupRect = transform.parent.GetComponent<RectTransform>();
        _canvas = GameObject.Find("Popups").GetComponent<Canvas>();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.delta / _canvas.scaleFactor;
        _popupRect.anchoredPosition += delta;

        Clamp();
    }

    void Clamp()
    {
        Vector3[] corners = new Vector3[4];
        _popupRect.GetWorldCorners(corners);

        Vector2 offset = Vector2.zero;

        if (corners[0].x < 0) // 왼쪽
            offset.x -= corners[0].x;
        if (corners[2].x > Screen.width) // 오른쪽
            offset.x -= corners[2].x - Screen.width;
        if (corners[0].y < 0) // 아래
            offset.y -= corners[0].y;
        if (corners[1].y > Screen.height) // 위
            offset.y -= corners[1].y - Screen.height;

        _popupRect.position += new Vector3(offset.x, offset.y, 0);
    }
}
