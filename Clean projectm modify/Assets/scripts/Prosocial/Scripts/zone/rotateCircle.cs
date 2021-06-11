using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCircle : MonoBehaviour
{

    public float rotation_x = 0.0f;
    public float rotation_y = 0.05f;
    public float rotation_z = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        rotation_y = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        rotate_object();
    }

    void rotate_object() {
        gameObject.transform.Rotate(rotation_x, rotation_y, rotation_z, Space.World);
    }

}
