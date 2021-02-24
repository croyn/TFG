using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMotionInTheSpace : GeneralMotionBehaviour
{
    /// <summary>
    /// point the object is aiming in the rendom motion
    /// </summary>
    private Vector3 targetposition;

    /// <summary>
    /// variable to determine the position of the next target in the roaming motion
    /// </summary>
    float angle, distance;


    /// <summary>
    /// get the first target position
    /// </summary>
    void Start()
    {
        getRandomTargetPosition();
    }

    /// <summary>
    /// move the object towards the target position
    /// </summary>
    void Update()
    {
        if (startmotion)
        {
            Quaternion targetrot = Quaternion.LookRotation(targetposition - transform.localPosition);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetposition, Time.deltaTime * movingspeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetrot, Time.deltaTime * rotatingspeed);
            if (Vector3.Distance(transform.localPosition, targetposition) < 10)
            {
                getRandomTargetPosition();
            }
        }
    }

    /// <summary>
    /// rturn a new point in the space to reach following moving along a circular-like trajectory
    /// </summary>
    void getRandomTargetPosition()
    {
        angle += Random.Range(10, 20);
        distance = Random.Range(10, 40);
        targetposition = (new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle))) * distance;
    }
}
