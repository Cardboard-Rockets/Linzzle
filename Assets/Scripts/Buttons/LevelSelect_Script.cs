using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect_Script : MonoBehaviour
{   
    [SerializeField] AnimationScript animationScript;
    void Start()
    {
        animationScript.MainMenuPanelOut();
    }
    public void GoToLevelOne()
    {
        SceneManager.LoadScene("Level_1");
    }
    public void GoToLevelTwo()
    {
        SceneManager.LoadScene("Level_2");
    }
    public void GoToLevelThree()
    {
        SceneManager.LoadScene("Level_3");
    }
}
