using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{

    public List<GameObject> LadoBase = new List<GameObject>();
    public List<GameObject> LadoIzquierda = new List<GameObject>();
    public List<GameObject> LadoDerecha = new List<GameObject>();
    public GameObject puntaCentral = null;
    public int indiceActualPoint = 0;
    public int typeTriangle = 0;//0 pequeño,1 normal,2 grande
    private int ladoMirando=0;//0 base , 1 izquierda , 2 derecha , 3 next
    public int id_triangle = 0;
    bool started = false;
    public int numLineasCapa1 = 0;
    public int numLineasCapa2 = 0;
    public int numLineasCapa3 = 0;

    // Start is called before the first frame update
    void Start()
    {
        ladoMirando = 0;
        indiceActualPoint = 0;
        
    }

    
    // Update is called once per frame
    void Update()
    {
        if (!started) {
            started = true;
            LlenarConexionesPuntos();
        }
        
    }

    public void deactivateAllPointsTriangle() {
        for (int i = 0; i < LadoBase.Count; i++)
        {
            //LadoBase[i].gameObject.SetActive(false);
            LadoBase[i].GetComponent<MeshRenderer>().enabled = false;

        }

        for (int i = 0; i < LadoDerecha.Count; i++)
        {
            // LadoDerecha[i].SetActive(false);
            LadoDerecha[i].GetComponent<MeshRenderer>().enabled = false;

        }
        for (int i = 0; i < LadoIzquierda.Count; i++)
        {
            //  LadoIzquierda[i].SetActive(false);
            LadoIzquierda[i].GetComponent<MeshRenderer>().enabled = false;

        }
        //puntaCentral.SetActive(false);
        puntaCentral.GetComponent<MeshRenderer>().enabled = false;

    }

    public void activateAllPointsTriangle()
    {
        for (int i = 0; i < LadoBase.Count; i++)
        {
           // LadoBase[i].SetActive(true);
            LadoBase[i].GetComponent<MeshRenderer>().enabled = true;
            LadoBase[i].GetComponent<pointsMandala>().moving = true;
        }

        for (int i = 0; i < LadoDerecha.Count; i++)
        {
           // LadoDerecha[i].SetActive(true);
            LadoDerecha[i].GetComponent<MeshRenderer>().enabled = true;
            LadoDerecha[i].GetComponent<pointsMandala>().moving = true;

        }
        for (int i = 0; i < LadoIzquierda.Count; i++)
        {
           // LadoIzquierda[i].SetActive(true);
            LadoIzquierda[i].GetComponent<MeshRenderer>().enabled = true;
            LadoIzquierda[i].GetComponent<pointsMandala>().moving = true;

        }
        //  puntaCentral.SetActive(true);
        puntaCentral.GetComponent<MeshRenderer>().enabled = true;
        puntaCentral.GetComponent<pointsMandala>().moving = true;
       
    }

    public bool checkPointsInposition() {

        bool resp = true;

        for (int i = 0; i < LadoBase.Count; i++)
        {
            resp=resp && !LadoBase[i].GetComponent<pointsMandala>().moving;

        }

        for (int i = 0; i < LadoDerecha.Count; i++)
        {
            resp = resp && !LadoDerecha[i].GetComponent<pointsMandala>().moving;
       

        }
        for (int i = 0; i < LadoIzquierda.Count; i++)
        {
            resp = resp && !LadoIzquierda[i].GetComponent<pointsMandala>().moving;
           

        }
        resp=resp && !puntaCentral.GetComponent<pointsMandala>().moving;

        return resp;
    }




    private List<GameObject> queLadoToca(bool sum) {

        if (sum) {
            ladoMirando = ladoMirando + 1;
            indiceActualPoint = 0;
        }
        List<GameObject> PointsList=null;
        switch (ladoMirando) {
            case 0:
                if (typeTriangle != 0) {
                    PointsList = LadoBase;
                }
                else
                {
                    PointsList = LadoIzquierda;
                }
                
                break;
            case 1:
                if (typeTriangle != 0)
                {
                    PointsList = LadoIzquierda;
                }
                else
                {
                    PointsList = LadoDerecha;
                }
                
                break;
            case 2:
                if (typeTriangle != 0)
                {
                    PointsList = LadoDerecha;
                }
                else
                {
                    return null;
                }
                
                break;
            default:
                return null;
                break;

        }

        return PointsList ;

    }

    

    public GameObject activatePointFromTriangle(bool activateLine = true) {
        List<GameObject> PointsList = queLadoToca(false);

        if (PointsList == null)
        {
           // Debug.Log("POINTS LIST NULL " + ladoMirando);
            return null;
        }
        else {
            GameObject actualPoint = PointsList[indiceActualPoint].gameObject;
            actualPoint.GetComponent<pointsMandala>().givePartner();
            while (actualPoint.GetComponent<pointsMandala>().actualPartnerPoint == null)
            {
                indiceActualPoint = indiceActualPoint + 1;
                if (indiceActualPoint >= PointsList.Count-1)
                {
                    PointsList = queLadoToca(true);
                    if (PointsList == null)
                    {
                        return null;
                    }
                    actualPoint = PointsList[indiceActualPoint].gameObject;



                }

                actualPoint = PointsList[indiceActualPoint].gameObject;
            }


           if (actualPoint != null && actualPoint.GetComponent<pointsMandala>().make_line)
            {
                indiceActualPoint = indiceActualPoint + 1;
                if (indiceActualPoint > PointsList.Count - 1)
                {
                    PointsList = queLadoToca(true);
                    if (PointsList == null)
                    {
                        return null;
                    }
                    actualPoint = PointsList[indiceActualPoint].gameObject;
                }
            }
            if (activateLine) {
                actualPoint.GetComponent<pointsMandala>().line_true();
            }
            
            return PointsList[indiceActualPoint].gameObject;
        }
        

        
    }


    public List<GameObject> givePointsOuterConnection() {
        return null;

    }

    public void sendNextLayerEveryPoint() {

        for(int i=0; i< LadoBase.Count; i++)
        {
            LadoBase[i].GetComponent<pointsMandala>().nextLayer();

        }

        for (int i = 0; i < LadoDerecha.Count; i++)
        {
            LadoDerecha[i].GetComponent<pointsMandala>().nextLayer();

        }
        for (int i = 0; i < LadoIzquierda.Count; i++)
        {
            LadoIzquierda[i].GetComponent<pointsMandala>().nextLayer();

        }

        ladoMirando = 0;

    }


    private void LlenarConexionesPuntos() {
        int indiceMax = -1;
        int indiceMin = -1;

        switch (typeTriangle) {
            //pequeño
            case 0:
                //thrid partner
                indiceMax = LadoIzquierda.Count - 1;
                indiceMin = 0;
                for (int i = indiceMin; i <= indiceMax; i++)
                {
                    numLineasCapa3 = numLineasCapa3 + 1;
                    LadoIzquierda[indiceMax - i].GetComponent<pointsMandala>().partnerPoint2 = LadoDerecha[i].gameObject;

                }
                break;
            //normal
            case 1:
                //primer partner
                indiceMax = 14;
                indiceMin = 0;
                
                for (int i = indiceMin; i <= indiceMax; i++) {
                    numLineasCapa1 = numLineasCapa1 + 2;
                    LadoIzquierda[i].GetComponent<pointsMandala>().partnerPoint = LadoBase[i].gameObject;
                    LadoDerecha[indiceMax - i].GetComponent<pointsMandala>().partnerPoint = LadoBase[i].gameObject;
                }

                //second partner
               
                indiceMax = LadoIzquierda.Count-1;
                indiceMin = 0;
                for (int i = indiceMin; i<=indiceMax; i++)
                {
                    numLineasCapa2 = numLineasCapa2 + 1;
                    LadoIzquierda[indiceMax- i].GetComponent<pointsMandala>().partnerPoint1 = LadoDerecha[i].gameObject;
                    
                }

                //third partner
                indiceMax = LadoIzquierda.Count-1;
                indiceMin = LadoIzquierda.Count-8;
              
                for (int i = 0; i <= indiceMax- indiceMin; i++)
                {
                    numLineasCapa3 = numLineasCapa3 + 1;
                    LadoIzquierda[indiceMax-i].GetComponent<pointsMandala>().partnerPoint2 = LadoDerecha[indiceMin+i].gameObject;

                }

                
                break;
            //grande
            case 2:
                //primer partner
                 indiceMax = 14;
                 indiceMin = 0;

                for (int i = indiceMin; i <= indiceMax; i++)
                {
                    numLineasCapa1 = numLineasCapa1 + 2;
                    LadoIzquierda[i].GetComponent<pointsMandala>().partnerPoint = LadoBase[i].gameObject;
                    LadoDerecha[indiceMax - i].GetComponent<pointsMandala>().partnerPoint = LadoBase[i].gameObject;
                }


                //second partner

                if (mandalamanager.instance == null) {
                    //Debug.Log("Instance  null");
                }

                GameObject tempTriangle = mandalamanager.instance.gameObject.GetComponent<mandalamanager>().giveBrotherTriangle(id_triangle);

                indiceMax = LadoDerecha.Count-1;
                indiceMin = 1;
                for (int i = indiceMin; i <= indiceMax; i++)
                {
                    numLineasCapa2 = numLineasCapa2 + 1;
                    tempTriangle.GetComponent<Triangle>().LadoIzquierda[indiceMax-i].GetComponent<pointsMandala>().partnerPoint1 = LadoDerecha[i].gameObject;
                }

                //thrid partner
                indiceMax = LadoIzquierda.Count - 1;
                indiceMin = 0;
                for (int i = indiceMin; i <= indiceMax; i++)
                {
                    numLineasCapa3 = numLineasCapa3 + 1;
                    LadoIzquierda[indiceMax - i].GetComponent<pointsMandala>().partnerPoint2 = LadoDerecha[i].gameObject;

                }


                break;
        }


    }



}
