using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniRiverZone : MonoBehaviour
{

    public ParticleSystem AffectedParticles = null; //Reference to particle System . The particle system need to be as the BaseRiver disable in the prefab
    public GameObject sphereObjective = null; //GameObject where the particle will go
    public GameObject initPoint;//GameObject that says where will go the particle system
    private Transform m_rTransform = null;//needed for the move mechanism
    // Array to store particles info
    private ParticleSystem.Particle[] m_rParticlesArray = null; //temporal array where the particles will be load
    private bool animationPlaying = false;//boolean that indicate the animation is playing
    private bool move_to_Objective = false; //boolean that indicate that the particle need to go to a certain point

    private bool m_bWorldPosition = false;//boolean that indicates if the coordinates are in world position
    private int m_iNumActiveParticles = 0; //num of active particles
    private Vector3 m_vParticlesTarget = Vector3.zero; //vector used for calculte the position 
    public ParticleSystem.MinMaxGradient colorTouch; //color of the particles 
    public ParticleSystem.MinMaxGradient colorNoTouch;//not used 

    void Start()
    {
        //init the configuration
        Setup();
        //set move to false
        move_to_Objective = false;
       
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    //function that play the effect river
    public void PlayRiver()
    {
        //indicate that we need to move the particles
        move_to_Objective = true;
        //if we can acces the particles system
        if (AffectedParticles != null)
        {
            //move the particle system to he initPoint where the rive will emanate particles
            AffectedParticles.gameObject.transform.position = initPoint.gameObject.transform.position;

            //play the particles system
            AffectedParticles.Play();
        }
    }

    //check if all the particles are in the objective
    public bool movingDone() {

        return !move_to_Objective;
    }


    //change the color of the particle system that emit the particles.
    public void ChangeColorTo(ParticleSystem.MinMaxGradient colorTouchfun, ParticleSystem.MinMaxGradient colorNoTouchfun)
    {
        //if we can acces
        if (AffectedParticles != null)
        {
            //get the color over life time module to change the color
            ParticleSystem.ColorOverLifetimeModule temp = AffectedParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;

            //set the varible from the script with the new colors
            colorTouch = colorTouchfun;
            colorNoTouch = colorNoTouchfun;
            //set the color from the particle system
            temp.color = colorNoTouchfun;


        }

       


    }

    
    void LateUpdate()
    {


    
        // Work only if we have something to work on and check if we are moving the particles
        if (AffectedParticles != null && sphereObjective != null && move_to_Objective )
        {

            // Let's fetch active particles info
            m_iNumActiveParticles = AffectedParticles.GetParticles(m_rParticlesArray);
            // The attractor's target is it's world space position
            m_vParticlesTarget = sphereObjective.transform.position;
            //AffectedParticles.GetComponentInChildren<Collider>().transform.position = sphereObjective.transform.position;
            // If the system is not simulating in world space, let's project the attractor's target in the system's local space
            if (!m_bWorldPosition)
                m_vParticlesTarget -= AffectedParticles.transform.position;



            //if there is no particles to move and the particles system is stop
            if (m_iNumActiveParticles == 0  && AffectedParticles.isStopped)
            {
                //we are not movin particles anymore
                move_to_Objective = false;

            }

            // For each active particle...
            for (int iParticle = 0; iParticle < m_iNumActiveParticles; iParticle++)
            { // The movement cursor is the opposite of the normalized particle's lifetime m_fCursor = 1.0f - (m_rParticlesArray[iParticle].lifetime / m_rParticlesArray[iParticle].startLifetime); // Are we over the activation treshold? if (m_fCursor >= ActivationTreshold)
                {
                    //check the distance beetwen the particle and the objective
                    float dist = Vector3.Distance(m_rParticlesArray[iParticle].position, m_vParticlesTarget);

                    // Interpolate the movement towards the target using the distance as factor					
                    m_rParticlesArray[iParticle].position = Vector3.Lerp(m_rParticlesArray[iParticle].position, m_vParticlesTarget, 20.0f * Time.deltaTime / dist);
                }
            }

            // Let's update the active particles
            AffectedParticles.SetParticles(m_rParticlesArray, m_iNumActiveParticles);
        }
    }

    public void Setup()
    {
        // If we have a system to setup...
        if (AffectedParticles != null)
        {
            // Prepare enough space to store particles info
            m_rParticlesArray = new ParticleSystem.Particle[AffectedParticles.main.maxParticles];
            // Is the particle system working in world space? Let's store this info
            m_bWorldPosition = AffectedParticles.main.simulationSpace == ParticleSystemSimulationSpace.World;
            // This the ratio of the total lifetime cursor to the "over treshold" section

        }
    }


}
