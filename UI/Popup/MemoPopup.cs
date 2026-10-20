using TMPro;
using UnityEngine;

public class MemoPopup : UIPopup
{
    [SerializeField] TMP_InputField _titleInput;
    [SerializeField] TMPMultiLineInput _memoInput;
    [SerializeField] GameObject _memoView;

    public MemoElement currentElement { get; private set; }
    public Transform memoRoot { get; private set; }

    private void Start()
    {
        _titleInput.onValueChanged.AddListener(OnTitleChanged);
        _memoInput.onValueChanged.AddListener(OnMemoChanged);
        memoRoot = GameObject.Find("MemoListRoot").transform;
    }

    public void SelectMemo(MemoElement element)
    {
        if (_memoView.activeSelf == false)
        {
            _memoView.SetActive(true);
        }

        if (currentElement != null)
        {
            currentElement.SetActiveLine(false);
        }

        currentElement = element;
        _titleInput.SetTextWithoutNotify(element.TitleText);
        _memoInput.SetTextWithoutNotify(element.MemoText);

        currentElement.SetActiveLine(true);
    }

    public void ClearMemoView()
    {
        currentElement = null;
        _titleInput.SetTextWithoutNotify("");
        _memoInput.SetTextWithoutNotify("");
        _memoView.SetActive(false);
    }

    private void OnTitleChanged(string text)
    {
        if (currentElement != null)
        {
            currentElement.TitleText = text;
        }
    }

    private void OnMemoChanged(string text)
    {
        if (currentElement != null)
        {
            currentElement.MemoText = text;
        }
    }
}
