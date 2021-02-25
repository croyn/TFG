using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneManager : MonoBehaviour
{
    
    public ParticleSystem AffectedParticles = null;

    public ParticleSystem positionParticles = null;

    private GameObject sphereObjective = null;
    //public GameObject mandalaZone = null;

    private float timePlayParticles=5.0f;
    private float currenttimePlayParticles = 0.0f;
    private float timeColor = 0.2f;
    private float timeExplosion = 0.0f;
    private Color ColorParticle;
    // Transform cache
    private Transform m_rTransform = null;
    // Array to store particles info
    private ParticleSystem.Particle[] m_rParticlesArray = null;
    // Is this particle system simulating in world space?
    private bool m_bWorldPosition = false;
    private bool move_to_Objective = false;
    private bool animationPlaying = false;
    private bool explosion = false;

    // Start is called before the first frame update
    void Start()
    {
        
        ColorParticle = Color.white;
        move_to_Objective = false;
        timeExplosion = 0.0f;
        explosion = false;
        //positionParticles.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (explosion)
        {

            positionParticles.Stop(true);
            sphereObjective = mandalamanager.instance.givePointFronTriangle();
            if (animationPlaying)
            {
                timeExplosion = timeExplosion + Time.deltaTime;
                if (timeExplosion > 1.0f)
                {
                    move_to_Objective = true;

                }
            }

        }
        else {
            //positionParticles.Play();
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("entro collision");
        if (AffectedParticles != null) {
            Debug.Log("entro collision");
            
            AffectedParticles.Play();
            
        }
    }

   

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entro collider");
      

        if (!explosion) {
            explosion = true;
            positionParticles.Stop(true);
        }


        if (sphereObjective != null && AffectedParticles!=null && explosion && animationPlaying==false) {

            AffectedParticles.Play();
            //sphereObjective.GetComponent<pointsMandala>().line_true();
            sphereObjective.GetComponent<pointsMandala>().allowAbsorv = true;
            animationPlaying = true;
        }

    }


    void Awake()
    {
        // Let's cache the transform
        //m_rTransform = this.transform;
        // Setup particle system info
        //AffectedParticles = gameObject.GetComponentInChildren(typeof(ParticleSystem)) as ParticleSystem;
        
        Setup();
    }

    // To store how many particles are active on each frame
    private int m_iNumActiveParticles = 0;
    // The attractor target
    private Vector3 m_vParticlesTarget = Vector3.zero;
    // A cursor for the movement interpolation


    public void activateZone()
    {
        gameObject.SetActive(true);
        explosion = false;
        positionParticles.Play();
        


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


            timeColor = timeColor + Time.deltaTime;

            if (m_iNumActiveParticles == 0)
            {
                timeExplosion = 0.0f;
                move_to_Objective = false;
                animationPlaying = false;
                explosion = false;
                gameObject.SetActive(false);
            }

            // For each active particle...
            for (int iParticle = 0; iParticle < m_iNumActiveParticles; iParticle++)
            { // The movement cursor is the opposite of the normalized particle's lifetime m_fCursor = 1.0f - (m_rParticlesArray[iParticle].lifetime / m_rParticlesArray[iParticle].startLifetime); // Are we over the activation treshold? if (m_fCursor >= ActivationTreshold)
                {
                   
                    
                    // Take over the particle system imposed velocity
                    //m_rParticlesArray[iParticle].velocity = Vector3.zero;

                   /* if (m_rParticlesArray[iParticle].color == Color.white && timeColor > 0.2f)
                    {
                        m_rParticlesArray[iParticle].color = Color.red;
                    }
                    else if(m_rParticlesArray[iParticle].color == Color.red && timeColor > 0.2f)
                    {
                        m_rParticlesArray[iParticle].color = Color.white;
                    }*/

                    float dist = Vector3.Distance(m_rParticlesArray[iParticle].position, m_vParticlesTarget);
                    
                    // Interpolate the movement towards the target with a nice quadratic easing					
                    m_rParticlesArray[iParticle].position = Vector3.Lerp(m_rParticlesArray[iParticle].position, m_vParticlesTarget, 50.0f*Time.deltaTime /dist);
                }
            }

            if (timeColor > 0.2f) {
                timeColor = 0.0f;
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
