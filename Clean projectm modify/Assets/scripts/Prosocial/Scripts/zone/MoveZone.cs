using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveZone : MonoBehaviour
{

    public GameObject initPoint;
    public GameObject finalPoint;
    public GameObject initPointScan;
    public GameObject finalPointScan;
    public ParticleSystem affectectParticles;
    public ParticleSystem affectectParticles2;
    private bool inPosition = false;
    public float velocity;
    public float velocityMoveScan;
    public ParticleSystem.MinMaxGradient colorTouch;
    public ParticleSystem.MinMaxGradient colorNoTouch;
    public bool allowMoving;
    public int controlCual ;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (allowMoving) {
            moveParticlesSystem(controlCual);
        }
       
    }


    public void initMoveZone(int cual) {
        controlCual = cual;
        if (affectectParticles != null) {
            if (cual == 0) {
                affectectParticles.gameObject.transform.position = initPoint.gameObject.transform.position;
            } else if (cual == 1)
            {
                affectectParticles.gameObject.transform.position = initPointScan.gameObject.transform.position;
            }
           
            if (affectectParticles.gameObject.GetComponent<collisionMovingZone>() != null)
            {
                affectectParticles.gameObject.GetComponent<collisionMovingZone>().allowCollision = true;
            }
            affectectParticles.Play();
        }


        if (affectectParticles2 != null)
        {
            affectectParticles2.gameObject.transform.position = initPoint.gameObject.transform.position;
            //affectectParticles2.Play();
        }
        
        allowMoving = true;
        inPosition = false;
    }


    public void moveParticlesSystem(int cual) {

        if (affectectParticles != null && finalPoint != null) {
            float dist = 0.0f;
            if (cual == 0)
            {
                dist = Vector3.Distance(affectectParticles.gameObject.transform.position, finalPoint.gameObject.transform.position);
                affectectParticles.gameObject.transform.position = Vector3.Lerp(affectectParticles.gameObject.transform.position, finalPoint.gameObject.transform.position, velocity * (Time.deltaTime / dist));
            }
            else if (cual == 1)
            {
                dist = Vector3.Distance(affectectParticles.gameObject.transform.position, finalPointScan.gameObject.transform.position);
                affectectParticles.gameObject.transform.position = Vector3.Lerp(affectectParticles.gameObject.transform.position, finalPointScan.gameObject.transform.position, velocityMoveScan * (Time.deltaTime / dist));
            }
            
            if (dist <= 0.05f)
            {
                inPosition = true;
                if (affectectParticles.gameObject.GetComponent<collisionMovingZone>() != null)
                {
                    affectectParticles.gameObject.GetComponent<collisionMovingZone>().allowCollision = false;
                    affectectParticles.gameObject.GetComponent<collisionMovingZone>().ForceOff();
                }
            }
        }

        if (affectectParticles2 != null && finalPoint != null)
        {
            float dist2 = Vector3.Distance(affectectParticles2.gameObject.transform.position, finalPoint.gameObject.transform.position);
            affectectParticles2.gameObject.transform.position = Vector3.Lerp(affectectParticles2.gameObject.transform.position, finalPoint.gameObject.transform.position, velocity * (Time.deltaTime / dist2));
            if (dist2 <= 0.05f)
            {
                inPosition = true;
                if (affectectParticles.gameObject.GetComponent<collisionMovingZone>() != null)
                {
                    affectectParticles.gameObject.GetComponent<collisionMovingZone>().allowCollision = false;
                    affectectParticles.gameObject.GetComponent<collisionMovingZone>().ForceOff();
                }
            }
        }
 

    }

    public void deactiveMoveZone() {
        if (affectectParticles != null) {
            if (affectectParticles.gameObject.GetComponent<collisionMovingZone>() != null) {
                affectectParticles.gameObject.GetComponent<collisionMovingZone>().allowCollision = false;
            } 
            affectectParticles.Stop();
        }
        if (affectectParticles2 != null)
        {
            affectectParticles2.Stop();
        }
        allowMoving = false;
         inPosition = false;

    }

    public bool isInPosition()
    {
        return inPosition;
    }

    public void ChangeColorTo(ParticleSystem.MinMaxGradient colorTouchfun, ParticleSystem.MinMaxGradient colorNoTouchfun)
    {
        if (affectectParticles != null)
        {

            ParticleSystem.ColorOverLifetimeModule temp = affectectParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;

            colorTouch = colorTouchfun;
            colorNoTouch = colorNoTouchfun;
            temp.color = colorNoTouchfun;


        }
     
        if (affectectParticles2 != null)
        {
            ParticleSystem.ColorOverLifetimeModule temp2 = affectectParticles2.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
            temp2.color = colorNoTouchfun;
        }

        
    }


 
   
}
