using UnityEngine;
using System.Collections.Generic;

public class RandomMusicPlayer : MonoBehaviour
{
    public AudioClip[] music;
    public bool playOnStart = true;
    public bool shuffle = true; 
    public AudioSource src;
    [Range(0f, 1f)] public float volume = 1f;

    private void Awake()
    {
        src.playOnAwake = false;
        src.loop = false;
        src.volume = volume;
    }

    private void Start()
    {
        if (playOnStart)
        {
            PlayNextTrack();
        }
    }

    private void Update()
    {
        if (!src.isPlaying && src.time == 0f)
        {
            PlayNextTrack();
        }
    }

    public void PlayNextTrack()
    {
        int randomIndex = Random.Range(0, music.Length);
        AudioClip nextTrack = music[randomIndex];

        src.clip = nextTrack;
        src.Play();
    }
}