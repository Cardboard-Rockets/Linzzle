using UnityEngine;
using UnityEngine.Tilemaps;

public class WireSpawn : MonoBehaviour
{
    [SerializeField] Tile[] tile; // »ндексы: 0 - пр€мой, 1 - угловой, 2 - отзеркаленный
    //[SerializeField] Grid grid;
    [SerializeField] Tilemap tileMap;
    [SerializeField] PlaceScript place;
    Vector3Int lastPlacedPosition;
    int lastDirection = 0; // 0-вправо, 90-вверх, 180-влево, 270-вниз

    void Update()
    {

        if (place.CurrentTile != null && place.tiles[PlaceScript.tileid] == tile[0] && MoneySystem.isAvailable())
        {
            Vector3Int currentPos = place.GetTilePositionFromMouse();

            // ≈сли начали новую линию (первый клик или новое место)
            if (Input.GetMouseButtonDown(0))
            {
                lastPlacedPosition = currentPos;
                // —тавим первый провод горизонтально (по умолчанию)
                Spawn(tileMap, tile[0], currentPos, 0);
            }

            // ѕродолжаем рисовать линию
            if (Input.GetMouseButton(0) && lastPlacedPosition != currentPos)
            {
                // ќпредел€ем направление от последней точки к текущей
                Vector3Int direction = currentPos - lastPlacedPosition;

                // ѕровер€ем, что двигаемс€ только по одной оси (не по диагонали)
                if (Mathf.Abs(direction.x) + Mathf.Abs(direction.y) == 1)
                {
                    // ќпредел€ем угол поворота дл€ пр€мого провода в зависимости от направлени€
                    int wireRotation = 0;
                    if (direction.x > 0) wireRotation = 0;      // вправо
                    else if (direction.x < 0) wireRotation = 180; // влево
                    else if (direction.y > 0) wireRotation = 90;  // вверх
                    else if (direction.y < 0) wireRotation = 270; // вниз

                    // ѕровер€ем, нужен ли угловой провод на предыдущей позиции
                    if (lastDirection != wireRotation && lastPlacedPosition != currentPos)
                    {
                        // —мотрим, с какого направлени€ мы пришли и куда идем
                        if (lastDirection == 0 && wireRotation == 90) // было вправо, стало вверх
                        {
                            Spawn(tileMap, tile[1], lastPlacedPosition, 0);
                        }
                        else if (lastDirection == 90 && wireRotation == 0) // было вверх, стало вправо
                        {
                            Spawn(tileMap, tile[2], lastPlacedPosition, 180);
                        }
                        else if (lastDirection == 90 && wireRotation == 180) // было вверх, стало влево
                        {
                            Spawn(tileMap, tile[1], lastPlacedPosition, 90);
                        }
                        else if (lastDirection == 180 && wireRotation == 90) // было влево, стало вверх
                        {
                            Spawn(tileMap, tile[2], lastPlacedPosition, 270);
                        }
                        else if (lastDirection == 180 && wireRotation == 270) // было влево, стало вниз
                        {
                            Spawn(tileMap, tile[1], lastPlacedPosition, 180);
                        }
                        else if (lastDirection == 270 && wireRotation == 180) // было вниз, стало влево
                        {
                            Spawn(tileMap, tile[2], lastPlacedPosition, 0);
                        }
                        else if (lastDirection == 270 && wireRotation == 0) // было вниз, стало вправо
                        {
                            Spawn(tileMap, tile[1], lastPlacedPosition, 270);
                        }
                        else if (lastDirection == 0 && wireRotation == 270) // было вправо, стало вниз
                        {
                            Spawn(tileMap, tile[2], lastPlacedPosition, 90);
                        }
                    }

                    // —тавим пр€мой провод на новой позиции
                    Spawn(tileMap, tile[0], currentPos, wireRotation);

                    lastPlacedPosition = currentPos;
                    lastDirection = wireRotation;
                }
            }

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

   
