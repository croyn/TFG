﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mandalamanager : MonoBehaviour
{
    public static mandalamanager instance;
    public List<GameObject> triangleList = new List<GameObject>();
    public List<GameObject> circleList = new List<GameObject>();
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
    private bool trampa = false;
    public bool trianglesDone = false;
    public bool circleDone = false;
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
        trampa = false;
        trianglesDone = false;
        circleDone = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public GameObject giveCentralPoint() {
        return centralPoint;
    }


    public void deactivateAllPoints() {

        for (int i = 0; i<triangleList.Count; i++) {
            triangleList[i].GetComponent<Triangle>().deactivateAllPointsTriangle();
        }
        for (int i = 0; i < circleList.Count; i++)
        {
            circleList[i].GetComponent<circlePart>().deactivatePoints();
        }


    }
    public void activateAllPoints()
    {

        for (int i = 0; i < triangleList.Count; i++)
        {
            triangleList[i].GetComponent<Triangle>().activateAllPointsTriangle();
        }
        for (int i = 0; i < circleList.Count; i++)
        {
            circleList[i].GetComponent<circlePart>().activatePoints();
        }
        
    }

    public bool checkPointsInPosition() {

        bool resp = true;
        for (int i = 0; i < triangleList.Count; i++)
        {
           resp=resp &&  triangleList[i].GetComponent<Triangle>().checkPointsInposition();
        }
        for (int i = 0; i < circleList.Count; i++)
        {
            resp = resp && circleList[i].GetComponent<circlePart>().checkPointsInposition();
           
        }
        return resp;
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

    public void ActivatePointsCircle()
    {
        GameObject actualCircle = null;
        GameObject actualPoint = null;
        for (int i = 0; i < numTrianglesLayer; i++)
        {
            actualCircle = circleList[i].gameObject;
            actualPoint = actualCircle.GetComponent<circlePart>().activatePointFromCircle();
        }

        if (actualPoint==null) {
            circleDone = true;
        }

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
            if (interLayer == 2 && layer ==1 && !trampa) {
                trampa = true;
                interLayer = 1;
                actualIndexTriangle = interLayer * numTrianglesLayer;
                for (int i = 0; i < numTrianglesLayer; i++)
                {

                    actualTriangle = triangleList[actualIndexTriangle + i].gameObject;
                    actualTriangle.GetComponent<Triangle>().sendNextLayerEveryPoint();

                }
                
            }
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
                else
                {
                    trianglesDone = true;

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
                        else {
                            trianglesDone = true;
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
