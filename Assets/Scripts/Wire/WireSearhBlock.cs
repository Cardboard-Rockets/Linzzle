using NUnit.Framework.Internal.Filters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WireSearh : MonoBehaviour
{
    [SerializeField] Tile[] tile;
    //[SerializeField] Grid grid;
    [SerializeField] Tilemap tileMap;
    [SerializeField] PlaceScript place;
    private Tile CurrentTile;
    
    Vector3Int PositionEnd;

    void Update()
    {
        
        if (place.CurrentTile != null && place.tiles[PlaceScript.tileid] == tile[0] && MoneySystem.isAvailable())
        {

            if (PositionEnd != place.GetTilePositionFromMouse()&& Input.GetMouseButton(0))
            {


                if (place.GetTilePositionFromMouse().x > PositionEnd.x)
                {
                    Spawn(tileMap, tile[0], PositionEnd, 0);
                }
                else if (place.GetTilePositionFromMouse().x < PositionEnd.x)
                {
                    Spawn(tileMap, tile[0], PositionEnd, 180);
                }
                else if (place.GetTilePositionFromMouse().y < PositionEnd.y)
                {
                    Spawn(tileMap, tile[0], PositionEnd, 270);
                }
                else if (place.GetTilePositionFromMouse().y > PositionEnd.y)
                {
                    Spawn(tileMap, tile[0], PositionEnd, 90);
                }

                


            }
            PositionEnd = place.GetTilePositionFromMouse();
        }
        //else if(place.tiles[PlaceScript.tileid] == tile[0]  && Input.GetMouseButtonUp(0)) { PlaceScript.tileid = 0; }

    }

    void Spawn(Tilemap map, Tile tile, Vector3Int position, int Degre)
    {
        map.SetTile(position, null);
        place.PlaceTileAtMousePosition(position, tile, tileMap);
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, Degre), Vector3.one);
        map.SetTransformMatrix(position, matrix);

    }



}