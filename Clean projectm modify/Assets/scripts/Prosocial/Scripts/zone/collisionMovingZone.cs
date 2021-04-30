using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script to change the visual effect of the moving zone when are touched or not.
public class collisionMovingZone : MonoBehaviour
{
    public bool allowCollision=false;//control of collision.
    public ParticleSystem affectectParticles;//particles that will be modified according to the collision in or out.
    
    // Start is called before the first frame update
    void Start()
    {
        allowCollision = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //function that set the group particles in active mode
    private void forceOn() {
        //check null in variable
        if (affectectParticles != null)
        {
            //access to the main module of the particle system.
            ParticleSystem.MainModule tempMainModule = affectectParticles.gameObject.GetComponent<ParticleSystem>().main;
            //setup the variable for a big group of particles effect so active mode
            tempMainModule.startSpeed = 5.0f;
            tempMainModule.startLifetime = 2.5f;
            tempMainModule.maxParticles = 10000;
            //acces to the emision module of the particle system
            ParticleSystem.EmissionModule tempEmisionModule = affectectParticles.gameObject.GetComponent<ParticleSystem>().emission;
            //setup the number of particles over time
            tempEmisionModule.rateOverTime = 800;
            //acces to the color Over Life time Module of the particle system
            ParticleSystem.ColorOverLifetimeModule temp = affectectParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
            //setup the color of the particles in the particle system
            temp.color = gameObject.transform.parent.GetComponent<MoveZone>().colorTouch;
            //play the particle system
            affectectParticles.Play();
        }
 #if UNITY_EDITOR
        else
        {
            Debug.Log("affectectParticles null in collisionMovingZone"); 
        }
#endif      
    }

    //function that set the group particles in inactive mode
    public void ForceOff()
    {
        //access to the main module of the particle system.
        ParticleSystem.MainModule tempMainModule = affectectParticles.gameObject.GetComponent<ParticleSystem>().main;
        //setup the variable for inactive mode
        tempMainModule.startSpeed = 0.3f;
        tempMainModule.startLifetime = 1.0f;
        tempMainModule.maxParticles = 10000;
        //acces to the emision module of the particle system
        ParticleSystem.EmissionModule tempEmisionModule = affectectParticles.gameObject.GetComponent<ParticleSystem>().emission;
        tempEmisionModule.rateOverTime = 200;
        //acces to the color Over Life time Module of the particle system
        ParticleSystem.ColorOverLifetimeModule temp = affectectParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
        //setup the color of the particles in the particle system
        temp.color = gameObject.transform.parent.GetComponent<MoveZone>().colorNoTouch;
        //play the particle system
        affectectParticles.Play();
    }




    //function that detects the event OnTriggerEnter with other colliders.
    private void OnTriggerEnter(Collider other)
    {
        //if we allow
        if (allowCollision)
        {
            //the particles setup go to active mode
            forceOn();

            //if name is Cube it take the parents name that is the player.
//#if !UNITY_EDITOR
            if (other.name == "Cube")
            {
                Logger.addTouchingMoving(gameObject.transform.parent.name, other.gameObject.transform.parent.name);
            }
            else
            {
                Logger.addTouchingMoving(gameObject.transform.parent.name, other.name);
            }
//#endif  

        }

    }


    //function that detects the event OnTriggerStay with other colliders.
    private void OnTriggerStay(Collider other)
    {
        //if we allow
        if (allowCollision)
        {
            //the particles setup go to active mode
            forceOn();
            //#if !UNITY_EDITOR
            //if name is Cube it take the parents name that is the player.
            if (other.name == "Cube")
            {
                Logger.addTouchingMoving(gameObject.transform.parent.name, other.gameObject.transform.parent.name);
            }
            else {
                Logger.addTouchingMoving(gameObject.transform.parent.name, other.name);
            }
            //#endif
        }

    }


    //function that detects the event OnTriggerExit with other colliders.
    private void OnTriggerExit(Collider other)
    {
        //if we allow
        if (allowCollision)
        {
            //the particles setup go to inactive mode
            ForceOff();

            //if name is Cube it take the parents name that is the player.
            if (other.name == "Cube")
            {
                Logger.addNotTouchingMoving(gameObject.transform.parent.name, other.gameObject.transform.parent.name);
               
            }
            else
            {
                Logger.addNotTouchingMoving(gameObject.transform.parent.name, other.name);
            }
            
        }
    }


}
