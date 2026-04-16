using UnityEngine;

[System.Serializable]
public class PlaceZone
{
    public Vector3Int NizLeft; // нижний левый
    public Vector3Int VerRight; // верхний правый

    public bool IsInside(Vector3Int pos)
    {
        int minX = Mathf.Min(NizLeft.x, VerRight.x);
        int maxX = Mathf.Max(NizLeft.x, VerRight.x);
        int minY = Mathf.Min(NizLeft.y, VerRight.y);
        int maxY = Mathf.Max(NizLeft.y, VerRight.y);

        return pos.x >= minX && pos.x <= maxX &&
               pos.y >= minY && pos.y <= maxY;
    }

}
