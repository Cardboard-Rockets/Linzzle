using UnityEngine;

public class ButtonChecker : MonoBehaviour
{
    private LevelData currentLevel;
    [SerializeField] private PhysicsSystem_Script pscr;
    [SerializeField] string filename;

    private void Awake()
    {
        LoadLevel(filename);
    }

    void LoadLevel(string levelName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(levelName);

        if (jsonFile == null)
        {
            Debug.LogError("JSON не найден!");
            return;
        }

        currentLevel = JsonUtility.FromJson<LevelData>(jsonFile.text);

        Debug.Log("Level загружен: " + levelName);
    }

    public void CheckGraph()
    {
        if (currentLevel == null)
        {
            Debug.LogError("GraphCheckButton: currentLevel не задан!");
            return;
        }

        bool isCorrect = GraphChecker.CheckGraph(currentLevel);
        pscr.isFrontGood = isCorrect;

        Debug.Log(isCorrect ? 
            "Граф совпадает с эталоном — правильный!" : 
            "Граф НЕ совпадает с эталоном — ошибка.");
    }
}