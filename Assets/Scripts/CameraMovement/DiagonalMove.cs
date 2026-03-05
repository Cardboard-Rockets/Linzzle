using UnityEngine;
using UnityEngine.EventSystems;

public class DiagonalMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cam; 
    public float Speed = 10f;
    public string targetTag = "CameraMoveObj";
    
    private bool isMouseOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    void Update()
    {
        if (isMouseOver)
        {
            cam.transform.Translate(Vector2.right * Speed * Time.deltaTime);
            cam.transform.Translate(Vector2.up * Speed * Time.deltaTime);
        }
    }
}