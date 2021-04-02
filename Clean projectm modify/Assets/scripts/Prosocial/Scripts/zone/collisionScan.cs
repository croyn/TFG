using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionScan : MonoBehaviour
{
    public float timeIn = 0.0f;
    public bool isIn;
    public Color actualColor;
    // Start is called before the first frame update
    void Start()
    {
        isIn = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (isIn)
        {

            timeIn = timeIn + Time.deltaTime;
        }
        else if (!isIn && timeIn > 0.0f) {
            timeIn = timeIn - Time.deltaTime;
            if (timeIn < 0.0f) {
                timeIn = 0.0f;
            }
        }

        if (checkTimeIn())
        {

            changeColorTo(1);
        }
        else {
            changeColorTo(0);
        }
    }

    private void changeColorTo(int cual) {
        //GameObject foot = transform.Find("footPrint").gameObject;
        if (true) {
            Renderer footMaterial = gameObject.GetComponent<Renderer>();
            switch (cual)
            {
                case 0:
                    footMaterial.material.SetColor("_Color", Color.white);
                    actualColor= Color.white;
                    break;
                case 1:
                    footMaterial.material.SetColor("_Color", Color.green);
                    actualColor = Color.green;
                    
                    break;
                case 2:
                    footMaterial.material.SetColor("_Color", Color.red);
                    actualColor = Color.red;
                    break;

            }

        }
        

    }

    private bool checkTimeIn() {
        if (timeIn > 3.0f) {
            return true;

        }
        return false;
    }


    private void OnTriggerStay(Collider other) {
        Debug.Log("Entro scan");
        isIn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Salgo scan");
        isIn = false;


    }
}
