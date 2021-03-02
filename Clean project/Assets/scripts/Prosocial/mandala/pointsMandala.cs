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
    public GameObject particles=null;
    // Start is called before the first frame update
    private int num_particles = 0;
    private ParticleSystem.Particle[] m_rParticlesArray = null;

    void Start()
    {
         lineaTemp = Instantiate(linea);
        LineRenderer temp = lineaTemp.GetComponent<LineRenderer>();
        temp.SetPosition(0, gameObject.transform.position);
        temp.SetPosition(1, gameObject.transform.position);
        Color tempColor=Color.gray;
        tempColor.a = 0.1f;
        temp.startColor = tempColor;
        temp.endColor = tempColor;
      
        allowAbsorv = false;
        //particles.transform.position=gameObject.transform.position;


    }

    // Update is called once per frame
    void Update()
    {


        assignarLinea();
        if (number_particles_catch > 5)
        {
            //make_line = true;
            doneAbsorv = true;
            allowAbsorv = false;
            gameObject.layer = 0;
        }

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
            line_true();

        
        

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
            //Debug.Log(punto);
            temp.SetPosition(1, punto);
            
            /*if (director.Equals(director)) {
                make_line = false;
            }*/

            //temp.transform.rotation = rotation;

        }
       
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("En bola" + other.name);
        Debug.Log("ENTRO BOLA");
        //Destroy(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("entro collision bola");
        
    }

    void OnParticleTrigger()
    {
        Debug.Log("entro trigger bola");
    }
   }
