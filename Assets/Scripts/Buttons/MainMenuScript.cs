using UnityEngine;
using UnityEngine.Video;
public class MainMenuScript : MonoBehaviour{
    [SerializeField] VideoPlayer Video;
    [SerializeField] AudioSource src;

    [SerializeField] AudioClip normal_music;
    [SerializeField] AudioClip error_music;

    [SerializeField] VideoClip lens;
    [SerializeField] VideoClip error;
    int i;

    void Start(){
        i = Random.Range(1, 11);
        if(i == 7){
            Video.clip = error;
            src.clip = error_music;
        }
        else
        {
            Video.clip = lens;
            src.clip = normal_music;
        }
        Video.Play();
        src.Play();
    }
}