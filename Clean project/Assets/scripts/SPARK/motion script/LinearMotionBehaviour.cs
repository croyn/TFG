using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMotionBehaviour : GeneralMotionBehaviour
{
    /// <summary>
    /// target to follow
    /// </summary>
    Transform target;

    /// <summary>
    /// Compute the motion
    /// </summary>
    void Update()
    {
        if (startmotion)//startmovement)
        {
            transform.position += transform.forward  * movingspeed;//speed;
            if (target != null)
            {
                transform.LookAt(target);
            }
        }
    }

    /// <summary>
    ///  set the target and start moving
    /// </summary>
    /// <param name="target">what to follow</param>
    public void setTarget(Transform target)
    {
        this.target = target;
        //speed += Random.Range(-0.2f, 0.2f);
        movingspeed += Random.Range(-0.2f, 0.2f); 
    }
}
