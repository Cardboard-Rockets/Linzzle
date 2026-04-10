using UnityEngine;
using UnityEngine.EventSystems;

public class ReworkedCameraSystem : MonoBehaviour
{

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