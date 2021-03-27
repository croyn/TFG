using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    /// <summary>
    /// List of all the effects prefab available in the system
    /// </summary>
    public GameObject[] EffectPrefabs;

    /// <summary>
    /// list of the effects already in use
    /// </summary>
    List<EffectType> effectsinuse;

    /// <summary>
    /// singleton to access the features of this function
    /// </summary>
    public static EffectManager instance;

    /// <summary>
    /// reference to the creation table containing the result of the combination of particles
    /// </summary>
    //public CreationTable creationtable;

    /// <summary>
    /// Associate the instance of the singleton and geenrated the different prefab of the available effects.
    /// </summary>
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            DestroyImmediate(this);
        }
        effectsinuse = new List<EffectType>();
        foreach (GameObject pref in EffectPrefabs) {
            GameObject g = GameObject.Instantiate(pref, Vector3.one * 1000, Quaternion.identity, transform);
            g = GameObject.Instantiate(pref, Vector3.one * 1000, Quaternion.identity, transform);
        }

        //readTableOfCreation();
    }

    /// <summary>
    /// Compute the result effect type of two colliding Type II particle
    /// </summary>
    /// <param name="t1"> first particle type</param>
    /// <param name="t2"> seconf particle type</param>
    /// <param name="farinteraction">True if the interaction ivolve two distant whiteholes, false if the interaction is made between the white hole and the player's particles. Default vale false.</param>
    /// <returns></returns>
    /*public EffectType computeEffect(ParticleType t1, ParticleType t2, bool farinteraction = false) {
        string result = "";
        CreationtableItem t = new CreationtableItem(t1, t2);
        foreach (CreationtableItem item in creationtable.content) {
            if (item == t) {
                result = item.result;
                break;
            }
        }

        if (result.StartsWith("generate") && farinteraction)
        {
            return EffectType.generate;
        }
        else {
            if (result.StartsWith("generate")) {
                //only if the effect would be generate but I'm creating it myself on the spot
                result = "explosion";
            }
            return (EffectType)Enum.Parse(typeof(EffectType), result); 
        }
    }*/

    /// <summary>
    /// Perform the effect in the right location
    /// </summary>
    /// <param name="t">type of effect to make</param>
    /// <param name="pos">position in which the effect must be shown</param>
    /// <returns>the object which perform the visual effect</returns>
    public GameObject SetEffect(EffectType t, Vector3 pos) {
        
        GameObject g;
        int index = ((int)t - 1) * 2;
        if (effectsinuse.Contains(t))
        {
            index++;
        }
        else {
            effectsinuse.Add(t);
        }
        g = transform.GetChild(index).gameObject;
        
        g.transform.position = pos;
        g.GetComponent<SpecificEffectManager>().Play();
        StartCoroutine(releaseeffect(t, 10, g));
        return g;
    }


    public GameObject SetEffect(ParticleType t1, ParticleType t2, Vector3 pos)
    {
        GameObject g;
        int index = ((int)t1.GetEffect() - 1) * 2;
        if (effectsinuse.Contains(t1.GetEffect()))
        {
            index++;
        }
        else
        {
            effectsinuse.Add(t1.GetEffect());
        }
        g = transform.GetChild(index).gameObject;

        g.transform.position = pos;
        g.GetComponent<SpecificEffectManager>().Play();
        StartCoroutine(releaseeffect(t1.GetEffect(), 10, g));

        index = ((int)t2.GetEffect() - 1) * 2;
        if (effectsinuse.Contains(t2.GetEffect()))
        {
            index++;
        }
        else
        {
            effectsinuse.Add(t2.GetEffect());
        }
        g = transform.GetChild(index).gameObject;

        g.transform.position = pos;
        g.GetComponent<SpecificEffectManager>().Play();
        StartCoroutine(releaseeffect(t2.GetEffect(), 10, g));

        return g;
    }

    /// <summary>
    /// Determin that the effect can be used for next interaction
    /// </summary>
    /// <param name="t">type of efect that has to be released</param>
    /// <param name="time"> amount of time before releasing</param>
    /// <param name="reward">the object which has shown the visual effect</param>
    /// <returns></returns>
    IEnumerator releaseeffect(EffectType t, float time, GameObject g) {
        yield return new WaitForSeconds(time);
        g.transform.position = Vector3.one * 1000;
        effectsinuse.Remove(t);
    }

    /// <summary>
    /// read the resource file where th combination of elements of the particles have been stored to determine the visual result
    /// </summary>
    /*private void readTableOfCreation() {
        string text = Resources.Load<TextAsset>("CreationTable").ToString();
        creationtable = JsonUtility.FromJson<CreationTable>(text);
    }*/
}
