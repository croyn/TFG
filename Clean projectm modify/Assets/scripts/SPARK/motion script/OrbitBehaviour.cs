using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitBehaviour : GeneralMotionBehaviour
{
    /// <summary>
    /// distance from the center of orbit (parent's position) at which the orbit must stabilize
    /// </summary>
    public float orbitalDistance = -1;

    /// <summary>
    /// perform the rotation movement around the father's position
    /// </summary>
    void Update()
    {
        if (startmotion)//startOrbit)
        {
            transform.RotateAround(transform.parent.position, Vector3.up, Time.deltaTime * rotatingspeed);//speed);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            if (orbitalDistance > 0) {
                Vector3 dir = transform.localPosition.normalized;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, dir * orbitalDistance, Time.deltaTime);
            }
        }
    }
}
