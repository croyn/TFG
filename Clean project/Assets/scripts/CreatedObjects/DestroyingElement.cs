using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PassingMovement))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Audiopan))]
public class DestroyingElement : MonoBehaviour
{
    /// <summary>
    /// reference to the linear autonomous movement
    /// </summary>
    PassingMovement passing;

    /// <summary>
    /// speed of motion of the element in the different movements
    /// </summary>
    public float speed;

    /// <summary>
    /// reference t the player's transform
    /// </summary>
    public Transform Player1, Player2;

    /// <summary>
    /// audioreference to make the movement sound
    /// </summary>
    public AudioSource audioplayer;

    public AudioClip movingaudio, crackingaudio;

    public float threshold = 8;

    /// <summary>
    /// typeof roaming particles
    /// </summary>
    public RoamingObjectType type;

    /// <summary>
    /// Handler to see if interaction with Asteroid started
    /// </summary>
    private bool firstTouch = true;

    /// <summary>
    /// set the speeds and the refernce to the players
    /// </summary>
    void Start()
    {
        passing = GetComponent<PassingMovement>();
        if (passing != null)
        {
            //passing.speed = speed;
            passing.movingspeed = speed;
        }
        Player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        Player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        audioplayer = GetComponent<AudioSource>();
        movingaudio = RoamingObjectType.Asteroid.GetMovingAudio();
        crackingaudio = RoamingObjectType.Asteroid.GetInteractingAudio();
    }

    float emissiontimer = 1;
    void Update() {
        emissiontimer -= Time.deltaTime;
        if (Vector3.Distance(transform.position, Player1.position) < threshold)
        {
            if (firstTouch)
            {
                firstTouch = !firstTouch;
                audioplayer.clip = crackingaudio;
                audioplayer.loop = true;
                audioplayer.Play();
            }

            if (Vector3.Distance(transform.position, Player2.position) < threshold)
            {
                if (emissiontimer < 0)
                {
                    transform.GetChild(1).GetComponent<ParticleSystem>().Emit(2);
                    emissiontimer = 1;
                }
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 0.05f);
                Player1.GetComponent<PlayerStateManager>().trackDestroyingElement(gameObject, true);
                Player2.GetComponent<PlayerStateManager>().trackDestroyingElement(gameObject, true);
                Logger.AddAsteroidTwoPlayerEventOnLog(gameObject.name, transform.position);
            }
            else
            {
                if (emissiontimer < 0)
                {
                    transform.GetChild(1).GetComponent<ParticleSystem>().Emit(1);
                    emissiontimer = 1;
                }
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 0.015f);
                Player1.GetComponent<PlayerStateManager>().trackDestroyingElement(gameObject, false);
                Player2.GetComponent<PlayerStateManager>().releaseDestroyingElement();
            }   
        }
        else
        {
            if (Vector3.Distance(transform.position, Player2.position) < threshold)
            {
                if (firstTouch)
                {
                    firstTouch = !firstTouch;
                    audioplayer.clip = crackingaudio;
                    audioplayer.loop = true;
                    audioplayer.Play();
                }

                if (emissiontimer < 0)
                {
                    transform.GetChild(1).GetComponent<ParticleSystem>().Emit(1);
                    emissiontimer = 1;
                }
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 0.015f);
                Player1.GetComponent<PlayerStateManager>().releaseDestroyingElement();
                Player2.GetComponent<PlayerStateManager>().trackDestroyingElement(gameObject, false);
            }
            else
            {
                Player1.GetComponent<PlayerStateManager>().releaseDestroyingElement();
                Player2.GetComponent<PlayerStateManager>().releaseDestroyingElement();

                if (!firstTouch)
                {
                    audioplayer.Stop();
                    playMovingAudio();
                    firstTouch = true;
                }
            }
        }

        if (ApplicationManager.isOutOfGameSpace(transform)) {
            transform.localScale = Vector3.one * 6;
        }
    }

    /// <summary>
    /// called if an object enters in contact with the roaming element
    /// </summary>
    /// <param name="collision">the other object</param>
    void OnTriggerEnter(Collider collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "AstralElement")
        {
            if (other.GetComponent<GeneratedObjectMovementmanager>().inOrbit) {
                other.transform.parent.parent.GetComponent<StarManager>().releasePlanet(other);
            }

            CreationEffectManager.instance.DestructionEffect(transform.localPosition, RoamingObjectType.Asteroid, other.GetComponent<GeneratedObjectMovementmanager>().Composedtypes[0].GetColor());
            RandomObjectmanager.instance.perfromDestructionSound(RoamingObjectType.Planet, transform.localPosition);
            Player1.GetComponent<PlayerStateManager>().releaseObject(other.gameObject);
            Player2.GetComponent<PlayerStateManager>().releaseObject(other.gameObject);
            GameObject.Destroy(other.gameObject);
            transform.position = new Vector3(-100, 0, 0);
            transform.localScale = Vector3.one * 6;
            audioplayer.Stop();
        }
    }

    /// <summary>
    /// detect if there is a collision
    /// </summary>
    /// <param name="collision">the colliding object</param>
    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "StarElement")
        {
            transform.position = new Vector3(-100, 0, 0);
            transform.localScale = Vector3.one * 6;
            audioplayer.Stop();
        }
    }

    public void playMovingAudio()
    {
        audioplayer.PlayOneShot(movingaudio, 1);
    }

    public void StopMovingAudio()
    {
        audioplayer.Stop();
    }
}
