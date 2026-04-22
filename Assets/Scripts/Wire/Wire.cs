using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class BaseWire : MonoBehaviour
{
    [SerializeField] protected PlaceScript place;

    [Header("Place Zones")]
    public List<PlaceZone> placeZones = new List<PlaceZone>();

    public int lastDirection = 0;
    public bool isDrawing = false;

    public Vector3Int lastPlacedPosition;
    public Vector3Int startPosition;
    public Vector3Int endPosition;

    // ===== INPUT HELP =====

    protected bool PointerDown()
    {
        return UniversalInput.Down();
    }

    protected bool PointerHold()
    {
        return UniversalInput.Hold();
    }

    protected bool PointerUp()
    {
        return UniversalInput.Up();
    }

    protected Vector3Int GetPointerCell()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(UniversalInput.Position());
        return place.grid.WorldToCell(pos);
    }

    protected bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
                return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        }

        return EventSystem.current.IsPointerOverGameObject();
    }

    // ===== ZONES =====

    protected bool IsInAnyZone(Vector3Int position)
    {
        if (placeZones == null || placeZones.Count == 0)
            return true;

        foreach (var zone in placeZones)
            if (zone.IsInside(position))
                return true;

        return false;
    }

    // ===== SPAWN =====

    public virtual void Spawn(Tilemap map, Tile tile, Vector3Int position, int degree)
    {
        if (!IsInAnyZone(position))
            return;

        map.SetTile(position, null);

        if (place != null)
            place.PlaceTileAtMousePosition(position, tile, map);

        Matrix4x4 matrix =
            Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, degree), Vector3.one);

        map.SetTransformMatrix(position, matrix);
    }

    // ===== UTILS =====

    public int GetRotationFromDirection(Vector3Int dir)
    {
        if (dir.x > 0) return 0;
        if (dir.x < 0) return 180;
        if (dir.y > 0) return 90;
        if (dir.y < 0) return 270;
        return 0;
    }

    public void ResetDrawing()
    {
        isDrawing = false;
        lastDirection = 0;
        startPosition = Vector3Int.zero;
        endPosition = Vector3Int.zero;
    }
}