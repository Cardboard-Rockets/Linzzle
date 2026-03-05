using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public AudioSource src;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            CheckSceneAndMuteMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneAndMuteMusic();
    }

    void CheckSceneAndMuteMusic()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if(currentSceneName!="MainMenu" && currentSceneName != "LevelSelect")
        {
            Destroy(gameObject);
        }
    }
}