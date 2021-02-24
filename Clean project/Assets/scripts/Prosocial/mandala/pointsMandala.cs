using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointsMandala : MonoBehaviour
{

    public GameObject partnerPoint = null;
    public bool make_line = false;
    private float opacityLine = 0.0f; // no se si puedo hacer eso en linea
    public GameObject linea = null;
    public int number_particles_catch=0;
    public bool doneAbsorv = false;
    public bool allowAbsorv = false;
    private GameObject lineaTemp;
    // Start is called before the first frame update
    void Start()
    {
         lineaTemp = Instantiate(linea);
        LineRenderer temp = lineaTemp.GetComponent<LineRenderer>();
        temp.SetPosition(0, gameObject.transform.position);
        temp.SetPosition(1, gameObject.transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {


        assignarLinea();

    }


    public void line_true()
    {
        make_line = true;
    }

    public bool done() {
        return doneAbsorv;
    }

    public void add_count_particle()
    {
        
            number_particles_catch = number_particles_catch + 1;
           
             
             if (number_particles_catch > 60)
             {
                  //make_line = true;
                 doneAbsorv = true;
                allowAbsorv = false;
             }


        //Debug.Log("Numero de particulas cogidas " + number_particles_catch);
    }


    void assignarLinea() {
        if (partnerPoint != null && make_line) {
            //linea.SetActive(true);
            lineaTemp.SetActive(true);


            LineRenderer temp = lineaTemp.GetComponent<LineRenderer>();
            temp.SetPosition(0, gameObject.transform.position);
            //linea.transform.position = gameObject.transform.position;

            
            //Vector3 director = partnerPoint.transform.position- gameObject.transform.position;
            //Vector3 director =   gameObject.transform.position - partnerPoint.transform.position;
            Vector3 punto = Vector3.Lerp( temp.GetPosition(1), partnerPoint.transform.position, 0.1f);


            /*Debug.Log("Partner " + partnerPoint.transform.position);
            Debug.Log("gameObject " + gameObject.transform.position);
            Debug.Log("Director " + director);*/
            Debug.Log(punto);
            temp.SetPosition(1, punto);
            
            /*if (director.Equals(director)) {
                make_line = false;
            }*/

            //temp.transform.rotation = rotation;

        }
       
    }

}
