using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Audiopan))]
[RequireComponent(typeof(AudioSource))]
public class RandomObjectmanager : MonoBehaviour
{
    /// <summary>
    /// singleton to access the features of this function
    /// </summary>
    public static RandomObjectmanager instance;

    /// <summary>
    /// list of the avaialble prefab to be created
    /// </summary>
    public GameObject[] listOfPrefab;

    /// <summary>
    /// number of radom meteoroid to be created
    /// </summary>
    public int spawncount_meteoroid;

    /// <summary>
    /// number of radom comet to be created
    /// </summary>
    public int spawncount_comet;

    /// <summary>
    /// number of radom asteroids to be created
    /// </summary>
    public int spawncount_asteroids;

    Dictionary<RoamingObjectType, int> spawncounts;
    Dictionary<RoamingObjectType, int> Existingcounts;
    Dictionary<RoamingObjectType, int> Poolcounts;

    [HideInInspector]
    public AudioSource audioplayer;

    bool initialpalcementended = false;

    /// <summary>
    /// Associate the instance of the singleton and generated indicated numebr of items.
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
        spawncounts = new Dictionary<RoamingObjectType, int>();
        Existingcounts = new Dictionary<RoamingObjectType, int>();
        Poolcounts = new Dictionary<RoamingObjectType, int>();
        spawncounts.Add(RoamingObjectType.Asteroid, spawncount_asteroids);
        spawncounts.Add(RoamingObjectType.Comet, spawncount_comet);
        spawncounts.Add(RoamingObjectType.Meteoroid, spawncount_meteoroid);
        Existingcounts.Add(RoamingObjectType.Asteroid, 0);
        Existingcounts.Add(RoamingObjectType.Comet, 0);
        Existingcounts.Add(RoamingObjectType.Meteoroid, 0);
        Poolcounts.Add(RoamingObjectType.Asteroid, 0);
        Poolcounts.Add(RoamingObjectType.Comet, 0);
        Poolcounts.Add(RoamingObjectType.Meteoroid, 0);
        StartCoroutine("initialGeneration");
        audioplayer = GetComponent<AudioSource>();
        
    }

    void Update() {
        if (initialpalcementended)
        {
            Existingcounts[RoamingObjectType.Asteroid] = 0;
            Existingcounts[RoamingObjectType.Comet] = 0;
            Existingcounts[RoamingObjectType.Meteoroid] = 0;

            for (int i = 1; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<SingleRoamingElementManager>() != null)
                {
                    Existingcounts[transform.GetChild(i).GetComponent<SingleRoamingElementManager>().type]++;
                }
                if (transform.GetChild(i).GetComponent<DestroyingElement>() != null)
                {
                    Existingcounts[transform.GetChild(i).GetComponent<DestroyingElement>().type]++;
                }
            }

            foreach (RoamingObjectType r in spawncounts.Keys)
            {
                if (spawncounts[r] > Existingcounts[r])
                {
                    Transform t = null;
                    if (Poolcounts[r] > 0)
                    {
                        for (int i = 0; i < transform.GetChild(0).childCount; i++)
                        {
                            if (transform.GetChild(0).GetChild(i).GetComponent<SingleRoamingElementManager>() != null)
                            {
                                if (transform.GetChild(0).GetChild(i).GetComponent<SingleRoamingElementManager>().type == r)
                                {
                                    t = transform.GetChild(0).GetChild(i);
                                }
                            }
                            if (transform.GetChild(0).GetChild(i).GetComponent<DestroyingElement>() != null)
                            {
                                if (transform.GetChild(0).GetChild(i).GetComponent<DestroyingElement>().type == r)
                                {
                                    t = transform.GetChild(0).GetChild(i);
                                }
                            }
                        }
                        t.parent = transform;
                        resetDirection(t.gameObject, 0, 1);
                    }
                    else
                    {
                        int index = 0;
                        switch (r)
                        {
                            case RoamingObjectType.Asteroid: index = 0; break;
                            case RoamingObjectType.Comet: index = 2; break;
                            case RoamingObjectType.Meteoroid: index = 1; break;
                        }
                        GameObject g = GameObject.Instantiate(listOfPrefab[index]);
                        g.transform.parent = transform;
                        g.transform.localPosition = new Vector3(50 * index, 0, 50);
                    }
                }
            }
        }
    }

    /// <summary>
    /// reset all the randomly moving objects
    /// </summary>
    public void ChangeWorld()
    {
        for (int i = 1; i < transform.childCount; i++) {
            transform.GetChild(i).localPosition = Vector3.right * 150 * (i + 1);
        }
    }

    /// <summary>
    /// instantiate in randomic order the roaming objects
    /// </summary>
    /// <returns></returns>
    IEnumerator initialGeneration() {
        int[] qtns = new int[] { spawncount_asteroids, spawncount_meteoroid, spawncount_comet };

        for (int i = 0; i < spawncount_asteroids + spawncount_meteoroid + spawncount_comet; i++) {
            int index = Random.Range(0, 3);
            if (qtns[index] > 0)
            {
                GameObject g = GameObject.Instantiate(listOfPrefab[index]);
                g.transform.parent = transform;
                if (index == 0)
                {
                    g.transform.localPosition = new Vector3(-100, 0, 0);
                }
                else {
                    g.transform.localPosition = new Vector3(50 * index, 0, 50);
                }
                qtns[index]--;
                yield return new WaitForSeconds(Random.Range(0, 5.0f));
            }
            else {
                i--;
            }
        }
        initialpalcementended = true;
    }

    public void resetDirection(GameObject g, int minresettime, int maxresettime) {
        int count = 0;
        if (g.GetComponent<SingleRoamingElementManager>() != null)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<SingleRoamingElementManager>() != null)
                {
                    if (transform.GetChild(i).GetComponent<SingleRoamingElementManager>().type == g.GetComponent<SingleRoamingElementManager>().type)
                    {
                        count++;
                    }
                }
            }

            if (count <= spawncounts[g.GetComponent<SingleRoamingElementManager>().type])
            {
                StartCoroutine(restart(g, minresettime, maxresettime));
            }
            else
            {
                g.transform.parent = transform.GetChild(0);
                g.transform.localPosition = new Vector3(50 * (int)g.GetComponent<SingleRoamingElementManager>().type, 0, 50);
                Poolcounts[g.GetComponent<SingleRoamingElementManager>().type]++;
            }
        }
        if (g.GetComponent<DestroyingElement>() != null)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<DestroyingElement>() != null)
                {
                    if (transform.GetChild(i).GetComponent<DestroyingElement>().type == g.GetComponent<DestroyingElement>().type)
                    {
                        count++;
                    }
                }
            }
            if (count <= spawncounts[g.GetComponent<DestroyingElement>().type])
            {
                StartCoroutine(restart(g, minresettime, maxresettime));
            }
            else
            {
                g.transform.parent = transform.GetChild(0);
                g.transform.localPosition = new Vector3(50 * (int)g.GetComponent<DestroyingElement>().type, 0, 50);
                Poolcounts[g.GetComponent<DestroyingElement>().type]++;
            }
        }
    }

    public void perfromDestructionSound(RoamingObjectType t, Vector3 position)
    {
        audioplayer.panStereo = -position.z / 50;
        audioplayer.PlayOneShot(t.GetDestructionAudio(), 1);
    }

    /// <summary>
    /// modify the number of roaming element avaialble in the game (shared among the different worlds
    /// The function do not apply immediately for reduction, but wait for the moment the elements of the right type exit the vision space
    /// </summary>
    /// <param name="t">type of elements (asteroid, meteoroid or comet)</param>
    /// <param name="amount">the new number of items to be spawned</param>
    public void SetNumberOfRoamingElement(RoamingObjectType t, int amount) {
        if(spawncounts.ContainsKey(t)){
            spawncounts[t] = amount;
        }
    }


    /// <summary>
    /// wait a random amount of time (between 5 and 30 seconds) then reset the movement and position
    /// </summary>
    /// <returns></returns>
    IEnumerator restart(GameObject g, int minresettime, int maxresettime)
    {
        if (g.tag == "DestructionElement")
        {
            g.GetComponent<DestroyingElement>().StopMovingAudio();
        }
        if (g.transform.childCount > 0)
        {
            g.transform.GetChild(0).gameObject.SetActive(false);
            if (g.transform.GetChild(0).GetComponent<ParticleSystem>() != null)
            {
                ParticleSystem.EmissionModule em = g.transform.GetChild(0).GetComponent<ParticleSystem>().emission;
                em.rateOverDistance = 0;
            }
        }
        float f = Random.Range(minresettime, maxresettime);
        if (g.tag != "DestructionElement")
        {
            yield return new WaitForSeconds(f);
        }
        else {
            AudioClip c = RoamingObjectType.Asteroid.GetCreationAudio();
            yield return new WaitForSeconds(f - c.length);
            audioplayer.panStereo = -g.transform.localPosition.z / 50;
            audioplayer.PlayOneShot(RoamingObjectType.Asteroid.GetCreationAudio(), 1);
            yield return new WaitForSeconds(c.length);
            g.GetComponent<DestroyingElement>().playMovingAudio();
        }
        g.GetComponent<PassingMovement>().setup();
    }
}
