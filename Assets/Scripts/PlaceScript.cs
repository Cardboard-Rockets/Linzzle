using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceScript : MonoBehaviour
{
    [Header("Tilemaps")]
    public Grid grid;
    public Tilemap tileMap;
    public static int tileid;
    public TileBase[] tiles;
    
    public TileBase CurrentTile;
    [SerializeField] PhysicsSystem_Script physicScript;
    [SerializeField] AudioManager audioManager;
    [SerializeField] TileInfoScript tscr;

    [Header("Building area coordinates")]
    public int xmin;
    public int xmax;
    public int ymin;
    public int ymax;


    void Update()
    {

        CurrentTile = tiles[tileid];
        if (Input.GetKeyDown(KeyCode.Q)) { tileid = 0; }

        if(CurrentTile == null){
            if(Input.GetMouseButtonDown(0)){
                tscr.ShowTileInfo();
            }
        }

        if (CurrentTile != null && isInArea() && MoneySystem.isAvailable())
        {
            if (Input.GetMouseButtonDown(0))
            {
                audioManager.PlayPlaceSound();
                Vector3Int cellpos = GetTilePositionFromMouse();
                PlaceTileAtMousePosition(cellpos, CurrentTile, tileMap);

                if (tileid != 7 && tileid != 8) { 
                    tileid = 0; 
                }
            }
            if ((tileid == 7 || tileid == 8) && Input.GetMouseButtonUp(0))
            {
               tileid = 0;
            }

            physicScript.RecalculateSystem();
        }
    }

    public Vector3Int GetTilePositionFromMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mousePosition);
    }

    public void PlaceTileAtMousePosition(Vector3Int p, TileBase tile, Tilemap Map)
    {
        if (Map.GetTile(p) == null)
        {
            tscr.HideInfoBlocks();// Скрываем информационные блоки после установки тайла
            Map.SetTile(p, tile);
            if (Map.GetTile(p) != null)
            {
                MoneySystem.buy(tileid);
            }
        }
    }

    bool isInArea()
    {
        Vector3Int cellpos = GetTilePositionFromMouse();
        return cellpos.x >= xmin && cellpos.x <= xmax &&
                cellpos.y >= ymin && cellpos.y <= ymax;
    }

    
}