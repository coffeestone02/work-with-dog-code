using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowManager
{
    #region DLLstuff
    const uint WS_VISIBLE = 0x10000000; // 창 생성 시 바로 화면에 보이는 상태로 만듦
    const uint WS_POPUP = 0x80000000; // 테두리 없는 창
    const uint SWP_FRAMECHANGED = 0x0020; // SetWindowLong으로 스타일 변경 직후 변경된 프레임을 다시 계산
    const int SWP_SHOWWINDOW = 0x40; // 위치, 크기 변경과 동시에 창을 보이는 상태로 표시
    const int SW_MINIMIZE = 6; // 창을 최소화
    const int GWL_STYLE = -16;

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(
        IntPtr hWnd, // window handle
        IntPtr hWndInsertAfter, // z order
        int X, // x position
        int Y, // y position
        int cx, // width
        int cy, // height
        uint uFlags // window flags.
    );

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLongPtr(
         IntPtr hWnd, // window handle
         int nIndex,
         uint dwNewLong
    );

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

    public IntPtr Hwnd;

    IntPtr HWND_TOPMOST = new IntPtr(-1);
    IntPtr HWND_NOTOPMOST = new IntPtr(-2);

    #endregion

    int _width;
    int _height;
    int _pipWidth = 400;
    int _pipHeight = 300;
    Camera _camera;

    public void Init()
    {
        if (Application.isEditor) return;
        Hwnd = GetActiveWindow();
        SetDefaultCamera();

        _width = Screen.currentResolution.width;
        _height = Screen.currentResolution.height;

        // 레지스트리 설정
        PlayerPrefs.SetInt("Screenmanager Resolution Width", _width);
        PlayerPrefs.SetInt("Screenmanager Resolution Height", _height);
        PlayerPrefs.Save();

        SetWindowLongPtr(Hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        SetWindowPos(Hwnd, HWND_NOTOPMOST, 0, 0, _width, _height,
                     SWP_SHOWWINDOW | SWP_FRAMECHANGED);

        _camera = Camera.main;
    }

    public void Restore()
    {
        if (Application.isEditor) return;
        Managers.UI.DeactivePIP();
        SetDefaultCamera();

        SetWindowLongPtr(Hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        SetWindowPos(Hwnd, HWND_NOTOPMOST, 0, 0, _width, _height,
                     SWP_SHOWWINDOW | SWP_FRAMECHANGED);
    }

    public void PIP()
    {
        if (Application.isEditor) return;
        Managers.UI.ActivePIP();
        SetPIPCamera();

        int x = _width - _pipWidth; // 화면 오른쪽 아래에 위치하도록
        int y = _height - _pipHeight - 50;

        SetWindowLongPtr(Hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        SetWindowPos(Hwnd, HWND_TOPMOST, x, y, _pipWidth, _pipHeight,
                     SWP_SHOWWINDOW | SWP_FRAMECHANGED);
    }

    public void Hide()
    {
        if (Application.isEditor) return;

        ShowWindow(Hwnd, SW_MINIMIZE); // 태스크바에 남기고 최소화
    }

    public void Exit()
    {
        Managers.Instance.SaveAndQuit();
    }

    private void SetDefaultCamera()
    {
        _camera = Camera.main;
        _camera.transform.localPosition = new Vector3(-6.65f, 2.79f, -10f);
        _camera.GetComponent<Camera>().orthographicSize = 10f;
    }

    private void SetPIPCamera()
    {
        _camera = Camera.main;
        _camera.transform.localPosition = new Vector3(-12f, 0f, -10f);
        _camera.GetComponent<Camera>().orthographicSize = 7f;
    }
}