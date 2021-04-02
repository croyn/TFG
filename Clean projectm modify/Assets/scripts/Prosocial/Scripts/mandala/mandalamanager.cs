using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mandalamanager : MonoBehaviour
{
    public static mandalamanager instance;
    public List<GameObject> triangleList = new List<GameObject>();
    private int indiceActualPoint=0;
    private int indiceTriangle = 0;
    public GameObject centralPoint = null;
    public int numTrianglesLayer=8;
    public int layer = 0;
    public int interLayer = 0;
    public Gradient color1;
    public Gradient color1Move;
    public Gradient color2;
    public Gradient color2Move;
    public Gradient color3;
    public Gradient color3Move;
    public Color colorSolid1;
    public Color colorSolid2;
    public Color colorSolid3;
    private Gradient actualColor = null;
    private bool controlTriangleVoid=true;

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
    }

    // Update is called once per frame
    void Update()
    {

    }


    public GameObject giveCentralPoint() {
        return centralPoint;
    }

    


    public Gradient switchColor() {
        switch (layer) {
            case 0:
                actualColor = color1;
                break;
            case 1:
                actualColor = color2;
                break;
           case 2:
                if (interLayer != 1)
                {
                    actualColor = color3;
                }
                else {
                    actualColor = color2;
                }
                
                
                break;


        }
        return actualColor;
    }
    public Gradient switchColorMove()
    {
        switch (layer)
        {
            case 0:
                actualColor = color1Move;
                break;
            case 1:
                actualColor = color2Move;
                break;
            case 2:
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
        return actualColor;
    }

    public GameObject givePointFronTriangle()
    {
        /*GameObject actualTriangle = triangleList[indiceTriangle].gameObject;
       

        /*GameObject actualPoint = sphereObjectivelist[indiceActualPoint].gameObject;

        while(actualPoint.GetComponent<pointsMandala>().partnerPoint == null) {
            indiceActualPoint = indiceActualPoint + 1;

            actualPoint = sphereObjectivelist[indiceActualPoint].gameObject;
        }

        if (actualPoint.GetComponent<pointsMandala>().number_particles_catch > 400) {
            indiceActualPoint = indiceActualPoint + 1;
        }*/

        /*GameObject toReturn= actualTriangle.GetComponent<Triangle>().givePoint();

        while (toReturn == null && indiceTriangle< triangleList.Capacity)
        {

            indiceTriangle = indiceTriangle + 1;
            actualTriangle = triangleList[indiceTriangle].gameObject;
            toReturn = actualTriangle.GetComponent<Triangle>().givePoint();

        }*/

        return null;

    }


    public GameObject giveBrotherTriangle(int whoiam) {


        int result = whoiam + 1;
        if (result > 7) {
            result = 0;
        }

        if (triangleList[result + 8].gameObject == null) {
            //Debug.Log("triangle null " + whoiam + " " + result);
        }
       // Debug.Log("triangle  " + whoiam + " " + result);

        return triangleList[result+8].gameObject;

    }

    public void ActivatePointsTriangles() {
        GameObject actualTriangle=null;
        GameObject actualPoint = null;
        int actualIndexTriangle = interLayer * numTrianglesLayer;
        for (int i = 0; i < numTrianglesLayer; i++) {
            actualTriangle = triangleList[actualIndexTriangle+i].gameObject;
            actualPoint = actualTriangle.GetComponent<Triangle>().activatePointFromTriangle();
        }



        if (actualPoint == null)
        {
            interLayer = interLayer + 1;
            if (!controlTriangleVoid) {
                timerZone.instance.gameObject.GetComponent<timerZone>().changeFaseTo(1);
                controlTriangleVoid = true;
            }
            
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
            }
            else
            {
                actualIndexTriangle = interLayer * numTrianglesLayer;
                for (int i = 0; i < numTrianglesLayer; i++)
                {
                    actualTriangle = triangleList[actualIndexTriangle + i].gameObject;
                    actualPoint = actualTriangle.GetComponent<Triangle>().activatePointFromTriangle(false);
                }
                //miro si el triangulo que tocaria es vacio.(no tiene partners)
                if (actualPoint == null)
                {
                    interLayer = interLayer + 1;
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
                    }
                }
            }


        }
        else {
            controlTriangleVoid = false;
        }
        

    }
    
}
