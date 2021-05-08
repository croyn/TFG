using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script used in the scanZones from the initial mechanic of the game.
public class simpleColliderZone : MonoBehaviour
{
    public GameObject miniRiver1;//reference miniRiver1 Object
    public GameObject miniRiver2;//reference miniRiver2 Object
    public GameObject miniRiver3;//reference miniRiver3 Object
    public GameObject zoneObjective;//reference Objective ZoneCacthParticles Object
    public GameObject moveZone;//reference movingZone reference
    bool collision = false;//for make only one time
    public bool allowCollision; //allows the collisions
    private bool isOver;//knowing all the flow of this object has finished.
    public float timeWait = 2.0f;//time to wait
    float timeWaiting;//current time
    bool timeOn;//knowing if time is going
    bool timeBetweenFasesControl;//control of flow beetwon fases
    public Gradient touch;//color when is touch the moving zone
    public Gradient notouch;//color when is not touch the moving zone

    // Start is called before the first frame update
    void Start()
    {
        //initialation of variables when instance
        allowCollision = false;
        isOver = false;
        timeOn = false;
        timeBetweenFasesControl = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if we detect a collision
        if (collision ) {
            //change the color to the miniRivers
            miniRiver1.GetComponent<miniRiverZone>().ChangeColorTo(touch, notouch);
            miniRiver2.GetComponent<miniRiverZone>().ChangeColorTo(touch, notouch);
            miniRiver3.GetComponent<miniRiverZone>().ChangeColorTo(touch, notouch);

            //play the minirivers
            miniRiver1.GetComponent<miniRiverZone>().PlayRiver();
            miniRiver2.GetComponent<miniRiverZone>().PlayRiver();
            miniRiver3.GetComponent<miniRiverZone>().PlayRiver();
            
            //start to check the time
            timeOn = true;
            //not allow more colision
            allowCollision = false;
            //Collision flow done.
            collision = false;
        }


        //time is On and is not over the flow of the script
        if (timeOn && !isOver) {
            //accumulate deltaTime to try to calculate a time wait
            timeWaiting = timeWaiting + Time.deltaTime;

            //when the varible is equal or more to the time that need to wait
            if (timeWaiting >= timeWait) {

                //if the varible of control is false
                if (!timeBetweenFasesControl)
                {
                    //change the color of the moving zone
                    moveZone.GetComponent<MoveZone>().ChangeColorTo(touch, notouch);
                    //inicialice the flow of the moving zone. 1 is for initialize from a scaning zone.
                    moveZone.GetComponent<MoveZone>().initMoveZone(1);
                    //disable the circle of particles in front of the user
                    transform.Find("Cube").gameObject.SetActive(false);
                    //disable the footPrint where the user is
                    transform.parent.transform.Find("footPrint").gameObject.SetActive(false);
                    //the flow is done
                    timeBetweenFasesControl = true;

                }

                //if the moving zone is in the final position and we have done the flow
                if (moveZone.GetComponent<MoveZone>().isInPosition() && timeBetweenFasesControl)
                {
                    //deactivate the moving zone
                    moveZone.GetComponent<MoveZone>().deactiveMoveZone();
                    //we activate the left catch zone and the footprint
                    Transform izquierda = zoneObjective.transform.Find("izquierdaZone");
                    izquierda.gameObject.GetComponent<zoneManager>().actiaveCircle(true);
                    //the flow of this script is over
                    isOver = true;

                }
                
            }
        }



    }

    //function that say if the total flow of the Script is over
    public bool isZoneOver() {
        return isOver;
    }


    //function that detects the event OnTriggerStay with other colliders.
    private void OnTriggerStay(Collider other)
    {
        //if we allow
        if (allowCollision)
        {
            //if zoneObjective is not null
            if (zoneObjective != null) {

                //find every catchZone in the objective Zone

                //right
                Transform derecha = zoneObjective.transform.Find("derechaZone");
                //front
                Transform frente = zoneObjective.transform.Find("frenteZone");
                //left
                Transform izquierda = zoneObjective.transform.Find("izquierdaZone");

                //active the circle area but not the footPrint
                derecha.gameObject.GetComponent<zoneManager>().actiaveCircle(false);
                frente.gameObject.GetComponent<zoneManager>().actiaveCircle(false);
                izquierda.gameObject.GetComponent<zoneManager>().actiaveCircle(false);
                //notice that we collide with something
                collision = true;
//#if !UNITY_EDITOR
                if (other.name == "Cube")
                {
                    Logger.addParticlesCatchScan(gameObject.transform.parent.name, other.gameObject.transform.parent.name,System.DateTime.Now);
                }
                else
                {
                    Logger.addParticlesCatchScan(gameObject.transform.parent.name, other.name, System.DateTime.Now);
                }
//#endif
                //Stop the actual particle System 
                Transform particles = gameObject.transform.Find("Particle System");
                particles.gameObject.GetComponent<ParticleSystem>().Stop();
            }
            else {
#if UNITY_EDITOR
                Debug.Log("Zone Objective is null in simpleColliderZone");
#endif
            }

        }

    }
}
