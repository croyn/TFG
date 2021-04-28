using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionMovingZone : MonoBehaviour
{
    public bool allowCollision=false;
    public ParticleSystem affectectParticles;
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

    private void forceOn() {
        ParticleSystem.MainModule tempMainModule = affectectParticles.gameObject.GetComponent<ParticleSystem>().main;
        tempMainModule.startSpeed = 5.0f;
        tempMainModule.startLifetime = 2.5f;
        tempMainModule.maxParticles = 10000;
        ParticleSystem.EmissionModule tempEmisionModule = affectectParticles.gameObject.GetComponent<ParticleSystem>().emission;
        tempEmisionModule.rateOverTime = 800;
 
        affectectParticles.Play();

        ParticleSystem.ColorOverLifetimeModule temp = affectectParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
        temp.color = gameObject.transform.parent.GetComponent<MoveZone>().colorTouch;
        affectectParticles.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(" COLLISION BOLA " + other.name);
        if (allowCollision)
        {

            forceOn();
            Logger.addTouchingMoving(gameObject.name, other.name);

        }

    }



    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(" COLLISION BOLA " + other.name);
        if (allowCollision)
        {
            forceOn();
        }

    }


    public void ForceOff() {
        ParticleSystem.MainModule tempMainModule = affectectParticles.gameObject.GetComponent<ParticleSystem>().main;
        tempMainModule.startSpeed = 0.3f;
        tempMainModule.startLifetime = 1.0f;
        tempMainModule.maxParticles = 10000;
        ParticleSystem.EmissionModule tempEmisionModule = affectectParticles.gameObject.GetComponent<ParticleSystem>().emission;
        tempEmisionModule.rateOverTime = 200;

        ParticleSystem.ColorOverLifetimeModule temp = affectectParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
        temp.color = gameObject.transform.parent.GetComponent<MoveZone>().colorNoTouch;

        affectectParticles.Play();
    }

    private void OnTriggerExit(Collider other)
    {

        if (allowCollision)
        {

            ForceOff();
            Logger.addNotTouchingMoving(gameObject.name, other.name);
        }
    }


}
