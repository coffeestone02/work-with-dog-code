using UnityEngine;
using System.Diagnostics;
using UnityEditor;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class ActiveAppList : UIPopup
{
    private void OnEnable()
    {
        Process[] activeApps = GetRunningProcess();
        CreateList(activeApps);
    }

    private void CreateList(Process[] activeApps)
    {
        Transform parent = GameObject.Find("ActiveAppListRoot").transform;

        foreach (Process p in activeApps)
        {
            // 이미 등록된 프로그램인 경우 제외함
            if (Managers.Work.UsingApps.ContainsKey(p.ProcessName))
            {
                continue;
            }

            GameObject instance = Managers.Resource.Instantiate("UI/Element/AppElement(Add)", parent);

            // 이름 설정
            TMP_Text nameText = instance.GetComponentInChildren<TMP_Text>();
            instance.name = nameText.text = p.ProcessName;

            // 프로세스 정보 붙이기
            ProcessInfo pi = instance.GetComponent<ProcessInfo>();
            if (pi == null)
            {
                pi = instance.AddComponent<ProcessInfo>();
            }

            pi.process = p;

            instance.transform.localScale = Vector3.one;
        }
    }

    public Process[] GetRunningProcess()
    {
        Process[] allApps = Process.GetProcesses();
        List<Process> activeApps = new List<Process>();

        foreach (var process in allApps)
        {
            try
            {
                if (process.MainWindowHandle != System.IntPtr.Zero)
                {
                    activeApps.Add(process);
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
        }

        return activeApps.ToArray();
    }
}
