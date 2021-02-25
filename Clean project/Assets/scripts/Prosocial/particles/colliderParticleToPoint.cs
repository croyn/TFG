using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderParticleToPoint : MonoBehaviour
{
    
    // Start is called before the first frame update
    
    void Start()
    {
        
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> outside = new List<ParticleSystem.Particle>();
    void OnParticleTrigger()
    {
        //Debug.Log("TRIGGER PARTICLE");
        // get the particles which matched the trigger conditions this frame
        int numEnter = gameObject.GetComponent<ParticleSystem>().GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        int numExit = gameObject.GetComponent<ParticleSystem>().GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
        int numInside = gameObject.GetComponent<ParticleSystem>().GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        int numOutside = gameObject.GetComponent<ParticleSystem>().GetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);

        if (numEnter > 0) {
            Debug.Log("TRIGGER enter");
        }
        if (numExit > 0)
        {
            Debug.Log("TRIGGER exit");
        }
        if (numInside > 0)
        {
            Debug.Log("TRIGGER inside");
        }
        if (numOutside > 0)
        {
            Debug.Log("TRIGGER outside");
        }

        // iterate through the particles which entered the trigger and make them red
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            p.startColor = new Color32(0, 255, 0, 255);
            enter[i] = p;
           
        }

        for (int i = 0; i < numInside; i++)
        {
            ParticleSystem.Particle p = inside[i];
            p.startColor = new Color32(0, 255, 0, 255);
            inside[i] = p;
            
           
        }

        for (int i = 0; i < numOutside; i++)
        {
            ParticleSystem.Particle p = outside[i];
            p.startColor = new Color32(0, 255, 0, 255);
            outside[i] = p;
            

        }
        // iterate through the particles which exited the trigger and make them green
        for (int i = 0; i < numExit; i++)
        {
            ParticleSystem.Particle p = exit[i];
            p.startColor = new Color32(0, 255, 0, 255);
            exit[i] = p;
            
        }

        // re-assign the modified particles back into the particle system
        gameObject.GetComponent<ParticleSystem>().SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        gameObject.GetComponent<ParticleSystem>().SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
        gameObject.GetComponent<ParticleSystem>().SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        gameObject.GetComponent<ParticleSystem>().SetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);
    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.Log("COLISION PARTICLER in " + other.name);


        /*if (other.name == "Objective") {
            other.GetComponent<pointsMandala>().add_count_particle();
        }
        if (other.name == "Objective2")
        {
            other.GetComponent<pointsMandala>().add_count_particle();
        }
        if (other.name == "Objective3")
        {
            other.GetComponent<pointsMandala>().add_count_particle();
        }*/

        if (other.tag == "objectiveTriangle")
        {
            if (other.GetComponent<pointsMandala>().allowAbsorv) {
                //other.SendMessage("OnCollisionEnter");
                other.GetComponent<pointsMandala>().add_count_particle();
                
                
            }
           
        }
    }

    

}
