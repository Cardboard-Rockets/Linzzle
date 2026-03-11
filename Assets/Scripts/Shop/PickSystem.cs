using UnityEngine;
using UnityEngine.Tilemaps;

public class PickSystem : MonoBehaviour
{
    public TileBase hdd;
    public TileBase cool;
    public TileBase ram;

    public void SetHdd()
    {
        PlaceScript.tileid = 1;
    }

    public void SetCool()
    {
        PlaceScript.tileid = 3;
    }

    public void SetRam()
    {
        PlaceScript.tileid = 2;
    }

    public void SetTable1()
    {
        PlaceScript.tileid = 4;
    }

    public void SetTable2()
    {
        PlaceScript.tileid = 5;
    }

    public void SetTable3()
    {
        PlaceScript.tileid = 6;
    }

    public void SetWireBlock()
    {
        PlaceScript.tileid = 7;
    }

    public void SetWirePower()
    {
        PlaceScript.tileid = 8;
    }
}
