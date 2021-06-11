using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mandalamanager : MonoBehaviour
{
    public static mandalamanager instance; //singleton instance
    public List<GameObject> triangleList = new List<GameObject>(); //a list with the gameobject triangle that are in the mandala
    public List<GameObject> circleList = new List<GameObject>(); //a list with the gameobject circle part that are in the mandala
    private int indiceActualPoint=0; //its need for knowing in what object of the list of the the mandala during the flow
    private int indiceTriangle = 0; //its need for knowing in what trianlge is the mandala during the flow
    public GameObject centralPoint = null; //reference to the gameobject that have the PointCentralMandala Script
    public int numTrianglesLayer=8; //Number of triangles for every layer
    public int layer = 0; //its need for knowing in what layer is the mandala during the flow
    public int interLayer = 0; //its need for knowing in what interlayer is the mandala during the flow 
    public Gradient color1;//color first layer particles
    public Gradient color1Move;//color to move zone particles configuration first layer
    public Gradient color2;//color second layer particles
    public Gradient color2Move;//color to move zone particles configuration second layer
    public Gradient color3;//color third layer particles
    public Gradient color3Move;//color to move zone particles configuration second layer
    public Color colorSolid1;//color lines first layer
    public Color colorSolid2;//color lines second layer
    public Color colorSolid3;//color lines third layer
    private Gradient actualColor = null;//actual gradient color using
    private bool controlTriangleVoid=true;//boolean to control if a triangle is empty and force the next triangle that is not empty
    private bool redirectFlow = false; //a boolean to cheat during the flow. Its needed to have the exact configuration that we need
    public bool trianglesDone = false; //boolean that says if the triangle part is done
    public bool circleDone = false; //boolean that says if the circle part is done

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //sphereObjectivelist = new List<GameObject>();
        indiceActualPoint = 0;
        indiceTriangle = 0;
        layer = 0;
        interLayer = 0;
        controlTriangleVoid = true;
        redirectFlow = false;
        trianglesDone = false;
        circleDone = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //give the number of lines that a layer have according to a base triangle or circle in that layer.
    public float actulgetNumberLines() {
        int num = 0;
        //calculate the actual index in the triangle list in that moment of the flow
        int actualIndexTriangle = interLayer * numTrianglesLayer;
        //acces to a triangle in that layer
        GameObject actualTriangle = triangleList[actualIndexTriangle].gameObject;

        //if the triangle part is not done
        if (!trianglesDone)
        {
            //se what layer to see the variable from the triangle script
            switch (layer)
            {
                case 0:
                    //number of lines set with a line in the layer one in this triangle
                    num = actualTriangle.GetComponent<Triangle>().numLineasCapa1;
                    break;
                case 1:
                    //number of lines set with a line in the layer two in this triangle
                    num = actualTriangle.GetComponent<Triangle>().numLineasCapa2;
                    break;
                case 2:
                    //number of lines set with a line in the layer tree in this triangle
                    num = actualTriangle.GetComponent<Triangle>().numLineasCapa3;
                    break;

            }
        }
        else {
            //if not triangle then is circle part

            //number of lines set with a line in the circle part
            num=circleList[0].GetComponent<circlePart>().numLineasCapa1;
        }

        //return 
        return (float)num;
    }

    //give the reference to the central point in the mandala
    public GameObject giveCentralPoint() {
        return centralPoint;
    }


    //function to deactivate All Points in the mandala
    public void deactivateAllPoints() {

        //for evevery triangle in the list
        for (int i = 0; i<triangleList.Count; i++) {
            //deactivate All Points in the triangle
            triangleList[i].GetComponent<Triangle>().deactivateAllPointsTriangle();
        }

        //for every circle Part in the list
        for (int i = 0; i < circleList.Count; i++)
        {
            //deactivate the Points
            circleList[i].GetComponent<circlePart>().deactivatePoints();
        }


    }


    //function to activate All Points in the mandala
    public void activateAllPoints()
    {
        //for evevery triangle in the list
        for (int i = 0; i < triangleList.Count; i++)
        {
            //activate All Points in the triangle
            triangleList[i].GetComponent<Triangle>().activateAllPointsTriangle();
        }

        //for every circle Part in the list
        for (int i = 0; i < circleList.Count; i++)
        {
            //activate the Points
            circleList[i].GetComponent<circlePart>().activatePoints();
        }
        
    }

    //Function that return a boolean to know if all the points are in the correct position when they move
    public bool checkPointsInPosition() {

        bool resp = true; //init in true

        //for every triangle in the list of triangles
        for (int i = 0; i < triangleList.Count; i++)
        {
            //if any triangle is false , resp=false
           resp=resp &&  triangleList[i].GetComponent<Triangle>().checkPointsInposition();
        }

        //for every circle Part in the list of circles
        for (int i = 0; i < circleList.Count; i++)
        {
            //if any circle part is false, resp=false
            resp = resp && circleList[i].GetComponent<circlePart>().checkPointsInposition();
           
        }

        return resp;
    }

    //Change according to the layer and interlayers that have in the flow the color of the configuration and return the actual Color of the particles
    public Gradient switchColor() {

        //if triangles part is not done
        if (!trianglesDone)
        {
            //according to the layer
            switch (layer)
            {
                case 0:
                    actualColor = color1;
                    break;
                case 1:
                    actualColor = color2;
                    break;
                case 2:
                    //according to the interlayer in the layer 2
                    if (interLayer != 1)
                    {
                        actualColor = color3;
                    }
                    else
                    {
                        actualColor = color2;
                    }


                    break;


            }
        }
        else {
            //circle color
            actualColor = color2;
        }
        
        return actualColor;
    }

    //Change according to the layer and interlayers that have in the flow the color of the configuration and return the actual Color of the moving zone
    public Gradient switchColorMove()
    {

        //if triangles part is not done
        if (!trianglesDone)
        {

            //according to the layer in the flow
            switch (layer)
            {
                case 0:
                    actualColor = color1Move;
                    break;
                case 1:
                    actualColor = color2Move;
                    break;
                case 2:
                    //according to the interLayer in the flow in the layer 2
                    if (interLayer != 1)
                    {
                        actualColor = color3Move;
                    }
                    else
                    {
                        actualColor = color2Move;
                    }
                    break;


            }
        }
        else {
            //color of the circle
            actualColor = color2Move;
        }
            
        return actualColor;
    }

  
    //function needed to the algoritm that set the lines in the triangles.
    //This function returns the triangle brother to set the lines beetwen diferents triangles
    public GameObject giveBrotherTriangle(int whoiam) {

        //calculate the next in list
        int result = whoiam + 1;
        if (result > 7) {
            result = 0;
        }


        if (triangleList[result + 8].gameObject == null) {
#if UNITY_EDITOR
            Debug.Log("triangle null in function giveBrotherTriangle " + whoiam + " " + result);
#endif
        }
       
        //return the triangle in the list that is the brother. This only is used on the second layer so the +8 is the first layer of 8 triangles
        return triangleList[result+8].gameObject;

    }

    //function that indicate to the Circle Part to sent a line and check if the circle is finish to change the flow
    public void ActivatePointsCircle()
    {
        GameObject actualCircle = null;
        GameObject actualPoint = null;

        //For evevery circle part
        for (int i = 0; i < numTrianglesLayer; i++)
        {
            //acces to the actual circlePart
            actualCircle = circleList[i].gameObject;
            //try to activate a point from the circle part
            actualPoint = actualCircle.GetComponent<circlePart>().activatePointFromCircle();
        }

        //if there is no point to activate
        if (actualPoint==null) {
            //change the fase in the timer zone to move beetween zones
            //timerZone.instance.gameObject.GetComponent<timerZone>().changeFaseTo(1);
            //indicate that all points in circle are done
            circleDone = true;
        }

    }


    //function that return wich audio need according to the layer
    public int whichSong() {
        int resp = 0;

        //if triangle part not done
        if (!trianglesDone)
        {
            switch (layer)
            {
                case 0:
                    //audio 0 - 48s
                    resp=0;
                    break;
                case 1:
                    //audo 1 - 32s
                    resp = 1;
                    break;
                case 2:
                    //audo 1 - 32s
                    resp =2;
                    break;


            }
        }
        else
        {
            //circle audio
            //audo 2 - 16s
            resp = 3;
        }


        return resp;
    }


    public bool ActivatePointsTriangles() {
        
        GameObject actualTriangle=null;
        GameObject actualPoint = null;
        //calculate the actual index of the triangles
        int actualIndexTriangle = interLayer * numTrianglesLayer;

        //for the number of triangles in each layer
        for (int i = 0; i < numTrianglesLayer; i++) {
            //acces to actual triangle
            actualTriangle = triangleList[actualIndexTriangle+i].gameObject;
            //activate a point in triangle to sent a line
            actualPoint = actualTriangle.GetComponent<Triangle>().activatePointFromTriangle();
        }


        //if there is no point to activate means that the triangles in that interlayer is done
        if (actualPoint == null)
        {
            //change the inter layer
            interLayer = interLayer + 1;
            //restart the activations and accumulated activations that are not catch
            centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable = 0.0f;
            centralPoint.GetComponent<PointCentralMandala>().numNotCatched = 0.0f;

            //if we are in the interLayer 2 and the layer 1 and the boolean trampa is false means that in the flow the cheat doest have done yer
            if (interLayer == 2 && layer ==1 && !redirectFlow) {
                //now the cheat of the flow is done
                redirectFlow = true;
                //force to go to interLayer 1 again
                interLayer = 1;
                //calculate the index
                actualIndexTriangle = interLayer * numTrianglesLayer;

                //for every triangle in that interlayer
                for (int i = 0; i < numTrianglesLayer; i++)
                {
                    //get acces to the triangle
                    actualTriangle = triangleList[actualIndexTriangle + i].gameObject;
                    //sent a message to every point in the triangle to change their configuration to next layer
                    actualTriangle.GetComponent<Triangle>().sendNextLayerEveryPoint();

                }
                
            }


            //check if the triangle is not void when the inter layer change. 
            if (!controlTriangleVoid) {
               
                //change to move from zones fase
               // timerZone.instance.gameObject.GetComponent<timerZone>().changeFaseTo(1);
                //restart the boolen to void
                controlTriangleVoid = true;
            }
            //if interlayer is >2 means that we need to change of fase
            if (interLayer > 2)
            {
                //restar interlayer
                interLayer = 0;

                //check if we can access to the next layer of triangles
                if (((layer * numTrianglesLayer) + numTrianglesLayer) < triangleList.Capacity)
                {
                    //if we can add 1 to layer
                    layer = layer + 1;
                    //se to every triangle a message to change the layer in the triangle
                    for (int i = 0; i < triangleList.Count; i++)
                    {
                        actualTriangle = triangleList[i].gameObject;
                        actualTriangle.GetComponent<Triangle>().sendNextLayerEveryPoint();
                        
                    }

                }
                else
                {
                    //if we cannot acces means that the triangles part are done.
                    trianglesDone = true;

                }
            }
            else
            {
                //now we check if the next activation will have a point or not.
                actualIndexTriangle = interLayer * numTrianglesLayer;
                for (int i = 0; i < numTrianglesLayer; i++)
                {
                    actualTriangle = triangleList[actualIndexTriangle + i].gameObject;
                    //in that case the false indicate that no sent a line only check if there is a point
                    actualPoint = actualTriangle.GetComponent<Triangle>().activatePointFromTriangle(false);
                }
                //if the point is empty means that there is no line to sent
                if (actualPoint == null)
                {
                    //force the next interlayer because in the actual the triangle is empty
                    interLayer = interLayer + 1;

                    //check again the flow of interlayer
                    if (interLayer > 2)
                    {
                        interLayer = 0;
                        if (((layer * numTrianglesLayer) + numTrianglesLayer) < triangleList.Capacity)
                        {
                            layer = layer + 1;
                            for (int i = 0; i < triangleList.Count; i++)
                            {
                                actualTriangle = triangleList[i].gameObject;
                                actualTriangle.GetComponent<Triangle>().sendNextLayerEveryPoint();

                            }

                        }
                        else {
                            trianglesDone = true;
                        }
                    }
                }
            }


        }
        else {
            //means that the actual triangle activating have lines to sent
            controlTriangleVoid = false;
        }

        
        return true;
    }
    
}
