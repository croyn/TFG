using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script that control the flow of the moving zones
public class MoveZone : MonoBehaviour
{

    public GameObject initPoint; //object that indicate the first position of appear the move zone
    public GameObject finalPoint; //object that indicate the final position of moving zone
    public GameObject initPointScan;//object that indicate the first position of appear the move zone on scan fase
    public GameObject finalPointScan;//object that indicate the final position of moving zone on scan fase
    public ParticleSystem affectectParticles; //main particles system
    public ParticleSystem affectectParticles2; //not used now. But used in other previous iteration to use 2 particles system in the moving zone
    private bool inPosition = false; //indicates if the particle system is on the final position
    public float velocity; //velocity of the moving zone beetwen initPoint and finalPoint
    public float velocityMoveScan; //velocity of the moving zone beetwen initPointScan and finalPointScan
    public ParticleSystem.MinMaxGradient colorTouch; //Color when the moving zone is touched
    public ParticleSystem.MinMaxGradient colorNoTouch;//color when the moving zone is not touched
    public bool allowMoving; //To control if the system allows to move the zone
    public int controlCual ; //Know wich variables use
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //if allow Moving is true
        if (allowMoving) {
            //move the system according wich variables is configurate in that moment
            moveParticlesSystem(controlCual);
        }
       
    }

    //function that initialize the moving zone .
    public void initMoveZone(int which) {
        //save wich variant we want to initialice
        controlCual = which;
        //check if there is the particles system 
        if (affectectParticles != null) {

            //if we want the normal configuration
            if (which == 0) {
                //start position of appearing the moving zone
                affectectParticles.gameObject.transform.position = initPoint.gameObject.transform.position;
            } else if (which == 1) //if we want the scan configuration
            {
                //start position scan of appearing the moving zone
                affectectParticles.gameObject.transform.position = initPointScan.gameObject.transform.position;
            }
           
            //check if the particle system have the script CollisionMovingZone
            if (affectectParticles.gameObject.GetComponent<collisionMovingZone>() != null)
            {
                //indicate to the script that collision is enabled
                affectectParticles.gameObject.GetComponent<collisionMovingZone>().allowCollision = true;
            }

            //play the particles
            affectectParticles.Play();
        }


        //not using the second particle system
        if (affectectParticles2 != null)
        {
            affectectParticles2.gameObject.transform.position = initPoint.gameObject.transform.position;
            //affectectParticles2.Play();
        }
        
        //activate move 
        allowMoving = true;
        //the moving zone is on initial point so not in the final position
        inPosition = false;
    }


    //Function to move from initPoint to final point according to wich configuration we want
    public void moveParticlesSystem(int which) {

        //if we have the gameObjects needed
        if (affectectParticles != null && finalPoint != null) {
            //init variable to know distance
            float dist = 0.0f;
            //if configuration normal
            if (which == 0)
            {
                //calculate the distance beetween the particleSystem and the final point
                dist = Vector3.Distance(affectectParticles.gameObject.transform.position, finalPoint.gameObject.transform.position);

                //we move the particle System according to  [velocity * (Time.deltaTime / dist))] to make a equidistant velocity on the Lerp function according to distance beetween points
                affectectParticles.gameObject.transform.position = Vector3.Lerp(affectectParticles.gameObject.transform.position, finalPoint.gameObject.transform.position, velocity * (Time.deltaTime / dist));
            }
            else if (which == 1) //scan configuration
            {
                //calculate the distance beetween the particleSystem and the final point
                dist = Vector3.Distance(affectectParticles.gameObject.transform.position, finalPointScan.gameObject.transform.position);
                //we move the particle System according to  [velocity * (Time.deltaTime / dist))] to make a equidistant velocity on the Lerp function according to distance beetween points
                affectectParticles.gameObject.transform.position = Vector3.Lerp(affectectParticles.gameObject.transform.position, finalPointScan.gameObject.transform.position, velocityMoveScan * (Time.deltaTime / dist));
            }
            
            //if the particle system and the objective are realy close
            if (dist <= 0.1f)
            {
                //we considerer that is in the correct final position
                inPosition = true;
                //if we can access the script
                if (affectectParticles.gameObject.GetComponent<collisionMovingZone>() != null)
                {
                    //we dont need to collision any more 
                    affectectParticles.gameObject.GetComponent<collisionMovingZone>().allowCollision = false;

                    //set to not touched the configuration of the zone
                    affectectParticles.gameObject.GetComponent<collisionMovingZone>().ForceOff();
                }
            }
        }


        //configuration for second particle system but not used now
        if (affectectParticles2 != null && finalPoint != null)
        {
            float dist2 = Vector3.Distance(affectectParticles2.gameObject.transform.position, finalPoint.gameObject.transform.position);
            affectectParticles2.gameObject.transform.position = Vector3.Lerp(affectectParticles2.gameObject.transform.position, finalPoint.gameObject.transform.position, velocity * (Time.deltaTime / dist2));
            if (dist2 <= 0.1f)
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

    //function to deactivate a move zone
    public void deactiveMoveZone() {
        //if we can acces the particle system
        if (affectectParticles != null) {
            //if we can acces the script
            if (affectectParticles.gameObject.GetComponent<collisionMovingZone>() != null) {
                //not allow collision
                affectectParticles.gameObject.GetComponent<collisionMovingZone>().allowCollision = false;
            } 
            //stop the particle system
            affectectParticles.Stop();
        }

        //not used
        if (affectectParticles2 != null)
        {
            affectectParticles2.Stop();
        }

        //dont move any more
        allowMoving = false;
        //restart variable
         inPosition = false;

    }

    //return if the moving zone is in position
    public bool isInPosition()
    {
        return inPosition;
    }


    //allows to change the color in the particle system of move zone
    public void ChangeColorTo(ParticleSystem.MinMaxGradient colorTouchfun, ParticleSystem.MinMaxGradient colorNoTouchfun)
    {
        //if we can acces the particle system
        if (affectectParticles != null)
        {
            //access the color over life time module from the particle system
            ParticleSystem.ColorOverLifetimeModule temp = affectectParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
            //set the varible on move zone to the new parameters
            colorTouch = colorTouchfun;
            colorNoTouch = colorNoTouchfun;
            //change the color on the particle system to notTouch
            temp.color = colorNoTouchfun;


        }
     
        //not used
        if (affectectParticles2 != null)
        {
            ParticleSystem.ColorOverLifetimeModule temp2 = affectectParticles2.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
            temp2.color = colorNoTouchfun;
        }

        
    }


 
   
}
