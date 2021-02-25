using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timerZone : MonoBehaviour
{

    float timeNextAppear = 0.0f;
    public float timeToNext=3.0f;
    public GameObject zone = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeNextAppear = timeNextAppear + Time.deltaTime;
        if (timeNextAppear >= timeToNext) {

            int result=Random.Range(0, 3);
            Transform temp=null;
            switch (result) {

                case 0:

                    temp = zone.transform.Find("derechaZone");
                    break;
                case 1:
                    temp = zone.transform.Find("frenteZone");
                    break;
                case 2:
                    temp = zone.transform.Find("izquierdaZone");
                    break;
            }
            if (temp != null) {
                temp.GetComponent<zoneManager>().activateZone();
            }
            

            timeNextAppear = 0.0f;

        }
    }
}
