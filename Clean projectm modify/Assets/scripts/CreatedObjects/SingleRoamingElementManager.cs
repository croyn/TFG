using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PassingMovement))]
[RequireComponent(typeof(FollowOnePLayer))]
[RequireComponent(typeof(FollowTwoPlayers))]
public class SingleRoamingElementManager : MonoBehaviour
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
    /// reference to the linear autonomous movement
    /// </summary>
    PassingMovement passing;

    /// <summary>
    /// reference to the movement around a single close player
    /// </summary>
    FollowOnePLayer followone;

    /// <summary>
    /// reference to the movement with two close players
    /// </summary>
    FollowTwoPlayers followtwo;

    /// <summary>
    /// speed of motion of the element in the different movements
    /// </summary>
    public float speed;

    /// <summary>
    /// typeof roaming particles
    /// </summary>
    public RoamingObjectType type;

    bool forcebehaviourflag = false;

    /// <summary>
    /// set the speeds and the refernce to the players
    /// </summary>
    void Start()
    {
        passing = GetComponent<PassingMovement>();
        if (passing != null)
        {
            passing.movingspeed = speed;//speed = speed;
        }
        followone = GetComponent<FollowOnePLayer>();
        if (followone != null)
        {
            followone.movingspeed = speed;//speed = speed;
        }
        followtwo = GetComponent<FollowTwoPlayers>();
        if (followtwo != null)
        {
            followtwo.movingspeed = speed;//speed = speed;
        }
        Player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        Player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        foreach (MeshRenderer m in transform.GetComponentsInChildren<MeshRenderer>()) {
            m.material.color = type.GetColor();
            m.material.SetColor("_EmissionColor", type.GetColor());
        }
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(type.GetColor(), 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.6f, 0.0f), new GradientAlphaKey(0, 1.0f) }
            );
        foreach (TrailRenderer t in transform.GetComponentsInChildren<TrailRenderer>()) { 
            t.colorGradient = grad;
        }
        foreach (ParticleSystem p in transform.GetComponentsInChildren<ParticleSystem>())
        {
            ParticleSystem.MainModule m = p.main;
            m.startColor = type.GetColor();
        }
    }

    public void releaseFromPlayer()
    {
        if (followone.startmotion)//.followtarget)
        {
            passing.setup(followone.direction);
        }
        else
        {
            //following two
            passing.setup(transform.forward);
        }
        followone.startmotion = false;
        followtwo.startmotion = false;
        forcebehaviourflag = true;
        StartCoroutine(resetBehaviour());
    }

    IEnumerator resetBehaviour() {
        yield return new WaitForSeconds(3f);
        forcebehaviourflag = false;
    }

    /// <summary>
    /// determine the type of motion required
    /// </summary>
    void Update()
    {
        if (!forcebehaviourflag)
        {
            if (Vector3.Distance(transform.position, Player1.position) < threshold)
            {
                if (Vector3.Distance(transform.position, Player2.position) < threshold)
                {
                    //the two players are both close to the object
                    followone.startmotion = false;//followtarget = false;
                    followtwo.setup(Player1, Player2);
                    Player1.GetComponent<PlayerStateManager>().followpayer(gameObject);
                    Player2.GetComponent<PlayerStateManager>().followpayer(gameObject);
                }
                else
                {
                    //be attracted by player1
                    if (passing.startmotion || followtwo.startmotion)//.followtarget)//(passing.startmoving || followtwo.followtarget)
                    {
                        followone.setup(Player1);
                        
                    }
                    if (followone.isOrbiting) {
                        Player1.GetComponent<PlayerStateManager>().followpayer(gameObject);
                        Player2.GetComponent<PlayerStateManager>().stopfollowpayer();
                    }
                    followtwo.startmotion = false;//.followtarget = false;
                }
                passing.resetted = false;
                passing.startmotion = false;
                //passing.startmoving = false;
            }
            else
            {
                if (Vector3.Distance(transform.position, Player2.position) < threshold)
                {
                    //be attracted by player2
                    if (passing.startmotion || followtwo.startmotion)//.followtarget)//(passing.startmoving || followtwo.followtarget)
                    {
                        followone.setup(Player2);
                        
                    }
                    if (followone.isOrbiting)
                    {
                        Player2.GetComponent<PlayerStateManager>().followpayer(gameObject);
                        Player1.GetComponent<PlayerStateManager>().stopfollowpayer();
                    }
                    passing.resetted = false;
                    passing.startmotion = false;
                    //passing.startmoving = false;
                    followtwo.startmotion = false;//.followtarget = false;
                }
                else
                {
                    if (!passing.resetted)
                    {
                        //enter here only if the object has been attracted by someone
                        if (followone.startmotion)//.followtarget)
                        {
                            passing.setup(followone.direction);
                        }
                        else
                        {
                            //following two
                            passing.setup(transform.forward);
                        }
                        //Player1.GetComponent<PlayerStateManager>().stopfollowpayer();
                        //Player2.GetComponent<PlayerStateManager>().stopfollowpayer();
                        followone.startmotion = false;//.followtarget = false;
                        followtwo.startmotion = false;//.followtarget = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// called if an object enters in contact with the roaming element
    /// </summary>
    /// <param name="collision">the other object</param>
    void OnTriggerEnter(Collider collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "AstralElement" || other.tag == "Capsule" )//|| other.tag == "RoamingObject")
        {
            if (transform.position.y < 100)
            {
                Vector3 pos;
                if (other.tag == "AstralElement")//"RoamingObject")
                {
                     pos = other.transform.localPosition;
                }
                else {
                    pos = other.transform.position - new Vector3(54, 0, 50);
                }
                CreationEffectManager.instance.GenerationEffect(pos, RoamingObjectType.Other);
                if (transform.childCount > 0)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }

                transform.position = Vector3.one * 100;
            }
            
        }
    }

    /// <summary>
    /// detect if there is a collision
    /// </summary>
    /// <param name="collision">the colliding object</param>
    void OnCollisionEnter(Collision collision) 
    {
        GameObject other = collision.gameObject;
        if (other.tag == "CoreStarElement")
        {
            transform.position = Vector3.one * 100;
            Vector3 pos = other.transform.localPosition;
            CreationEffectManager.instance.GenerationEffect(pos, RoamingObjectType.Other);
        }
        
    }
}
