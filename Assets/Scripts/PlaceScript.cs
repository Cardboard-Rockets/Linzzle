
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceScript : MonoBehaviour
{
    public Grid grid;
    public Tilemap tileMap;
    public static int tileid;
    public TileBase[] tiles;
    public TileBase CurrentTile;


    public int xmin;
    public int xmax;
    public int ymin;
    public int ymax;

    void Update()
    {
        CurrentTile = tiles[tileid];
        if (CurrentTile!=null && Input.GetMouseButtonDown(0) && isInArea() && MoneySystem.isAvailable())
        {
            Vector3Int cellpos =  GetTilePositionFromMouse();

            if(tileMap.GetTile(cellpos)==null)
            {
                PlaceTileAtMousePosition(CurrentTile);
                MoneySystem.buy(tileid);
            }
            tileid = 0;

        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(GetTilePositionFromMouse());
        }
    }

    Vector3Int GetTilePositionFromMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mousePosition);
    }

    void PlaceTileAtMousePosition(TileBase tile)
    {
        tileMap.SetTile(GetTilePositionFromMouse(), tile);
    }

    bool isInArea()
    {
        Vector3Int cellpos = GetTilePositionFromMouse();
        return cellpos.x > xmin && cellpos.x < xmax &&
                cellpos.y > ymin && cellpos.y < ymax;
    }
}
