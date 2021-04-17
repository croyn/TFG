using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circlePart : MonoBehaviour
{

    public List<GameObject> circlePoints = new List<GameObject>();
    public int indiceActualPoint = 0;
    private bool started = false;
    public int numLineasCapa1=0;
    // Start is called before the first frame update
    void Start()
    {
        started = false;
        indiceActualPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            started = true;
            LlenarConexionesPuntos();
        }
    }

    public void activatePoints() {
        for (int i = 0; i < circlePoints.Count; i++) {
           // circlePoints[i].SetActive(true);
            circlePoints[i].GetComponent<MeshRenderer>().enabled = true;
            circlePoints[i].GetComponent<pointsMandala>().moving = true;
        }

    }

    public void deactivatePoints() {
        for (int i = 0; i < circlePoints.Count; i++)
        {
            //circlePoints[i].SetActive(false);
            circlePoints[i].GetComponent<MeshRenderer>().enabled = false;

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


    public GameObject activatePointFromCircle(bool activateLine = true) {

        GameObject actualPoint = circlePoints[indiceActualPoint].gameObject;
        actualPoint.GetComponent<pointsMandala>().givePartner();
        while (actualPoint.GetComponent<pointsMandala>().actualPartnerPoint == null)
        {
            indiceActualPoint = indiceActualPoint + 1;
            if (indiceActualPoint <= circlePoints.Count - 1)
            {
                actualPoint = circlePoints[indiceActualPoint].gameObject;
                
            }
            else {
                
                return null;
            }

            
        }


        if (actualPoint != null && actualPoint.GetComponent<pointsMandala>().make_line)
        {
            indiceActualPoint = indiceActualPoint + 1;
            if (indiceActualPoint <= circlePoints.Count - 1)
            {
                actualPoint = circlePoints[indiceActualPoint].gameObject;
                
            }
            else {
                return null;
            }
        }
        if (activateLine)
        {
            actualPoint.GetComponent<pointsMandala>().line_true();
        }

        return circlePoints[indiceActualPoint].gameObject;
    }

    private void LlenarConexionesPuntos()
    {
        for (int i = 0; i <= circlePoints.Count/2; i++)
        {
            numLineasCapa1 = numLineasCapa1 + 1;
            circlePoints[i].GetComponent<pointsMandala>().partnerPoint = circlePoints[i+ (circlePoints.Count / 2)].gameObject;

        }
    }

 }
