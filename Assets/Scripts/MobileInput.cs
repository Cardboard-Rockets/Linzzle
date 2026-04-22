using UnityEngine;
using UnityEngine.EventSystems;

public static class MobileInput
{
    public static bool Down(int index = 0)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            return Input.GetTouch(0).phase == TouchPhase.Began;
        return false;
#else
        return Input.GetMouseButtonDown(index);
#endif
    }

    public static bool Hold(int index = 0)
    {
#if UNITY_ANDROID || UNITY_IOS
        return Input.touchCount > 0;
#else
        return Input.GetMouseButton(index);
#endif
    }

    public static bool Up(int index = 0)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            return Input.GetTouch(0).phase == TouchPhase.Ended ||
                   Input.GetTouch(0).phase == TouchPhase.Canceled;
        return false;
#else
        return Input.GetMouseButtonUp(index);
#endif
    }

    public static Vector2 Position()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;
        return Vector2.zero;
#else
        return Input.mousePosition;
#endif
    }

    public static bool OverUI()
    {
        if (EventSystem.current == null) return false;

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        return false;
#else
        return EventSystem.current.IsPointerOverGameObject();
#endif
    }
}