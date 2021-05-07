using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script used to detect and manage a collisión with footprint in the fase of scan
public class collisionScan : MonoBehaviour
{
    public float timeIn = 0.0f; //control the time that a user is in the foot
    public bool isIn;//know if it is in
    public Color actualColor; //Actual color of the footprint
    public Color ColorGreen;//Color for the footprint when is scanned
    bool playSong;
    // Start is called before the first frame update
    void Start()
    {
        //initialation of the variable when instanciate
        isIn = false;
        timeIn = 0.0f;
        playSong = false;
    }

    // Update is called once per frame
    void Update()
    {

        //if user in
        if (isIn)
        {
            //add time if doesn go > 20
            if (timeIn + Time.deltaTime <= 3.1f) {
                timeIn = timeIn + Time.deltaTime;
                if (checkTimeIn())
                {

                    if (!playSong) {
                        audioController.instance.playAudio(6);
                        playSong = true;
                    }
                    
                    //color green
                    changeColorTo(1);
                }
            }
                
            
        }
        else if (!isIn && timeIn > 0.0f) { //if not in and time >0
            //substract time
            timeIn = timeIn - Time.deltaTime;

            if (timeIn < 0.0f) {
                timeIn = 0.0f;
            }

            if (checkTimeIn())
            {
                //color green
                changeColorTo(1);
            }
            else
            {
                //color white
                changeColorTo(0);
                if (playSong)
                {
                    playSong = false;
                }
            }
        }

        //if true
        
    }

    //change de color to 0 white,1 variable colorGreen,2 red (not used).
    public void changeColorTo(int which) {
        
            //get the renderer of the actual GameObject
            Renderer footMaterial = gameObject.GetComponent<Renderer>();

            //which color
            switch (which)
            {
                case 0:
                    footMaterial.material.SetColor("_Color", Color.white);
                    actualColor= Color.white;
                    break;
                case 1:
                //footMaterial.material.SetColor("_Color", ColorGreen);
                footMaterial.material.SetColor("_Color", Color.green);
                actualColor = Color.green;
                    
                    break;
                case 2:
                    footMaterial.material.SetColor("_Color", Color.red);
                    actualColor = Color.red;
                    break;

            }    

    }


    //allows us to know if the time in the footprint is okay to know the user is scanned
    private bool checkTimeIn() {
        if (timeIn >= 3.0f) {
            return true;

        }
        return false;
    }

    //function that detects the event OnTriggerStay with other colliders.
    private void OnTriggerStay(Collider other) {

        //user is in
        isIn = true;

        //#if !UNITY_EDITOR
        if (other.name == "Cube")
        {
            Logger.addScannedIn(gameObject.transform.parent.name, other.gameObject.transform.parent.name, System.DateTime.Now);
        }
        else
        {
            Logger.addScannedIn(gameObject.transform.parent.name, other.name, System.DateTime.Now);
        }
        //#endif
    }

    private void OnTriggerExit(Collider other)
    {
        //user is out
        isIn = false;
        //#if !UNITY_EDITOR
        if (other.name == "Cube")
        {
            Logger.addScannedOut(gameObject.transform.parent.name, other.gameObject.transform.parent.name, System.DateTime.Now);
        }
        else
        {
            Logger.addScannedOut(gameObject.transform.parent.name, other.name, System.DateTime.Now);
        }
        //#endif

    }
}
