using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOT USED ANYMORE IN THIS FLOW
public class zoneManagerMove : MonoBehaviour
{
    public ParticleSystem AffectedParticles = null;

    public ParticleSystem positionParticles = null;
    public GameObject cube = null;


    private float timeExplosion = 0.0f;
    public Gradient GradientColor;
    private bool animationPlaying = false;

    private bool explosion = false;
    private bool allowCollision = false;
    private float whenCollision = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.ColorOverLifetimeModule temp = AffectedParticles.colorOverLifetime;
        temp.color = GradientColor;

        ParticleSystem.ColorOverLifetimeModule temp2 = positionParticles.colorOverLifetime;
        temp2.color = GradientColor;
        timeExplosion = 0.0f;
        explosion = false;
        allowCollision = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (explosion)
        {

            positionParticles.Stop(true);
            //updateSpherePoint();
            if (animationPlaying)
            {
                timeExplosion = timeExplosion + Time.deltaTime;
                
            }


        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("entro collision");
        if (AffectedParticles != null)
        {
           // Debug.Log("entro collision");

            AffectedParticles.Play();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("entro collider");

        if (allowCollision)
        {
            whenCollision = Time.time;

            allowCollision = false;
            if (!explosion)
            {
                explosion = true;
                positionParticles.Stop(true);
                positionParticles.gameObject.SetActive(false);

                
                if (AffectedParticles != null && explosion && animationPlaying == false)
                {

                    AffectedParticles.Play();
                    //sphereObjective.GetComponent<pointsMandala>().line_true();
                
                    animationPlaying = true;
                }
            }

        }





    }

    private void OnTriggerStay(Collider other)
    {
        if (allowCollision)
        {
            whenCollision = Time.time;

            allowCollision = false;
            if (!explosion)
            {
                explosion = true;
                positionParticles.Stop(true);
                positionParticles.gameObject.SetActive(false);


                if (AffectedParticles != null && explosion && animationPlaying == false)
                {

                    AffectedParticles.Play();
                    //sphereObjective.GetComponent<pointsMandala>().line_true();

                    animationPlaying = true;
                }
            }

        }


    }

    public void actiaveCircle()
    {
        cube.SetActive(true);
        cube.GetComponent<MeshRenderer>().enabled = true;

    }

    public void deactiaveCircle()
    {
        cube.SetActive(true);
        cube.GetComponent<MeshRenderer>().enabled = false;
    }


    public void activateZone()
    {
        // gameObject.SetActive(true);
        explosion = false;
        positionParticles.gameObject.SetActive(true);
        positionParticles.Play();
        allowCollision = true;
        animationPlaying = false;


    }

    public void deactivateZone()
    {

        positionParticles.Stop();
        positionParticles.gameObject.SetActive(false);
        allowCollision = false;
        whenCollision = 0.0f;
        
    }
}
