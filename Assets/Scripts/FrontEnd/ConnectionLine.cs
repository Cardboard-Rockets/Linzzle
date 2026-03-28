using UnityEngine;

public class ConnectionLine : MonoBehaviour
{
    public RectTransform rect;
    public RectTransform a;
    public RectTransform b;
    public bool followMouse = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (a == null) return;

        Vector2 posA = a.position;
        Vector2 posB = followMouse ? Input.mousePosition : (b != null ? b.position : posA);

        Vector2 dir = posB - posA;
        rect.position = posA + dir / 2f;
        rect.sizeDelta = new Vector2(dir.magnitude, 5f);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rect.rotation = Quaternion.Euler(0, 0, angle);
    }
}