using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FollowTwoPlayers))]
public class StarManager : MonoBehaviour
{
    /// <summary>
    /// reference t the player's transform
    /// </summary>
    public Transform Player1, Player2;

    /// <summary>
    /// limit distance of the two players from the center of the object to  interact with it
    /// </summary>
    public float threshold = 7.5f;

    /// <summary>
    /// reference to the movement script associated to the interaction of the star
    /// </summary>
    public FollowTwoPlayers followtwo;

    /// <summary>
    /// number of orbiting object allowed (max 4)
    /// </summary>
    [Range(0,4)]
    public int numOrbits = 2;

    /// <summary>
    /// speed to follow the players
    /// </summary>
    //public float speed;

    bool activateeffect = false;

    bool collapsing = false;

    bool firstCall = true;

    /// <summary>
    /// reference to the component to track objects enetring the gravitational field
    /// </summary>
    SphereCollider triggerArea;

    /// <summary>
    /// list of points where the plaents will find the new orbit position associated to the mname of the orbiting object
    /// </summary>
    Dictionary<string, Vector3> elementtovectororigin = new Dictionary<string, Vector3>();

    /// <summary>
    /// list of the element orbiting the star and the associated orbit
    /// </summary>
    Dictionary<string, int> elementtonumberoforbit = new Dictionary<string, int>();

    /// <summary>
    /// retrienve the refence to players and set the interaction area
    /// </summary>
    void Start()
    {
        /*followtwo = GetComponent<FollowTwoPlayers>();
        if (followtwo != null)
        {
            followtwo.movingspeed = speed;//speed = speed;
        }
        Player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        Player2 = GameObject.FindGameObjectWithTag("Player2").transform;*/
        
    }

    public void setOrbit(int orbitvalue) {
        numOrbits = orbitvalue;
        setAreaOfAttraction(0.8f * (numOrbits));

        for (int i = numOrbits + 1; i < transform.childCount-1; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// determine if the players can interact with the star and move the newly orbiting object to the new orbits
    /// </summary>
    void Update()
    {
        /*if (Vector3.Distance(transform.position, Player1.position) < threshold && possibledrag)
        {
            if (Vector3.Distance(transform.position, Player2.position) < threshold && possibledrag)
            {
                Player1.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, true, threshold);
                Player2.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, true, threshold);
                followtwo.setup(Player1, Player2);
                if (!followtwo.followtarget)
                {
                    Logger.AddStarMoveEventOnLog(gameObject.name);
                }
                //GetComponent<SphereCollider>().enabled = false;
                inmovement = true;
            }
            else
            {
                Player1.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, false, threshold);
                followtwo.followtarget = false;
                StartCoroutine("releasedStar");
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, Player2.position) < threshold)
            {
                Player2.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, false, threshold);
            }
            followtwo.followtarget = false;
            StartCoroutine("releasedStar");
        }*/
        for (int i = 0; i < numOrbits; i++) {
            if (transform.GetChild(0).childCount > i)
            {
                if (elementtovectororigin.ContainsKey(transform.GetChild(0).GetChild(i).name))
                {
                    if (elementtovectororigin[transform.GetChild(0).GetChild(i).name] != Vector3.zero)
                    {
                        Vector3 v = transform.GetChild(elementtonumberoforbit[transform.GetChild(0).GetChild(i).name] + 1).GetChild(0).position;
                        transform.GetChild(0).GetChild(i).transform.position = Vector3.MoveTowards(transform.GetChild(0).GetChild(i).transform.position, v, Time.deltaTime*5);
                        if (Vector3.Distance(transform.GetChild(0).GetChild(i).transform.position, v) < 0.5f)
                        {
                            transform.GetChild(0).GetChild(i).GetComponent<OrbitBehaviour>().startmotion = true;//.startOrbit = true;
                            elementtovectororigin[transform.GetChild(0).GetChild(i).name] = Vector3.zero;
                            transform.GetChild(elementtonumberoforbit[transform.GetChild(0).GetChild(i).name] + 1).GetChild(0).gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        int numberorbitingpalnet = 0;
        for (int i = 0; i < transform.GetChild(0).childCount && !collapsing; i++) {
            if (transform.GetChild(0).GetChild(i).GetComponent<OrbitBehaviour>() != null) {
                if (transform.GetChild(0).GetChild(i).GetComponent<OrbitBehaviour>().startmotion)
                {
                    numberorbitingpalnet++;
                }
            }
        }
        if (numberorbitingpalnet == numOrbits || collapsing)
            {
                bool finish = true;
                collapsing = true;
                for (int i = 0; i < transform.GetChild(0).childCount; i++)
                {
                    Transform t = transform.GetChild(0).GetChild(i);
                    t.GetComponent<OrbitBehaviour>().startmotion = false;//startOrbit = false;
                    t.localPosition = Vector3.MoveTowards(t.localPosition, Vector3.zero, Time.deltaTime / 2);
                    if (Vector3.Distance(t.localPosition, Vector3.zero) > 0.05)
                    {
                        finish = false;
                    }
                    t.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
                if (finish)
                {
                    int nextworld = -1;
                    do
                    {
                        nextworld = Random.Range(0, WorldParticleAssingment.getNumberOfWorld());
                    } while (nextworld == ApplicationManager.instance.chosenWorld);
                    ObjectGeneratorManager.instance.CreateBlackHole(nextworld, gameObject);
                }
            }
    }

    /// <summary>
    /// an element has entred the gravitational field
    /// </summary>
    /// <param name="other">the entering eleemnt</param>
    void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "AstralElement" || other.tag == "ChosenPlanet") && transform.GetChild(0).childCount < numOrbits)// && !inmovement
        {
            if (!other.GetComponent<FollowTwoPlayers>().startmotion)//.followtarget)
            {
                other.transform.parent = transform.GetChild(0);
                if (other.GetComponent<OrbitBehaviour>() == null)
                {
                    other.gameObject.AddComponent<OrbitBehaviour>();
                }
                OrbitBehaviour orbit = other.GetComponent<OrbitBehaviour>();
                orbit.rotatingspeed = 50;//speed = 50;
                orbit.startmotion = false;//startOrbit = false;
                //other.GetComponent<GeneratedObjectMovementmanager>().attachedToOrbit();
                //Logger.AddPlanetAddedToOrbitEventOnLog(other.name, gameObject.name, other.transform.position);
                int freeorbit = getFreeOrbit();

                Vector3 direction = (other.transform.position - transform.position).normalized;

                if (elementtovectororigin.ContainsKey(other.name))
                {
                    elementtovectororigin[other.name] = transform.GetChild(freeorbit + 1).GetChild(0).position;//direction * (freeorbit + 1) * 0.8f;
                    elementtonumberoforbit[other.name] = freeorbit;
                }
                else
                {
                    elementtovectororigin.Add(other.name, transform.GetChild(freeorbit + 1).GetChild(0).position);//direction * (freeorbit + 1) * 0.8f);
                    elementtonumberoforbit.Add(other.name, freeorbit);
                }
                orbit.orbitalDistance = (freeorbit + 1) * (0.8f - 0.1f * freeorbit);
            }
        }
        if ((other.tag == "Capsule") && transform.GetChild(0).childCount < numOrbits)
        {
            GameObject planet = GameObject.FindGameObjectWithTag("ChosenPlanet");
            if (planet != null)
            {
                planet.GetComponent<FollowTwoPlayers>().startmotion = false;
                planet.tag = "AstralElement";
                gameObject.layer = LayerMask.NameToLayer("orbitingPlanet");
                planet.transform.parent = transform.GetChild(0);
                if (planet.GetComponent<OrbitBehaviour>() == null)
                {
                    planet.gameObject.AddComponent<OrbitBehaviour>();
                }
                OrbitBehaviour orbit = planet.GetComponent<OrbitBehaviour>();
                orbit.rotatingspeed = 50;//speed = 50;
                orbit.startmotion = false;//startOrbit = false;

                if (firstCall)
                {
                    planet.GetComponent<GeneratedObjectMovementmanager>().attachedToOrbit();
                    Logger.AddPlanetAddedToOrbitEventOnLog(planet.name, gameObject.name, planet.transform.position);
                }

                firstCall = !firstCall;

                int freeorbit = getFreeOrbit();

                Vector3 direction = (planet.transform.position - transform.position).normalized;

                if (elementtovectororigin.ContainsKey(planet.name))
                {
                    elementtovectororigin[planet.name] = transform.GetChild(freeorbit + 1).GetChild(0).position;//direction * (freeorbit + 1) * 0.8f;
                    elementtonumberoforbit[planet.name] = freeorbit;
                }
                else
                {
                    elementtovectororigin.Add(planet.name, transform.GetChild(freeorbit + 1).GetChild(0).position);//direction * (freeorbit + 1) * 0.8f);
                    elementtonumberoforbit.Add(planet.name, freeorbit);
                }
                orbit.orbitalDistance = (freeorbit + 1) * (0.8f - 0.1f * freeorbit);
            }
        }
        if (other.tag == "StarElement" && gameObject.layer == LayerMask.NameToLayer("ExistingStar") && other.gameObject.layer != LayerMask.NameToLayer("ExistingStar"))
        {
            other.transform.position = other.transform.position - (transform.position - other.transform.position).normalized*0.1f;
        }

    }

    //void OnTriggerEnter(Collider other) {
    //    if (other.tag == "StarElement" && gameObject.layer == LayerMask.NameToLayer("ExistingStar") && other.gameObject.layer != LayerMask.NameToLayer("ExistingStar"))
    //    {
    //        other.transform.position = other.transform.position - (transform.position - other.transform.position).normalized;
    //    }
    //}

    /// <summary>
    /// release a planet form the orbit to move it away
    /// </summary>
    /// <param name="reward">teh objhect to release</param>
    public void releasePlanet(GameObject g)
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++) {
            if (transform.GetChild(0).GetChild(i).gameObject.GetInstanceID() == g.GetInstanceID()) {
                transform.GetChild(elementtonumberoforbit[g.name] + 1).GetChild(0).gameObject.SetActive(true);
                g.transform.parent = transform.parent;
                g.GetComponent<GeneratedObjectMovementmanager>().detatchToOrbit();
                elementtovectororigin.Remove(g.name);
                elementtonumberoforbit.Remove(g.name);
                //Logger.AddPlanetRemovedFromOrbitEventOnLog(g.name, gameObject.name, g.transform.position);
            }
        }
    }

    /// <summary>
    /// detrmine which is the closest orbit to place a planet
    /// </summary>
    /// <returns>index of the orbit</returns>
    private int getFreeOrbit() {
        
        for (int i = 0; i < numOrbits; i++) {
            bool found = false;
            foreach (int s in elementtonumberoforbit.Values) {
                if (s == i) {
                    found = true;
                }
            }
            if(!found){
                return i;
            }
        }
        return transform.GetChild(0).childCount;
    }

    /*IEnumerator releasedStar() {
        yield return new WaitForSeconds(2);
        if (!followtwo.followtarget) {
            //GetComponent<SphereCollider>().enabled = true;
            inmovement = false;
        }
    }*/

    /// <summary>
    /// set the area in which the planets are going to be attracted by the star
    /// </summary>
    /// <param name="dimensionfiled">the dimension of the are. Default 0.8*number of orbits</param>
    public void setAreaOfAttraction(float dimensionfiled) {
        triggerArea = GetComponent<SphereCollider>();
        triggerArea.radius = dimensionfiled;
    }

    /// <summary>
    /// Turns slowly red the first free orbit in the star
    /// </summary>
    public void HighlightFreeOrbit() {
        int nextfreeorbit = getFreeOrbit();
        activateeffect = true;
        StartCoroutine(effectHighlight(transform.GetChild(nextfreeorbit + 1).gameObject));
        StartCoroutine(blink(transform.GetChild(nextfreeorbit + 1).gameObject));
    }

    /// <summary>
    /// stop highlighting the free orbit in the star
    /// </summary>
    public void StopHighlightFreeOrbit()
    {
        activateeffect = false;
    }

    /// <summary>
    /// perform the fadeing on the selected orbit
    /// </summary>
    /// <param name="g">the orbit to be highlighted</param>
    /// <returns></returns>
    IEnumerator effectHighlight(GameObject g) {
        do {
            g.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.Lerp(g.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), Color.yellow * 4, Time.deltaTime));
            g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(g.transform.GetChild(0).GetComponent<SpriteRenderer>().color, Color.yellow, Time.deltaTime);
            yield return new WaitForFixedUpdate();

        } while (activateeffect);

        do
        {
            StopCoroutine(blink(g));
            g.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.Lerp(g.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), Color.white * 4, Time.deltaTime));
            g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(g.transform.GetChild(0).GetComponent<SpriteRenderer>().color, Color.white, Time.deltaTime);
            yield return new WaitForFixedUpdate();
        } while (g.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor").b < 3.9);

        g.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.white * 4);
        g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator blink(GameObject g)
    {
        do
        {
            g.GetComponent<MeshRenderer>().enabled = true;
            g.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(.6f);
            g.GetComponent<MeshRenderer>().enabled = false;
            g.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(.3f);

        } while (activateeffect);
        g.GetComponent<MeshRenderer>().enabled = true;
        g.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }

}
