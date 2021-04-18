using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour
{
    // Start is called before the first frame update
    public static audioController instance;
    public AudioSource audioSource;
    public AudioClip audio1;
    public AudioClip audio2;
    public AudioClip audio3;
    public AudioClip audio4;
    public float volume = 0.5f;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playAudio(int wich) {
        AudioClip actual=null;
        switch (wich) {
            case 0:
                actual = audio1;
                break;
            case 1:
                actual = audio2;
                break;
            case 2:
                actual = audio3;
                break;
            case 3:
                actual = audio4;
                break;


        }
        if (actual != null) {
            audioSource.PlayOneShot(actual, volume);
        }
        

    }


}
