using UnityEngine;

public class ReworkedCameraSystem : MonoBehaviour
{
    public bool borderSwitch;

    void Start()
    {
        borderSwitch = false;
    }

    void Update()
    {
        if (!borderSwitch)
            return;

        // БОЛЬШЕ НЕ ДВИГАЕМ КАМЕРУ И НЕ CLAMP'ИМ ЕЁ
        // этот скрипт теперь только логический переключатель
    }
}