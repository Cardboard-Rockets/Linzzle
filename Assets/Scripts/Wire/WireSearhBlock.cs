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
    Vector3Int PositionEnd;

    void Update()
    {
        if (place.CurrentTile != null&& place.tiles[PlaceScript.tileid] == tile[0] && MoneySystem.isAvailable()) 
        {

            PositionStart = place.GetTilePositionFromMouse();

            //Debug.Log(PositionStart);
            if (Input.GetMouseButton(0))
            {
                if(PositionEnd!= place.GetTilePositionFromMouse()){
                    PositionEnd = place.GetTilePositionFromMouse();

                    Debug.Log(PositionEnd);
                    place.PlaceTileAtMousePosition(PositionEnd, tile[0], tileMap); 
                }
            }
        }
    }

}
