using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class WorkManager
{
    public Dictionary<string, Process> UsingApps = new Dictionary<string, Process>();
    bool _isWorking;
    double _startTime = 0;
    double _pausedTime = 0;
    TMP_Text _workTimeText;

    public bool IsWorking
    {
        get
        {
            return _isWorking;
        }
        set
        {
            _isWorking = value;
        }
    }

    public double workTime
    {
        get
        {
            if (_isWorking)
            {
                return Time.timeAsDouble - _startTime + _pausedTime;
            }
            else
            {
                return _pausedTime;
            }
        }
    }

    public void Init()
    {
        // 이전 데이터 불러오기
        _pausedTime = Managers.Data.playerData.WorkTime;

        // 디스플레이 텍스트 갱신
        _workTimeText = GameObject.Find("WorkTimeText").GetComponent<TMP_Text>();
        if (_workTimeText != null)
        {
            SetTimeText();
        }
    }

    public void OnUpdate()
    {
        if (UsingApps.Count <= 0)
        {
            PauseWork();
            return;
        }

        // 현재 작업중인 프로세스 가져옴
        string usingProcessName = GetActiveProcessName();
        if (string.IsNullOrEmpty(usingProcessName) == false && UsingApps.ContainsKey(usingProcessName))
        {
            StartWork();
        }
        else // 지정된 프로그램 외 다른 프로그램 사용시 타이머 중지
        {
            PauseWork();
        }

        SetTimeText();
    }

    private string GetActiveProcessName()
    {
        try
        {
            IntPtr hwnd = WindowManager.GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                return null;

            WindowManager.GetWindowThreadProcessId(hwnd, out int pid);
            using (Process process = Process.GetProcessById(pid))
            {
                return process.ProcessName;
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogWarning($"프로세스 접근 실패: {e.Message}");
            return null;
        }
    }

    public void StartWork()
    {
        if (IsWorking) return;

        _startTime = Time.timeAsDouble;
        IsWorking = true;
    }

    public void PauseWork()
    {
        if (IsWorking == false) return;

        _pausedTime += Time.timeAsDouble - _startTime;
        IsWorking = false;
    }

    public void ResetWork()
    {
        _startTime = _pausedTime = 0;
        IsWorking = false;
        _workTimeText.text = "00:00:00";
    }

    /// <summary>
    /// 작업 시간 텍스트 갱신
    /// </summary>
    public void SetTimeText()
    {
        TimeSpan time = TimeSpan.FromSeconds(workTime);
        int total = (int)time.TotalHours;
        _workTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", total, time.Minutes, time.Seconds);
    }
}
