using UnityEngine;
using UnityEngine.UI;

public class TooltipAutoSetup : MonoBehaviour
{
    private static readonly (string objName, string label, int tileid, string desc)[] ButtonData =
    {
        ("HDD_BUTTON", "HDD", 1, "Жёсткий диск для хранения данных\nЖесткий диск размером 1000ГБ"),
        ("COOLER_BUTTON", "Охлаждение", 3, "Снижает температуру компонентов\nЛучшее средство для того, чтобы всё было работало без проблем"),
        ("RAM_BUTTON", "RAM", 2, "Оперативная память\nОперативная память размером 32ГБ"),
        ("TABLE_BUTTON1", "GET Обработчик", 4, "Обрабатывает поступаемые запросы типа 'GET'\nОбрабатывает 1000 запросов"),
        ("TABLE_BUTTON2", "POST Обработчик", 5, "Обрабатывает поступаемые запросы типа 'POST'\nОбрабатывает 1000 запросов"),
        ("TABLE_BUTTON3", "PUT Обработчик", 6, "Обрабатывает поступаемые запросы типа 'PUT'\nОбрабатывает 1000 запросов"),
        ("WIRE_BLOCK_BUTTON", "Провод", 7, "Блок проводки"),
        ("WIRE_ELECTRICITY_BUTTON","Кабель питания", 8, "Подаёт питание на компоненты"),
    };

    void Awake()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas не найден");
            return;
        }

        TooltipFactory.CreateTooltip(canvas);

        foreach (var data in ButtonData)
        {
            GameObject btn = GameObject.Find(data.objName);
            if (btn == null)
            {
                Debug.LogWarning($"Объект {data.objName} не найден");
                continue;
            }

            if (btn.GetComponent<Button>() == null)
                btn.AddComponent<Button>();

            TooltipTrigger trigger = btn.GetComponent<TooltipTrigger>();
            if (trigger == null)
                trigger = btn.AddComponent<TooltipTrigger>();

            trigger.itemName = data.label;
            trigger.tileid = data.tileid;
            trigger.description = data.desc;
        }
    }
}