using UnityEngine;

public class DragCameraScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float dragSpeed = 1.5f;
    [SerializeField] PlaceScript placeScript;
    [SerializeField] ReworkedCameraSystem rcs;

    [Header("Map Limits")]
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    Camera cam;

    private Vector2 lastPos;
    private bool isDragging;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!rcs.borderSwitch) return;
        if (placeScript.CurrentTile != null) return;
        if (MobileInput.OverUI()) return;

        HandleDrag();
    }

    void HandleDrag()
    {
        if (MobileInput.Down())
        {
            lastPos = MobileInput.Position();
            isDragging = false;
        }

        if (MobileInput.Hold())
        {
            Vector2 current = MobileInput.Position();
            Vector2 delta = current - lastPos;

            if (delta.magnitude > 5f)
                isDragging = true;

            if (isDragging)
            {
                Vector3 move = new Vector3(-delta.x, -delta.y, 0f)
                               * dragSpeed * Time.deltaTime;

                Vector3 target = transform.position + move;
                transform.position = ClampPosition(target);
            }

            lastPos = current;
        }

        if (MobileInput.Up())
        {
            isDragging = false;
        }
    }

    Vector3 ClampPosition(Vector3 pos)
    {
        float vert = cam.orthographicSize;
        float horz = vert * cam.aspect;

        pos.x = Mathf.Clamp(pos.x, minX + horz, maxX - horz);
        pos.y = Mathf.Clamp(pos.y, minY + vert, maxY - vert);

        return pos;
    }
}