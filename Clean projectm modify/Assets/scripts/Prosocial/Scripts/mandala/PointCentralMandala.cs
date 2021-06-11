using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCentralMandala : MonoBehaviour
{

    public bool allowAbsorv = false; //if the system allows absorv
    public int num_particles_absorv = 0; //number of particles absorv
    public int numParticlesNextLine = 60; //number of particles need for next line
    public float numActivationAvailable; //number of activation Available
    public float numNotCatched; //number of activation doesnt activate it
    float timeOff = 0.0f; //time beetween calls

    // Start is called before the first frame update
    void Start()
    {
        allowAbsorv = false;
        numActivationAvailable = 0.0f;
        
        timeOff = 0.0f;
        numNotCatched = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //controltime
        timeOff = timeOff+ Time.deltaTime;

        chargeMandala();




    }


    private void chargeMandala() {
        if (numActivationAvailable >= 1.0f && timeOff > 0.1f)//if we have a number availabe >1 
        {
#if UNITY_EDITOR
            Debug.Log("Central numActive " + numActivationAvailable);
#endif
            //substract 1
            numActivationAvailable = numActivationAvailable - 1.0f;

            //check if the part of triangles in mandala is done
            if (!mandalamanager.instance.trianglesDone)
            {
                //activate a Point in the mandala to send a line in the triangle part
                if (mandalamanager.instance.ActivatePointsTriangles())
                {
#if UNITY_EDITOR
                    Debug.Log("Lanzo linea");
#endif
                }
                //reboot the time
                timeOff = 0.0f;
            }
            else if (mandalamanager.instance.trianglesDone && !mandalamanager.instance.circleDone) //if the triangle part is done but the circle doesnt
            {
                mandalamanager.instance.ActivatePointsCircle(); //activate a Point in tne mandala to send in the circle part
                timeOff = 0.0f;//reboot the time
            }


        }
    }


    public void addParticle() {

        if (allowAbsorv) {


            //the syncronous system doestn use this . Its all control by the timerZone
            /*num_particles_absorv = num_particles_absorv + 1;
            
            if (num_particles_absorv % numParticlesNextLine == 0) {
                if (!mandalamanager.instance.trianglesDone)
                {
                    mandalamanager.instance.ActivatePointsTriangles();
                }
                else if(mandalamanager.instance.trianglesDone && !mandalamanager.instance.circleDone ){
                    mandalamanager.instance.ActivatePointsCircle();
                }
                
            }*/

            




        }
    }

    

}
