using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class WireSpawnBlue : BaseWire
{
    [SerializeField] Tile[] tile;
    [SerializeField] Tilemap tileMap;

    [Header("Connection Tiles")]
    [SerializeField] Tile serverTiles;
    [SerializeField] Tile backTiles;
    [SerializeField] Tile frontTiles;

    private Tile[] Tiles = new Tile[2];

    public bool isConnectedToBack = false;
    public bool isConnectedToFront = false;

    void Start()
    {
        Tiles[0] = backTiles;
        Tiles[1] = frontTiles;
    }

    void Update()
    {
        if (place == null || place.CurrentTile == null) return;
        if (PlaceScript.tileid != 7) return;
        if (MoneySystem.isAvailable() == false) return;
        if (IsPointerOverUI()) return;

        Vector3Int pos = GetPointerCell();

        if (PointerDown())
            StartDrawing(pos);

        else if (PointerHold() && isDrawing && lastPlacedPosition != pos)
            ContinueDrawing(pos);

        else if (PointerUp())
        {
            FinishDrawing();
            ResetDrawing();
        }
    }

    void StartDrawing(Vector3Int pos)
    {
        ResetDrawing();
        startPosition = pos;
        lastPlacedPosition = pos;
        isDrawing = true;

        CheckStart(pos);
    }

    void CheckStart(Vector3Int pos)
    {
        Vector3Int[] dirs =
        {
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.up,
            Vector3Int.down
        };

        int[] rot = {0,180,90,270};

        for (int i = 0; i < dirs.Length; i++)
        {
            var t = tileMap.GetTile(pos + dirs[i]);

            if (t == serverTiles || t == backTiles || t == frontTiles)
            {
                lastDirection = (rot[i] + 180) % 360;
                Spawn(tileMap, tile[0], pos, lastDirection);
                return;
            }
        }
    }

    void ContinueDrawing(Vector3Int pos)
    {
        Vector3Int dir = pos - lastPlacedPosition;
        int rot = GetRotationFromDirection(dir);

        if (lastDirection != rot)
            Spawn(tileMap, tile[1], lastPlacedPosition, rot);

        if (tileMap.GetTile(pos) == null)
            Spawn(tileMap, tile[0], pos, rot);

        lastPlacedPosition = pos;
        lastDirection = rot;
    }

    void FinishDrawing()
    {
        endPosition = lastPlacedPosition;
        Validate();
    }

    void Validate()
    {
        bool backStart = CheckForTileAtPosition(tileMap, startPosition, backTiles);
        bool frontStart = CheckForTileAtPosition(tileMap, startPosition, frontTiles);

        bool backEnd = CheckForTileAtPosition(tileMap, endPosition, backTiles);
        bool frontEnd = CheckForTileAtPosition(tileMap, endPosition, frontTiles);

        if ((backStart && frontEnd) || (backEnd && frontStart))
            isConnectedToFront = true;

        if ((backStart && frontEnd) || (backEnd && frontStart))
            isConnectedToBack = true;
    }

    public bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        if (Application.isMobilePlatform && Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

        return EventSystem.current.IsPointerOverGameObject();
    }

    public bool CheckForTileAtPosition(Tilemap map, Vector3Int pos, Tile target)
    {
        if (target == null) return false;

        if (map.GetTile(pos) == target)
            return true;

        return false;
    }
}