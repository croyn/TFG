using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCollisionDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject g = collision.gameObject;
        PassingMovement p = g.GetComponent<PassingMovement>();
        if (p != null) {
            if (p.startmotion){//p.startmoving) {
                p.changeToPerpendicularDirection(transform.position);
            }
        }
        if (g.tag == "AstralElement") {
            g.transform.localPosition += (-(transform.position - new Vector3(50, 0, 50)) + (g.transform.position - new Vector3(50, 0, 50))).normalized * gameObject.GetComponent<SphereCollider>().radius*1.5f;
        }
    }
}
