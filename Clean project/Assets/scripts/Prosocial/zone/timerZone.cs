using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timerZone : MonoBehaviour
{
    float timeStartAppear = 0.0f;
    float timeNextAppear = 0.0f;
    public float timeToNext=1.0f;
    public float timeToOffZones = 1.0f;
    float timeNextOff = 0.0f;
    public List<GameObject> zoneList = null;
    private List<Transform> zoneListActive = new List<Transform>();
    bool active = false;
    
    // Start is called before the first frame update
    void Start()
    {
        timeNextAppear = 0.0f;
        timeNextOff = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeNextAppear = timeNextAppear + Time.deltaTime;

        if (timeNextAppear >= timeToNext)
        {



            activateZones();


            timeNextAppear = 0.0f;

        }
        else if (timeNextAppear >= timeToNext- timeToOffZones) {
            if (active)
            {
                    getAllTimes();
                    deactivateZones();
            }
        }     

    }

    void getAllTimes() {
        bool control = true;
        Transform temp = null;
        List<float> times = new List<float>();
        float timeCollision = 0.0f;
        Debug.Log("Count list " + zoneListActive.Count);
        for (int i = 0; i < zoneListActive.Count; i++)
        {

            temp = zoneListActive[i];
            timeCollision = temp.GetComponent<zoneManager>().getWhenCollision();
            times.Add(timeCollision);

        }

        while (times.Count >0 && control) {
            times.Sort();
            float timeCalc = times[times.Count - 1] - times[0];
            Debug.Log("time calc" + timeCalc);
            if (timeCalc <= 0.2 && timeCalc > 0.0)
            {
                for (int i = 0; i < zoneListActive.Count; i++)
                {
                    ParticleSystem.MainModule ps = zoneListActive[i].GetComponent<zoneManager>().AffectedParticles.main;
                    ps.maxParticles = 25*times.Count;
                    
                    control = false;
                }
            }
            else {
                times.RemoveAt(0);
               
            }

        }

        if (times.Count == 0) {
            for (int i = 0; i < zoneListActive.Count; i++)
            {
                ParticleSystem.MainModule ps = zoneListActive[i].GetComponent<zoneManager>().AffectedParticles.main;
                ps.maxParticles = 5;
                control = false;
            }
        }


    }

    

    void activateZones() {
        Transform temp = null;
        for (int i = 0; i < zoneList.Capacity; i++)
        {
            int result = Random.Range(0, 3);
            
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
                
                zoneListActive.Add(temp);
                // temp.GetComponent<zoneManager>().updateSpherePoint();
            }
        }

        timeStartAppear = Time.time;
        active = true;

    }


    public void deactiveZone() {

    }

    void deactivateZones() {

        
        Transform temp = null;

        for (int i = 0; i < zoneListActive.Count;i++)
        {
            
            temp = zoneListActive[i];
            temp.GetComponent<zoneManager>().deactivateZone();
            
        }
        zoneListActive.Clear();
        active = false;
    }

}
