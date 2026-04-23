using UnityEngine;
using UnityEngine.Tilemaps;

public class WireSpawnRed : BaseWire
{

    [SerializeField] public Zone zone;


    [SerializeField] Tile[] tile; // 0 - прямой, 1 - угловой, 2 - отзеркаленный
    [SerializeField] Tilemap tileMap;

    [Header("Source (Щиток)")]
    [SerializeField] Tile sourceTile; // тайл щитка, с которого начинается провод

    [Header("Targets")]
    [SerializeField] Tile serverTile;
    [SerializeField] Tile backTile;
    [SerializeField] Tile frontTile;

    private Tile[] targetTiles = new Tile[3]; // 0-back, 1-front, 2-server

    [Header("Global Connection Status")]
    public bool isConnectedToServer = false;
    public bool isConnectedToBack = false;
    public bool isConnectedToFront = false;

    void Start()
    {
        placeZones = zone.Zones;
        if (place == null)
            Debug.LogError("PlaceScript reference is missing! Assign it to BaseWire component.");

        targetTiles[0] = backTile;
        targetTiles[1] = frontTile;
        targetTiles[2] = serverTile;
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
        if (!IsInAnyZone(startPos))
            return;

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

            if (sourceTile != null && neighborTile == sourceTile)
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
        CheckEndConnectionRed(endPosition);
        ValidateConnectionRed();
    }

    // Переопределяем логику окончания подключения для трёх типов целей
    void CheckEndConnectionRed(Vector3Int pos)
    {
        Vector3Int[] neighbors = new Vector3Int[] { pos + Vector3Int.right, pos + Vector3Int.left, pos + Vector3Int.up, pos + Vector3Int.down };
        int[] neighborDirections = new int[] { 0, 180, 90, 270 };

        for (int i = 0; i < neighbors.Length; i++)
        {
            TileBase neighborTile = tileMap.GetTile(neighbors[i]);

            // Проверяем каждый тип цели
            for (int targetIndex = 0; targetIndex < targetTiles.Length; targetIndex++)
            {
                if (targetTiles[targetIndex] != null && neighborTile == targetTiles[targetIndex])
                {
                    int rotation = neighborDirections[i];
                    if (rotation != lastDirection)
                    {
                        // Выбираем нужный угловой тайл и его поворот
                        if (neighbors[i] == pos + Vector3Int.left && lastDirection == 90)
                            Spawn(tileMap, tile[1], pos, 90);
                        else if (neighbors[i] == pos + Vector3Int.left && lastDirection == 270)
                            Spawn(tileMap, tile[2], pos, 0);
                        else if (neighbors[i] == pos + Vector3Int.right && lastDirection == 90)
                            Spawn(tileMap, tile[2], pos, 180);
                        else if (neighbors[i] == pos + Vector3Int.right && lastDirection == 270)
                            Spawn(tileMap, tile[1], pos, 270);
                        else if (neighbors[i] == pos + Vector3Int.up && lastDirection == 0)
                            Spawn(tileMap, tile[1], pos, 0);
                        else if (neighbors[i] == pos + Vector3Int.up && lastDirection == 180)
                            Spawn(tileMap, tile[2], pos, 270);
                        else if (neighbors[i] == pos + Vector3Int.down && lastDirection == 0)
                            Spawn(tileMap, tile[2], pos, 90);
                        else if (neighbors[i] == pos + Vector3Int.down && lastDirection == 180)
                            Spawn(tileMap, tile[1], pos, 180);
                    }
                    return;
                }
            }
        }
    }

    void ValidateConnectionRed()
    {
        bool hasSourceAtStart = CheckForTileAtPosition(tileMap, startPosition, sourceTile);
        bool hasBackAtEnd = CheckForTileAtPosition(tileMap, endPosition, backTile);
        bool hasFrontAtEnd = CheckForTileAtPosition(tileMap, endPosition, frontTile);
        bool hasServerAtEnd = CheckForTileAtPosition(tileMap, endPosition, serverTile);

        // Также возможен случай, когда щиток в конце, а цель в начале – но по логике провод идёт от щитка, поэтому достаточно одного направления
        if (hasSourceAtStart && hasBackAtEnd)
        {
            isConnectedToBack = true;
            Debug.Log("Красный провод подключён к БЕК!");
        }
        if (hasSourceAtStart && hasFrontAtEnd)
        {
            isConnectedToFront = true;
            Debug.Log("Красный провод подключён к ФРОНТ!");
        }
        if (hasSourceAtStart && hasServerAtEnd)
        {
            isConnectedToServer = true;
            Debug.Log("Красный провод подключён к СЕРВЕРУ!");
        }
    }
}