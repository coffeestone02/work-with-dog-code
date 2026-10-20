using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleBarDragHandler : MonoBehaviour, IPointerDownHandler
{
    const int WM_NCLBUTTONDOWN = 0xA1;
    const int HTCAPTION = 2;

    public void OnPointerDown(PointerEventData eventData)
    {
        // os에서 창 이동 메시지 처리하도록 보내기
        WindowManager.ReleaseCapture();
        WindowManager.SendMessage(Managers.Window.Hwnd, WM_NCLBUTTONDOWN, (IntPtr)HTCAPTION, IntPtr.Zero);
    }
}
