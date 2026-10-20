using System.Diagnostics;
using TMPro;
using UnityEngine;

public class AppButton : BaseButton
{
    Transform _appListRoot;
    string _removeAppElement = "UI/Element/AppElement(Remove)";

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        switch (name)
        {
            case "OpenActiveAppListButton":
                OpenActiveAppList();
                break;
            case "AddAppButton":
                AddApp();
                break;
            case "RemoveAppButton":
                RemoveApp();
                break;
        }
    }

    private void OpenActiveAppList()
    {
        Managers.Work.PauseWork();
        Managers.UI.ShowPopup("ActiveAppListPopup");
    }

    private void AddApp()
    {
        if (_appListRoot == null)
        {
            _appListRoot = GameObject.Find("AppListRoot").transform;
        }

        GameObject element = transform.parent.gameObject;

        // UsingApps 딕셔너리에 추가
        Process p = element.GetComponent<ProcessInfo>().process;
        Managers.Work.UsingApps.Add(p.ProcessName, p);

        // 앱리스트에 추가
        GameObject addedElement = Managers.Resource.Instantiate(_removeAppElement, _appListRoot);

        // 프로세스 정보 추가
        ProcessInfo pi = addedElement.GetComponent<ProcessInfo>();
        pi.process = p;

        // instance 이름 설정
        TMP_Text nameText = addedElement.GetComponentInChildren<TMP_Text>();
        addedElement.name = p.ProcessName;
        nameText.text = p.ProcessName;

        addedElement.transform.localScale = Vector3.one;
        Destroy(element);
    }

    private void RemoveApp()
    {
        GameObject parent = transform.parent.gameObject;
        using (Process removed = parent.GetComponent<ProcessInfo>().process)
        {
            Managers.Work.UsingApps.Remove(removed.ProcessName);
        }
        Destroy(parent);
    }
}
