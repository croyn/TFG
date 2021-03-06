﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointsMandala : MonoBehaviour
{

    public GameObject partnerPoint = null;
    public Color ColorLine1;
    public GameObject partnerPoint1 = null;
    public Color ColorLine2 ;
    public GameObject partnerPoint2 = null;
    public Color ColorLine3;

    public GameObject actualPartnerPoint=null;

    private int wichLayer = 0;
    public bool make_line = false;
    private float opacityLine = 0.2f; // no se si puedo hacer eso en linea
    public GameObject linea = null;
    public int number_particles_catch=0;
    public bool doneAbsorv = false;
    public bool allowAbsorv = false;
    private GameObject lineaTemp;
    public GameObject particles=null;
    // Start is called before the first frame update
    private int num_particles = 0;
    private ParticleSystem.Particle[] m_rParticlesArray = null;
    public float timeFirstLine = 2.0f;
    private float firstLinesControl = 2.0f;
    public bool firstLineDraw = false;

    void Start()
    {
        wichLayer = 0;
        lineaTemp = Instantiate(linea);
        LineRenderer temp = lineaTemp.GetComponent<LineRenderer>();
        temp.SetPosition(0, gameObject.transform.position);
        temp.SetPosition(1, gameObject.transform.position);
        Color tempColor=Color.gray;
        tempColor.a = 0.2f;
        temp.startColor = tempColor;
        temp.endColor = tempColor;
      
        allowAbsorv = false;
        //particles.transform.position=gameObject.transform.position;
        make_line = true;
        firstLineDraw = false;
        givePartner();



    }


    public void nextLayer() {

        wichLayer = wichLayer + 1;
        givePartner();
        lineaTemp = Instantiate(linea);
        LineRenderer temp = lineaTemp.GetComponent<LineRenderer>();
        temp.SetPosition(0, gameObject.transform.position);
        temp.SetPosition(1, gameObject.transform.position);
        Color tempColor = Color.gray;
        tempColor.a = 0.2f;
        temp.startColor = tempColor;
        temp.endColor = tempColor;

        allowAbsorv = false;
        //particles.transform.position=gameObject.transform.position;
        make_line = true;
        firstLineDraw = false;
        firstLinesControl = timeFirstLine;

    }

     Color whatColor() {
        switch (wichLayer)
        {
            case 0:
                return ColorLine1;
                break;
            case 1:
                return ColorLine2;
                break;
            case 2:
                return ColorLine3;
                break;
            default:
                return Color.gray;
                break;

        }


    }

    // Update is called once per frame
    void Update()
    {
        firstLinesControl = firstLinesControl - Time.deltaTime;
        if (firstLineDraw == false) {
            assignarLinea();
        }
        if (firstLinesControl <= 0.0f && firstLineDraw == false) {
            
            firstLineDraw = true;
            make_line = false;
            lineaTemp = Instantiate(linea);
            LineRenderer temp = lineaTemp.GetComponent<LineRenderer>();
            temp.SetPosition(0, gameObject.transform.position);
            temp.SetPosition(1, gameObject.transform.position);
            Color tempColor = whatColor();
            tempColor.a = 0.6f;
            temp.startColor = tempColor;
            temp.endColor = tempColor;
        }


        if (firstLineDraw) {
            assignarLinea();
            if (number_particles_catch > 5)
            {
                //make_line = true;
                doneAbsorv = true;
                allowAbsorv = false;
                gameObject.layer = 0;
            }
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



    public void givePartner() {

       
        switch (wichLayer) {
            case 0:
                actualPartnerPoint = partnerPoint;
                break;
            case 1:
                actualPartnerPoint = partnerPoint1;
                break;
            case 2:
                actualPartnerPoint = partnerPoint2;
                break;
            default:
                actualPartnerPoint = null;
                break;

        }

        
    }

    void assignarLinea() {
        givePartner();
        if (actualPartnerPoint != null && make_line  ) {
         
            lineaTemp.SetActive(true);


            LineRenderer temp = lineaTemp.GetComponent<LineRenderer>();
            temp.SetPosition(0, gameObject.transform.position);

            Vector3 punto = Vector3.Lerp( temp.GetPosition(1), actualPartnerPoint.transform.position, 0.05f);

            temp.SetPosition(1, punto);

        }
       
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("En bola" + other.name);
        //Debug.Log("ENTRO BOLA");
        //Destroy(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("entro collision bola");
        
    }

    void OnParticleTrigger()
    {
       // Debug.Log("entro trigger bola");
    }
   }
