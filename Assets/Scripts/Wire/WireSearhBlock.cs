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
    Vector3Int PositionStart;
    Vector3Int Castom;
    Vector3Int PositionEnd;

    void Update()
    {
        if (place.CurrentTile != null&& place.tiles[PlaceScript.tileid] == tile[0] && MoneySystem.isAvailable()) 
        {

            PositionStart = place.GetTilePositionFromMouse();

            //Debug.Log(PositionStart);
            if (Input.GetMouseButton(0))
            {
                if (PositionEnd != place.GetTilePositionFromMouse())
                {
                    Castom = place.GetTilePositionFromMouse();
                    
                    if ( place.GetTilePositionFromMouse().x > PositionEnd.x) {
                        
                        Spawn(tileMap, tile[0], Castom, 0);  
                    }
                    if (place.GetTilePositionFromMouse().x < PositionEnd.x) {
                        
                        Spawn(tileMap, tile[0],Castom, 180);
                    }
                    
                    PositionEnd = place.GetTilePositionFromMouse();

                    //Debug.Log(PositionEnd);
                }
            }
        }
    }

    void Spawn( Tilemap map, Tile tile, Vector3Int position,int Degre)
    {
        map.SetTile(position, null);
        place.PlaceTileAtMousePosition(position, tile, tileMap);
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f,0f, Degre), Vector3.one);
        map.SetTransformMatrix(position, matrix);
        
    }
    

    
}