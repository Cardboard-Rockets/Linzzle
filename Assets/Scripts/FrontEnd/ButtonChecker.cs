using UnityEngine;

public class ButtonChecker : MonoBehaviour
{
    [SerializeField] private LevelData currentLevel;
    [SerializeField] private PhysicsSystem_Script pscr;

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