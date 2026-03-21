using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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
    public static bool isConnectedToBack = false;
    public static bool isConnectedToFront = false;

    void Update()
    {


        if (place.CurrentTile != null && place.tiles[PlaceScript.tileid] == tile[0] && MoneySystem.isAvailable())
        {
            Vector3Int currentPos = place.GetTilePositionFromMouse();

            // Если начали новую линию (первый клик или новое место)
            if (Input.GetMouseButtonDown(0))
            {
                StartDrawingLine(currentPos);
            }

            // Продолжаем рисовать линию
            if (Input.GetMouseButton(0) && isDrawing && lastPlacedPosition != currentPos)
            {
                ContinueDrawingLine(currentPos);
            }
            // Конец рисования (отпустили кнопку)
            if (Input.GetMouseButtonUp(0))
            {
                ResetDrawing();
            }
        }
    }

    // Начало рисования линии
    void StartDrawingLine(Vector3Int startPos)
    {
        lastPlacedPosition = startPos;
        isDrawing = true;
    }
    void ResetDrawing()
    {
        isDrawing = false;
        lastDirection = 0;
        // lastPlacedPosition не сбрасываем, так как он используется для сравнения
    }

    // Продолжение рисования линии
    void ContinueDrawingLine(Vector3Int currentPos)
    {
        // Определяем направление от последней точки к текущей
        Vector3Int direction = currentPos - lastPlacedPosition;

        
        int wireRotation = GetRotationFromDirection(direction);

        // Проверяем, нужен ли угловой провод на предыдущей позиции
        if (lastDirection != wireRotation && lastPlacedPosition != currentPos)
        {
            PlaceCornerWire(lastPlacedPosition, lastDirection, wireRotation);
        }

        // Ставим прямой провод на новой позиции
        if (tileMap.GetTile(currentPos) == null)
        {
            Spawn(tileMap, tile[0], currentPos, wireRotation);
        }

        lastPlacedPosition = currentPos;
        lastDirection = wireRotation;
        
    }

    // Получение угла поворота из направления
    int GetRotationFromDirection(Vector3Int direction)
    {
        if (direction.x > 0) return 0;      // вправо
        else if (direction.x < 0) return 180; // влево
        else if (direction.y > 0) return 90;  // вверх
        else if (direction.y < 0) return 270; // вниз
        return 0;
    }

    // Размещение углового провода в зависимости от направления движения
    void PlaceCornerWire(Vector3Int position, int fromDirection, int toDirection)
    {
        // Смотрим, с какого направления мы пришли и куда идем
        if (fromDirection == 0 && toDirection == 90) // было вправо, стало вверх
        {
            Spawn(tileMap, tile[1], position, 0);
        }
        else if (fromDirection == 90 && toDirection == 0) // было вверх, стало вправо
        {
            Spawn(tileMap, tile[2], position, 180);
        }
        else if (fromDirection == 90 && toDirection == 180) // было вверх, стало влево
        {
            Spawn(tileMap, tile[1], position, 90);
        }
        else if (fromDirection == 180 && toDirection == 90) // было влево, стало вверх
        {
            Spawn(tileMap, tile[2], position, 270);
        }
        else if (fromDirection == 180 && toDirection == 270) // было влево, стало вниз
        {
            Spawn(tileMap, tile[1], position, 180);
        }
        else if (fromDirection == 270 && toDirection == 180) // было вниз, стало влево
        {
            Spawn(tileMap, tile[2], position, 0);
        }
        else if (fromDirection == 270 && toDirection == 0) // было вниз, стало вправо
        {
            Spawn(tileMap, tile[1], position, 270);
        }
        else if (fromDirection == 0 && toDirection == 270) // было вправо, стало вниз
        {
            Spawn(tileMap, tile[2], position, 90);
        }
    }

    public void Spawn(Tilemap map, Tile tile, Vector3Int position, int Degre)
    {
        map.SetTile(position, null);
        place.PlaceTileAtMousePosition(position, tile, tileMap);
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, Degre), Vector3.one);
        map.SetTransformMatrix(position, matrix);
    }
}

