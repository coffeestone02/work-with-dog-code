using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDButton : BaseButton
{
    Transform _activeAppListRoot;

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        switch (name)
        {
            case "ResetButton":
                Reset();
                break;
            case "CloseButton":
                ClosePopup();
                break;
            case "OpenLanguageButton":
                Managers.UI.ShowPopup("LanguagePopup");
                break;
            case "SetLanguageButton":
                SetLanguage();
                break;
        }
    }

    private void Reset()
    {
        Managers.Work.ResetWork();
    }

    private void ClosePopup()
    {
        GameObject popup = transform.parent.parent.gameObject; // Button -> Header -> Popup
        if (popup.name == "ActiveAppListPopup")
        {
            if (_activeAppListRoot == null)
            {
                _activeAppListRoot = GameObject.Find("ActiveAppListRoot").transform;
            }

            foreach (Transform child in _activeAppListRoot)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }

        Managers.UI.HidePopup(popup.name);
    }

    private void SetLanguage()
    {
        string language = transform.GetChild(0).name;
        Managers.Language.ChangeLocale(language);
    }

}
