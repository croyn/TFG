using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ParticlePoolsManager : MonoBehaviour
{
    /// <summary>
    /// reference to the instance of the pool manager
    /// </summary>
    public static ParticlePoolsManager instance;

    /// <summary>
    /// number of simulaneously visible pools
    /// </summary>
    public int numberOfPoolVisible;

    /// <summary>
    /// list of particle type avaliable in the world
    /// </summary>
    public ParticleType[] avaialbletype;

    /// <summary>
    /// list of particle type not yet present in the space
    /// </summary>
    private List<ParticleType> possibleTypeToCreate = new List<ParticleType>();

    /// <summary>
    /// prefab to build the particle pools
    /// </summary>
    public GameObject particlePoolPrefab;

    /// <summary>
    /// set of positions to make the pools appear
    /// </summary>
    private Vector3[] placement_positions;

    /// <summary>
    /// dictioanry containing the available indexes and the corresponding location in the space
    /// </summary>
    private Dictionary<int, Vector3> available_positions;

    /// <summary>
    /// dictioanry associating to each world the set of available positions
    /// </summary>
    private Dictionary<int, Vector3[]> world_available_positions = new Dictionary<int, Vector3[]>();

    /// <summary>
    /// reference to the two players' position
    /// </summary>
    [HideInInspector]
    public Transform Player1, Player2;


    /// <summary>
    /// get the proper referencies and then set up particles for the current initial world
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            GameObject.DestroyImmediate(this);
        }
        Player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        Player2 = GameObject.FindGameObjectWithTag("Player2").transform;

        placement_positions = new Vector3[8];
        world_available_positions = new Dictionary<int, Vector3[]>();
        available_positions = new Dictionary<int,Vector3>();
    }

    /// <summary>
    /// set the placement positions for the current world
    /// </summary>
    /// <param name="worldindex">index of the world to be set</param>
    private void setPlacementPosition(int worldindex) {
        available_positions.Clear();
        float angletomove = 360f / placement_positions.Length;
        float randomangle = UnityEngine.Random.Range(-angletomove/2, angletomove/2);
        for (int i = 0; i < placement_positions.Length; i++)
        {
            float randomlenght = UnityEngine.Random.Range(20, 41);
            placement_positions[i] = new Vector3(Mathf.Cos((angletomove * i + randomangle) * Mathf.Deg2Rad) * randomlenght, 0, Mathf.Sin((angletomove * i + randomangle) * Mathf.Deg2Rad) * randomlenght);
            available_positions.Add(i, placement_positions[i]);
        }
        world_available_positions.Add(ApplicationManager.instance.chosenWorld, placement_positions);
    }

    /// <summary>
    /// generate and position the pools in the world (only the first time)
    /// </summary>
    private void initialPlacement() {
        for (int i = 0; i < numberOfPoolVisible; i++) {
            GameObject g = GameObject.Instantiate(particlePoolPrefab);
            placeOnePool(Mathf.CeilToInt(placement_positions.Length/numberOfPoolVisible) * i, g, true);
        } 
    }

    /// <summary>
    /// place one pool in the new position
    /// </summary>
    /// <param name="position">index of the position array to move the pool</param>
    /// <param name="reward">the pool to move</param>
    private void placeOnePool(int position, GameObject g, bool firsttime = false)
    {
        ParticleType p = possibleTypeToCreate.ElementAt(UnityEngine.Random.Range(0, possibleTypeToCreate.Count));
        possibleTypeToCreate.Remove(p);
        available_positions.Remove(position);
        g.GetComponent<ParticlePoolManager>().type = p;
        g.transform.parent = transform.GetChild(ApplicationManager.instance.chosenWorld);
        g.transform.localPosition = placement_positions[position];
        if (g.transform.GetChild(0).GetComponent<ParticleSystem>() != null)
        {
            ParticleSystem.MainModule m = g.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            if (firsttime)
            {
                float r = UnityEngine.Random.Range(0, 15);
                m.startDelay = r;
                g.GetComponent<SphereCollider>().enabled = false;
                StartCoroutine(reactivateCollider(g, r));
            }
            else {
                m.startDelay = 0;
            }
        }
    }

    private IEnumerator reactivateCollider(GameObject g, float time)
    {
        yield return new WaitForSeconds(time);
        g.GetComponent<SphereCollider>().enabled = true;
    }

    /// <summary>
    /// change one pool's position after being emptied
    /// </summary>
    /// <param name="g">the pool to move</param>
    public void changeOnePoolPosition(GameObject g)
    {
        possibleTypeToCreate.Add(g.GetComponent<ParticlePoolManager>().type);

        for (int i = 0; i < placement_positions.Length; i++)
        {
            if (Vector3.Distance(placement_positions[i], g.transform.localPosition) < 1)
            {
                available_positions.Add(i, placement_positions[i]);
                break;
            }
        }

        int newindex = 0;
        float maxDistance = Mathf.NegativeInfinity;
        foreach ( int i in available_positions.Keys)
        {
            Vector3 shift = new Vector3(50,0,50);
            float dist = Vector3.Distance(Player1.position - shift, available_positions[i]) + Vector3.Distance(Player2.position - shift, available_positions[i]);
            if(dist > maxDistance){
                newindex = i;
                maxDistance = dist;
            }
            
        }
        placeOnePool(newindex, g);
    }

    /// <summary>
    /// Deactivate the White holes of a world and create the ones of the new world
    /// </summary>
    public void ChangeWorld()
    {
        for (int i = 0; i < transform.childCount; i++) {
            if (i == ApplicationManager.instance.chosenWorld)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                avaialbletype = WorldParticleAssingment.gettheworldparticles(i);
                possibleTypeToCreate.Clear();
                possibleTypeToCreate.AddRange(avaialbletype);

                if(world_available_positions.ContainsKey(i)){
                    placement_positions = world_available_positions[i];
                }else{
                    setPlacementPosition(i);
                }

                if (transform.GetChild(i).childCount < numberOfPoolVisible)
                {
                    //if the number of child is = numberOfPoolVisible means that the pools have already been geenrated for the world
                    initialPlacement();
                }
                for (int j = 0; j < transform.GetChild(i).childCount; j++) {
                    transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                }
            }
            else {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void StopPoolsInTheWorld() {
        for (int j = 0; j < transform.GetChild(ApplicationManager.instance.chosenWorld).childCount; j++)
        {
            transform.GetChild(ApplicationManager.instance.chosenWorld).GetChild(j).gameObject.SetActive(false);
        }
    }

    public void Pulsate(ParticleType type) {
        if (type == ParticleType.All)
        {
            for (int j = 0; j < transform.GetChild(ApplicationManager.instance.chosenWorld).childCount; j++)
            {
                transform.GetChild(ApplicationManager.instance.chosenWorld).GetChild(j).GetComponent<ParticlePoolManager>().StartPulsating(0.5f, true);
            }
        }
        else {
            int inttype = (int)type;
            for (int i = 1; i < Enum.GetValues(typeof(ParticleType)).Length; i++)
            {
                if ((i%2 == inttype%2) && i != inttype)
                {
                    for (int j = 0; j < transform.GetChild(ApplicationManager.instance.chosenWorld).childCount; j++)
                    {
                        if (transform.GetChild(ApplicationManager.instance.chosenWorld).GetChild(j).GetComponent<ParticlePoolManager>().type == (ParticleType)i)
                        {
                            transform.GetChild(ApplicationManager.instance.chosenWorld).GetChild(j).GetComponent<ParticlePoolManager>().StartPulsating(0.5f,true);
                        }
                    }
                }
            }
        }
    }
    public void StopPulsate()
    {
        for (int j = 0; j < transform.GetChild(ApplicationManager.instance.chosenWorld).childCount; j++)
        {
            transform.GetChild(ApplicationManager.instance.chosenWorld).GetChild(j).GetComponent<ParticlePoolManager>().StopPulsating(true);
        }
    }

    /// <summary>
    /// bring one random pool closer to the position
    /// </summary>
    /// <param name="position">the position in which the player is</param>
    /// <param name="threshold">the radius of the circle around position on which place the white hole</param>
    public void BringCloser(Vector3 position, float threshold)
    {
        StartCoroutine(bringCloserToPlace(position, threshold));
    }

    private IEnumerator bringCloserToPlace(Vector3 position, float threshold)
    {
        GameObject g = transform.GetChild(ApplicationManager.instance.chosenWorld).GetChild(UnityEngine.Random.Range(0, transform.GetChild(ApplicationManager.instance.chosenWorld).childCount)).gameObject;
        for (int i = 0; i < placement_positions.Length; i++)
        {
            if (Vector3.Distance(placement_positions[i], g.transform.localPosition) < 1)
            {
                available_positions.Add(i, placement_positions[i]);
                break;
            }
        }
        g.GetComponent<ParticlePoolManager>().resetPosition();
        float angle = UnityEngine.Random.Range(0, 360);
        Vector3 pos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle)) * threshold;
        yield return new WaitUntil(() => g.GetComponent<ParticlePoolManager>().isreadytoburst == PoolStates.idle);
        possibleTypeToCreate.Add(g.GetComponent<ParticlePoolManager>().type);
        ParticleType p = possibleTypeToCreate.ElementAt(UnityEngine.Random.Range(0, possibleTypeToCreate.Count));
        possibleTypeToCreate.Remove(p);
        g.GetComponent<ParticlePoolManager>().type = p;
        g.transform.parent = transform.GetChild(ApplicationManager.instance.chosenWorld);
        g.transform.position = position + pos;
        if (g.transform.GetChild(0).GetComponent<ParticleSystem>() != null)
        {
            ParticleSystem.MainModule m = g.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            m.startDelay = 0;
        }
    }

    public void changeStartedAlone() {
        for (int j = 0; j < transform.GetChild(ApplicationManager.instance.chosenWorld).childCount; j++)
        {
            transform.GetChild(ApplicationManager.instance.chosenWorld).GetChild(j).GetComponent<ParticlePoolManager>().changestartedalone(); ;
        }
    }
}
