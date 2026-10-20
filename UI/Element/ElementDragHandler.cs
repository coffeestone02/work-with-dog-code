using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform _root = null;
    Transform _element = null;

    GameObject _tempObject = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _element = this.transform.parent;
        _root = _element.parent; // ToDoListContent, MemoListContent

        _tempObject = Managers.Resource.Instantiate("UI/Element/ToDoElement", _root); // 임시 오브젝트 만들기

        GameObject hideIamge = _tempObject.transform.Find("Hide").gameObject;
        hideIamge.SetActive(true);
        _tempObject.name = "TempElement";
        _tempObject.transform.SetSiblingIndex(_element.GetSiblingIndex()); // 임시 오브젝트를 드래그중인 오브젝트의 위치로 설정
        _element.SetParent(GameObject.Find("Popups").transform); // 그리고 드래그중인 오브젝트를 가장 앞쪽으로 빼줌

        _element.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float offsetX = _element.GetComponent<RectTransform>().rect.width / 1.8f;
        _element.position = new Vector3(eventData.position.x + offsetX, eventData.position.y, _element.position.z); // 드래그중인 오브젝트의 위치를 마우스 위치로 설정

        // 위치 계산
        int newSiblingIndex = _root.childCount;
        for (int i = 0; i < _root.childCount; i++)
        {
            Transform child = _root.GetChild(i);

            RectTransform childRect = child.GetComponent<RectTransform>();
            float childHeight = childRect.rect.height;
            float deadzone = childHeight * 0.7f; // 드래그 중인 오브젝트가 자식 요소의 절반 높이 이상으로 이동했을 때 위치 변경

            if (eventData.position.y > child.position.y + deadzone)
            {
                newSiblingIndex = i;
                break;
            }
        }

        _tempObject.transform.SetSiblingIndex(newSiblingIndex); // 계산된 위치로 설정
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _element.SetParent(_root);
        _element.SetSiblingIndex(_tempObject.transform.GetSiblingIndex());
        _element.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        DestroyImmediate(_tempObject);

        // 드래그가 끝난 후, 모든 자식 요소의 order를 업데이트
        for (int i = 0; i < _root.childCount; i++)
        {
            Transform child = _root.GetChild(i);

            if (child.GetComponent<ToDoElement>() != null)
                child.GetComponent<ToDoElement>().Order = i;
            else if (child.GetComponent<MemoElement>() != null)
                child.GetComponent<MemoElement>().Order = i;
        }
    }
}