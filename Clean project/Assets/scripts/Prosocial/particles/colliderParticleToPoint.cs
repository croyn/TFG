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


    void OnParticleCollision(GameObject other)
    {
        // Debug.Log("COLISION PARTICLER in " + other.name);


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
                other.GetComponent<pointsMandala>().add_count_particle();
            }
           
        }
    }

}
