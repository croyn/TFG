using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneManager : MonoBehaviour
{
    
    private ParticleSystem AffectedParticles = null;

    private GameObject sphereObjective = null;
    //public GameObject mandalaZone = null;

    private float timePlayParticles=5.0f;
    private float currenttimePlayParticles = 0.0f;



    // Transform cache
    private Transform m_rTransform = null;
    // Array to store particles info
    private ParticleSystem.Particle[] m_rParticlesArray = null;
    // Is this particle system simulating in world space?
    private bool m_bWorldPosition = false;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hola");
    }

    // Update is called once per frame
    void Update()
    {
        sphereObjective = mandalamanager.instance.givePointFronTriangle();
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
        /* if (AffectedParticles != null)
         {
             Debug.Log("entro collider");

             GameObject point = mandalamanager.instance.givePoint();
             ParticleAttractorBhv hinge = point.GetComponent(typeof(ParticleAttractorBhv)) as ParticleAttractorBhv;
             hinge.AffectedParticles = AffectedParticles;
             AffectedParticles.Play();
         }*/


        if (sphereObjective != null && AffectedParticles!=null) {

            AffectedParticles.Play();
            sphereObjective.GetComponent<pointsMandala>().line_true();
            sphereObjective.GetComponent<pointsMandala>().allowAbsorv = true;
        }

    }


    void Awake()
    {
        // Let's cache the transform
        //m_rTransform = this.transform;
        // Setup particle system info
        AffectedParticles = gameObject.GetComponentInChildren(typeof(ParticleSystem)) as ParticleSystem;
        Setup();
    }

    // To store how many particles are active on each frame
    private int m_iNumActiveParticles = 0;
    // The attractor target
    private Vector3 m_vParticlesTarget = Vector3.zero;
    // A cursor for the movement interpolation

    void LateUpdate()
    {
        //sphereObjective = mandalamanager.instance.givePointFronTriangle();
        // Work only if we have something to work on :)
        if (AffectedParticles != null && sphereObjective!=null)
        {
            
            // Let's fetch active particles info
            m_iNumActiveParticles = AffectedParticles.GetParticles(m_rParticlesArray);
            // The attractor's target is it's world space position
            m_vParticlesTarget = sphereObjective.transform.position;
            // If the system is not simulating in world space, let's project the attractor's target in the system's local space
            if (!m_bWorldPosition)
                m_vParticlesTarget -= AffectedParticles.transform.position;

            // For each active particle...
            for (int iParticle = 0; iParticle < m_iNumActiveParticles; iParticle++)
            { // The movement cursor is the opposite of the normalized particle's lifetime m_fCursor = 1.0f - (m_rParticlesArray[iParticle].lifetime / m_rParticlesArray[iParticle].startLifetime); // Are we over the activation treshold? if (m_fCursor >= ActivationTreshold)
                {
                   

                    // Take over the particle system imposed velocity
                    m_rParticlesArray[iParticle].velocity = Vector3.zero;
                    if (m_rParticlesArray[iParticle].color == Color.white)
                    {
                        m_rParticlesArray[iParticle].color = Color.blue;
                    }
                    else {
                        m_rParticlesArray[iParticle].color = Color.white;
                    }

                    
                    // Interpolate the movement towards the target with a nice quadratic easing					
                    m_rParticlesArray[iParticle].position = Vector3.Lerp(m_rParticlesArray[iParticle].position, m_vParticlesTarget, 1.0f*Time.deltaTime);
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
