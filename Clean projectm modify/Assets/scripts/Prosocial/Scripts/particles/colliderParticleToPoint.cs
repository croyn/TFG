using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//not using
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

        //if the tag and layer of the object match
        if (other.tag == "objectiveTriangle" && other.layer==11)
        {
            //flow in which we control what number of particles is going to the system. But not using in the actual syncronus flow
            if (other.GetComponent<PointCentralMandala>().allowAbsorv) {
                //other.SendMessage("OnCollisionEnter");
                other.GetComponent<PointCentralMandala>().addParticle();
                
                
            }
           
        }
    }

    

}
