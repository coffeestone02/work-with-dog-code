using TMPro;
using UnityEngine.EventSystems;

public class TMPMultiLineInput : TMP_InputField
{
    // 엔터키 입력 시 줄바꿈이 되도록 하기 위해 OnSubmit을 오버라이드하여 아무 동작도 하지 않도록 합니다.
    public override void OnSubmit(BaseEventData eventData) { }
}
