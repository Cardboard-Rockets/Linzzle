using UnityEngine;

public class ConnectionLine : MonoBehaviour
{
    public RectTransform rect;
    public RectTransform a;
    public RectTransform b;
    public bool followMouse = false;
    public Canvas canvas;

    private void Awake()
{
    rect = GetComponent<RectTransform>();
    canvas = GetComponentInParent<Canvas>();
}

    private void Update()
{
    if (a == null) return;

    Vector2 posA, posB;

    RectTransform canvasRect = canvas.GetComponent<RectTransform>();

    // A
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        canvasRect,
        RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, a.position),
        canvas.worldCamera,
        out posA
    );

    // B
    if (followMouse)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            canvas.worldCamera,
            out posB
        );
    }
    else
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, b.position),
            canvas.worldCamera,
            out posB
        );
    }

    Vector2 dir = posB - posA;

    rect.anchoredPosition = posA + dir / 2f;
    rect.sizeDelta = new Vector2(dir.magnitude, 5f);

    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    rect.localRotation = Quaternion.Euler(0, 0, angle);
}
}