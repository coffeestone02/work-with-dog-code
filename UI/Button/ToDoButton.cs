using UnityEngine;

public class ToDoButton : BaseButton
{
    Transform _todoRoot;
    string _todoElement = "UI/Element/ToDoElement";

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        switch (name)
        {
            case "OpenToDoButton":
                Managers.UI.ShowPopup("ToDoPopup");
                break;
            case "AddToDoButton":
                AddToDo();
                break;
            case "RemoveToDoButton":
                RemoveToDo();
                break;
        }
    }

    private void AddToDo()
    {
        if (_todoRoot == null)
        {
            _todoRoot = GameObject.Find("ToDoListRoot").transform;
        }

        GameObject newToDo = Managers.Resource.Instantiate(_todoElement, _todoRoot);
        newToDo.name = "Element " + _todoRoot.childCount.ToString();
        newToDo.GetComponent<ToDoElement>().Order = _todoRoot.childCount - 1;
    }

    private void RemoveToDo()
    {
        GameObject parent = transform.parent.gameObject;
        Destroy(parent);
    }
}
