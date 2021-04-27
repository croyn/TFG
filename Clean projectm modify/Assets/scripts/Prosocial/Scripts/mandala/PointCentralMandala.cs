using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCentralMandala : MonoBehaviour
{

    public bool allowAbsorv = false;
    public int num_particles_absorv = 0;
    public float timeNextLine=0.0f;
    public float maxNextLine = 1.0f;
    public int numParticlesNextLine = 60;
    public float numActivationAvailable;
    public float numNotCatched;
    float timeOff = 0.0f;

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
        /*timeNextLine = timeNextLine+ Time.deltaTime;

        if (timeNextLine >= maxNextLine) {
           
            timeNextLine = 0.0f;
        }*/
        if (Input.GetKey(KeyCode.Space))
        {
            mandalamanager.instance.ActivatePointsTriangles();
        }
        timeOff = timeOff+ Time.deltaTime;

        if (numActivationAvailable >=1.0f && timeOff > 0.01f)//&&
        {
            Debug.Log("Central numActive " + numActivationAvailable);
            numActivationAvailable = numActivationAvailable - 1.0f;
            if (!mandalamanager.instance.trianglesDone)
            {

                if (mandalamanager.instance.ActivatePointsTriangles()) {
                    Debug.Log("Lanzo linea");
                }
                timeOff = 0.0f;
            }
            else if (mandalamanager.instance.trianglesDone && !mandalamanager.instance.circleDone)
            {
                mandalamanager.instance.ActivatePointsCircle();
                timeOff = 0.0f;
            }
            
                
        }
       /* if (numActivationAvailable == 0.0f)
        {
            allowLine = false;
        }*/
    }

    public void addParticle() {

        if (allowAbsorv) {
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
