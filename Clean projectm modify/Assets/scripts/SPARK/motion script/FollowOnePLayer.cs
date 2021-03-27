using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOnePLayer : GeneralMotionBehaviour
{
    /// <summary>
    /// target object to follow
    /// </summary>
    [HideInInspector]
    public Transform target;

    /// <summary>
    /// original direction before the initial motion (to be restored at the end)
    /// </summary>
    Vector3 originaldir;

    /// <summary>
    /// actual direction to be followed to stay between the two target
    /// </summary>
    [HideInInspector]
    public Vector3 direction;

    /// <summary>
    /// determine the direction of the spin around the target
    /// </summary>
    Vector3 spin_up;

    public bool isOrbiting = false;

    /// <summary>
    /// compute the motion
    /// </summary>
    void Update()
    {
        if (startmotion)//followtarget)
        {
            direction = (transform.forward + (target.position - transform.position) + originaldir).normalized;
            if (Vector3.Distance(target.position, transform.position) > 5)
            {
                transform.position += direction * movingspeed;//speed;
                isOrbiting = false;
            }
            else
            {
                transform.RotateAround(target.position, spin_up, Time.deltaTime * 1000 * movingspeed);// * speed);
                isOrbiting = true;
            }
        }
    }

    /// <summary>
    /// set the target motion and start moving
    /// </summary>
    /// <param name="t">the target to follow</param>
    public void setup(Transform t) {
        target = t;
        //followtarget = true;
        startmotion = true;
        originaldir = transform.forward;
        if (transform.position.x < target.position.x)
        {
            spin_up = Vector3.up;
        }
        else {
            spin_up = Vector3.down;
        }
        isOrbiting = false;
    }
}
