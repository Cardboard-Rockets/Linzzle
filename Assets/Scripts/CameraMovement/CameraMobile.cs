using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDrag2D : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public PlaceScript placeScript;

    [Header("Movement Settings")]
    public float dragSpeed = 1f;

    [Header("Camera Bounds")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private Vector3 lastTouchPosition;
    private bool isDragging;

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        // Блок камеры при установке тайла
        if (placeScript != null && placeScript.CurrentTile != null)
        {
            isDragging = false;
            return;
        }

        HandleInput();
    }

    void HandleInput()
    {
        // Мобильный ввод
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Игнор UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                lastTouchPosition = cam.ScreenToWorldPoint(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector3 currentPosition = cam.ScreenToWorldPoint(touch.position);
                Vector3 difference = lastTouchPosition - currentPosition;

                cam.transform.position += difference * dragSpeed;
                lastTouchPosition = cam.ScreenToWorldPoint(touch.position);

                ClampCamera();
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }

        // ПК ввод (для теста в редакторе)
        else if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            isDragging = true;
            lastTouchPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = lastTouchPosition - currentPosition;

            cam.transform.position += difference * dragSpeed;
            lastTouchPosition = cam.ScreenToWorldPoint(Input.mousePosition);

            ClampCamera();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void LateUpdate()
    {
        // Жёсткий постоянный clamp (на случай любого внешнего движения)
        ClampCamera();
    }

    void ClampCamera()
    {
        Vector3 pos = cam.transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        cam.transform.position = pos;
    }
}