using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public Dictionary<string, UIPopup> PopupDict = new Dictionary<string, UIPopup>(); // 전체 팝업
    GameObject _popupRoot;

    public GameObject UIRoot;
    public GameObject PIPRoot;

    public void Init()
    {
        _popupRoot = GameObject.Find("Popups");
        UIPopup[] popups = _popupRoot.GetComponentsInChildren<UIPopup>(true);
        foreach (UIPopup popup in popups)
        {
            PopupDict.Add(popup.name, popup);
        }

        UIRoot = GameObject.Find("@UI_Root");
        PIPRoot = GameObject.Find("@PIP_Root");
        PIPRoot.SetActive(false);
    }

    public void ActivePIP()
    {
        UIRoot.SetActive(false);
        PIPRoot.SetActive(true);
    }

    public void DeactivePIP()
    {
        UIRoot.SetActive(true);
        PIPRoot.SetActive(false);
    }

    public void ShowPopup(string popupName)
    {
        PopupDict.TryGetValue(popupName, out UIPopup popup);
        if (popup != null)
        {
            popup.ShowPopup();
        }
    }

    public void HidePopup(string popupName)
    {
        PopupDict.TryGetValue(popupName, out UIPopup popup);
        if (popup != null)
        {
            popup.HidePopup();
        }
    }
}
