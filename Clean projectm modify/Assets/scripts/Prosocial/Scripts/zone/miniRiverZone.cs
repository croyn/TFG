using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniRiverZone : MonoBehaviour
{

    public ParticleSystem AffectedParticles = null;
    public GameObject sphereObjective = null;
    public GameObject initPoint;
    private Transform m_rTransform = null;
    // Array to store particles info
    private ParticleSystem.Particle[] m_rParticlesArray = null;
    private bool animationPlaying = false;
    private bool move_to_Objective = false;

    private bool m_bWorldPosition = false;
    private int m_iNumActiveParticles = 0;
    private Vector3 m_vParticlesTarget = Vector3.zero;
    public ParticleSystem.MinMaxGradient colorTouch;
    public ParticleSystem.MinMaxGradient colorNoTouch;

    void Start()
    {
        Setup();
        move_to_Objective = false;
       
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void PlayRiver()
    {
        move_to_Objective = true;
        if (AffectedParticles != null)
        {
            AffectedParticles.gameObject.transform.position = initPoint.gameObject.transform.position;

            AffectedParticles.Play();
        }
    }

    public bool movingDone() {

        return !move_to_Objective;
    }
    public void ChangeColorTo(ParticleSystem.MinMaxGradient colorTouchfun, ParticleSystem.MinMaxGradient colorNoTouchfun)
    {
        if (AffectedParticles != null)
        {

            ParticleSystem.ColorOverLifetimeModule temp = AffectedParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;

            colorTouch = colorTouchfun;
            colorNoTouch = colorNoTouchfun;
            temp.color = colorNoTouchfun;


        }

       


    }
    void LateUpdate()
    {


        //sphereObjective = mandalamanager.instance.givePointFronTriangle();
        // Work only if we have something to work on :)
        if (AffectedParticles != null && sphereObjective != null && move_to_Objective)
        {

            // Let's fetch active particles info
            m_iNumActiveParticles = AffectedParticles.GetParticles(m_rParticlesArray);
            // The attractor's target is it's world space position
            m_vParticlesTarget = sphereObjective.transform.position;
            //AffectedParticles.GetComponentInChildren<Collider>().transform.position = sphereObjective.transform.position;
            // If the system is not simulating in world space, let's project the attractor's target in the system's local space
            if (!m_bWorldPosition)
                m_vParticlesTarget -= AffectedParticles.transform.position;



            // m_iNumActiveParticles = AffectedParticles.GetParticles(m_rParticlesArray);
            if (m_iNumActiveParticles == 0)
            {
                
                move_to_Objective = false;

            }

            // For each active particle...
            for (int iParticle = 0; iParticle < m_iNumActiveParticles; iParticle++)
            { // The movement cursor is the opposite of the normalized particle's lifetime m_fCursor = 1.0f - (m_rParticlesArray[iParticle].lifetime / m_rParticlesArray[iParticle].startLifetime); // Are we over the activation treshold? if (m_fCursor >= ActivationTreshold)
                {

                    float dist = Vector3.Distance(m_rParticlesArray[iParticle].position, m_vParticlesTarget);

                    // Interpolate the movement towards the target with a nice quadratic easing					
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
