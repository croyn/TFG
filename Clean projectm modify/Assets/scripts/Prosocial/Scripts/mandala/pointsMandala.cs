using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointsMandala : MonoBehaviour
{

    public GameObject partnerPoint = null; //what point is the objective for the first line
    public Color ColorLine1;//color first line
    public GameObject partnerPoint1 = null;//what point is the objective for the second line
    public Color ColorLine2;//color second line
    public GameObject partnerPoint2 = null;//what point is the objective for the thrid line
    public Color ColorLine3;//color  third line
    public GameObject actualPartnerPoint=null; //what is the actual objective for the line
    private int wichLayer = 0; //which layer is the point
    public bool make_line = false; //control if is allow make_line
    public GameObject linea = null;//reference to a base line object on scene
    public int number_particles_catch=0;//not used. Before every particle go to a every point not only to middle.
    public bool doneAbsorv = false;//indicate if is done absorving particles
    public bool allowAbsorv = false;//allow collision
    private GameObject lineaTemp;//temp line use it
    public float timeFirstLine = 2.0f; //control when its stat first line
    private float firstLinesControl = 5.0f; //control when stop
    public bool firstLineDraw = false; //control if it is done
    public bool retriveColors = false;//control if we have the color configuration
    public Vector3 positionFinal;//final position 
    public Vector3 positionRandom;//random position to appear at first
    public int controlFase = 0; //what fase the point is
    public bool moving = false; //control if it is moving
    public float velocity; //velocity of move the point

    // Start is called before the first frame update
    void Start()
    {
        wichLayer = 0;//setup layer 0

        // if we have a base line
        if (linea != null) {
            //make a instantiation 
        lineaTemp = Instantiate(linea);
            //acces to the lineRenderer
        LineRenderer temp = lineaTemp.GetComponent<LineRenderer>();
            //change first and final position to the point
        temp.SetPosition(0, gameObject.transform.position);
        temp.SetPosition(1, gameObject.transform.position);
            //change to gray
        Color tempColor=Color.gray;
            //opacity of the line
        tempColor.a = 0.2f;
            //color at the start and the end of the line
        temp.startColor = tempColor;
        temp.endColor = tempColor;
        }
        
        //the color are not setup
        retriveColors = false;
        //not allow absorv
        allowAbsorv = false;
        //make_line true to make the first one in gray
        make_line = true;
        //the first line its is not done yet
        firstLineDraw = false;
        //setup the partner
        givePartner();
        //what fase is the point
        controlFase=0;
        //setup variables
        positionFinal = gameObject.transform.position;
        positionRandom = gameObject.transform.position;
        //not moving
        moving = false;
    }

    //function that setup the random initial position of the point
    void givePositionRandom() {
        //get random new positon
        positionRandom = new Vector3(Random.Range(-20.0f + positionRandom.x, 20.0f + positionRandom.x), 0, Random.Range(-20.0f + positionRandom.z, 20.0f + positionRandom.z));
        //setup on the object
        gameObject.transform.position = positionRandom;
        //not moving
        moving = false;
        //what fase
        controlFase = 0;
    }


    //function to setup all the colors according to the mandalascript configuration on the mandalamanager object
    void giveColors() {
        //if we are in a triangle
        if (gameObject.GetComponentInParent<Triangle>() != null) {
            //get acces to the type of triangle
        int triangleType = gameObject.GetComponentInParent<Triangle>().typeTriangle;
            //acces the colors
        ColorLine1 = mandalamanager.instance.colorSolid1;
        ColorLine2 = mandalamanager.instance.colorSolid2;

            //if the triange type is different to 2 
        if (triangleType != 2)
        {

            ColorLine3 = mandalamanager.instance.colorSolid3;
        }
        else {
            ColorLine3 = mandalamanager.instance.colorSolid2;
        }
        }
    }


    //function that make the flow to switch layer on a point
    public void nextLayer() {

        //change the layer
        wichLayer = wichLayer + 1;

        //acces the partner
        givePartner();

        //if there is a reference
        if (linea != null) {
            //if there is a previous line
            if (lineaTemp != null)
            {
                //associate to the mandala to the mechanic of go mandala bigger
                lineaTemp.transform.parent = mandalamanager.instance.transform;

            }
            //instanciate a new line
            lineaTemp = Instantiate(linea);
            //set the line at the same position as the point
            LineRenderer temp = lineaTemp.GetComponent<LineRenderer>();
            temp.SetPosition(0, gameObject.transform.position);
            temp.SetPosition(1, gameObject.transform.position);
            //set color to gray
            Color tempColor = Color.gray;
            tempColor.a = 0.2f;
            //color at the start and end of the line
            temp.startColor = tempColor;
            temp.endColor = tempColor;
            //doesnt allow to absorv
            allowAbsorv = false;
            //make the line
            make_line = true;
            //first line in gray is not done
            firstLineDraw = false;
            //control
            firstLinesControl = timeFirstLine;
        }
        

       

    }

    //what color is the line according to the point layer
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

    //function that move the point beetween the random initial position and the final real position 
    void moveSphere() {

        //calc the distance
        float dist = Vector3.Distance(positionRandom, positionFinal);
        //a calc over velocity and dist to move correctly
        positionRandom = Vector3.Lerp(positionRandom, positionFinal, velocity * (Time.deltaTime / dist));
        //change the position of the point to the newone
        gameObject.transform.position = positionRandom;
        //if the point is realy near the final position
        if (dist <= 0.05f)
        {
            //change the point to the final position
            gameObject.transform.position = positionFinal;
            //the point is not moving anymore
            moving = false;
            //change the fase to line
            controlFase = 1;
            
        }


    }



    // Update is called once per frame
    void Update()
    {
        //to setup the first time the color. Its need to all the object be instantiate so it not work on start
        if (!retriveColors) {
            //get the configuration color of each layer
            giveColors();
            //done
            retriveColors = true;
            //setup the random positon
            givePositionRandom();
        }

        //if it is moving and we are in fase moving
        if (moving && controlFase==0) {
            //move the point
            moveSphere();
        }else if (controlFase == 1) //if we are in fase sent lines
        { 
            //control the time of a line going
        firstLinesControl = firstLinesControl - Time.deltaTime;
        if (firstLineDraw == false) {
                // make the line go
            assignarLinea();
        }
        if (firstLinesControl <= 0.0f && firstLineDraw == false) {//if the first line is not draw
                
                //if we have acces to the line
           if (linea != null)
            {
                    //the first line now it is setted
            firstLineDraw = true;
                    //not allow to send a line
            make_line = false;
                    //instantiate a new one
            lineaTemp = Instantiate(linea);
                    //accces to the line render
            LineRenderer temp = lineaTemp.GetComponent<LineRenderer>();
                    //change the start and the end position to the same as the point
            temp.SetPosition(0, gameObject.transform.position);
            temp.SetPosition(1, gameObject.transform.position);
                    //give the color we need in this layer
            Color tempColor = whatColor();
                    //change opacity to 0.9
            tempColor.a = 0.9f;
                    //change the color at the start and end of the line
            temp.startColor = tempColor;
            temp.endColor = tempColor;
            }
         }

        //if the first line up
        if (firstLineDraw) {
                //assignate the line
            assignarLinea();
                //another flow its not used but control the number of particles absobsrv
            if (number_particles_catch > 5)
            {
                //make_line = true;
                doneAbsorv = true;
                allowAbsorv = false;
                gameObject.layer = 0;
            }
        }

        }
    }

    //says if the line is going
    public void line_true()
    {
        make_line = true;
    }

    //says if it is fill up with particles . Not used in the actual flow
    public bool done() {
        return doneAbsorv;
    }

    //not used. Add particles to the point
    public void add_count_particle()
    {
        
            number_particles_catch = number_particles_catch + 1;
            line_true();

        
        

        //Debug.Log("Numero de particulas cogidas " + number_particles_catch);
    }


    //setup the actual partner according to the point layer
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



    //make the line go to the objective point
    void assignarLinea() {
        givePartner();//setup what is the objective
        //if can acces and have permision to make a line
        if (actualPartnerPoint != null && make_line  ) {
            //enable line
            lineaTemp.SetActive(true);

            //acces to the linerenderer
            LineRenderer temp = lineaTemp.GetComponent<LineRenderer>();
            //set the initial point
            temp.SetPosition(0, gameObject.transform.position);
            //calc the temp final point
            Vector3 punto = Vector3.Lerp( temp.GetPosition(1), actualPartnerPoint.transform.position, 0.15f);
            //set the final of the line
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
