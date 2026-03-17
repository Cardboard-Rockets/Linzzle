using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Повесь этот скрипт на Canvas.
/// Он сам создаст Tooltip и навесит триггеры на все кнопки магазина.
/// </summary>
public class TooltipAutoSetup : MonoBehaviour
{
    private static readonly (string objName, string label, int tileid, string desc)[] ButtonData =
    {
        ("HDD_BUTTON",             "HDD",               1, "Жёсткий диск для хранения данных"),
        ("COOLER_BUTTON",          "Охлаждение",        3, "Снижает температуру компонентов"),
        ("RAM_BUTTON",             "RAM",               2, "Оперативная память"),
        ("TABLE_BUTTON1",          "Материнская плата 1", 4, "Основная плата, тип 1"),
        ("TABLE_BUTTON2",          "Материнская плата 2", 5, "Основная плата, тип 2"),
        ("TABLE_BUTTON3",          "Материнская плата 3", 6, "Основная плата, тип 3"),
        ("WIRE_BLOCK_BUTTON",      "Провод",            7, "Блок проводки"),
        ("WIRE_ELECTRICITY_BUTTON","Кабель питания",    8, "Подаёт питание на компоненты"),
    };

    void Awake()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) { Debug.LogError("TooltipAutoSetup: Canvas не найден"); return; }

        CreateTooltip(canvas);

        foreach (var data in ButtonData)
        {
            GameObject btn = GameObject.Find(data.objName);
            if (btn == null) { Debug.LogWarning($"TooltipAutoSetup: объект {data.objName} не найден"); continue; }

            if (btn.GetComponent<Button>() == null)
                btn.AddComponent<Button>();

            TooltipTrigger trigger = btn.GetComponent<TooltipTrigger>();
            if (trigger == null) trigger = btn.AddComponent<TooltipTrigger>();

            trigger.itemName    = data.label;
            trigger.tileid      = data.tileid;
            trigger.description = data.desc;
        }

        Debug.Log("TooltipAutoSetup: готово.");
    }

    private void CreateTooltip(Canvas canvas)
    {
        GameObject root = new GameObject("Tooltip");
        root.transform.SetParent(canvas.transform, false);
        root.transform.SetAsLastSibling();

        RectTransform rootRect = root.AddComponent<RectTransform>();
        // Pivot в левом верхнем углу — tooltip рисуется правее и ниже курсора
        rootRect.anchorMin = rootRect.anchorMax = Vector2.zero;
        rootRect.pivot     = new Vector2(0f, 1f);
        rootRect.sizeDelta = new Vector2(260, 90);

        // Фон — тёмный с закруглёнными углами через outline
        Image bg = root.AddComponent<Image>();
        bg.color = new Color(0.08f, 0.08f, 0.10f, 0.95f);

        Outline outline = root.AddComponent<Outline>();
        outline.effectColor    = new Color(0.4f, 0.4f, 0.5f, 0.6f);
        outline.effectDistance = new Vector2(1.5f, -1.5f);

        // Тексты — крупнее чем раньше
        TextMeshProUGUI nameText  = CreateText(root, "NameText",  20, FontStyles.Bold,   new Vector2(12, -14));
        TextMeshProUGUI priceText = CreateText(root, "PriceText", 16, FontStyles.Normal, new Vector2(12, -42));
        TextMeshProUGUI descText  = CreateText(root, "DescText",  13, FontStyles.Italic, new Vector2(12, -66));
        descText.color = new Color(0.72f, 0.72f, 0.78f, 1f);

        TooltipSystem system = root.AddComponent<TooltipSystem>();
        system.SetupReferences(root, nameText, priceText, descText, canvas);
    }

    private TextMeshProUGUI CreateText(GameObject parent, string name, float size, FontStyles style, Vector2 pos)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);

        RectTransform rt    = go.AddComponent<RectTransform>();
        rt.anchorMin        = new Vector2(0, 1);
        rt.anchorMax        = new Vector2(1, 1);
        rt.pivot            = new Vector2(0, 1);
        rt.anchoredPosition = pos;
        rt.sizeDelta        = new Vector2(-24, size + 6);

        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.fontSize  = size;
        tmp.fontStyle = style;
        tmp.color     = Color.white;
        tmp.text      = "";
        return tmp;
    }
}

// ─────────────────────────────────────────────────────
// TooltipSystem
// ─────────────────────────────────────────────────────
public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;

    private GameObject      _root;
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _priceText;
    private TextMeshProUGUI _descText;
    private RectTransform   _rect;
    private Canvas          _canvas;

    private const float OffsetX =  20f;
    private const float OffsetY = -20f;
    private const float Padding =  12f;

    public void SetupReferences(GameObject root, TextMeshProUGUI name,
        TextMeshProUGUI price, TextMeshProUGUI desc, Canvas canvas)
    {
        _root      = root;
        _nameText  = name;
        _priceText = price;
        _descText  = desc;
        _rect      = root.GetComponent<RectTransform>();
        _canvas    = canvas;
        instance   = this;
        root.SetActive(false);
    }

    void Update()
    {
        if (_root != null && _root.activeSelf)
            FollowCursor();
    }

    public static void Show(string label, int price, string description = "")
    {
        if (instance == null) return;
        instance._nameText.text  = label;
        instance._priceText.text = "Цена: " + price + " $";
        instance._descText.text  = description;
        instance._descText.gameObject.SetActive(!string.IsNullOrEmpty(description));
        instance._root.SetActive(true);
        Canvas.ForceUpdateCanvases();
        instance.FollowCursor();
    }

    public static void Hide()
    {
        if (instance == null) return;
        instance._root.SetActive(false);
    }

    void FollowCursor()
    {
        // Переводим позицию мыши напрямую в пиксели Canvas (Overlay режим = screen pixels)
        float scale = _canvas.scaleFactor;
        float w     = _rect.sizeDelta.x * scale;
        float h     = _rect.sizeDelta.y * scale;

        float x = Input.mousePosition.x + OffsetX * scale;
        float y = Input.mousePosition.y + OffsetY * scale; // вниз от курсора

        // Не выходим за правый и нижний края
        x = Mathf.Clamp(x, Padding, Screen.width  - w - Padding);
        y = Mathf.Clamp(y, h + Padding, Screen.height - Padding);

        // Переводим обратно в Canvas-координаты
        _rect.position = new Vector3(x, y, 0f);
    }
}

// ─────────────────────────────────────────────────────
// TooltipTrigger
// ─────────────────────────────────────────────────────
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string itemName;
    public int    tileid;
    public string description;

    public void OnPointerEnter(PointerEventData _)
    {
        int price = (MoneySystem.price != null && tileid >= 0 && tileid < MoneySystem.price.Length)
            ? MoneySystem.price[tileid] : 0;
        TooltipSystem.Show(itemName, price, description);
    }

    public void OnPointerExit(PointerEventData _)
    {
        TooltipSystem.Hide();
    }
}
