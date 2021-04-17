using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleColliderZone : MonoBehaviour
{
    public GameObject miniRiver1;
    public GameObject miniRiver2;
    public GameObject miniRiver3;
    public GameObject zoneObjective;
    public GameObject moveZone;
    bool collision = false;
    public bool allowCollision;
    private bool isOver;
    public float timeWait = 2.0f;
    float timeWaiting;
    bool timeOn;
    bool timeBetweenFasesControl;
    public Gradient touch;
    public Gradient notouch;
    // Start is called before the first frame update
    void Start()
    {
        allowCollision = false;
        isOver = false;
        timeOn = false;
        timeBetweenFasesControl = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (collision ) {
            miniRiver1.GetComponent<miniRiverZone>().ChangeColorTo(touch, notouch);
            miniRiver2.GetComponent<miniRiverZone>().ChangeColorTo(touch, notouch);
            miniRiver3.GetComponent<miniRiverZone>().ChangeColorTo(touch, notouch);
            miniRiver1.GetComponent<miniRiverZone>().PlayRiver();
            miniRiver2.GetComponent<miniRiverZone>().PlayRiver();
            miniRiver3.GetComponent<miniRiverZone>().PlayRiver();
            
            timeOn = true;
            allowCollision = false;
            collision = false;
        }

        if (timeOn && !isOver) {
            timeWaiting = timeWaiting + Time.deltaTime;
            if (timeWaiting >= timeWait) {
                if (!timeBetweenFasesControl)
                {
                    
                    moveZone.GetComponent<MoveZone>().ChangeColorTo(touch, notouch);
                    moveZone.GetComponent<MoveZone>().initMoveZone(1);
                    transform.Find("Cube").gameObject.SetActive(false);
                    transform.parent.transform.Find("footPrint").gameObject.SetActive(false);
                    timeBetweenFasesControl = true;

                }
                if (moveZone.GetComponent<MoveZone>().isInPosition() && timeBetweenFasesControl)
                {

                    moveZone.GetComponent<MoveZone>().deactiveMoveZone();

                    isOver = true;

                }
                
            }
        }



    }

    public bool isZoneOver() {
        return isOver;
    }

    private void OnTriggerStay(Collider other)
    {
        if (allowCollision)
        {
            //Debug.Log("Entro collider bola simple");
            Transform derecha=zoneObjective.transform.Find("derechaZone");
            Transform frente = zoneObjective.transform.Find("frenteZone");
            Transform izquierda = zoneObjective.transform.Find("izquierdaZone");
            derecha.gameObject.GetComponent<zoneManager>().actiaveCircle();
            frente.gameObject.GetComponent<zoneManager>().actiaveCircle();
            izquierda.gameObject.GetComponent<zoneManager>().actiaveCircle();
            collision = true;
            Transform particles = gameObject.transform.Find("Particle System");
            particles.gameObject.GetComponent<ParticleSystem>().Stop();
        }
       
    }
}
