using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class TooltipFactory
{
    public static void CreateTooltip(Canvas canvas)
    {
        GameObject root = new GameObject("Tooltip");
        root.transform.SetParent(canvas.transform, false);
        root.transform.SetAsLastSibling();

        RectTransform rect = root.AddComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = Vector2.zero;
        rect.pivot = new Vector2(0f, 1f);
        rect.sizeDelta = new Vector2(260, 120);

        // Отключаем блокировку мыши
        CanvasGroup cg = root.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        cg.interactable = false;

        Image bg = root.AddComponent<Image>();
        bg.color = new Color(0.08f, 0.08f, 0.10f, 0.95f);
        bg.raycastTarget = false;

        Outline outline = root.AddComponent<Outline>();
        outline.effectColor = new Color(0.4f, 0.4f, 0.5f, 0.6f);
        outline.effectDistance = new Vector2(1.5f, -1.5f);

        var nameText  = CreateText(root, "NameText", 20, FontStyles.Bold,   new Vector2(12, -14));
        var priceText = CreateText(root, "PriceText",16, FontStyles.Normal, new Vector2(12, -42));
        var descText  = CreateText(root, "DescText", 13, FontStyles.Italic, new Vector2(12, -66));

        TooltipSystem system = root.AddComponent<TooltipSystem>();
        system.SetupReferences(root, nameText, priceText, descText, canvas);
    }

    private static TextMeshProUGUI CreateText(GameObject parent, string name, float size, FontStyles style, Vector2 pos)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);

        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(-24, size + 6);

        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = size;
        tmp.fontStyle = style;
        tmp.color = Color.white;
        tmp.raycastTarget = false;

        return tmp;
    }
}