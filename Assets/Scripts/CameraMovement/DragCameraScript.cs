using UnityEngine;
using UnityEngine.EventSystems;

public class DragCameraScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 15f;
    [SerializeField] float dragSpeed = 1.5f;
    [SerializeField] PlaceScript placeScript;
    [SerializeField] ReworkedCameraSystem rcs;

    [Header("Map Limits")]
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    Camera cam;

    private Vector3 lastMousePos;
    private bool isDragging;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if(rcs.borderSwitch){
    if(placeScript.CurrentTile == null){
    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        return;
    HandleDrag();
    }

    transform.position = ClampPosition(transform.position);
        }
    }

    void HandleDrag()
    {
    if (Input.GetMouseButtonDown(0))
    {
        lastMousePos = Input.mousePosition;
        isDragging = false;
    }

    if (Input.GetMouseButton(0))
    {
        Vector3 delta = Input.mousePosition - lastMousePos;

        if (delta.magnitude > 5f)
            isDragging = true;

        if (isDragging)
        {
            Vector3 move = new Vector3(-delta.x, -delta.y, 0f) * dragSpeed * Time.deltaTime;

            Vector3 targetPos = transform.position + move;

            transform.position = ClampPosition(targetPos);
        }

        lastMousePos = Input.mousePosition;
    }
    }

   Vector3 ClampPosition(Vector3 pos)
    {
    float vertExtent = cam.orthographicSize;
    float horzExtent = vertExtent * cam.aspect;

    pos.x = Mathf.Clamp(pos.x, minX + horzExtent, maxX - horzExtent);
    pos.y = Mathf.Clamp(pos.y, minY + vertExtent, maxY - vertExtent);

    return pos;
    
    }
}