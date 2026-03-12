using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static bool isPaused;
    public GameObject pauseMenu;

    void Start()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePause();
        }
    }

    public void togglePause()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void toMainMenu(){
        SceneManager.LoadScene("MainMenu"); 
    }
}
