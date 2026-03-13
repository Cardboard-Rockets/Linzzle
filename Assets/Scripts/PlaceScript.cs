
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceScript : MonoBehaviour
{
    public Grid grid;
    public Tilemap tileMap;
    public static int tileid;
    public TileBase[] tiles;
    public TileBase CurrentTile;
    [SerializeField] PhysicsSystem_Script physicScript;


    public int xmin;
    public int xmax;
    public int ymin;
    public int ymax;

    void Update()
    {
        CurrentTile = tiles[tileid];
        if (CurrentTile!=null && Input.GetMouseButtonDown(0) && isInArea() && MoneySystem.isAvailable())
        {
            if(tileid==7 || tileid==8){
                if(Input.GetMouseButton(0)){
                    Vector3Int cellpos =  GetTilePositionFromMouse();
                    PlaceTileAtMousePosition(GetTilePositionFromMouse(),CurrentTile,tileMap);
                }
                if(Input.GetMouseButtonUp(0)){
                    tileid = 0;
                }
            }
            else{
            Vector3Int cellpos =  GetTilePositionFromMouse();

                PlaceTileAtMousePosition(GetTilePositionFromMouse(),CurrentTile,tileMap);
                tileid = 0;
            }
            physicScript.RecalculateSystem();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(GetTilePositionFromMouse());
        }
    }

    public Vector3Int GetTilePositionFromMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mousePosition);
    }

    public void PlaceTileAtMousePosition(Vector3Int p,TileBase tile,Tilemap Map)
    {
        if (Map.GetTile(p) == null)
        {
            MoneySystem.buy(tileid);
            Map.SetTile(p, tile);
        }
    }

    bool isInArea()
    {
        Vector3Int cellpos = GetTilePositionFromMouse();
        return cellpos.x > xmin && cellpos.x < xmax &&
                cellpos.y > ymin && cellpos.y < ymax;
    }
}
