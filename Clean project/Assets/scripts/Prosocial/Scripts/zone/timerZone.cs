using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timerZone : MonoBehaviour
{
    public static timerZone instance;
    float timeStartAppear = 0.0f;
    float timeNextAppear = 0.0f;
    public float timeToNext=1.0f;
    public float timeToOffZones = 1.0f;
    float timeNextOff = 0.0f;
    public List<GameObject> zoneList = null;
    public List<GameObject> zoneListMove = null;
    private List<Transform> zoneListActive = new List<Transform>();
    private List<Transform> zoneListActiveMove = new List<Transform>();
    public GameObject pointCentralMandala;
    bool active = false;
    private int ControlWhichZone = 2;
    private int ControlWhichMove = 0;
    public int controlFase = 0; //0 absorv , 1 move
    Gradient actualGradient = null;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        timeNextAppear = 0.0f;
        timeNextOff = 0.0f;
        controlFase = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeNextAppear = timeNextAppear + Time.deltaTime;

        if (timeNextAppear >= timeToNext)
        {


            if (controlFase == 0)
            {
                activateZones();
            }
            else if (controlFase == 1) {
                activateZonesMove();
                if (ControlWhichMove == -1)
                {
                    deactivateZonesMove();
                    changeFaseTo(0);
                    ControlWhichMove = 0;
                }
            }
            


            timeNextAppear = 0.0f;

        }
        else if (timeNextAppear >= timeToNext- timeToOffZones) {
            if (active)
            {
                    //getAllTimes();
                   
                if (controlFase == 0)
                {
                    deactivateZones();
                }
                else if (controlFase == 1)
                {
                    activateNextMove();
                    deactivateZonesMove();
                   
                }
            }
        }     

    }


    public void changeFaseTo(int cual) {

        switch (cual) {
            case 0://absorv
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = true;
                deactivateZonesMove();
                activateZonesCircles();
                break;
            case 1://move
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = false;
                deactivateZones();
                deactivateZonesCircles();


                break;

        }

        controlFase = cual;
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
        times.Clear();


    }

    private void ChangeZone() {

        switch (ControlWhichZone) {
            case 0: ControlWhichZone = 1;
                break;
            case 1: ControlWhichZone = 2;
                break;
            case 2: ControlWhichZone = 0;
                break;
        }
        
    }

    private void ChangeZoneMove()
    {
        ControlWhichMove = ControlWhichMove + 1;


        if (ControlWhichMove > 7) {
            ControlWhichMove = -1;
        }

    }

    void activateZonesMove() {

        Transform temp = null;
        Transform temp1 = null;
        
        for (int i = 0; i < zoneListMove.Capacity; i++)
        {
            //int result = Random.Range(0, 3);
            //result = 1;

            switch (ControlWhichMove)
            {

                case 0:
                    temp = zoneListMove[i].transform.Find("Move_0");
                   // temp1= zoneListMove[i].transform.Find("Move_1");
                    break;
                case 1:
                    temp = zoneListMove[i].transform.Find("Move_1");
                    //temp1 = zoneListMove[i].transform.Find("Move_2");
                    break;
                case 2:
                    temp = zoneListMove[i].transform.Find("Move_2");
                    //temp1 = zoneListMove[i].transform.Find("Move_3");
                    break;
                case 3:
                    temp = zoneListMove[i].transform.Find("Move_3");
                    temp1 = zoneListMove[i].transform.Find("Move_4");
                    break;
                case 4:
                    temp = zoneListMove[i].transform.Find("Move_4");
                   // temp1 = zoneListMove[i].transform.Find("Move_5");
                    break;
                case 5:
                    temp = zoneListMove[i].transform.Find("Move_5");
                   // temp1 = zoneListMove[i].transform.Find("Move_6");
                    break;
                case 6:
                    temp = zoneListMove[i].transform.Find("Move_6");
                    //temp1 = zoneListMove[i].transform.Find("Move_6");
                    break;
                case 7:
                    temp = null;
                    break;
            }

            if (temp != null)
            {
                temp.GetComponent<zoneManagerMove>().activateZone();
                //temp1.GetComponent<zoneManagerMove>().actiaveCircle();
                zoneListActiveMove.Add(temp);
                // temp.GetComponent<zoneManager>().updateSpherePoint();
            }
        }

        timeStartAppear = Time.time;
        active = true;
        ChangeZoneMove();
        
        
    }

    void activateNextMove() {
        Transform temp = null;
        for (int i = 0; i < zoneListMove.Count; i++)
        {
            //int result = Random.Range(0, 3);
            //result = 1;
            Debug.Log("Control num" + ControlWhichMove);
            switch (ControlWhichMove)
            {

                case 0:
                    temp = zoneListMove[i].transform.Find("Move_0");
                    break;
                case 1:
                    temp = zoneListMove[i].transform.Find("Move_1");
                    break;
                case 2:
                    temp = zoneListMove[i].transform.Find("Move_2");
                    break;
                case 3:
                    temp = zoneListMove[i].transform.Find("Move_3");
                    break;
                case 4:
                    temp = zoneListMove[i].transform.Find("Move_4");
                    break;
                case 5:
                    temp = zoneListMove[i].transform.Find("Move_5");
                    break;
                case 6:
                    temp = zoneListMove[i].transform.Find("Move_6");
                    break;
                case 7:
                    temp = null;
                    break;
                default:
                    temp = null;
                    break;
            }

            if (temp != null)
            {
                Debug.Log("no null");
                temp.GetComponent<zoneManagerMove>().actiaveCircle();

                //zoneListActiveMove.Add(temp);
                // temp.GetComponent<zoneManager>().updateSpherePoint();
            }
        }

    }

    void activateZones() {
        Transform temp = null;
        ChangeZone();
        Gradient colorActual = mandalamanager.instance.switchColor();
        for (int i = 0; i < zoneList.Count; i++)
        {
            //int result = Random.Range(0, 3);
            //result = 1;
            
            switch (ControlWhichZone)
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
                temp.GetComponent<zoneManager>().ChangeColorTo(colorActual);
                temp.GetComponent<zoneManager>().activateZone();
                
                zoneListActive.Add(temp);
                // temp.GetComponent<zoneManager>().updateSpherePoint();
            }
        }
        
        timeStartAppear = Time.time;
        active = true;

    }


    public void deactivateZonesMove() {
        Transform temp = null;
        
        for (int i = 0; i < zoneListActiveMove.Count; i++)
        {

            temp = zoneListActiveMove[i];
            temp.GetComponent<zoneManagerMove>().deactivateZone();
            temp.GetComponent<zoneManagerMove>().deactiaveCircle();

        }
        zoneListActiveMove.Clear();
        active = false;
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

    void deactivateZonesCircles() {
        Transform temp;
        Transform temp1;
        Transform temp2;


        for (int i = 0; i < zoneList.Count; i++) {

            temp = zoneList[i].transform.Find("derechaZone");
            temp1 = zoneList[i].transform.Find("frenteZone");
            temp2 = zoneList[i].transform.Find("izquierdaZone");

            temp.gameObject.GetComponent<zoneManager>().deactiaveCircle();
            temp1.gameObject.GetComponent<zoneManager>().deactiaveCircle();
            temp2.gameObject.GetComponent<zoneManager>().deactiaveCircle();
        }
    }

    void activateZonesCircles()
    {
        Transform temp;
        Transform temp1;
        Transform temp2;


        for (int i = 0; i < zoneList.Count; i++)
        {

            temp = zoneList[i].transform.Find("derechaZone");
            temp1 = zoneList[i].transform.Find("frenteZone");
            temp2 = zoneList[i].transform.Find("izquierdaZone");

            temp.gameObject.GetComponent<zoneManager>().actiaveCircle();
            temp1.gameObject.GetComponent<zoneManager>().actiaveCircle();
            temp2.gameObject.GetComponent<zoneManager>().actiaveCircle();
        }
    }

}
