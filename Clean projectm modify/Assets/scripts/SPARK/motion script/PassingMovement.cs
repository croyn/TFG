using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingMovement : GeneralMotionBehaviour
{
    /// <summary>
    /// initial postion on the border of the system
    /// </summary>
    Vector3 startingposition;

    /// <summary>
    /// final position on the border of the screen
    /// </summary>
    Vector3 endingPosition;

    /// <summary>
    /// flag to determine if the object has to be resetted
    /// </summary>
    [HideInInspector]
    public bool resetted = true;

    /// <summary>
    /// min and max amount of time to start the motion of the object
    /// </summary>
    public int respawntimemax, respawntimemin;

    /// <summary>
    /// Inital reset of positon
    /// </summary>
    void Start()
    {
        RandomObjectmanager.instance.resetDirection(gameObject, respawntimemin, respawntimemax);
    }

    /// <summary>
    /// compute the linear motion of the object on the vector made by (endingPosition - startingposition)
    /// </summary>
    void Update()
    {
        if(startmotion){//startmoving){
            transform.position += (endingPosition - startingposition)* Time.deltaTime * movingspeed ;//speed;
            transform.localRotation = Quaternion.LookRotation((endingPosition - startingposition).normalized);
            if (transform.position.x > 100 || transform.position.z > 100 || transform.position.x < 0 || transform.position.z < 0)
            {
                if (transform.childCount > 0)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    if (transform.GetChild(0).GetComponent<ParticleSystem>() != null)
                    {
                        ParticleSystem.EmissionModule em = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
                        em.rateOverDistance = 0;
                    }
                }
                startmotion = false;
                //startmoving = false;
                RandomObjectmanager.instance.resetDirection(gameObject, respawntimemin, respawntimemax);
            }
            else
            {
                if (transform.childCount > 0)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    if (transform.GetChild(0).GetComponent<ParticleSystem>() != null)
                    {
                        ParticleSystem.EmissionModule em = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
                        em.rateOverDistance = 10;
                    }
                }
            }
        }
    }

    /// <summary>
    /// compute an initial and final position on the opposite part of the screen
    /// </summary>
    public void setup() {
        if (Random.Range(-10, 10) > 0)
        {
            int f = Mathf.RoundToInt(Random.Range(0, 2)) *100;
            int f_revert = (f== 100)? 0:100;
            startingposition = new Vector3(Random.Range(30, 70), 0, f);
            endingPosition = new Vector3(Random.Range(30, 70), 0, f_revert);
        }
        else {
            int f = Mathf.RoundToInt(Random.Range(0, 2)) * 100;
            int f_revert = (f == 100) ? 0 : 100;
            startingposition = new Vector3(f, 0, Random.Range(30, 70));
            endingPosition = new Vector3(f_revert, 0, Random.Range(30, 70));
        }
        transform.position = startingposition;
        startmotion = true;
        //startmoving = true;
        resetted = true;
    }

    /// <summary>
    /// reset the linear motion considering a predetrmined direction
    /// </summary>
    /// <param name="dir">the direction to be followed</param>
    public void setup(Vector3 dir){
        startingposition = Vector3.zero;
        endingPosition = dir;
        startmotion = true;
        //startmoving = true;
        resetted = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void changeToPerpendicularDirection(Vector3 deflectorobjectposition)
    {
        endingPosition = - endingPosition + startingposition;
        startingposition = transform.position;
        startmotion = true;
        //startmoving = true;
        resetted = true;
        StartCoroutine(boucespeedback());
    }

    private IEnumerator boucespeedback() {
        float speedmultiplier = 6;
        //speed = speed * speedmultiplier;
        movingspeed = movingspeed * speedmultiplier;
        yield return new WaitForSeconds(0.5f);
        //speed = speed / speedmultiplier;
        movingspeed = movingspeed / speedmultiplier;
    }
}
