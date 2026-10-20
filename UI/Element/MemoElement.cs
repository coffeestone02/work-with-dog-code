using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MemoElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text _listTitle;
    MemoPopup _memoPopup;
    GameObject activeLine;

    string _titleText;
    public string MemoText;
    public int Order;

    public string TitleText
    {
        get
        {
            return _titleText;
        }
        set
        {
            _titleText = value;
            _listTitle.text = value;
        }
    }

    private void Awake()
    {
        _memoPopup = GameObject.Find("MemoPopup").GetComponent<MemoPopup>();
        activeLine = transform.Find("ActiveLine").gameObject;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _memoPopup.SelectMemo(this);
    }

    public void SetActiveLine(bool isActive)
    {
        activeLine.SetActive(isActive);
    }
}
