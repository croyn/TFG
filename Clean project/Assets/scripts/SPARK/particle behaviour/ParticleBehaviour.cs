using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LinearMotionBehaviour))]
[RequireComponent(typeof(OrbitBehaviour))]
public class ParticleBehaviour : MonoBehaviour
{
    /// <summary>
    /// amount of time the particle survive before disappearing
    /// </summary>
    public float lifespan = 50;

    /// <summary>
    /// constant to determine the initial life amount for the particles of type II
    /// </summary>
    public static float maxlifespan = 30;

    /// <summary>
    /// the type of particle
    /// </summary>
    public ParticleType type;

    /// <summary>
    /// the player who create the stream of particle, this object belong
    /// </summary>
    public GameObject creator{get; set;}

    /// <summary>
    /// particle has the possibility to be used?
    /// </summary>
    public bool readyToJoin = false;

    /// <summary>
    /// reference to the gameobject of the player this particle is attached to.
    /// </summary>
    public GameObject attachedplayer;

    float orbitdistance = 5;

    /// <summary>
    /// Start the linear motion
    /// </summary>
    void Start() {
        GetComponent<LinearMotionBehaviour>().startmotion = true;//startmovement = true;
        lifespan = maxlifespan;
        GetComponent<OrbitBehaviour>().orbitalDistance = orbitdistance;
    }

    /// <summary>
    /// update the remaining lifetime and eventually destroy the object
    /// </summary>
    void Update()
    {
        lifespan -= Time.deltaTime;
        
        if (lifespan < 0) {
            if (transform.parent != null)
            {
                if (transform.parent.GetComponent<PlayerStateManager>() != null)
                {
                    transform.parent.GetComponent<PlayerStateManager>().PlayerUsedAPresent(type);
                }
            }
            GameObject.Destroy(gameObject);
        }
    }

    /// <summary>
    /// change behaviour from moving towards player to orbiting around them
    /// </summary>
    public void ShiftBehaviour() {
        StartCoroutine("waittoShiftMovementType");
    }

    /// <summary>
    /// change behaviour from moving towards player to orbiting around them
    /// </summary>
    /// <returns></returns>
    IEnumerator waittoShiftMovementType()
    {
        GetComponent<LinearMotionBehaviour>().startmotion = false;//startmovement = false;
        yield return new WaitForSeconds(0.1f);
        transform.parent = transform;
        GetComponent<OrbitBehaviour>().startmotion = true;//startOrbit = true;
        GetComponent<OrbitBehaviour>().orbitalDistance = 0;
    }

    /// <summary>
    /// detect the collision of the particle with other rigidbodies
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if ((other.tag == "RoamingObject" && readyToJoin)) // (other.tag == "AstralElement") || (other.tag == "StarElement") ||
        {
            if (transform.parent != null)
            {
                if (transform.parent.tag == "Player1" || transform.parent.tag == "Player2")
                {
                    transform.parent.GetComponent<PlayerStateManager>().PlayerUsedAPresent(type);
                }
            }
            GetComponent<SphereCollider>().enabled = false;
            

            GameObject planet = ObjectGeneratorManager.instance.SubstitutePlanetToAsteroid(other.gameObject, attachedplayer);
            attachedplayer.GetComponent<PlayerStateManager>().redirectFlow(planet.transform, ParticleType.All);
            
            transform.parent = other.transform;
            lifespan = 0;
            ShiftBehaviour();
            StartCoroutine("cleanParticles");
        }
        
        if (other.tag == "Effect" || other.tag == "StarElement")
        {
            lifespan = 0;
        }
    }

    /// <summary>
    /// destroy the particle after 5 second of time
    /// </summary>
    /// <returns></returns>
    IEnumerator cleanParticles()
    {
        yield return new WaitForSeconds(5f);
        if(transform.parent.GetComponent<PlayerStateManager>() != null){
            transform.parent.GetComponent<PlayerStateManager>().PlayerUsedAPresent(type);
        }
        Destroy(gameObject);
    }
}
