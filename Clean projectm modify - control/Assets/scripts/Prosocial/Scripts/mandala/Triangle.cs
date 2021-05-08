using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    //reference Base line of points
    public List<GameObject> LadoBase = new List<GameObject>();
    //reference left line of points
    public List<GameObject> LadoIzquierda = new List<GameObject>();
    //reference right line of points
    public List<GameObject> LadoDerecha = new List<GameObject>();
    //reference to the point on the front point of the triangle
    public GameObject puntaCentral = null;
    //what point the triangle is looking
    public int indiceActualPoint = 0;
    public int typeTriangle = 0;//0 pequeño(small),1 normal(normal),2 grande(big)
    private int ladoMirando=0;//what line of points the triangle is looking.0 base , 1 izquierda(left) , 2 derecha(right) , 3 next
    public int id_triangle = 0;//identify triangle
    //know if the setup is done
    bool started = false;
    //control the number of lines of each layer
    public int numLineasCapa1 = 0;//layer 1
    public int numLineasCapa2 = 0;//layer 2
    public int numLineasCapa3 = 0;//layer 3

    // Start is called before the first frame update
    void Start()
    {
        //setup the line of points the triangle is looking start in layer 1
        ladoMirando = 0;
        //what point is looking
        indiceActualPoint = 0;
        
    }

    
    // Update is called once per frame
    void Update()
    {
        //if not started. Its need to make this way to know that the points are instanciate . If it is done in Start() , doesn work.
        if (!started) {
            //start
            started = true;
            //fillup the line configuratios in every points of the triangle
            LlenarConexionesPuntos();
        }
        
    }

    //function that allows to hide all the points for the triangle
    public void deactivateAllPointsTriangle() {

        //for the line of points base
        for (int i = 0; i < LadoBase.Count; i++)
        {
            //set to invisible
            LadoBase[i].GetComponent<MeshRenderer>().enabled = false;

        }

        //for the right  line of points 
        for (int i = 0; i < LadoDerecha.Count; i++)
        {
            //set to invisible
            LadoDerecha[i].GetComponent<MeshRenderer>().enabled = false;

        }

        //for the left line of points
        for (int i = 0; i < LadoIzquierda.Count; i++)
        {
            //set to invisible
            LadoIzquierda[i].GetComponent<MeshRenderer>().enabled = false;

        }
        

        //the point goes invisible to
        puntaCentral.GetComponent<MeshRenderer>().enabled = false;

    }


    //set to visible all the points in the triangle and activate the movement
    public void activateAllPointsTriangle()
    {

        //for the line of points base
        for (int i = 0; i < LadoBase.Count; i++)
        {
           //set visible and say that need to move
            LadoBase[i].GetComponent<MeshRenderer>().enabled = true;
            LadoBase[i].GetComponent<pointsMandala>().moving = true;
        }

        //for the right  line of points 
        for (int i = 0; i < LadoDerecha.Count; i++)
        {
            //set visible and say that need to move
            LadoDerecha[i].GetComponent<MeshRenderer>().enabled = true;
            LadoDerecha[i].GetComponent<pointsMandala>().moving = true;

        }

        //for the left line of points
        for (int i = 0; i < LadoIzquierda.Count; i++)
        {
            //set visible and say that need to move
            LadoIzquierda[i].GetComponent<MeshRenderer>().enabled = true;
            LadoIzquierda[i].GetComponent<pointsMandala>().moving = true;

        }
        
        //set visible and say that need to move
        puntaCentral.GetComponent<MeshRenderer>().enabled = true;
        puntaCentral.GetComponent<pointsMandala>().moving = true;
       
    }


    //function that give us if all the points on the triangle are in their correct final position
    public bool checkPointsInposition() {

        //init
        bool resp = true;

        //for the line of points base
        for (int i = 0; i < LadoBase.Count; i++)
        {
            //if their still moving
            resp=resp && !LadoBase[i].GetComponent<pointsMandala>().moving;

        }
        //for the right  line of points 
        for (int i = 0; i < LadoDerecha.Count; i++)
        {
            //if their still moving
            resp = resp && !LadoDerecha[i].GetComponent<pointsMandala>().moving;

        }

        //for the left line of points
        for (int i = 0; i < LadoIzquierda.Count; i++)
        {
            //if their still moving
            resp = resp && !LadoIzquierda[i].GetComponent<pointsMandala>().moving;
        }

        //if their still moving
        resp = resp && !puntaCentral.GetComponent<pointsMandala>().moving;

        //final
        return resp;
    }


    //control what line of points the triangle are and if we need to pass to the next
    private List<GameObject> queLadoToca(bool sum) {

        //if need to change
        if (sum) {
            //add 1
            ladoMirando = ladoMirando + 1;
            //restart what point
            indiceActualPoint = 0;
        }

        List<GameObject> PointsList=null;

        //what line of points what we want. We setup the flow of each type of triangle here too.
        switch (ladoMirando) {
            case 0://first layer
                //if typeTriangle is not 0
                if (typeTriangle != 0) {
                    //base line of points
                    PointsList = LadoBase;
                }
                else
                {
                    //left line of points
                    PointsList = LadoIzquierda;
                }
                
                break;
            case 1://layer 2
                //if typeTriangle is not 0
                if (typeTriangle != 0)
                {
                    //left line of points
                    PointsList = LadoIzquierda;
                }
                else
                {
                    //right line of points
                    PointsList = LadoDerecha;
                }
                
                break;
            case 2:
                //if typeTriangle is not 0
                if (typeTriangle != 0)
                {
                    //right line of points
                    PointsList = LadoDerecha;
                }
                else
                {
                    //nothing
                    return null;
                }
                
                break;
            default:
                return null;
                break;

        }

        //final list of points
        return PointsList ;

    }

    
    //function that allows to activate a point to sent a line according the configuration their have. We can use it to check if we can activate something.
    //returns a gameObject if it can activate something or null if not. The arg allows to decide if activate the line or not.
    public GameObject activatePointFromTriangle(bool activateLine = true) {
        //acces to what lines of points the triangle is
        List<GameObject> PointsList = queLadoToca(false);

        //if there is something
        if (PointsList == null)
        {
           // Debug.Log("POINTS LIST NULL " + ladoMirando);
           //triangle is finished or empty
            return null;
        }
        else {
            //acces to the point on the list
            GameObject actualPoint = PointsList[indiceActualPoint].gameObject;
            //setup the objective point to sent the line
            actualPoint.GetComponent<pointsMandala>().givePartner();
            //if the point is null still going until the list is over
            while (actualPoint.GetComponent<pointsMandala>().actualPartnerPoint == null)
            {
                indiceActualPoint = indiceActualPoint + 1;
                //control the points in a line
                if (indiceActualPoint >= PointsList.Count-1)
                {
                    //change of line of points
                    PointsList = queLadoToca(true);
                    if (PointsList == null)
                    {
                        //triangle done
                        return null;
                    }
                    //change the point
                    actualPoint = PointsList[indiceActualPoint].gameObject;



                }
                //change
                actualPoint = PointsList[indiceActualPoint].gameObject;
            }

            //check if the line is done 
           if (actualPoint != null && actualPoint.GetComponent<pointsMandala>().make_line)
            {
                //if the line is sent change the point
                indiceActualPoint = indiceActualPoint + 1;
                //check if we need to change the list of points
                if (indiceActualPoint > PointsList.Count - 1)
                {
                    //change the line of points
                    PointsList = queLadoToca(true);
                    //if null
                    if (PointsList == null)
                    {
                        //triangle done
                        return null;
                    }
                    //change point
                    actualPoint = PointsList[indiceActualPoint].gameObject;
                }
            }

           //if activa Line
            if (activateLine) {
                //active the line beetween points
                actualPoint.GetComponent<pointsMandala>().line_true();
            }
            
            //return that point
            return PointsList[indiceActualPoint].gameObject;
        }
        

        
    }

   
    //function that allows to sent to every point of the triangle that need to change of layer configuration
    public void sendNextLayerEveryPoint()
    {
        //base line of points
        for(int i=0; i< LadoBase.Count; i++)
        {
            //next layer
            LadoBase[i].GetComponent<pointsMandala>().nextLayer();

        }

        //right lines of points
        for (int i = 0; i < LadoDerecha.Count; i++)
        {
            //next layer
            LadoDerecha[i].GetComponent<pointsMandala>().nextLayer();

        }

        //left lines of points
        for (int i = 0; i < LadoIzquierda.Count; i++)
        {
            //next layer
            LadoIzquierda[i].GetComponent<pointsMandala>().nextLayer();

        }

        //restart line of points that the triangle is looking
        ladoMirando = 0;

    }

    //function that make the algorithm of association of objective and initial positon of the lines in each type of triangle
    private void LlenarConexionesPuntos() {
        //init
        int indiceMax = -1;
        int indiceMin = -1;

        //according to the type of triangle is configurate
        switch (typeTriangle) {
            //pequeño(little)
            case 0:
                //thrid partner
            
                indiceMax = LadoIzquierda.Count - 1;
                indiceMin = 0;
                for (int i = indiceMin; i <= indiceMax; i++)
                {
                    //control the number of line that it need to sent
                    numLineasCapa3 = numLineasCapa3 + 1;
                    LadoIzquierda[indiceMax - i].GetComponent<pointsMandala>().partnerPoint2 = LadoDerecha[i].gameObject;

                }
                break;
            //normal
            case 1:
                //first partner
                indiceMax = 14;
                indiceMin = 0;
                
                for (int i = indiceMin; i <= indiceMax; i++) {
                    //check number of lines to sent
                    numLineasCapa1 = numLineasCapa1 + 2;
                    LadoIzquierda[i].GetComponent<pointsMandala>().partnerPoint = LadoBase[i].gameObject;
                    LadoDerecha[indiceMax - i].GetComponent<pointsMandala>().partnerPoint = LadoBase[i].gameObject;
                }

                //second partner
               
                indiceMax = LadoIzquierda.Count-1;
                indiceMin = 0;
                for (int i = indiceMin; i<=indiceMax; i++)
                {
                    //check number of lines to sent
                    numLineasCapa2 = numLineasCapa2 + 1;
                    LadoIzquierda[indiceMax- i].GetComponent<pointsMandala>().partnerPoint1 = LadoDerecha[i].gameObject;
                    
                }

                //third partner
                indiceMax = LadoIzquierda.Count-1;
                indiceMin = LadoIzquierda.Count-8;
              
                for (int i = 0; i <= indiceMax- indiceMin; i++)
                {
                    //check number of lines to sent
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
                    //check number of lines to sent
                    numLineasCapa1 = numLineasCapa1 + 2;
                    LadoIzquierda[i].GetComponent<pointsMandala>().partnerPoint = LadoBase[i].gameObject;
                    LadoDerecha[indiceMax - i].GetComponent<pointsMandala>().partnerPoint = LadoBase[i].gameObject;
                }


                //second partner

                if (mandalamanager.instance == null) {
                    //Debug.Log("Instance  null");
                }

                //obtain the brother triangle to have acces to their points in the association according to the actual id_triangle
                GameObject tempTriangle = mandalamanager.instance.gameObject.GetComponent<mandalamanager>().giveBrotherTriangle(id_triangle);

                indiceMax = LadoDerecha.Count-1;
                indiceMin = 1;

                for (int i = indiceMin; i <= indiceMax; i++)
                {
                    //check number of lines to sent
                    numLineasCapa2 = numLineasCapa2 + 1;
                    tempTriangle.GetComponent<Triangle>().LadoIzquierda[indiceMax-i].GetComponent<pointsMandala>().partnerPoint1 = LadoDerecha[i].gameObject;
                }

                //thrid partner
                indiceMax = LadoIzquierda.Count - 1;
                indiceMin = 0;
                for (int i = indiceMin; i <= indiceMax; i++)
                {
                    //check number of lines to sent
                    numLineasCapa3 = numLineasCapa3 + 1;
                    LadoIzquierda[indiceMax - i].GetComponent<pointsMandala>().partnerPoint2 = LadoDerecha[i].gameObject;

                }


                break;
        }


    }



}
