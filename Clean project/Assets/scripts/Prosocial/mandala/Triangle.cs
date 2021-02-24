using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{

    public List<GameObject> PointsList = new List<GameObject>();
    private int indiceActualPoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject givePoint()
    {
        

        GameObject actualPoint = PointsList[indiceActualPoint].gameObject;

        while (actualPoint.GetComponent<pointsMandala>().partnerPoint == null)
        {
            indiceActualPoint = indiceActualPoint + 1;

            actualPoint = PointsList[indiceActualPoint].gameObject;
        }

        if (actualPoint.GetComponent<pointsMandala>().done())
        {
            indiceActualPoint = indiceActualPoint + 1;
        }



        return PointsList[indiceActualPoint].gameObject;
    }
}
