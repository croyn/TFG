using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioCiruclos : MonoBehaviour
{

    public Material circuloBase0;
    public Material circuloBase1;
    public Material circuloBase2;
    public Material circuloBase3;
    public int cualCiruclo;

    // Start is called before the first frame update
    void Start()
    {

        activeCircleMaterial();

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void activeCircleMaterial() {

        Renderer temp = gameObject.GetComponent<Renderer>();
        switch (cualCiruclo)
        {
            case 0:

                temp.material = circuloBase0;
                break;
            case 1:
                temp.material = circuloBase1;
                break;
            case 2:
                temp.material = circuloBase2;
                break;
            case 3:
                temp.material = circuloBase3;
                break;

        }
    }

}
