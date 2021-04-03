using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circlePart : MonoBehaviour
{

    public List<GameObject> circlePoints = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activatePoints() {
        for (int i = 0; i < circlePoints.Count; i++) {
            circlePoints[i].SetActive(true);
            circlePoints[i].GetComponent<pointsMandala>().moving = true;
        }

    }

    public void deactivatePoints() {
        for (int i = 0; i < circlePoints.Count; i++)
        {
            circlePoints[i].SetActive(false);

        }

    }


    public bool checkPointsInposition()
    {

        bool resp = true;

        for (int i = 0; i < circlePoints.Count; i++)
        {
            resp = resp && !circlePoints[i].GetComponent<pointsMandala>().moving;

        }

       

        return resp;
    }

}
