using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionMovingZone : MonoBehaviour
{
    public bool allowCollision=false;
    public ParticleSystem affectectParticles2;
    // Start is called before the first frame update
    void Start()
    {
        allowCollision = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    


    private void OnTriggerStay(Collider other)
    {
        Debug.Log(" COLLISION BOLA " + other.name);
        if (allowCollision)
        {

            affectectParticles2.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (allowCollision)
        {

            affectectParticles2.Stop();
        }
    }


}
