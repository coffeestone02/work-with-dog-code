using UnityEngine;

public class Background : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    float _backgroundLength;
    float _scrollSpeed = 4f;
    [SerializeField] Transform _otherBackground;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.linearVelocity = new Vector2(-_scrollSpeed, 0);
        _backgroundLength = GetComponentInChildren<SpriteRenderer>().bounds.size.x - 1f;
    }

    void Update()
    {
        ScollingBackground();
        RepositionBackground();
    }

    private void ScollingBackground()
    {
        if (Managers.Work.IsWorking)
            _rigidbody.linearVelocity = new Vector2(-_scrollSpeed, 0);
        else
            _rigidbody.linearVelocity = Vector2.zero;
    }

    private void RepositionBackground()
    {
        if (transform.position.x < -_backgroundLength)
        {
            float newX = _otherBackground.position.x + _backgroundLength;
            transform.position = new Vector2(newX, transform.position.y);
        }
    }
}
