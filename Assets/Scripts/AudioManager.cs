using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip[] sounds;
    
    public void PlayButtonSound()
    {
        src.clip = sounds[0];
        src.Play();
    }

    public void PlayWorkingSound()
    {
        src.clip = sounds[1];
        src.Play();
    }

    public void PlayPickupSound()
    {
        src.clip = sounds[2];
        src.Play();
    }

    public void PlayExplodeSound()
    {
        src.clip = sounds[3];
        src.Play();
    }
    public void PlayPlaceSound()
    {
        src.clip = sounds[4];
        src.Play();
    }

    public void StopPlaying()
    {
        src.clip = null;
        src.Stop();
    }
}
