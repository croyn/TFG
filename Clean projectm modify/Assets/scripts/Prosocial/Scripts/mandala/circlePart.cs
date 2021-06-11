using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circlePart : MonoBehaviour
{

    public List<GameObject> circlePoints = new List<GameObject>(); //list of point that are in the circle part
    public int indiceActualPoint = 0;//what point is looking 
    private bool started = false; //if the setup is done
    public int numLineasCapa1=0; //number of lines in the layer
    // Start is called before the first frame update
    void Start()
    {
        started = false;
        indiceActualPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if it is not started
        if (!started)
        {
            //now it is
            started = true;
            //make algorithm to setup the partens to make the line
            FillConectionsPoints();
        }
    }


    //function that allows to set visible the points of the circle part and enable the movement
    public void activatePoints() {
        for (int i = 0; i < circlePoints.Count; i++) {
           // circlePoints[i].SetActive(true);
            circlePoints[i].GetComponent<MeshRenderer>().enabled = true;
            circlePoints[i].GetComponent<pointsMandala>().moving = true;
        }

    }

    //function that allow to set invi the points of the circle part
    public void deactivatePoints() {
        for (int i = 0; i < circlePoints.Count; i++)
        {
            //circlePoints[i].SetActive(false);
            circlePoints[i].GetComponent<MeshRenderer>().enabled = false;

        }

    }

    //function that returns if the points of the circle part are moving
    public bool checkPointsInposition()
    {
        //init
        bool resp = true;
        //for every point on the circle part
        for (int i = 0; i < circlePoints.Count; i++)
        {
            //check if moving
            resp = resp && !circlePoints[i].GetComponent<pointsMandala>().moving;

        }

       
        //final 
        return resp;
    }

    //function that active a point to sent a line to the partner and return a gameobject if it can or return if the circle part is done
    public GameObject activatePointFromCircle(bool activateLine = true) {
        //acces to the actual point
        GameObject actualPoint = circlePoints[indiceActualPoint].gameObject;
        //giver partner
        actualPoint.GetComponent<pointsMandala>().givePartner();
        //check if null to pass to the next
        while (actualPoint.GetComponent<pointsMandala>().actualPartnerPoint == null)
        {
            //next
            indiceActualPoint = indiceActualPoint + 1;
            //if we can
            if (indiceActualPoint <= circlePoints.Count - 1)
            {
                //this is the actual point
                actualPoint = circlePoints[indiceActualPoint].gameObject;
                
            }
            else {
                //if not is done
                return null;
            }

            
        }

        //if actual point is null or is the line is done
        if (actualPoint != null && actualPoint.GetComponent<pointsMandala>().make_line)
        {
            //next
            indiceActualPoint = indiceActualPoint + 1;
            //if i can
            if (indiceActualPoint <= circlePoints.Count - 1)
            {
                //is this
                actualPoint = circlePoints[indiceActualPoint].gameObject;
                
            }
            else {
                //circle part done
                return null;
            }
        }
        //if true active line
        if (activateLine)
        {
            //active line
            actualPoint.GetComponent<pointsMandala>().line_true();
        }

        //this is the point
        return circlePoints[indiceActualPoint].gameObject;
    }

    //function that make the algorithm to asociate points to make lines
    private void FillConectionsPoints()
    {
        //for the 1/2 of the points
        for (int i = 0; i <= circlePoints.Count/2; i++)
        {
            //count a line
            numLineasCapa1 = numLineasCapa1 + 1;
            //asociate the immediate next point in the other 1/2
            circlePoints[i].GetComponent<pointsMandala>().partnerPoint = circlePoints[i+ (circlePoints.Count / 2)].gameObject;

        }
    }

 }
