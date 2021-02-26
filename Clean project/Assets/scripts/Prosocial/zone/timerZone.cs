using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timerZone : MonoBehaviour
{

    float timeNextAppear = 0.0f;
    public float timeToNext=1.0f;
    public List<GameObject> zoneList = null;

    // Start is called before the first frame update
    void Start()
    {
        timeNextAppear = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeNextAppear = timeNextAppear + Time.deltaTime;
        if (timeNextAppear >= timeToNext) {

            for (int i = 0; i < zoneList.Capacity; i++) {
                int result = Random.Range(0, 3);
                Transform temp = null;
                switch (result)
                {

                    case 0:

                        temp = zoneList[i].transform.Find("derechaZone");
                        break;
                    case 1:
                        temp = zoneList[i].transform.Find("frenteZone");
                        break;
                    case 2:
                        temp = zoneList[i].transform.Find("izquierdaZone");
                        break;
                }
                if (temp != null)
                {
                    temp.GetComponent<zoneManager>().activateZone();
                   // temp.GetComponent<zoneManager>().updateSpherePoint();
                }
            }

            
            

            timeNextAppear = 0.0f;

        }
    }
}
