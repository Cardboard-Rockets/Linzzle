using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class WireSpawnRed : BaseWire
{
    [SerializeField] Tile[] tile;
    [SerializeField] Tilemap tileMap;

    [Header("Targets")]
    [SerializeField] Tile sourceTile;
    [SerializeField] Tile serverTile;
    [SerializeField] Tile backTile;
    [SerializeField] Tile frontTile;

    private Tile[] targets = new Tile[3];

    public bool isConnectedToServer;
    public bool isConnectedToBack;
    public bool isConnectedToFront;

    void Start()
    {
        targets[0] = backTile;
        targets[1] = frontTile;
        targets[2] = serverTile;
    }

    void Update()
    {
        if (place == null || place.CurrentTile == null) return;
        if (PlaceScript.tileid != 8) return;
        if (!MoneySystem.isAvailable()) return;
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
        var dirs = new Vector3Int[]
        {
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.up,
            Vector3Int.down
        };

        int[] rot = {0,180,90,270};

        for (int i = 0; i < dirs.Length; i++)
        {
            if (tileMap.GetTile(pos + dirs[i]) == sourceTile)
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
        bool s = Check(sourceTile, startPosition);
        bool b = Check(backTile, endPosition);
        bool f = Check(frontTile, endPosition);
        bool sr = Check(serverTile, endPosition);

        if (s && b) isConnectedToBack = true;
        if (s && f) isConnectedToFront = true;
        if (s && sr) isConnectedToServer = true;
    }

    bool Check(Tile t, Vector3Int pos)
    {
        return tileMap.GetTile(pos) == t;
    }

    bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        if (Application.isMobilePlatform && Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

        return EventSystem.current.IsPointerOverGameObject();
    }
}