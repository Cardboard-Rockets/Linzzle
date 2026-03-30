using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInfoScript : MonoBehaviour
{
    [Header("TileInfoBlocks")]
    [SerializeField] TileBase FE;
    [SerializeField] TileBase BE;
    [SerializeField] TileBase S;
    [SerializeField] GameObject ExtraTileInfoBlock;
    [SerializeField] GameObject TileInfoBlock;
    [SerializeField] GameObject ExBu;
    [SerializeField] GameObject DeBu;

    private GameObject currentActiveInfoBlock;
    private Vector3Int lastTilePosition;
    private bool isInfoBlockActive = false;
    [SerializeField] TMPro.TextMeshProUGUI ExtraText;
    [SerializeField] TMPro.TextMeshProUGUI DefText;
    [SerializeField] TMPro.TextMeshProUGUI DefDescText;
    [SerializeField] TMPro.TextMeshProUGUI ExtraDecsText;
    private int sellid;
    private string defdesc = "";
    private string extradesc = "";


    [Header("ScriptConnectors")]
    [SerializeField] PhysicsSystem_Script physsys;
    [SerializeField] PlaceScript pscr;
    [SerializeField] MoneySystem msys;

    struct TileData
    {
        public string name;
        public string desc;
    }

    void Start(){
        ExtraTileInfoBlock.SetActive(false);
        TileInfoBlock.SetActive(false);
    }

    void Update(){
        if (isInfoBlockActive)
        {
            UpdateInfoBlockPosition();
        }
    }

    public void ShowTileInfo()
    {
        Vector3Int tp = pscr.GetTilePositionFromMouse();
        TileBase clickedTile = pscr.tileMap.GetTile(tp);
        
        if (clickedTile != null)
        {
            lastTilePosition = tp;
            isInfoBlockActive = true;
            
            bool isSpecialTile = (clickedTile == FE || clickedTile == BE || clickedTile == S);
            currentActiveInfoBlock = isSpecialTile ? TileInfoBlock : ExtraTileInfoBlock;
            GameObject inactiveBlock = isSpecialTile ? ExtraTileInfoBlock : TileInfoBlock;

            
            
            TileData data = GetTileName();
            DefText.text = data.name;
            ExtraText.text = data.name;
            DefDescText.text = data.desc;
            ExtraDecsText.text = data.desc;
            PositionBlockAtTile(currentActiveInfoBlock, tp);
            currentActiveInfoBlock.SetActive(true);
            if (isSpecialTile)
            {
                DeBu.SetActive(true);
                if (clickedTile == S)
                {
                    DeBu.SetActive(false);
                }
            }
            else
            {
                ExBu.SetActive(true);
                if (data.name == "Электро щиток")
                {
                    ExBu.SetActive(false);
                }
            }
            
            inactiveBlock.SetActive(false);
        }
        else
        {
            HideInfoBlocks();
        }
    }

    public void UpdateInfoBlockPosition()
    {
        if (currentActiveInfoBlock != null && isInfoBlockActive)
        {
            PositionBlockAtTile(currentActiveInfoBlock, lastTilePosition);
        }
    }

    void PositionBlockAtTile(GameObject block, Vector3Int tilePos)
    {
        Vector3 worldPos = pscr.tileMap.GetCellCenterWorld(tilePos);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        block.GetComponent<RectTransform>().position = screenPos;
    }

    public void HideInfoBlocks()
    {
        TileInfoBlock.SetActive(false);
        ExtraTileInfoBlock.SetActive(false);
        isInfoBlockActive = false;
        currentActiveInfoBlock = null;
    }

    public void SellTile()
    {
        Vector3Int tp = pscr.GetTilePositionFromMouse();
        TileBase clickedTile = pscr.tileMap.GetTile(tp);
        if (clickedTile != null && clickedTile != S && clickedTile != FE && clickedTile != BE)
        {
            for (int i = 0; i < pscr.tiles.Length; i++)
            {
                if (clickedTile == pscr.tiles[i])
                {
                    sellid = i;
                    break;
                }
            }
        
            MoneySystem.money += MoneySystem.price[sellid];
            pscr.tileMap.SetTile(tp, null);
            HideInfoBlocks();
        }
    }

    TileData GetTileName()
{
    TileData data = new TileData();
    Vector3Int tp = pscr.GetTilePositionFromMouse();
    TileBase clickedTile = pscr.tileMap.GetTile(tp);
    int j = 0;
    
    if (clickedTile == S)
    {
        j = 9;
    }
    else if (clickedTile == FE)
    {
        j = 10;
    }
    else if (clickedTile == BE)
    {
        j = 11;
    }
    else
    {
        for (int i = 0; i < pscr.tiles.Length; i++)
        {
            if (clickedTile == pscr.tiles[i])
            {
                j = i;
                break;
            }
        }
    }

    switch(j)
    {
        case 0: data.desc = "Подает электричество ко всем модулям" ; data.name = "Электро щиток"; break;
        case 1: data.desc = "Объем памяти: 1тБ\nПовышает температуру на 60" ; data.name = "HDD"; break;
        case 2: data.desc = "Объем памяти: 32гБ\nПовышает температуру на 40" ; data.name =  "RAM"; break;
        case 3: data.desc = "Понижает температруру на 20" ; data.name =  "Кулер"; break;
        case 4: data.desc = "Обрабатывает 1000 запросов" ; data.name = "GET Обработчик"; break;
        case 5: data.desc = "Обрабатывает 1000 запросов" ; data.name = "POST Обработчик"; break;
        case 6: data.desc = "Обрабатывает 1000 запросов" ; data.name = "PUT Обработчик"; break;
        case 7: data.desc = "" ; data.name = "Провод для данных"; break;
        case 8: data.desc = "" ; data.name = "Провод электропитания"; break;
        case 9: data.desc = "" ; data.name = "Сервер"; break;
        case 10: data.desc = "" ; data.name = "Front-End модуль"; break;
        case 11: data.desc = "" ; data.name = "Back-End модуль"; break;
        case 12: data.desc = "" ; data.name = "Провод для данных"; break;
        case 13: data.desc = "" ; data.name = "Провод для данных"; break;
    }
    return data;
}
public string isFEorBE()
    {
    Vector3Int tp = pscr.GetTilePositionFromMouse();
    TileBase clickedTile = pscr.tileMap.GetTile(tp);
        if(clickedTile == FE)
        {
              return "Front";  
        }
        if(clickedTile == BE)
        {
            return "Back";
        }
        else{return"";}
    }
}



