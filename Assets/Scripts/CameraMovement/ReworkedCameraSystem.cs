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

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 move = Vector3.zero;

        // LEFT
        if (Input.mousePosition.x <= leftBorder)
        {
            move.x = -1;
        }

        // RIGHT
        if (Input.mousePosition.x >= Screen.width - rightBorder)
        {
            move.x = 1;
        }

        // DOWN
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
    }
}