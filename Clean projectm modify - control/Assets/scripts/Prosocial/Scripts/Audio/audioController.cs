using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour
{
    // Start is called before the first frame update
    public static audioController instance; //singleton
    public AudioSource audioSource;//GameObject audio source
    public AudioClip audioFull1;//1 audio file
    public AudioClip audioFull2;//2 audio file
    public AudioClip audioFull3;//3 audio file
    public AudioClip audioCircle;//4 audio file
    public AudioClip audioTransition;//5 audio file
    public AudioClip audioAppearMandala;//6 audio file
    public AudioClip audioGoodScan;//7 audio file
    public AudioClip audioEnding;//8 audio file
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
                actual = audioFull1;
                break;
            case 1:
                actual = audioFull2;
                break;
            case 2:
                actual = audioFull3;
                break;
            case 3:
                actual = audioCircle;
                break;
            case 4:
                actual = audioTransition;
                break;
            case 5:
                actual = audioAppearMandala;
                break;
            case 6:
                actual = audioGoodScan;
                break;
            case 7:
                actual = audioEnding;
                break;


        }

        //if there is a audio
        if (actual != null) {
            //play one time
            audioSource.PlayOneShot(actual, volume);
        }
        

    }


}
