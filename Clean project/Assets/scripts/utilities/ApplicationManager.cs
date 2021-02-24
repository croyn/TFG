using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    /// <summary>
    /// singleton of the class
    /// </summary>
    public static ApplicationManager instance;

    /// <summary>
    /// identifier of the current world
    /// </summary>
    public int chosenWorld = 0;

    /// <summary>
    /// set of sprites background
    /// TODO: to be removed for a sparkling effect on the floor instead of space
    /// </summary>
    public Sprite[] listofbackgrounds;

    /// <summary>
    /// prefab to place the background for each world
    /// </summary>
    public GameObject worldbackgroundprefab;

    /// <summary>
    /// set the instance of the singleton
    /// </summary>
    void Awake()
    {
        if(instance == null){
            instance = this;
        }else{
            DestroyImmediate(this);
        }

        new WorldParticleAssingment();
        Cursor.visible = false;

    }

    /// <summary>
    /// set up world for each element in the WorldParticleAssingment table
    /// TODO: probably to be changed the part on the background (also in the prefab) for more sparkle like shining ministar animated
    /// </summary>
    void Start() {
        GameObject emptyobject = new GameObject();
        for (int i = 0; i < WorldParticleAssingment.getNumberOfWorld(); i++)
        {
            GameObject g = GameObject.Instantiate(emptyobject);
            g.transform.parent = ParticlePoolsManager.instance.transform;
            g.name = "world " + i;
            g.transform.localPosition = Vector3.zero;

            GameObject background = GameObject.Instantiate(worldbackgroundprefab, g.transform);
            background.transform.parent = ObjectGeneratorManager.instance.transform;
            background.name = "world " + i;
            //background.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = listofbackgrounds[i];
            background.transform.localPosition = Vector3.zero;

            // Add a random ambient sound to each world
            AudioSource audio = background.AddComponent<AudioSource>();
            audio.loop = true;
            string path = "Sounds/ambient_";
            path += (Random.Range(0, 4)).ToString();
            AudioClip audioClip = Resources.Load<AudioClip>(path);
            audio.clip = audioClip;
            audio.volume = .5f;
            audio.Play();

            g = GameObject.Instantiate(emptyobject);
            g.transform.parent = RewardManager.instance.transform;
            g.name = "world " + i;
            g.transform.localPosition = Vector3.zero;

        }
        emptyobject.hideFlags = HideFlags.HideInHierarchy;
        ParticlePoolsManager.instance.ChangeWorld();
        ObjectGeneratorManager.instance.ChangeWorld();
        RewardManager.instance.ChangeWorld();
    }

    /// <summary>
    /// check for the keypad commands (emenrgency commands)
    /// </summary>
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    /// <summary>
    /// Perform the changes to the world
    /// </summary>
    /// <param name="newWorld">identifier of the new world</param>
    public void ChangeWorld(int newWorld) {
        chosenWorld = newWorld;
        RandomObjectmanager.instance.ChangeWorld();
        ParticlePoolsManager.instance.ChangeWorld();
        ObjectGeneratorManager.instance.ChangeWorld();
        RewardManager.instance.ChangeWorld();
    }

    /// <summary>
    /// return the background associated to that world
    /// </summary>
    /// <param name="worldid">index of the required world</param>
    /// <returns></returns>
    public Sprite getBackground(int worldid) {
        return listofbackgrounds[worldid];
    }

    public static bool isOutOfGameSpace(GameObject g) {
        return g.transform.position.x <= 0 || g.transform.position.x >= 100 || g.transform.position.z <= 0 || g.transform.position.z >= 100;
    }
    public static bool isOutOfGameSpace(Transform t)
    {
        return t.position.x <= 0 || t.position.x >= 100 || t.position.z <= 0 || t.position.z >= 100;
    }
}
