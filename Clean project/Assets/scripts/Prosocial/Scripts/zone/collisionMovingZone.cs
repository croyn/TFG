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

    


    private void OnTriggerStay(Collider other)
    {
        Debug.Log(" COLLISION BOLA " + other.name);
        if (allowCollision)
        {

            //affectectParticles2.Play();

           ParticleSystem.MainModule tempMainModule = affectectParticles.gameObject.GetComponent<ParticleSystem>().main;
            tempMainModule.startSpeed = 5.0f;
            tempMainModule.startLifetime = 2.5f;
            tempMainModule.maxParticles = 10000;
            ParticleSystem.EmissionModule tempEmisionModule = affectectParticles.gameObject.GetComponent<ParticleSystem>().emission;
            tempEmisionModule.rateOverTime = 800;
            //affectectParticles.Stop();
            affectectParticles.Play();
            /*ParticleSystem.MainModule tempMainModule2 = affectectParticles2.gameObject.GetComponent<ParticleSystem>().main;
            tempMainModule2.startSpeed = 5.0f;
            tempMainModule2.startLifetime = 2.5f;
            tempMainModule2.maxParticles = 10000;
            ParticleSystem.EmissionModule tempEmisionModule2 = affectectParticles2.gameObject.GetComponent<ParticleSystem>().emission;
            tempEmisionModule2.rateOverTime = 800;
            //affectectParticles.Stop();*/
            affectectParticles.Play();

            //affectectParticles2.Play();
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (allowCollision)
        {

            //affectectParticles2.Stop();
            ParticleSystem.MainModule tempMainModule = affectectParticles.gameObject.GetComponent<ParticleSystem>().main;
            tempMainModule.startSpeed = 0.3f;
            tempMainModule.startLifetime = 1.0f;
            tempMainModule.maxParticles = 10000;
            ParticleSystem.EmissionModule tempEmisionModule=affectectParticles.gameObject.GetComponent<ParticleSystem>().emission;
            tempEmisionModule.rateOverTime = 200;
            //affectectParticles.Stop();

           /* ParticleSystem.MainModule tempMainModule2 = affectectParticles2.gameObject.GetComponent<ParticleSystem>().main;
            tempMainModule2.startSpeed = 0.3f;
            tempMainModule2.startLifetime = 1.0f;
            tempMainModule2.maxParticles = 10000;
            ParticleSystem.EmissionModule tempEmisionModule2 = affectectParticles2.gameObject.GetComponent<ParticleSystem>().emission;
            tempEmisionModule2.rateOverTime = 200;*/

            affectectParticles.Play();
            //affectectParticles2.Play();
        }
    }


}
