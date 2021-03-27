using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(FollowOnePLayer))]
[RequireComponent(typeof(FollowTwoPlayers))]
[RequireComponent(typeof(AudioSource))]
public class GeneratedObjectMovementmanager : MonoBehaviour
{
    /// <summary>
    /// reference t the player's transform
    /// </summary>
    public Transform Player1, Player2;
    
    /// <summary>
    /// limit distance of the two players from the center of the object to  interact with it
    /// </summary>
    public float threshold = 10;

    /// <summary>
    /// reference to the movement with two close players
    /// </summary>
    FollowTwoPlayers followtwo;

    /// <summary>
    /// reference to the indicators
    /// </summary>
    private Indicator Indicator1, Indicator2;

    /// <summary>
    /// reference to IA for getting game states
    /// </summary>
    private IA IA;

    /// <summary>
    /// determine if the geenrated plaent is orbiting or not
    /// </summary>
    public bool inOrbit = false;

    /// <summary>
    /// speed of motion of the element in the different movements
    /// </summary>
    public float speed;


    public ParticleType[] Composedtypes;

    public int particlecount = 0;

    AudioSource audioplayer;

    /// <summary>
    /// set the speeds and the refernce to the players
    /// </summary>
    void Start()
    {
        audioplayer = GetComponent<AudioSource>();
        followtwo = GetComponent<FollowTwoPlayers>();

        if (followtwo != null)
        {
            followtwo.movingspeed = speed;//.speed = speed;
        }

        IA = IA.instance;

        Player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        Player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        Indicator1 = Player1.GetChild(3).GetComponent<Indicator>();
        Indicator2 = Player2.GetChild(3).GetComponent<Indicator>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<OrbitBehaviour>() != null)
            {
                transform.GetChild(i).GetComponent<OrbitBehaviour>().startmotion = true; //startOrbit = true;
            }
        }
    }

    /// <summary>
    /// determine the type of motion required
    /// </summary>
    void Update()
    {
        if (!inOrbit)
        {
            if (Vector3.Distance(transform.position, Player1.position) < threshold)
            {
                if (Vector3.Distance(transform.position, Player2.position) < threshold)
                {
                    if (Player1.GetComponent<PlayerStateManager>().abletointeract && Player2.GetComponent<PlayerStateManager>().abletointeract)
                    {

                        if (!followtwo.startmotion)
                        {
                            Logger.AddPlanetMoveEventOnLog(gameObject.name, transform.position);
                        }

                        followtwo.setup(Player1, Player2);

                        Player1.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, true, threshold);
                        Player2.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, true, threshold);
                    }
                }
                else
                {
                    Player1.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, false, threshold);
                    followtwo.startmotion = false;//followtarget = false;

                    if (IA.GetIndicatorsNeeded())
                    {
                        Indicator1.startIndicator(Player2, false);

                        if (IA.GetPointersNeeded())
                        {
                            Indicator2.startIndicator(Player1, true);
                        }
                        else
                        {
                            Indicator2.stopIndicator();
                        }
                    }
                    else
                    {
                        Indicator1.stopIndicator();
                        Indicator2.stopIndicator();
                    }

                    if (transform.GetComponent<BoxCollider>() != null)
                    {
                        transform.GetComponent<BoxCollider>().enabled = true;
                    }
                }
            }
            else
            {

                if (Vector3.Distance(transform.position, Player2.position) < threshold)
                {
                    Player2.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, false, threshold);

                    if (IA.GetIndicatorsNeeded())
                    {
                        Indicator2.startIndicator(Player1, false);

                        if (IA.GetPointersNeeded())
                        {
                            Indicator1.startIndicator(Player2, true);
                        }
                        else
                        {
                            Indicator1.stopIndicator();
                        }
                    }
                    else
                    {
                        Indicator1.stopIndicator();
                        Indicator2.stopIndicator();
                    }
                }
                else
                {
                    Indicator1.stopIndicator();
                    Indicator2.stopIndicator();
                }

                followtwo.startmotion = false;//followtarget = false;
                if (transform.GetComponent<BoxCollider>() != null)
                {
                    transform.GetComponent<BoxCollider>().enabled = true;
                }
            }
        }
        /*else {
            if (Vector3.Distance(transform.position, Player1.position) < threshold / 2)
            {
                if (Vector3.Distance(transform.position, Player2.position) < threshold / 2)
                {
                    Player1.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, true, threshold);
                    Player2.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, true, threshold);
                    transform.parent.parent.GetComponent<StarManager>().releasePlanet(gameObject);

                    followtwo.setup(Player1, Player2);

                    inOrbit = false;
                    if (GetComponent<OrbitBehaviour>() != null)
                    {
                        GetComponent<OrbitBehaviour>().startmotion = false;//startOrbit = false;
                    }

                }
                else
                {
                    Player1.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, false, threshold);
                    if (IA.GetIndicatorsNeeded())
                    {
                        Indicator1.startIndicator(Player2, false);
                        if (IA.GetPointersNeeded())
                        {
                            Indicator2.startIndicator(Player1, true);
                        }
                        else
                        {
                            Indicator2.stopIndicator();
                        }
                    }
                    else
                    {
                        Indicator1.stopIndicator();
                        Indicator2.stopIndicator();
                    }
                }
            }
            else {
                if (Vector3.Distance(transform.position, Player2.position) < threshold / 2)
                {
                    Player2.GetComponent<PlayerStateManager>().SignalInteractingElement(transform, false, threshold);
                    if (IA.GetIndicatorsNeeded())
                    {
                        Indicator2.startIndicator(Player1, false);
                        if (IA.GetPointersNeeded())
                        {
                            Indicator1.startIndicator(Player2, true);
                        }
                        else
                        {
                            Indicator1.stopIndicator();
                        }
                    }
                    else
                    {
                        Indicator1.stopIndicator();
                        Indicator2.stopIndicator();
                    }
                }
            }
        }*/
    }

    /// <summary>
    /// set the object as orbiting around a star
    /// </summary>
    public void attachedToOrbit()
    {
        inOrbit = true;
        followtwo.startmotion = false;//followtarget = false;
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<OrbitBehaviour>() != null) {
                //transform.GetChild(i).GetComponent<OrbitBehaviour>().speed = transform.GetChild(i).GetComponent<OrbitBehaviour>().speed * 5;
                transform.GetChild(i).GetComponent<OrbitBehaviour>().rotatingspeed = transform.GetChild(i).GetComponent<OrbitBehaviour>().rotatingspeed * 5;
            }
        }
        audioplayer.PlayOneShot(RoamingObjectType.Planet.GetInteractingAudio(), 1);
        gameObject.layer = LayerMask.NameToLayer("orbitingPlanet");
    }

    /// <summary>
    /// reset the object non orbiting around the star anymore
    /// </summary>
    public void detatchToOrbit()
    {
        inOrbit = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<OrbitBehaviour>() != null)
            {
                //transform.GetChild(i).GetComponent<OrbitBehaviour>().speed = transform.GetChild(i).GetComponent<OrbitBehaviour>().speed / 5;
                transform.GetChild(i).GetComponent<OrbitBehaviour>().rotatingspeed = transform.GetChild(i).GetComponent<OrbitBehaviour>().rotatingspeed / 5;
            }
        }
        gameObject.layer = LayerMask.NameToLayer("Planets");
    }



    public void ImprovePlanet(GameObject[] particles) {
        
        foreach (GameObject g in particles)
        {
            if( Composedtypes[0]== g.GetComponent<ParticleBehaviour>().type){
                particlecount ++;
                transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Composedtypes[0].GetColor() * Mathf.Min(5f, (particlecount + 5) / 10f));
            }
        }
        
    }
}
