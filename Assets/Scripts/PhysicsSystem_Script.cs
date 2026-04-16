using UnityEngine;
using UnityEngine.Tilemaps;

public class PhysicsSystem_Script : MonoBehaviour
{
    [Header("TileMap")]
    [SerializeField] Tilemap blocks;

    [Header("MainBlocks")]
    [SerializeField] TileBase Server;

    [Header("ExtraBlocks")]
    [SerializeField] TileBase Hdd;
    [SerializeField] TileBase Ram;
    [SerializeField] TileBase Cooler;
    [SerializeField] TileBase Tab1;
    [SerializeField] TileBase Tab2;
    [SerializeField] TileBase Tab3;

    [Header("Variables")]
    [SerializeField] Vector3Int ServerPostion;
    [SerializeField] int MemoryAmount;
    [SerializeField] int RamAmount;
    [SerializeField] float Temperature;
    [SerializeField] float CoolantAmount;
    [SerializeField] int Type1Endure;
    [SerializeField] int Type2Endure;
    [SerializeField] int Type3Endure;
    [SerializeField] float Timer1 = 0;
    [SerializeField] AudioManager audioManager;

    [Header("Attack")]
    [SerializeField] public int RamNeed;
    [SerializeField] public int MemoryNeed;
    [SerializeField] public int Type1Amount;
    [SerializeField] public int Type2Amount;
    [SerializeField] public int Type3Amount;
    [SerializeField] bool isAttacking;

    [Header("Check")]
    [SerializeField] public bool isLose;
    [SerializeField] public bool isWin;
    [SerializeField] public bool isBackGood;
    [SerializeField] public bool isFrontGood;
    [SerializeField] WireSpawnBlue wsb;
    [SerializeField] WireSpawnRed wsr;




    void Start()
    {
        ServerPostion = GetServerPosition();
        RecalculateSystem();
    }

    void Update()
{
    if (isAttacking)
    {
        Timer1 += Time.deltaTime;
    }

    if (Timer1 >= 3f)
    {
        isAttacking = false;
        audioManager.StopPlaying();
        CheckAttack();
    }
}

    public void RecalculateSystem()
    {
        ResetValues();
        ServerCheck(ServerPostion);
        Temperature -= CoolantAmount;
    }

    void ResetValues()
    {
        MemoryAmount = 0;
        RamAmount = 0;
        Temperature = 0;
        CoolantAmount = 0;
        Type1Endure = 0;
        Type2Endure = 0;
        Type3Endure = 0;
    }

    void ServerCheck(Vector3Int pos)
    {
        Vector3Int[] neighbors =
        {
            new Vector3Int(pos.x+1,pos.y,0),
            new Vector3Int(pos.x-1,pos.y,0),
            new Vector3Int(pos.x,pos.y+1,0),
            new Vector3Int(pos.x,pos.y-1,0),
            new Vector3Int(pos.x+1,pos.y+1,0),
            new Vector3Int(pos.x+1,pos.y-1,0),
            new Vector3Int(pos.x-1,pos.y+1,0),
            new Vector3Int(pos.x-1,pos.y-1,0)
        };

        foreach (Vector3Int n in neighbors)
        {
            BlockCheck(n);
        }
    }

    Vector3Int GetServerPosition()
    {
        for (int i = -50; i <= 50; i++)
        {
            for (int j = -50; j <= 50; j++)
            {
                Vector3Int pos = new Vector3Int(i, j, 0);

                if (blocks.GetTile(pos) == Server)
                    return pos;
            }
        }

        return Vector3Int.zero;
    }

    void BlockCheck(Vector3Int pos)
    {
        TileBase tile = blocks.GetTile(pos);
        if (tile == null) return;

        if (tile == Hdd)
        {
            Temperature += 40;
            MemoryAmount += 1000;
        }
        else if (tile == Ram)
        {
            Temperature += 10;
            RamAmount += 32;
        }
        else if (tile == Cooler)
        {
            CoolantAmount += 50;
        }
        else if (tile == Tab1)
        {
            Type1Endure += 1000;
            Temperature += 1;
        }
        else if (tile == Tab2)
        {
            Type2Endure += 1000;
            Temperature += 1;
        }
        else if (tile == Tab3)
        {
            Type3Endure += 1000;
            Temperature += 1;
        }
    }

    public void Attack(){
        //anim.play("Shaking");
        audioManager.PlayWorkingSound();
        isAttacking = true;
    }

    public void CheckAttack(){
        if(Temperature<=110 && (Type1Endure - Type1Amount) >=0 && (Type2Endure - Type2Amount) >=0 && (Type3Endure - Type3Amount) >=0
         && (RamAmount - RamNeed) >=0 && (MemoryAmount - MemoryNeed) >=0 && isBackGood && isFrontGood
         && wsb.isConnectedToBack && wsb.isConnectedToFront && wsr.isConnectedToBack && wsr.isConnectedToFront && wsr.isConnectedToServer){
            Debug.Log("You WIN!");
            isWin = true;
            Timer1 = 0;
         }
         else{
            audioManager.PlayExplodeSound();
            Debug.Log("You LOSE!");
            isLose = true;
            Timer1 = 0;
         }
    }
}