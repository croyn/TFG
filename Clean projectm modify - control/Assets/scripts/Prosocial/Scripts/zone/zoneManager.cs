using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneManager : MonoBehaviour
{
    //particles system for the explosion effect
    public ParticleSystem AffectedParticles = null;
    //particles system for the group of particles appearing
    public ParticleSystem positionParticles = null;
    //gameObject objetive where the particle will go
    private GameObject sphereObjective = null;
    //control the time to explode
    private float timeExplosion = 0.0f;
    public Gradient GradientColor; //color for both particles system
    public Gradient GradientColorMove;//color for both particles system move zone . Its used to allow access to the move zone to a needed color
    // Transform cache
    private Transform m_rTransform = null;
    // Array to store particles info
    private ParticleSystem.Particle[] m_rParticlesArray = null;
    // Is this particle system simulating in world space?
    private bool m_bWorldPosition = false;
    //boolean that indicates if the particles are moving to the objective
    private bool move_to_Objective = false;
    //boolean that indicates if the animation is playing
    private bool animationPlaying = false;
    //boolena that indicates if there is explosion
    private bool explosion = false;
    //boolean that allows to collide
    private bool allowCollision = false;
    //for saving the time when the player collides
    private System.DateTime whenCollision ;
    //boolean that indicates if a player touch the particles
    private bool catched = false;
    //string to save the name of the player that touch the particles
    private string WhatUser;
    // To store how many particles are active on each frame
    private int m_iNumActiveParticles = 0;
    // The attractor target
    private Vector3 m_vParticlesTarget = Vector3.zero;
    // A cursor for the movement interpolation

    // Start is called before the first frame update
    void Start()
    {
        
     //set the base color configuration for both particle system
        ParticleSystem.ColorOverLifetimeModule temp= AffectedParticles.colorOverLifetime;
        temp.color = GradientColor;
        ParticleSystem.ColorOverLifetimeModule temp2 = positionParticles.colorOverLifetime;
        temp2.color = GradientColor;
        //set the initial not active configuration
        move_to_Objective = false;
        timeExplosion = 0.0f;
        explosion = false;
        catched = false;
        allowCollision = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if explosion 
        if (explosion)
        {
            //stop the group of particles that appear
            positionParticles.Stop(true);
            //if animation is playing
            if (animationPlaying)
            {
                //save the time
                timeExplosion = timeExplosion + Time.deltaTime;
                //set moving is true
                move_to_Objective = true;
                if (timeExplosion > 1.0f)
                {
                    

                }
            }
           
            
        }
        else {
            //positionParticles.Play();
        }

    }

    //get for WhatUser cath the zone
    public string getUserCatch()
    {
        return WhatUser;

    }

    //get for knowing if there are no more particles to move
    public bool isMovingParticles() {

        return move_to_Objective;
    }

    //get to know if some one touch it
    public bool isCatched() {

        return catched;
    }

    //function that set the configuration of the zone in explosion mode
    public void activeExplosion() {

        //if there was no explosion before
        if (!explosion)
        {
            //check a explosion
            explosion = true;
            //stop the position particles
            positionParticles.Stop(true);
            //disable the particleSystem
            positionParticles.gameObject.SetActive(false);
            //In previous iteratios it allow us to redirect every particle to every point that have lines. Now goes to the central point in the mandala.
            updateSpherePoint();

            //check acces and booleans that check if we only do one time this
            if (sphereObjective != null && AffectedParticles != null && explosion && animationPlaying == false)
            {
                //play the system that emanate the explosion
                AffectedParticles.Play();
                //set the pointCentralMandala allow to collide
                sphereObjective.GetComponent<PointCentralMandala>().allowAbsorv = true;
                //set the layer 11 that allows colisions with particles
                sphereObjective.layer = 11;
                //set anination playing
                animationPlaying = true;
            }
        }

    }

    //set the objective gameObject for the particles
    public void updateSpherePoint() {
        sphereObjective = mandalamanager.instance.giveCentralPoint();

    }

    //not using
    private void OnCollisionEnter(Collision collision)
    {
    
        if (AffectedParticles != null) {
    
            
            AffectedParticles.Play();
            
        }
    }

   
    //detects a collider enter
    private void OnTriggerEnter(Collider other)
    {
      //if we allow collision
        if (allowCollision) {
            
            //not anymore
            allowCollision = false;
            //if is not catched before
            if (!catched)
            {
                //save values of time
                whenCollision = System.DateTime.Now;
                //now is catched
                catched = true;
                //what user catched the zone
                if (other.name == "Cube")
                {
                    
                    WhatUser = gameObject.transform.parent.name;

                }
                else
                {
                    WhatUser = other.gameObject.name;
                }
                
            }

        }
        


        

    }

    //detect if a collider stay in 
    private void OnTriggerStay(Collider other)
    {

        //if we allow collision
        if (allowCollision)
        {
            //not anymore
            allowCollision = false;
            //if is not catched before
            if (!catched)
            {
                //save values of time
                whenCollision = System.DateTime.Now;
                //now is catched
                catched = true;
                //what user catched the zone
                if (other.name == "Cube")
                {

                    WhatUser = gameObject.transform.parent.name;

                }
                else
                {
                    WhatUser = other.gameObject.name;
                }
            }

        }
    }

    //return the time when was catched
    public System.DateTime getWhenCollision() {
        return whenCollision;
    }

    void Awake()
    {
        // Let's cache the transform
        //m_rTransform = this.transform;
        // Setup particle system info
        //AffectedParticles = gameObject.GetComponentInChildren(typeof(ParticleSystem)) as ParticleSystem;
        
        Setup();
    }

    

    public void activateZone()
    {

       // gameObject.SetActive(true);
       //restart variables
        explosion = false;
        catched = false;
        WhatUser = "";
        //enable the group of particles that appear
        positionParticles.gameObject.SetActive(true);
        //play the particle system
        positionParticles.Play();
        //enable the collision
        allowCollision = true;


    }

    //change the color for both particles system
    public void ChangeColorTo(Gradient color, Gradient color2) {
        //get acces to color module
        ParticleSystem.ColorOverLifetimeModule temp = positionParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
        //set the new color
        temp.color = color;
        //get acces to color module
        ParticleSystem.ColorOverLifetimeModule temp2 = AffectedParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
        //set color
        temp2.color = color;
        //set the same color for color of the move
        GradientColorMove = color2;

    }


    //function that active the circle where particles will appear and allows to active footprint to
    public void actiaveCircle(bool activateFoot=true)
    {
        //enable the gameobject that have a mesh circle
        transform.Find("Cube").gameObject.SetActive(true);
        //find according to the parent object what footprint need
        Transform tempfoot = gameObject.transform.parent.Find("footPrint");
        //active or not the footprint
        tempfoot.gameObject.SetActive(activateFoot);

    }

    //function to disactive the circle
    public void deactiaveCircle() {
        //disable the cube with circle mesh
        transform.Find("Cube").gameObject.SetActive(false);
        //disable the footprint too
        Transform tempfoot = gameObject.transform.parent.Find("footPrint");
        tempfoot.gameObject.SetActive(false);
    }

    //function used in scan fase to active the footprint and the collisionScan script
    public void activateFootPrint() {
        //find the foot print according to the parent gameobject
        Transform tempfoot = gameObject.transform.parent.Find("footPrint");
        //enable the script
        tempfoot.GetComponent<collisionScan>().enabled = true;
        //enable the footprint
        tempfoot.gameObject.SetActive(true);
    }

    //function used in scan fase to deactive the footprint and the collision script
    public void deactivateFootPrint()
    {
        //find the foot print according to the parent gameobject
        Transform tempfoot = gameObject.transform.parent.Find("footPrint");
        //disable the script
        tempfoot.GetComponent<collisionScan>().enabled = false;
        //disable the footprint
        tempfoot.gameObject.SetActive(false);
    }


    //function to deactivateAllTheZone
    public void deactivateZone() {
        //stop the particles
        positionParticles.Stop();
        //disable the object
        positionParticles.gameObject.SetActive(false);
        //not allow collision
        allowCollision = false;
        
    }


   

    void LateUpdate()
    {
        

        //sphereObjective = mandalamanager.instance.givePointFronTriangle();
        // Work only if we have something to work on :)
        if (AffectedParticles != null && sphereObjective!=null && move_to_Objective)
        {
            
            // Let's fetch active particles info
            m_iNumActiveParticles = AffectedParticles.GetParticles(m_rParticlesArray);
            // The attractor's target is it's world space position
            m_vParticlesTarget = sphereObjective.transform.position;
            AffectedParticles.GetComponentInChildren<Collider>().transform.position = sphereObjective.transform.position;
            // If the system is not simulating in world space, let's project the attractor's target in the system's local space
            if (!m_bWorldPosition)
                m_vParticlesTarget -= AffectedParticles.transform.position;


           
           // m_iNumActiveParticles = AffectedParticles.GetParticles(m_rParticlesArray);
            if (m_iNumActiveParticles == 0 )
            {
                //restart variables
                timeExplosion = 0.0f;
                move_to_Objective = false;
                animationPlaying = false;
                explosion = false;
                
            }

            // For each active particle...
            for (int iParticle = 0; iParticle < m_iNumActiveParticles; iParticle++)
            { // The movement cursor is the opposite of the normalized particle's lifetime m_fCursor = 1.0f - (m_rParticlesArray[iParticle].lifetime / m_rParticlesArray[iParticle].startLifetime); // Are we over the activation treshold? if (m_fCursor >= ActivationTreshold)
                {
                   
                    float dist = Vector3.Distance(m_rParticlesArray[iParticle].position, m_vParticlesTarget);
                    
                    // Interpolate the movement towards the target.			
                    m_rParticlesArray[iParticle].position = Vector3.Lerp(m_rParticlesArray[iParticle].position, m_vParticlesTarget, 50.0f*Time.deltaTime /dist);
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
