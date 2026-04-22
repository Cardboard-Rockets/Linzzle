// ===============================
// PlaceScript.cs
// ===============================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlaceScript : MonoBehaviour
{
    [SerializeField] public Zone zone;
    private List<PlaceZone> Zones = new List<PlaceZone>();

    [Header("Tilemaps")]
    public Grid grid;
    public Tilemap tileMap;

    public static int tileid;
    public TileBase[] tiles;
    public TileBase CurrentTile;

    [SerializeField] PhysicsSystem_Script physicScript;
    [SerializeField] AudioManager audioManager;
    [SerializeField] TileInfoScript tscr;

    void Start()
    {
        Zones = zone.Zones;
    }

    void Update()
    {
        CurrentTile = tiles[tileid];

        if (Input.GetKeyDown(KeyCode.Q))
            tileid = 0;

        // просмотр информации
        if (CurrentTile == null)
        {
            if (UniversalInput.Down())
                tscr.ShowTileInfo();
        }

        // защита от UI
        if (CurrentTile != null && IsPointerOverUI())
            return;

        // проводка
        if ((tileid == 7 || tileid == 8) && UniversalInput.Up())
        {
            tileid = 0;
        }

        // установка блоков
        if (CurrentTile != null &&
            IsInAnyZone(GetTilePositionFromPointer()) &&
            MoneySystem.isAvailable())
        {
            if (UniversalInput.Down())
            {
                audioManager.PlayPlaceSound();

                Vector3Int cellPos = GetTilePositionFromPointer();

                PlaceTileAtMousePosition(cellPos, CurrentTile, tileMap);

                if (tileid != 7 && tileid != 8)
                    tileid = 0;
            }

            physicScript.RecalculateSystem();
        }
    }

    bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        if (UniversalInput.IsMobile())
        {
            if (Input.touchCount > 0)
                return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

            return false;
        }

        return EventSystem.current.IsPointerOverGameObject();
    }

    protected bool IsInAnyZone(Vector3Int position)
    {
        if (Zones == null || Zones.Count == 0)
            return true;

        foreach (var zone in Zones)
        {
            if (zone.IsInside(position))
                return true;
        }

        return false;
    }

    public Vector3Int GetTilePositionFromPointer()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(UniversalInput.Position());
        return grid.WorldToCell(pos);
    }

    public Vector3Int GetTilePositionFromMouse()
    {
        return GetTilePositionFromPointer();
    }

    public void PlaceTileAtMousePosition(Vector3Int p, TileBase tile, Tilemap map)
    {
        if (map.GetTile(p) == null)
        {
            tscr.HideInfoBlocks();

            map.SetTile(p, tile);

            if (map.GetTile(p) != null)
                MoneySystem.buy(tileid);
        }
    }
}