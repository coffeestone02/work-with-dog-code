using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    Button _btn;

    protected virtual void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnButtonClicked);
    }

    protected virtual void OnButtonClicked()
    {
        Managers.Audio.PlaySFX("SFXButton");
    }
}
