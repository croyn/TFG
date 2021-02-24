using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private SpriteRenderer pointer;
    private SpriteRenderer hint;
    private bool indicatorOnShow = false;
    private Vector3 otherPlayer;
    private IA IA;

    // Start is called before the first frame update
    void Start()
    {
        IA = IA.instance;
        pointer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        pointer.enabled = false;
        hint = transform.GetChild(1).GetComponent<SpriteRenderer>();
        hint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (indicatorOnShow)
        {
            transform.LookAt(otherPlayer);            
        }
    }

    /// <summary>
    /// Activates an indicator icon to help other player
    /// </summary>
    /// <param name="target">the location of the player being asked for help</param>
    /// <param name="typePointer">the type of indicator which should be used</param>
    public void startIndicator(Transform target, bool typePointer)
    {
        /// TEMPORARILY REMOVED AS FUNCTIONALITY SHOWED NO IMPACT
        
        //indicatorOnShow = true;
        //otherPlayer = target.position;

        //if (typePointer)
        //{
        //    hint.enabled = false;
        //    pointer.enabled = true;
        //}
        //else
        //{
        //    pointer.enabled = false;
        //    hint.enabled = true;
        //}
    }

    /// <summary>
    /// Deactivates all indicators
    /// </summary>
    public void stopIndicator()
    {
        indicatorOnShow = false;
        pointer.enabled = false;
        hint.enabled = false;
    }

    /// <summary>
    /// Get state of indicators
    /// </summary>
    public bool GetIndicatorStatus()
    {
        return indicatorOnShow;
    }



}
