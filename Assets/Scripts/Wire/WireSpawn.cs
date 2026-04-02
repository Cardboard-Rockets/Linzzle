using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class WireSpawn : MonoBehaviour
{
    [SerializeField] Tile[] tile; // Индексы: 0 - прямой, 1 - угловой, 2 - отзеркаленный
    [SerializeField] Tilemap tileMap;
    [SerializeField] PlaceScript place;
    Vector3Int lastPlacedPosition;
    int lastDirection = 0; // 0-вправо, 90-вверх, 180-влево, 270-вниз
    bool isDrawing = false;
    Vector3Int startPosition;
    Vector3Int endPosition;

    [Header("Connection Tiles")]
    [SerializeField] Tile serverTiles; // Тайлы сервера (индекс 0 - сервер)
    [SerializeField] Tile backTiles;   // Тайлы бека (индекс 1 - бек)
    [SerializeField] Tile frontTiles;  // Тайлы фронта (индекс 2 - фронт)

    [Header("Global Connection Status")]
    public  bool isConnectedToBack = false;
    public  bool isConnectedToFront = false;

    void Update()
    {
        if (place.CurrentTile != null && place.tiles[PlaceScript.tileid] == tile[0] && MoneySystem.isAvailable())
        {
            Vector3Int currentPos = place.GetTilePositionFromMouse();

            if (Input.GetMouseButtonDown(0))
            {
                StartDrawingLine(currentPos);
            }

            if (Input.GetMouseButton(0) && isDrawing && lastPlacedPosition != currentPos)
            {
                ContinueDrawingLine(currentPos);
            }

            if (Input.GetMouseButtonUp(0))
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

        // Проверяем, начинается ли провод от сервера, бека или фронта
        CheckAndAdjustStartConnection(startPos);
    }

    void CheckAndAdjustStartConnection(Vector3Int pos)
    {
        // Проверяем соседние клетки на наличие сервера, бека или фронта
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

            // Проверка на сервер
            if (serverTiles != null &&  neighborTile == serverTiles)
            {
                // Ставим первый провод с правильным направлением от сервера
                int rotation = (neighborDirections[i] + 180) % 360;
                Spawn(tileMap, tile[0], pos, rotation);
                lastDirection = rotation;
                return;
            }

            // Проверка на бек
            if (backTiles != null && neighborTile == backTiles)
            {
                int rotation = (neighborDirections[i] + 180) % 360;
                Spawn(tileMap, tile[0], pos, rotation);
                lastDirection = rotation;
                return;
            }

            // Проверка на фронт
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
        Vector3Int direction = currentPos - lastPlacedPosition;

        
            int wireRotation = GetRotationFromDirection(direction);

            if (lastDirection != wireRotation && lastPlacedPosition != currentPos)
            {
                TileBase previousTile = tileMap.GetTile(lastPlacedPosition);
                if (previousTile != tile[1] && previousTile != tile[2])
                {
                    PlaceCornerWire(lastPlacedPosition, lastDirection, wireRotation);
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

        // Проверяем подключение к беку или фронту в конце линии
        CheckEndConnection(endPosition);

        // Проверяем, создана ли полная связка от сервера до бека/фронта
        ValidateConnection();
    }

    void CheckEndConnection(Vector3Int pos)
    {
        // Проверяем соседние клетки на наличие бека или фронта
        Vector3Int[] neighbors = new Vector3Int[] { pos + Vector3Int.right, pos + Vector3Int.left, pos + Vector3Int.up, pos + Vector3Int.down };

        int[] neighborDirections = new int[] { 0, 180, 90, 270 };

        for (int i = 0; i < neighbors.Length; i++)
        {
            TileBase neighborTile = tileMap.GetTile(neighbors[i]);

            int rotation = neighborDirections[i];
            // Проверка на бек
            if (backTiles != null && neighborTile == backTiles)
            {
                // Поворачиваем последний провод к беку
                if (rotation != lastDirection)
                {
                    if (neighbors[i]== pos + Vector3Int.left && lastDirection==90) { Spawn(tileMap, tile[1], pos, 90); }
                    else if(neighbors[i] == pos + Vector3Int.left && lastDirection == 270) { Spawn(tileMap, tile[2], pos, 0); }

                    if (neighbors[i] == pos + Vector3Int.right && lastDirection == 90) { Spawn(tileMap, tile[2], pos, 180); }
                    else if (neighbors[i] == pos + Vector3Int.right && lastDirection == 270) { Spawn(tileMap, tile[1], pos, 270); }

                    if (neighbors[i] == pos + Vector3Int.up && lastDirection == 0) { Spawn(tileMap, tile[1], pos, 0); }
                    else if (neighbors[i] == pos + Vector3Int.up && lastDirection == 180) { Spawn(tileMap, tile[2], pos, 270); }

                    if (neighbors[i] == pos + Vector3Int.down && lastDirection == 0) { Spawn(tileMap, tile[2], pos, 90); }
                    else if (neighbors[i] == pos + Vector3Int.down && lastDirection == 180) { Spawn(tileMap, tile[1], pos, 180); }
                }
                return;
            }

            // Проверка на фронт
            if (frontTiles != null && neighborTile == frontTiles)
            {
                // Поворачиваем последний провод к фронту

                if (rotation != lastDirection)
                {
                    if (neighbors[i] == pos + Vector3Int.left && lastDirection == 90) { Spawn(tileMap, tile[1], pos, 90); }
                    else if (neighbors[i] == pos + Vector3Int.left && lastDirection == 270) { Spawn(tileMap, tile[2], pos, 0); }

                    if (neighbors[i] == pos + Vector3Int.right && lastDirection == 90) { Spawn(tileMap, tile[2], pos, 180); }
                    else if (neighbors[i] == pos + Vector3Int.right && lastDirection == 270) { Spawn(tileMap, tile[1], pos, 270); }

                    if (neighbors[i] == pos + Vector3Int.up && lastDirection == 0) { Spawn(tileMap, tile[1], pos, 0); }
                    else if (neighbors[i] == pos + Vector3Int.up && lastDirection == 180) { Spawn(tileMap, tile[2], pos, 270); }

                    if (neighbors[i] == pos + Vector3Int.down && lastDirection == 0) { Spawn(tileMap, tile[2], pos, 90); }
                    else if (neighbors[i] == pos + Vector3Int.down && lastDirection == 180) { Spawn(tileMap, tile[1], pos, 180); }
                }
                return;
            }
        }
    }
    

    void ValidateConnection()
    {
        // Проверяем, соединены ли сервер с беком или фронтом
        bool hasServerAtStart = CheckForTileAtPosition(startPosition, serverTiles);
        bool hasBackAtEnd = CheckForTileAtPosition(endPosition, backTiles);
        bool hasFrontAtEnd = CheckForTileAtPosition(endPosition, frontTiles);

        // Также проверяем обратную ситуацию (сервер в конце, бек/фронт в начале)
        bool hasServerAtEnd = CheckForTileAtPosition(endPosition, serverTiles);
        bool hasBackAtStart = CheckForTileAtPosition(startPosition, backTiles);
        bool hasFrontAtStart = CheckForTileAtPosition(startPosition, frontTiles);

        // Обновляем глобальные переменные
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

    bool CheckForTileAtPosition(Vector3Int pos, Tile targetTiles)
    {
        if (targetTiles == null ) return false;

        // Проверяем саму позицию
        TileBase tileAtPos = tileMap.GetTile(pos);
        if (tileAtPos != null && tileAtPos == targetTiles)
            return true;

        // Проверяем соседние позиции
        Vector3Int[] neighbors = new Vector3Int[]
        {
            pos + Vector3Int.right,
            pos + Vector3Int.left,
            pos + Vector3Int.up,
            pos + Vector3Int.down
        };

        foreach (var neighbor in neighbors)
        {
            TileBase neighborTile = tileMap.GetTile(neighbor);
            if (neighborTile != null && neighborTile == targetTiles)
                return true;
        }

        return false;
    }

    

    int GetRotationFromDirection(Vector3Int direction)
    {
        if (direction.x > 0) return 0;
        else if (direction.x < 0) return 180;
        else if (direction.y > 0) return 90;
        else if (direction.y < 0) return 270;
        return 0;
    }

    void PlaceCornerWire(Vector3Int position, int fromDirection, int toDirection)//Позиция, поворот до, поворот после
    {
        if(tileMap.GetTile(position) == tile[0]) {
            if (fromDirection == 0 && toDirection == 90)
            {
                Spawn(tileMap, tile[1], position, 0);
            }
            else if (fromDirection == 90 && toDirection == 0)
            {
                Spawn(tileMap, tile[2], position, 180);
            }
            else if (fromDirection == 90 && toDirection == 180)
            {
                Spawn(tileMap, tile[1], position, 90);
            }
            else if (fromDirection == 180 && toDirection == 90)
            {
                Spawn(tileMap, tile[2], position, 270);
            }
            else if (fromDirection == 180 && toDirection == 270)
            {
                Spawn(tileMap, tile[1], position, 180);
            }
            else if (fromDirection == 270 && toDirection == 180)
            {
                Spawn(tileMap, tile[2], position, 0);
            }
            else if (fromDirection == 270 && toDirection == 0)
            {
                Spawn(tileMap, tile[1], position, 270);
            }
            else if (fromDirection == 0 && toDirection == 270)
            {
                Spawn(tileMap, tile[2], position, 90);
            }
        }
    }

    void ResetDrawing()
    {
        isDrawing = false;
        lastDirection = 0;
        startPosition = Vector3Int.zero;
        endPosition = Vector3Int.zero;
    }

    public void Spawn(Tilemap map, Tile tile, Vector3Int position, int Degre)
    {
        map.SetTile(position, null);
        
        place.PlaceTileAtMousePosition(position, tile, map);
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, Degre), Vector3.one);
        map.SetTransformMatrix(position, matrix);
    }
}