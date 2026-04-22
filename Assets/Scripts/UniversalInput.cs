// ===============================
// UniversalInput.cs
// ===============================
using UnityEngine;

public static class UniversalInput
{
    public static bool Down()
    {
        if (Input.touchCount > 0)
            return Input.GetTouch(0).phase == TouchPhase.Began;

        return Input.GetMouseButtonDown(0);
    }

    public static bool Hold()
    {
        if (Input.touchCount > 0)
        {
            TouchPhase phase = Input.GetTouch(0).phase;
            return phase == TouchPhase.Moved || phase == TouchPhase.Stationary;
        }

        return Input.GetMouseButton(0);
    }

    public static bool Up()
    {
        if (Input.touchCount > 0)
            return Input.GetTouch(0).phase == TouchPhase.Ended ||
                   Input.GetTouch(0).phase == TouchPhase.Canceled;

        return Input.GetMouseButtonUp(0);
    }

    public static Vector2 Position()
    {
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;

        return Input.mousePosition;
    }

    public static bool IsMobile()
    {
        return Application.isMobilePlatform;
    }
}