using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(RandomMotionInTheSpace))]
[RequireComponent(typeof(LinearMotionBehaviour))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(OrbitBehaviour))]
public class Reward : MonoBehaviour
{
    /// <summary>
    /// state of the reward
    /// </summary>
    RewardState state = RewardState.animated;

    /// <summary>
    /// speed of the aniamtion to be performed
    /// </summary>
    public float animationspeed;

    /// <summary>
    /// speed of the rotation speed of the animation
    /// </summary>
    [Range(0, 1)]
    public float rotatinspeed;

    /// <summary>
    /// counter for the animation
    /// </summary>
    private float rotcounter;

    public bool faceorbitdirection = true;

    //RandomMotionInTheSpace mot;
    LinearMotionBehaviour mot;
    RotatingOnYourself rot;
    OrbitBehaviour orb;

    Transform target;

    AudioSource audioplayer;

    void Start() {
        //mot = GetComponent<RandomMotionInTheSpace>();//gameObject.AddComponent<RandomMotionInTheSpace>();
        mot = GetComponent<LinearMotionBehaviour>();
        orb = GetComponent<OrbitBehaviour>();
        orb.rotatingspeed = 50 * (1 + GameObject.FindGameObjectsWithTag("Reward").Length * 0.1f);//speed = 50;
        rot = transform.GetChild(0).GetChild(0).gameObject.AddComponent<RotatingOnYourself>();
        audioplayer = GetComponent<AudioSource>();
        audioplayer.PlayOneShot(RoamingObjectType.Reward.GetCreationAudio(), 1);
        
    }
    

    /// <summary>
    /// if the state is correct perform the animation
    /// </summary>
    void Update()
    {
        if (state == RewardState.animated)
        {
            foreach (TrailRenderer r in transform.GetChild(0).GetChild(0).GetComponentsInChildren<TrailRenderer>()) {
                r.enabled = false;
            }
            Color c = transform.GetChild(0).GetChild(0).GetComponentsInChildren<MeshRenderer>()[0].material.color;
            c = Color.Lerp(c, Color.white, Time.deltaTime * animationspeed);
            foreach (MeshRenderer m in transform.GetChild(0).GetChild(0).GetComponentsInChildren<MeshRenderer>())
            {
                m.material.color = c;
            }
            rotcounter += 0.01f;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotcounter * 360 * rotatinspeed, 0));

            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * animationspeed);
            if (transform.localScale.x < 1.1f)
            {
                startRoamingReward();
            }

        }

        if (state == RewardState.roaming) {
            if (Vector3.Distance(transform.position, target.position) < 1) {
                CreationEffectManager.instance.GenerationEffect(transform.localPosition, RoamingObjectType.Reward);
                audioplayer.PlayOneShot(RoamingObjectType.Reward.GetDestructionAudio(), 1);
                state = RewardState.aroundplayer;
                transform.parent = target;
                mot.startmotion = false;//startmovement = false;
                orb.startmotion = true;//startOrbit = true;
                orb.orbitalDistance = 6; 

            }
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0.5f, Time.deltaTime * 0.5f);
        }
        if (state == RewardState.aroundplayer) {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0.5f, Time.deltaTime * 0.5f);
            if (faceorbitdirection)
            {
                transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.down * 90);
            }
        }
    }

    /// <summary>
    /// stop the animation and start the free roaming behaviour
    /// </summary>
    public void startRoamingReward()
    {
        foreach (TrailRenderer r in transform.GetChild(0).GetChild(0).GetComponentsInChildren<TrailRenderer>())
        {
            r.enabled = true;
        }
        state = RewardState.roaming;
        foreach (MeshRenderer m in transform.GetChild(0).GetChild(0).GetComponentsInChildren<MeshRenderer>())
        {
            m.material.color = Color.white;
        }
        //mot = gameObject.AddComponent<RandomMotionInTheSpace>();
        transform.localScale = Vector3.one;
        /*mot.movingspeed = 0.2f;
        mot.rotatingspeed = 0.5f;
        mot.startmotion = true;*/
        mot.setTarget(target);
        mot.movingspeed = 0.2f;//speed = 0.2f;
        Debug.Log(gameObject.name + " " + mot.movingspeed);
        mot.startmotion = true;//startmovement = true;
        
        RewardItem i = RewardManager.instance.getPropertiesOfReward(gameObject.name.Split('(')[0]);
        rot.axis = (rotationaxis)System.Enum.Parse(typeof(rotationaxis), i.RotationAxis);
        //rot.rotationspeed = i.RotationSpeed;
        rot.rotatingspeed = i.RotationSpeed;
        rot.startmotion = true;
        audioplayer.PlayOneShot(RoamingObjectType.Reward.GetMovingAudio(), 1);
    }

    public void SetTargetPlayer(Transform target) {
        this.target = target;
    }
}

/// <summary>
/// state in which the reward may be
/// </summary>
enum RewardState { 
    animated, roaming, aroundplayer
}
