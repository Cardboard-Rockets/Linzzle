using UnityEngine;

public class LevelButtons_Script : MonoBehaviour
{
    [Header("ScriptConnections")]
    [SerializeField] PhysicsSystem_Script physicScript;
    [SerializeField] LevelManager levelManager;

    [Header("OtherStuff")]
    public AudioSource src;
    public bool muted;

    [Header("InfoBlock")]
    [SerializeField] GameObject InfoBlock;
    [SerializeField] TMPro.TextMeshProUGUI Tab1Info;
    [SerializeField] TMPro.TextMeshProUGUI Tab2Info;
    [SerializeField] TMPro.TextMeshProUGUI Tab3Info;
    [SerializeField] TMPro.TextMeshProUGUI RamInfo;
    [SerializeField] TMPro.TextMeshProUGUI MemoryInfo;
    [SerializeField] TMPro.TextMeshProUGUI LevelInfo;

    void Start(){
        InfoBlock.SetActive(false);
        LevelInfo.text = "Уровень " + levelManager.CurrentLevel; 

        Tab1Info.text = physicScript.Type1Amount + " запросов типа GET";
        Tab2Info.text = physicScript.Type2Amount + " запросов типа POST";
        Tab3Info.text = physicScript.Type3Amount + " запросов типа PUT";

        RamInfo.text = physicScript.RamNeed + "гБ оперативной памяти";
        MemoryInfo.text = physicScript.MemoryNeed + "гБ внутренней памяти";
    }

    public void MuteButton()
    {
        muted = !muted;
        if (muted)
        {
            src.mute = true;
        }
        else
        {
            src.mute = false;
        }
    }

    public void StartButton()
    {
        physicScript.Attack();
    }

    public void ShowInfo(){
        InfoBlock.SetActive(!InfoBlock.activeSelf);
    }
}
