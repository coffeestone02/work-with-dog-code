using UnityEngine;

public class MemoButton : BaseButton
{
    Transform _memoPopup;
    Transform _memoRoot;
    string _memoElement = "UI/Element/MemoElement";

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        switch (name)
        {
            case "OpenMemoButton":
                Managers.UI.ShowPopup("MemoPopup");
                break;
            case "AddMemoButton":
                AddMemo();
                break;
            case "RemoveMemoButton":
                RemoveMemo();
                break;
        }
    }

    private void AddMemo()
    {
        if (_memoRoot == null)
        {
            _memoRoot = GameObject.Find("MemoListRoot").transform;
        }

        GameObject newMemo = Managers.Resource.Instantiate(_memoElement, _memoRoot);
        newMemo.name = "Element " + _memoRoot.childCount.ToString();
        newMemo.GetComponent<MemoElement>().Order = _memoRoot.childCount - 1;
    }

    private void RemoveMemo()
    {
        if (_memoPopup == null)
        {
            _memoPopup = GameObject.Find("MemoPopup").transform;
        }

        GameObject parent = transform.parent.gameObject;
        MemoPopup memoPopup = _memoPopup.GetComponent<MemoPopup>();

        // 보고 있는 메모를 닫는 경우
        if (memoPopup.currentElement != null && memoPopup.currentElement.gameObject == parent)
        {
            memoPopup.ClearMemoView();
        }

        Destroy(parent);
    }
}
