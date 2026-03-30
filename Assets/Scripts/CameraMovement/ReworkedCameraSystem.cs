using UnityEngine;
using UnityEngine.EventSystems;

public class ReworkedCameraSystem : MonoBehaviour
{
    public float speed = 15f;

    public float leftBorder = 20f;
    public float rightBorder = 20f;
    public float topBorder = 120f;
    public float bottomBorder = 120f;

    public float verticalDelay = 0.25f;
    float verticalTimer = 0;

    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    public bool borderSwitch;

    Camera cam;

    void Start()
    {
        borderSwitch = false;
        cam = Camera.main;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 move = Vector3.zero;

        if (Input.mousePosition.x <= leftBorder)
        {
            move.x = -1;
        }

        if (Input.mousePosition.x >= Screen.width - rightBorder)
        {
            move.x = 1;
        }

        if (Input.mousePosition.y <= bottomBorder)
        {
            verticalTimer += Time.deltaTime;

            if (verticalTimer >= verticalDelay)
                move.y = -1;
        }
        else if (Input.mousePosition.y >= Screen.height - topBorder)
        {
            verticalTimer += Time.deltaTime;

            if (verticalTimer >= verticalDelay)
                move.y = 1;
        }
        else
        {
            verticalTimer = 0;
        }
        transform.position += move * speed * Time.deltaTime;

        if(borderSwitch){
        CameraBorder();
        }
    }

    void CameraBorder()
    {
        Vector3 pos = transform.position;
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        pos.x = Mathf.Clamp(pos.x, minX + horzExtent, maxX - horzExtent);
        pos.y = Mathf.Clamp(pos.y, minY + vertExtent, maxY - vertExtent);

        transform.position = pos;
    }
}