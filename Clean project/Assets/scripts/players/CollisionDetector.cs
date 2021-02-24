using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CollisionDetector : MonoBehaviour
{
    /// <summary>
    /// state of the player
    /// </summary>
    PlayerStateManager player;

    /// <summary>
    /// set the instance of the associated player state
    /// </summary>
    void Start() {
        player = GetComponent<PlayerStateManager>();
    }

    /// <summary>
    /// Event activated when something enter the trigger area
    /// </summary>
    /// <param name="other">the other object</param>
    void OnTriggerEnter(Collider other) {
        if (other.tag == "ParticlePool") {
            /*if (player.PlayerCanBurstParticles())
            {*/
            other.GetComponent<ParticlePoolManager>().StartActing(gameObject.tag);
            IA.instance.updateWhiteHolePlayerState(gameObject.tag, whiteHoleState.ignorant, whiteHoleState.aware, other.GetComponent<ParticlePoolManager>().type);
                //player.PlayerUsedAPresent(ParticleType.All);
            /*}
            else {
                if (other.GetComponent<ParticlePoolManager>().BurstEffect(player.getPresentTypes()))
                {
                    player.PlayerUsedAPresent(ParticleType.All);
                }
            }*/
        }
    }

    /// <summary>
    /// Event activated when something exit the trigger area
    /// </summary>
    /// <param name="other">the other object</param>
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "ParticlePool")
        {
            other.GetComponent<ParticlePoolManager>().StopParticles();
            IA.instance.exitingWhiteHoles();
        }
    }

    /// <summary>
    /// Event activated when something collide with the player
    /// </summary>
    /// <param name="other">the other object</param>
    void OnCollisionEnter(Collision collision) {
        GameObject other = collision.gameObject;
        if (other.tag == "Particle")
        {
            if (other.GetComponent<ParticleBehaviour>().creator.name != gameObject.name && !other.GetComponent<OrbitBehaviour>().startmotion)//.startOrbit)
            {
                if (other.GetComponent<ParticleBehaviour>().creator.tag != "ParticlePool")
                {
                    ObjectGeneratorManager.instance.ClearPool(other.GetComponent<ParticleBehaviour>().type);
                }
                player.PlayerReveidedAPresent(other.GetComponent<ParticleBehaviour>().type);
                StartCoroutine(setParticleActivable(other.GetComponent<ParticleBehaviour>()));
                other.GetComponent<ParticleBehaviour>().attachedplayer = gameObject;
                StartCoroutine("waittoShiftMovementType", other);
            }
        }
    }

    IEnumerator setParticleActivable(ParticleBehaviour b) {
        yield return new WaitForSeconds(0.5f);
        b.readyToJoin = true;
    }

    /// <summary>
    /// change the motion pattern of the collided element from linear motion to orbit
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    IEnumerator waittoShiftMovementType(GameObject other) {

        other.GetComponent<LinearMotionBehaviour>().startmotion = false;//.startmovement = false;
        yield return new WaitForSeconds(0.1f);
        other.layer = LayerMask.NameToLayer("particles");
        other.transform.parent = transform;
        other.GetComponent<OrbitBehaviour>().startmotion = true;//startOrbit = true;
        other.GetComponent<OrbitBehaviour>().rotatingspeed = 200;
        
    }
}
