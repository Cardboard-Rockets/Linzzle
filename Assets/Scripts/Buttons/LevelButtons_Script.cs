using UnityEngine;

public class LevelButtons_Script : MonoBehaviour
{
    [Header("ScriptConnections")]
    [SerializeField] PhysicsSystem_Script physicScript;

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
        LevelInfo.text = "Уровень " + physicScript.CurrentLevel; 

        Tab1Info.text = physicScript.Type1Amount + " запросов типа 1";
        Tab2Info.text = physicScript.Type2Amount + " запросов типа 2";
        Tab3Info.text = physicScript.Type3Amount + " запросов типа 3";

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
