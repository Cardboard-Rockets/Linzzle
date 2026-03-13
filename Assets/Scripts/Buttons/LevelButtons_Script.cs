using UnityEngine;

public class LevelButtons_Script : MonoBehaviour
{
    [SerializeField] PhysicsSystem_Script physicScript;
    public AudioSource src;
    bool muted;
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
}
