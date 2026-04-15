using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Buttons_Script : MonoBehaviour
{
  private double timer = 0;
  private bool startTimer;
  void Update()
  {
    if(startTimer){
    timer+=Time.deltaTime;
    }
    if (timer >= 1f)
    {
      LoadLevelSelect();
      startTimer = false;
    }
  }
  public void GoToLevelSelect(){
    startTimer = true;
  }

  public void LoadLevelSelect()
  {
    SceneManager.LoadScene("LevelSelect");
  }

  public void Quit()
  {
    Application.Quit();
  }

  public void OpenGitHub()
  {
    Application.OpenURL("https://github.com/Cardboard-Rockets/Linzzle");
  }
}
