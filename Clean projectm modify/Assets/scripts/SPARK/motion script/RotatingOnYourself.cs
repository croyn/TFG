using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingOnYourself : GeneralMotionBehaviour
{
    /// <summary>
    /// axis on which perform the rotation
    /// </summary>
    public rotationaxis axis = rotationaxis.x;

    /// <summary>
    /// internal counter for the rotation
    /// </summary>
    private float internalrotation = 0;

    /// <summary>
    /// values of the angle in deg on the other axis not interested into the rotation
    /// </summary>
    private float value1, value2;

    /// <summary>
    /// get the initial values to not be modified
    /// </summary>
    void Start() {
        switch (axis) {
            case rotationaxis.x: internalrotation = transform.localRotation.eulerAngles.x; value1 = transform.localRotation.eulerAngles.y; value2 = transform.localRotation.eulerAngles.z; break;
            case rotationaxis.y: internalrotation = transform.localRotation.eulerAngles.y; value1 = transform.localRotation.eulerAngles.x; value2 = transform.localRotation.eulerAngles.z; break;
            case rotationaxis.z: internalrotation = transform.localRotation.eulerAngles.z; value1 = transform.localRotation.eulerAngles.x; value2 = transform.localRotation.eulerAngles.y; break;
            }
    }

    /// <summary>
    /// perform the motion
    /// </summary>
    void Update()
    {
        if (startmotion)
        {
            internalrotation += Time.deltaTime * rotatingspeed;//rotationspeed;
            switch (axis) {
                case rotationaxis.x: transform.localRotation = Quaternion.Euler(internalrotation, value1, value2); break;
                case rotationaxis.y: transform.localRotation = Quaternion.Euler(value1, internalrotation, value2); break;
                case rotationaxis.z: transform.localRotation = Quaternion.Euler(value1, value2, internalrotation); break;
            }
            
        }
    }
}

public enum rotationaxis { 
    x,y,z
}
