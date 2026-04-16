using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseWire : MonoBehaviour
{
    [SerializeField] protected PlaceScript place; // Назначается в инспекторе или через код
    //[SerializeField] protected Zone zone;
    [Header("Place Zones")]
    public List<PlaceZone> placeZones = new List<PlaceZone>();


    public int lastDirection = 0;
    public bool isDrawing = false;
    public Vector3Int lastPlacedPosition;
    public Vector3Int startPosition;
    public Vector3Int endPosition;

    

   

    public virtual void PlaceCornerWire(Tilemap tileMap, Tile[] tile, Vector3Int position, int fromDirection, int toDirection)
    {
        if (tileMap.GetTile(position) == tile[0])
        {
            if (fromDirection == 0 && toDirection == 90)
                Spawn(tileMap, tile[1], position, 0);
            else if (fromDirection == 90 && toDirection == 0)
                Spawn(tileMap, tile[2], position, 180);
            else if (fromDirection == 90 && toDirection == 180)
                Spawn(tileMap, tile[1], position, 90);
            else if (fromDirection == 180 && toDirection == 90)
                Spawn(tileMap, tile[2], position, 270);
            else if (fromDirection == 180 && toDirection == 270)
                Spawn(tileMap, tile[1], position, 180);
            else if (fromDirection == 270 && toDirection == 180)
                Spawn(tileMap, tile[2], position, 0);
            else if (fromDirection == 270 && toDirection == 0)
                Spawn(tileMap, tile[1], position, 270);
            else if (fromDirection == 0 && toDirection == 270)
                Spawn(tileMap, tile[2], position, 90);
        }
    }


    protected bool IsInAnyZone(Vector3Int position)
    {
        if (placeZones == null || placeZones.Count == 0)
            return true; // если зон нет — разрешаем везде

        foreach (var zone in placeZones)
        {
            if (zone.IsInside(position))
                return true;
        }

        return false;
    }


    public virtual void Spawn(Tilemap map, Tile tile, Vector3Int position, int Degre)
    {
        if (!IsInAnyZone(position))
            return;

        map.SetTile(position, null);

        if (place != null) place.PlaceTileAtMousePosition(position, tile, map);



        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, Degre), Vector3.one);
        map.SetTransformMatrix(position, matrix);
    }

    public int GetRotationFromDirection(Vector3Int direction)
    {
        if (direction.x > 0) return 0;
        else if (direction.x < 0) return 180;
        else if (direction.y > 0) return 90;
        else if (direction.y < 0) return 270;
        return 0;
    }

    public bool CheckForTileAtPosition(Tilemap tileMap, Vector3Int pos, Tile targetTiles)
    {
        if (targetTiles == null) return false;

        TileBase tileAtPos = tileMap.GetTile(pos);
        if (tileAtPos != null && tileAtPos == targetTiles) return true;

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

    public void ResetDrawing()
    {
        isDrawing = false;
        lastDirection = 0;
        startPosition = Vector3Int.zero;
        endPosition = Vector3Int.zero;
    }

    public virtual void CheckEndConnection(Vector3Int pos, Tile[] Tiles, Tilemap tileMap, Tile[] tile)
    {
        Vector3Int[] neighbors = new Vector3Int[] { pos + Vector3Int.right, pos + Vector3Int.left, pos + Vector3Int.up, pos + Vector3Int.down };
        int[] neighborDirections = new int[] { 0, 180, 90, 270 };

        for (int i = 0; i < neighbors.Length; i++)
        {
            TileBase neighborTile = tileMap.GetTile(neighbors[i]);
            int rotation = neighborDirections[i];

            // Проверка на бек
            if (Tiles[0] != null && neighborTile == Tiles[0])
            {
                if (rotation != lastDirection)
                {
                    if (neighbors[i] == pos + Vector3Int.left && lastDirection == 90) Spawn(tileMap, tile[1], pos, 90);
                    else if (neighbors[i] == pos + Vector3Int.left && lastDirection == 270) Spawn(tileMap, tile[2], pos, 0);
                    else if (neighbors[i] == pos + Vector3Int.right && lastDirection == 90) Spawn(tileMap, tile[2], pos, 180);
                    else if (neighbors[i] == pos + Vector3Int.right && lastDirection == 270) Spawn(tileMap, tile[1], pos, 270);
                    else if (neighbors[i] == pos + Vector3Int.up && lastDirection == 0) Spawn(tileMap, tile[1], pos, 0);
                    else if (neighbors[i] == pos + Vector3Int.up && lastDirection == 180) Spawn(tileMap, tile[2], pos, 270);
                    else if (neighbors[i] == pos + Vector3Int.down && lastDirection == 0) Spawn(tileMap, tile[2], pos, 90);
                    else if (neighbors[i] == pos + Vector3Int.down && lastDirection == 180) Spawn(tileMap, tile[1], pos, 180);
                }
                return;
            }

            // Проверка на фронт
            if (Tiles[1] != null && neighborTile == Tiles[1])
            {
                if (rotation != lastDirection)
                {
                    if (neighbors[i] == pos + Vector3Int.left && lastDirection == 90) Spawn(tileMap, tile[1], pos, 90);
                    else if (neighbors[i] == pos + Vector3Int.left && lastDirection == 270) Spawn(tileMap, tile[2], pos, 0);
                    else if (neighbors[i] == pos + Vector3Int.right && lastDirection == 90) Spawn(tileMap, tile[2], pos, 180);
                    else if (neighbors[i] == pos + Vector3Int.right && lastDirection == 270) Spawn(tileMap, tile[1], pos, 270);
                    else if (neighbors[i] == pos + Vector3Int.up && lastDirection == 0) Spawn(tileMap, tile[1], pos, 0);
                    else if (neighbors[i] == pos + Vector3Int.up && lastDirection == 180) Spawn(tileMap, tile[2], pos, 270);
                    else if (neighbors[i] == pos + Vector3Int.down && lastDirection == 0) Spawn(tileMap, tile[2], pos, 90);
                    else if (neighbors[i] == pos + Vector3Int.down && lastDirection == 180) Spawn(tileMap, tile[1], pos, 180);
                }
                return;
            }
        }
    }
}