using UnityEngine;

public class LevelButtons_Script : MonoBehaviour
{
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
}
