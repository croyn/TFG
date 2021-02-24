using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FiducialController))]
[RequireComponent(typeof(CollisionDetector))]
public class PlayerStateManager : MonoBehaviour
{
    /// <summary>
    /// true if the player has particles around him
    /// </summary>
    public bool playerHasPresent {get{ return presentreceived.Count > 0;}}

    /// <summary>
    /// list of particles types of the particles that are surrounding the player
    /// </summary>
    List<ParticleType> presentreceived;

    /// <summary>
    /// reference to the transform of the element interacting with it
    /// </summary>
    public Transform interactigElement = null, destroyingElement = null;

    /// <summary>
    /// threshold after which the reference element is to be cosidered ineffective
    /// </summary>
    public float threshold;

    /// <summary>
    /// true if the player has enough strenght to interact with the object alone, false if he needs the other player
    /// </summary>
    public bool enoughStrenght, speedupdistruction;

    /// <summary>
    /// reference to the particle system used to visualize the connection between a player and an object
    /// </summary>
    private ParticleSystem playerToObjectStream;

    /// <summary>
    /// Capsule which encloses the interactive element
    /// </summary>
    public GameObject capsule;
    public GameObject halfCapsule;

    /// <summary>
    /// Indicator System to point out collaboration opportunity
    /// </summary>
    private Indicator Indicator;

    /// <summary>
    /// 
    /// </summary>
    public bool abletointeract = true;

    private bool meaningfullyConnectedToPlanet = false;
    private bool meaningfullyConnectedToAsteroid = false;

    public GameObject followingCometOrMeteoroid = null;
 
    /// <summary>
    /// initialize the list and get the reference of the particle system
    /// </summary>
    void Start()
    {
        presentreceived = new List<ParticleType>();

        playerToObjectStream = transform.GetChild(2).GetComponent<ParticleSystem>();
        playerToObjectStream.gameObject.SetActive(false);

        capsule = GameObject.FindGameObjectWithTag("Capsule");
        resetCapsule();

        halfCapsule = playerToObjectStream.transform.Find("Half_Capsule").gameObject;

        Indicator = transform.GetChild(3).GetComponent<Indicator>();

    }

    public void resetCapsule() {

        capsule.transform.position = new Vector3(0, 0, 0);
        capsule.GetComponent<SphereCollider>().enabled = false;
        capsule.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void followpayer(GameObject followobject) {
        if (followingCometOrMeteoroid != null && followingCometOrMeteoroid != followobject) {
            followingCometOrMeteoroid.GetComponent<SingleRoamingElementManager>().releaseFromPlayer();
        }
        followingCometOrMeteoroid = followobject;
    }
    public void stopfollowpayer()
    {
        followingCometOrMeteoroid = null;
    }

    /// <summary>
    /// player has received particles from the other player
    /// </summary>
    /// <param name="t">type of particle received</param>
    public void PlayerReveidedAPresent(ParticleType t) {
        if (!presentreceived.Contains(t))
        {
            presentreceived.Add(t);
        }
    }

    /// <summary>
    /// player has used the particles around him for an effect or to create a planet
    /// </summary>
    public void PlayerUsedAPresent(ParticleType t)
    {
        if (t != ParticleType.All)
        {
            presentreceived.Remove(t);
        }else{
            presentreceived.Clear();
        }
        StartCoroutine("DestroyPresent");
    }

    /// <summary>
    /// The player has particles to use?
    /// </summary>
    /// <returns>true if the player has particles available</returns>
    /*public bool PlayerCanBurstParticles() {
        return !playerHasPresent;
    }*/

    /// <summary>
    /// which partile types surround the player?
    /// </summary>
    /// <returns>the list of particle received</returns>
    public ParticleType[] getPresentTypes() {
        return presentreceived.ToArray();
    }

    /// <summary>
    /// remove the particles still orbiting around the player when an effect required them to be freed
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyPresent() {
        yield return null;
        float timetowait = 0.05f / transform.childCount;
        for (int i = 0; i < transform.childCount; i++) {
            if(transform.GetChild(i).GetComponent<ParticleBehaviour>() != null){
                transform.GetChild(i).GetComponent<ParticleBehaviour>().lifespan = timetowait*i +1;
            }
        }
    }

    /// <summary>
    /// signal the player that an object want to interact with him
    /// </summary>
    /// <param name="t">the object to interact with</param>
    /// <param name="b">the player has enough strenght to interact with the object</param>
    /// <param name="th">threshold to move the interactive object</param>
    public void SignalInteractingElement(Transform t, bool b, float th) {
        if (interactigElement == null || t.gameObject.name == interactigElement.transform.name || b)
        {
            if (interactigElement == null)
            {
                if (t.gameObject.GetComponent<StarManager>() != null)
                {
                    if (!enoughStrenght)
                    {
                        //Logger.AddStarCloseByPlayerEventOnLog(gameObject.name, t.name, transform.position);
                    }
                }
                else
                {
                    /*if (!enoughStrenght)
                    {
                        //Logger.AddPlanetCloseByPlayerEventOnLog(gameObject.name, t.name, transform.position);
                    }*/
                    StartCoroutine(CheckForMeaningfulConnection());
                    List<GameObject> particles = new List<GameObject>();
                    for (int i = 0; i < transform.childCount; i++) {
                        if (transform.GetChild(i).GetComponent<ParticleBehaviour>() != null && System.Array.Exists(t.GetComponent<GeneratedObjectMovementmanager>().Composedtypes, elemenet => elemenet == transform.GetChild(i).GetComponent<ParticleBehaviour>().type)) {
                            particles.Add(transform.GetChild(i).gameObject);
                            
                        }
                    }
                    t.GetComponent<GeneratedObjectMovementmanager>().ImprovePlanet(particles.ToArray());
                    foreach (ParticleType tp in t.GetComponent<GeneratedObjectMovementmanager>().Composedtypes)
                    {
                        redirectFlow(t, tp);
                    }
                }
            }
            interactigElement = t;
            threshold = th;
            enoughStrenght = b;
            
        }
    }

    IEnumerator CheckForMeaningfulConnection() { 
        yield return new WaitForSeconds(2);
        if (!enoughStrenght && IA.instance.IsPlanetContinuousTouch())
        {
            Logger.AddPlanetCloseByPlayerEventOnLog(gameObject.name, interactigElement.name, transform.position);
        }
    }

    /// <summary>
    /// modify the emission rate of the stream of particles to connect the object and the player
    /// </summary>
    void Update() {
        if (interactigElement != null)
        {
            if (Vector3.Distance(transform.position, interactigElement.position) > threshold)
            {
                if (interactigElement.gameObject.GetComponent<StarManager>() != null)
                {
                    if (!enoughStrenght)
                    {
                        //Logger.AddStarDroppedEventOnLog(gameObject.name, interactigElement.name, interactigElement.position);
                    }
                }
                else
                {
                    if (!enoughStrenght)
                    {
                        //Logger.AddPlanetDroppedEventOnLog(gameObject.name, interactigElement.name, interactigElement.position);
                    }
                }
                if (interactigElement.tag == "ChosenPlanet")
                {
                    interactigElement.tag = "AstralElement";
                }
                interactigElement = null;

                // Hide Connector & Stream
                playerToObjectStream.gameObject.SetActive(false);
                resetCapsule();
                abletointeract = true;

                // Stop Indicator
                Indicator.stopIndicator();

                // Falsify interactions with planets
                IA.instance.SetPlanetTouching(false);
                IA.instance.SetPlanetConnecting(false);
                IA.instance.EndPlanetContinousTouch();
                IA.instance.EndPlanetContinousConnection();

            }
            if (interactigElement != null)
            {
                // Make sure objects are in higher position then the target
                float target_x = interactigElement.position.x;
                float target_z = interactigElement.position.z;
                Vector3 target = new Vector3(target_x, 0, target_z);

                // Adjust connector and stream to target & position
                playerToObjectStream.gameObject.SetActive(true);
                playerToObjectStream.transform.LookAt(target);

                ParticleSystem.EmissionModule em = playerToObjectStream.emission;
                ParticleSystem.MainModule main = playerToObjectStream.main;

                main.startSpeed = Vector3.Distance(transform.position, interactigElement.position);

                if (enoughStrenght)
                {

                    abletointeract = false;
                    halfCapsule.GetComponent<MeshRenderer>().enabled = false;

                    // Intensify stream
                    em.rateOverTime = 7;
                    main.startSize = .7f;

                    // Lock object between players
                    capsule.transform.position = interactigElement.transform.position;
                    capsule.GetComponent<SphereCollider>().enabled = true;
                    if (interactigElement.tag == "AstralElement")
                    {
                        interactigElement.tag = "ChosenPlanet";
                    }

                    // Stop Indicator
                    Indicator.stopIndicator();

                    IA.instance.SetPlanetTouching(false);
                    IA.instance.SetPlanetConnecting(true);
                    IA.instance.EndPlanetContinousTouch();

                }
                else
                {

                    abletointeract = true;
                    halfCapsule.transform.position = interactigElement.transform.position;
                    halfCapsule.GetComponent<MeshRenderer>().enabled = true;

                    // Weaken stream
                    em.rateOverTime = 5;
                    main.startSize = .3f;

                    // Release object again
                    resetCapsule();

                    IA.instance.SetPlanetTouching(true);
                    IA.instance.SetPlanetConnecting(false);
                    IA.instance.EndPlanetContinousConnection();

                }
                playerToObjectStream.Play();
            }
        }
        else {

        }

        if (destroyingElement != null) {
            if (ApplicationManager.isOutOfGameSpace(destroyingElement)) {
                if (meaningfullyConnectedToAsteroid)
                {
                    Logger.AddAsteroidDroppedEventOnLog(gameObject.name, destroyingElement.name, transform.position);
                }
                meaningfullyConnectedToAsteroid = false; destroyingElement = null;
                speedupdistruction = false;
                playerToObjectStream.gameObject.SetActive(false);
                abletointeract = true;
                return;
            }
            // Make sure objects are in higher position then the target
            Vector3 target = destroyingElement.transform.position + Vector3.down * 2;

            // Adjust connector and stream to target & position

            playerToObjectStream.gameObject.SetActive(true);
            playerToObjectStream.transform.LookAt(target);

            ParticleSystem.EmissionModule em = playerToObjectStream.emission;
            ParticleSystem.MainModule main = playerToObjectStream.main;

            main.startSpeed = Vector3.Distance(transform.position, destroyingElement.position);

            halfCapsule.GetComponent<MeshRenderer>().enabled = false;

            if (speedupdistruction)
            {

                abletointeract = false;

                // Intensify stream
                em.rateOverTime = 7;
                main.startSize = .7f;


                // Stop Indicator
                Indicator.stopIndicator();

            }
            else
            {

                abletointeract = true;

                // Weaken stream
                em.rateOverTime = 5;
                main.startSize = .3f;

            }
            playerToObjectStream.Play();
            
        }
    }

    public void releaseObject(GameObject g)
    {
        if (interactigElement == null) {
            return;
        }

        if (g == null) {
            interactigElement = null;
            enoughStrenght = false;
            playerToObjectStream.gameObject.SetActive(false);
            resetCapsule();
            abletointeract = true;
            return;
        }
        
        if (interactigElement.name == g.name)
        {
            interactigElement = null;
            enoughStrenght = false;
            playerToObjectStream.gameObject.SetActive(false);
            resetCapsule();
            abletointeract = true;
        }
    }

    /// <summary>
    /// redirect the already sent particles towards a new target
    /// </summary>
    /// <param name="newtarget"> the new object the particole need to converge to</param>
    public void redirectFlow(Transform newtarget, ParticleType t)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ParticleBehaviour>() != null)
            {
                if (transform.GetChild(i).GetComponent<ParticleBehaviour>().type == t || t == ParticleType.All)
                {
                    transform.GetChild(i).GetComponent<LinearMotionBehaviour>().setTarget(newtarget);
                    transform.GetChild(i).GetComponent<LinearMotionBehaviour>().startmotion = true;//.startmovement = true;
                    transform.GetChild(i).GetComponent<LinearMotionBehaviour>().movingspeed = 0.1f;//.speed = 0.1f;
                    transform.GetChild(i).GetComponent<OrbitBehaviour>().startmotion = false;//startOrbit = false;
                    transform.GetChild(i).GetComponent<ParticleBehaviour>().lifespan = 1f;
                }
                
            }
            
        }
    }

    public void trackDestroyingElement(GameObject g, bool hasstenght) {
        destroyingElement = g.transform;
        speedupdistruction = hasstenght;
        StartCoroutine(CheckForMeaningfulConnectionAsteroid());
        //Logger.AddAsteroidCloseByPlayerEventOnLog(gameObject.name, destroyingElement.name, transform.position);
    }

    private IEnumerator CheckForMeaningfulConnectionAsteroid()
    {
        yield return new WaitForSeconds(1);
        if (destroyingElement != null)
        {
            Logger.AddAsteroidCloseByPlayerEventOnLog(gameObject.name, destroyingElement.name, transform.position);
            meaningfullyConnectedToAsteroid = true;
        }
    }

    public void releaseDestroyingElement()
    {
        if (destroyingElement == null) {
            return;
        }
        if (meaningfullyConnectedToAsteroid)
        {
            Logger.AddAsteroidDroppedEventOnLog(gameObject.name, destroyingElement.name, transform.position);
        }
        meaningfullyConnectedToAsteroid = false;
        destroyingElement = null;
        speedupdistruction = false;
        playerToObjectStream.gameObject.SetActive(false);
        abletointeract = true;
    }
}
