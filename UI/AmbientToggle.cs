using UnityEngine;
using UnityEngine.UI;

public class AmbientToggle : MonoBehaviour
{
    Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        Managers.Audio.PlaySFX("SFXToggle");
        if (isOn)
        {
            Managers.Audio.PlayAmbient(transform.parent.name);
        }
        else
        {
            Managers.Audio.PauseAmbient(transform.parent.name);
        }
    }
}
