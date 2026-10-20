using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToDoElement : MonoBehaviour
{
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Toggle _toggle;

    public bool IsOn
    {
        get
        {
            return _toggle.isOn;
        }
        set
        {
            _toggle.isOn = value;
        }
    }

    public string TodoText
    {
        get
        {
            return _inputField.text;
        }
        set
        {
            _inputField.text = value;
        }
    }
    public int Order;

    private void Awake()
    {
        _inputField.onValueChanged.AddListener(OnTextChanged);
    }

    private void OnTextChanged(string text)
    {
        if (text == null) return;
        TodoText = text;
    }

    void OnDestroy()
    {
        _inputField.onValueChanged.RemoveListener(OnTextChanged);
    }
}
