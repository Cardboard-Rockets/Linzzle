using UnityEngine;
using UnityEngine.Tilemaps;

public class WireSpawnBlue : BaseWire
{

    [SerializeField] public Zone zone;

    
    
    [SerializeField] Tile[] tile; // 0 - прямой, 1 - угловой, 2 - отзеркаленный
    [SerializeField] Tilemap tileMap;
    // поле place унаследовано от BaseWire - не дублируем!

    [Header("Connection Tiles")]
    [SerializeField] Tile serverTiles;
    [SerializeField] Tile backTiles;
    [SerializeField] Tile frontTiles;

    private Tile[] Tiles = new Tile[2]; // [0] - back, [1] - front

    [Header("Global Connection Status")]
    public bool isConnectedToBack = false;
    public bool isConnectedToFront = false;

    void Start()
    {
        
        placeZones = zone.Zones;
        // Убеждаемся, что ссылка на PlaceScript задана (можно в инспекторе для BaseWire)
        if (place == null)
            Debug.LogError("PlaceScript reference is missing! Assign it to BaseWire component.");

        Tiles[0] = backTiles;
        Tiles[1] = frontTiles;
    }

    void Update()
    {
        if (place != null && place.CurrentTile != null && place.tiles[PlaceScript.tileid] == tile[0] && MoneySystem.isAvailable())
        {
            Vector3Int currentPos = place.GetTilePositionFromMouse();

            if (Input.GetMouseButtonDown(0))
            {
                StartDrawingLine(currentPos);
            }
            else if (Input.GetMouseButton(0) && isDrawing && lastPlacedPosition != currentPos)
            {
                ContinueDrawingLine(currentPos);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                FinishDrawingLine();
                ResetDrawing();
            }
        }
    }

    void StartDrawingLine(Vector3Int startPos)
    {
       

        ResetDrawing();
        startPosition = startPos;
        lastPlacedPosition = startPos;
        isDrawing = true;

        CheckAndAdjustStartConnection(startPos);
    }

    void CheckAndAdjustStartConnection(Vector3Int pos)
    {
        Vector3Int[] neighbors = new Vector3Int[]
        {
            pos + Vector3Int.right,
            pos + Vector3Int.left,
            pos + Vector3Int.up,
            pos + Vector3Int.down
        };
        int[] neighborDirections = new int[] { 0, 180, 90, 270 };

        for (int i = 0; i < neighbors.Length; i++)
        {
            TileBase neighborTile = tileMap.GetTile(neighbors[i]);

            if (serverTiles != null && neighborTile == serverTiles)
            {
                int rotation = (neighborDirections[i] + 180) % 360;
                Spawn(tileMap, tile[0], pos, rotation);
                lastDirection = rotation;
                return;
            }
            if (backTiles != null && neighborTile == backTiles)
            {
                int rotation = (neighborDirections[i] + 180) % 360;
                Spawn(tileMap, tile[0], pos, rotation);
                lastDirection = rotation;
                return;
            }
            if (frontTiles != null && neighborTile == frontTiles)
            {
                int rotation = (neighborDirections[i] + 180) % 360;
                Spawn(tileMap, tile[0], pos, rotation);
                lastDirection = rotation;
                return;
            }
        }
    }

    void ContinueDrawingLine(Vector3Int currentPos)
    {
        if (!IsInAnyZone(currentPos))
            return;

        Vector3Int direction = currentPos - lastPlacedPosition;
        int wireRotation = GetRotationFromDirection(direction);

        if (lastDirection != wireRotation && lastPlacedPosition != currentPos)
        {
            TileBase previousTile = tileMap.GetTile(lastPlacedPosition);
            if (previousTile != tile[1] && previousTile != tile[2])
            {
                PlaceCornerWire(tileMap, tile, lastPlacedPosition, lastDirection, wireRotation);
            }
        }

        if (tileMap.GetTile(currentPos) == null)
        {
            Spawn(tileMap, tile[0], currentPos, wireRotation);
        }

        lastPlacedPosition = currentPos;
        lastDirection = wireRotation;
    }

    void FinishDrawingLine()
    {
        if (!isDrawing) return;
        endPosition = lastPlacedPosition;
        CheckEndConnection(endPosition, Tiles, tileMap, tile);
        ValidateConnectionB();
    }

    void ValidateConnectionB()
    {
        bool hasServerAtStart = CheckForTileAtPosition(tileMap, startPosition, serverTiles);
        bool hasBackAtEnd = CheckForTileAtPosition(tileMap, endPosition, backTiles);
        bool hasFrontAtEnd = CheckForTileAtPosition(tileMap, endPosition, frontTiles);

        bool hasServerAtEnd = CheckForTileAtPosition(tileMap, endPosition, serverTiles);
        bool hasBackAtStart = CheckForTileAtPosition(tileMap, startPosition, backTiles);
        bool hasFrontAtStart = CheckForTileAtPosition(tileMap, startPosition, frontTiles);

        if ((hasServerAtStart && hasBackAtEnd) || (hasServerAtEnd && hasBackAtStart))
        {
            isConnectedToBack = true;
            Debug.Log("Подключение к БЕК установлено!");
        }

        if ((hasServerAtStart && hasFrontAtEnd) || (hasServerAtEnd && hasFrontAtStart))
        {
            isConnectedToFront = true;
            Debug.Log("Подключение к ФРОНТ установлено!");
        }
    }
}