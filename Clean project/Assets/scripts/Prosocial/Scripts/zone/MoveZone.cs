using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveZone : MonoBehaviour
{

    public GameObject initPoint;
    public GameObject finalPoint;
    public ParticleSystem affectectParticles;
    public ParticleSystem affectectParticles2;
    private bool inPosition = false;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        moveParticlesSystem();
    }


    public void initMoveZone() {

        affectectParticles.gameObject.transform.position = initPoint.gameObject.transform.position;
        affectectParticles2.gameObject.transform.position = initPoint.gameObject.transform.position;
        affectectParticles.Play();
        affectectParticles2.Play();
        inPosition = false;
    }


    public void moveParticlesSystem() {

        if (affectectParticles != null && finalPoint != null && affectectParticles2 != null) {
            float dist = Vector3.Distance(affectectParticles.gameObject.transform.position, finalPoint.gameObject.transform.position);
            affectectParticles.gameObject.transform.position = Vector3.Lerp(affectectParticles.gameObject.transform.position, finalPoint.gameObject.transform.position, 3.0f*(Time.deltaTime/dist));
            affectectParticles2.gameObject.transform.position = Vector3.Lerp(affectectParticles2.gameObject.transform.position, finalPoint.gameObject.transform.position, 3.0f * (Time.deltaTime / dist));
            if (dist <= 0.05f) {
                inPosition = true;
            }
        }

       

    }

    public void deactiveMoveZone() {
        affectectParticles.Stop();
        affectectParticles2.Stop();
        inPosition = false;

    }

    public bool isInPosition()
    {
        return inPosition;
    }

}
