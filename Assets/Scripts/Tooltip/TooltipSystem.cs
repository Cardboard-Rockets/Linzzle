using UnityEngine;
using TMPro;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;

    private GameObject root;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI priceText;
    private TextMeshProUGUI descText;

    private RectTransform rect;
    private Canvas canvas;

    private const float OffsetX = 20f;
    private const float OffsetY = -20f;
    private const float Padding = 12f;

    public void SetupReferences(
        GameObject rootObj,
        TextMeshProUGUI name,
        TextMeshProUGUI price,
        TextMeshProUGUI desc,
        Canvas canvasRef)
    {
        root = rootObj;
        nameText = name;
        priceText = price;
        descText = desc;
        canvas = canvasRef;
        rect = root.GetComponent<RectTransform>();

        instance = this;

        root.SetActive(false);
    }

    void Update()
    {
        if (root != null && root.activeSelf)
            FollowCursor();
    }

    public static void Show(string label, int price, string description)
    {
        if (instance == null) return;

        instance.nameText.text = label;
        instance.priceText.text = $"Цена: {price}";
        instance.descText.text = description;

        instance.root.SetActive(true);
        Canvas.ForceUpdateCanvases();
        instance.FollowCursor();
    }

    public static void Hide()
    {
        if (instance == null) return;
        instance.root.SetActive(false);
    }

    void FollowCursor()
    {
        float scale = canvas.scaleFactor;

        float x = Input.mousePosition.x + OffsetX * scale;
        float y = Input.mousePosition.y + OffsetY * scale;

        rect.position = new Vector3(x, y, 0f);
    }
}