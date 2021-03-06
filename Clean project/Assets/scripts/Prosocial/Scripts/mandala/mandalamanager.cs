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
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //sphereObjectivelist = new List<GameObject>();
        indiceActualPoint = 0;
        indiceTriangle = 0;
        layer = 0;
        interLayer = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public GameObject giveCentralPoint() {
        return centralPoint;
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
            Debug.Log("triangle null " + whoiam + " " + result);
        }
        Debug.Log("triangle  " + whoiam + " " + result);

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
        


        if (actualPoint == null) {
            interLayer = interLayer + 1;
           if(interLayer>2)
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


            }

            
        }
        

    }
    
}
