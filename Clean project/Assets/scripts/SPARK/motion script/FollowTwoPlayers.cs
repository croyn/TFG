using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTwoPlayers : GeneralMotionBehaviour
{
    /// <summary>
    /// reference to the two objects to follow
    /// </summary>
    [HideInInspector]
    public Transform target1, target2;

    /// <summary>
    /// original direction before the initial motion (to be restored at the end)
    /// </summary>
    Vector3 originaldir;

    /// <summary>
    /// actual direction to be followed to stay between the two target
    /// </summary>
    Vector3 direction;

    /// <summary>
    /// Compute the motion
    /// </summary>
    void Update()
    {
        if (startmotion)//followtarget)
        {
            direction = (((target1.position + target2.position) / 2 - transform.position));
            if (direction.magnitude > 1)
            {
                transform.position += direction.normalized * movingspeed;//speed;
            }
        }
    }

    /// <summary>
    /// set the two target to follow  and start moving
    /// </summary>
    /// <param name="t">first target</param>
    /// <param name="t2">second target</param>
    public void setup(Transform t, Transform t2)
    {
        target1 = t;
        target2 = t2;
        //followtarget = true;
        startmotion = true;
    }
}
