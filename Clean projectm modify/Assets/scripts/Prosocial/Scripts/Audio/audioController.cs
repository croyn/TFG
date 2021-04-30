using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour
{
    // Start is called before the first frame update
    public static audioController instance; //singleton
    public AudioSource audioSource;//GameObject audio source
    public AudioClip audio1;//1 audio file
    public AudioClip audio2;//2 audio file
    public AudioClip audio3;//3 audio file
    public AudioClip audio4;//4 audio file
    public float volume = 0.5f;//control global volume
    void Start()
    {
        //singleton
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //function to acces from anywhere to the audio we need and play it
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

        //if there is a audio
        if (actual != null) {
            //play one time
            audioSource.PlayOneShot(actual, volume);
        }
        

    }


}
