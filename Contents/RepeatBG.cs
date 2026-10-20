using UnityEngine;

public class RepeatBG : MonoBehaviour
{
    [Header("동일 구성의 컨테이너 2개")]
    [SerializeField] private Transform containerA;
    [SerializeField] private Transform containerB;

    [Header("스크롤 속도 (레이어별로 다르게 → 패럴랙스)")]
    [SerializeField] private float scrollSpeed = 3f;

    [Header("틈 방지 오버랩 (픽셀아트: 0.02 ~ 0.05 권장)")]
    [SerializeField] private float overlapUnits = 0.02f;

    private float containerWidth;  // 실제 시각적 가로 폭
    private float leftEdgeOffset;  // pivot → 왼쪽 시각 경계 오프셋
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        CalculateBounds(containerA);

        // B를 A 바로 오른쪽에 정렬
        SetX(containerB, containerA.position.x + containerWidth);
    }

    void LateUpdate()
    {
        if (Managers.Work.IsWorking == false) return;

        float delta = scrollSpeed * Time.deltaTime;
        containerA.Translate(Vector3.left * delta);
        containerB.Translate(Vector3.left * delta);

        float camLeft = mainCam.transform.position.x
                        - mainCam.orthographicSize * mainCam.aspect;

        // 화면 왼쪽 밖으로 나간 컨테이너를 상대방 오른쪽으로 이동
        if (IsOffScreenLeft(containerA, camLeft))
            SetX(containerA, containerB.position.x + containerWidth);

        if (IsOffScreenLeft(containerB, camLeft))
            SetX(containerB, containerA.position.x + containerWidth);
    }

    // ── 핵심: 모든 자식 SpriteRenderer를 묶어 실제 Bounds 계산 ──
    private void CalculateBounds(Transform container)
    {
        var renderers = container.GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length == 0) return;

        Bounds bounds = renderers[0].bounds;
        foreach (var r in renderers)
            bounds.Encapsulate(r.bounds);

        // overlapUnits만큼 폭을 줄여 재배치 시 미세하게 겹치도록 → 틈 방지
        containerWidth = bounds.size.x - overlapUnits;

        // pivot이 시각적 왼쪽 끝에서 얼마나 떨어져 있는지 저장
        leftEdgeOffset = bounds.min.x - container.position.x;
    }

    // 오른쪽 시각 끝이 카메라 왼쪽 끝보다 왼쪽이면 off-screen
    private bool IsOffScreenLeft(Transform t, float camLeft)
    {
        float rightVisualEdge = t.position.x + leftEdgeOffset + containerWidth;
        return rightVisualEdge < camLeft;
    }

    private void SetX(Transform t, float x)
    {
        t.position = new Vector3(x, t.position.y, t.position.z);
    }
}
