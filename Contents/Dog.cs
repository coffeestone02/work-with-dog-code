using UnityEngine;
using UnityEngine.EventSystems;

public class Dog : MonoBehaviour, IPointerClickHandler
{
    Animator _animator;

    public void OnPointerClick(PointerEventData eventData)
    {
        Managers.Audio.PlaySFX("SFXDog");
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Managers.Work.IsWorking)
        {
            _animator.SetBool("isRun", true);
        }
        else
        {
            _animator.SetBool("isRun", false);
        }
    }
}
