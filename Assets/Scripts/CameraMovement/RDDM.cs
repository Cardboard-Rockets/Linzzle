using UnityEngine;
using UnityEngine.EventSystems;

public class RDDM : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cam; 
    public float Speed = 2f;
    public string targetTag = "CameraMoveObj";
    public float Timer =0;
    
    private bool isMouseOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        Timer = 0;
    }

    void Update()
    {
        Timer+=Time.deltaTime;
        if(Timer>=1){
        if (isMouseOver)
        {
            cam.transform.Translate(Vector2.right * Speed * Time.deltaTime);
            cam.transform.Translate(Vector2.down * Speed * Time.deltaTime);
        }
        }
    }
}