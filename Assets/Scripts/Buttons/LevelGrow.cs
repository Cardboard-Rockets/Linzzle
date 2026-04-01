using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GlowOutlineCustom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    public float glowOffset = 20f;
    public float speed = 5f;
    public float maxAlpha = 0.7f;
    public Color glowColor = Color.yellow;
    [SerializeField] Sprite image;

    private GameObject glowObject;
    private Image glowImage;
    private float currentAlpha;
    private bool isHovered = false;
    private RectTransform mainRect;

    void Start()
    {
        mainRect = GetComponent<RectTransform>();
        CreateGlow();
        UpdateAlpha(0);
    }

    void CreateGlow()
    {
        Canvas parentCanvas = GetComponentInParent<Canvas>();

        glowObject = new GameObject("GlowOutline");
        glowObject.transform.SetParent(parentCanvas.transform);
        glowObject.transform.localScale = Vector3.one;

        glowImage = glowObject.AddComponent<Image>();
        glowImage.raycastTarget = false;
        glowImage.color = new Color(glowColor.r, glowColor.g, glowColor.b, 0);
        glowImage.sprite = image;
      

        RectTransform rect = glowImage.GetComponent<RectTransform>();
        rect.sizeDelta = mainRect.sizeDelta + new Vector2(glowOffset * 2, glowOffset * 2);
        rect.anchoredPosition = mainRect.anchoredPosition;

        glowObject.transform.SetSiblingIndex(transform.GetSiblingIndex());
    }

    void Update()
    {
        if (glowObject != null)
        {
            RectTransform rect = glowImage.GetComponent<RectTransform>();
            rect.anchoredPosition = mainRect.anchoredPosition;
            rect.sizeDelta = mainRect.sizeDelta + new Vector2(glowOffset * 2, glowOffset * 2);

            if (isHovered)
            {
                currentAlpha += Time.deltaTime * speed;
                if (currentAlpha > maxAlpha) currentAlpha = maxAlpha;
            }
            else
            {
                currentAlpha -= Time.deltaTime * speed;
                if (currentAlpha < 0) currentAlpha = 0;
            }

            UpdateAlpha(currentAlpha);
        }
    }

    void UpdateAlpha(float alpha)
    {
        if (glowImage != null)
        {
            glowImage.color = new Color(glowColor.r, glowColor.g, glowColor.b, alpha);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        if (glowObject != null)
        {
            glowObject.transform.SetAsFirstSibling();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    void OnDestroy()
    {
        if (glowObject != null)
        {
            Destroy(glowObject);
        }
    }
}