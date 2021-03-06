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

    public

    // Start is called before the first frame update
    void Start()
    {
        allowAbsorv = false;
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
    }

    public void addParticle() {

        if (allowAbsorv) { 
            num_particles_absorv = num_particles_absorv + 1;
            
            if (num_particles_absorv % numParticlesNextLine == 0 && num_particles_absorv<8400) {
                mandalamanager.instance.ActivatePointsTriangles();
            }
        }
    }

    

}
