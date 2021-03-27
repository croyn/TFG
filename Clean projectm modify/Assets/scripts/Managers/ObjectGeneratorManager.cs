using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(Audiopan))]
[RequireComponent(typeof(AudioSource))]
public class ObjectGeneratorManager : MonoBehaviour
{
    /// <summary>
    /// reference to the first White Hole that generated the particle flow 
    /// </summary>
    public ParticlePoolManager p1;
    /// <summary>
    /// reference to the second White Hole that generated the particle flow 
    /// </summary>
    public ParticlePoolManager p2;

    /// <summary>
    /// singleton to access the features of this function
    /// </summary>
    public static ObjectGeneratorManager instance;

    /// <summary>
    /// list of planet prefab that can be genrated
    /// </summary>
    public GameObject[] prefabPlanetToGenerate;

    /// <summary>
    /// list of star prefab that can be genrated
    /// </summary>
    public GameObject[] prefabStarToGenerate;

    /// <summary>
    /// tunnel prefab that can be genrated
    /// </summary>
    public GameObject prefabTunnel;

    /// <summary>
    /// used to pass the information to the new blackhole on which world to transfer things
    /// </summary>
    private int blackholeworld = -1;

    /// <summary>
    /// the materials used for the planets
    /// </summary>
    public Material planetmaterial;

    /// <summary>
    /// reference to the prefab of the moon to be used to add to the planet
    /// </summary>
    public GameObject moonprefab;

    /// <summary>
    /// reference to the prefab of the ring to be used to add to the planet
    /// </summary>
    public GameObject ringprefab;

    /// <summary>
    /// reference to the audioplayer surce used for creation a nd destruction sounds
    /// </summary>
    [HideInInspector]
    public AudioSource audioplayer;

    /// <summary>
    /// Associate the instance of the singleton and prepare the initial world
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            DestroyImmediate(this);
        }
    }

    void Start() {
        audioplayer = GetComponent<AudioSource>();
    }

    /// <summary>
    /// check if two pools have been fired close eought and in case ask what effect has to be shown.
    /// In case effect is a geenration, find the correct element in the list of prefab to be geenrated and place it in the middle of the two pools.
    /// Then it redirects the particle flows to the effect.
    /// Finally search for "out of border" eleemnts and destroy them
    /// </summary>
    void Update()
    {
        if (p1 != null && p2 != null) {

            if (((int)p1.type + (int)p2.type )%2 == 0)
            {
                GenerateStar();
                
            }
            else {
                GenerateEffect();
            }

        }
        List<GameObject> toDestroy = new List<GameObject>();
        for (int i = 0; i < transform.GetChild(ApplicationManager.instance.chosenWorld).childCount; i++)
        {
            if (Vector3.Distance(transform.position, transform.GetChild(ApplicationManager.instance.chosenWorld).GetChild(i).position) > 45)
            {
                toDestroy.Add(transform.GetChild(ApplicationManager.instance.chosenWorld).GetChild(i).gameObject);
            }
        }
        for(int i = toDestroy.Count-1; i > -1;i--){
            GameObject g = toDestroy.ElementAt(0);
            if(g.GetComponent<StarManager>() != null){
                CreationEffectManager.instance.DestructionEffect(g.transform.localPosition, RoamingObjectType.Star, g.transform.GetChild(g.transform.childCount-1).GetComponent<MeshRenderer>().material.color);
            }else{
                CreationEffectManager.instance.DestructionEffect(g.transform.localPosition, RoamingObjectType.Planet, g.transform.GetChild(0).GetComponent<MeshRenderer>().material.color);
            }
            GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerStateManager>().releaseObject(g);
            GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerStateManager>().releaseObject(g);
            GameObject.Destroy(toDestroy.ElementAt(0));
        }
    }

    /// <summary>
    /// Record that a whitehole has been activated
    /// </summary>
    /// <param name="particlepool">reference to the activated white hole</param>
    public void ParticleBoolFired(ParticlePoolManager particlepool)
    {
        
        if (p1 == null)
        {
            p1 = particlepool;
        }
        else
        {
            if (p2 == null)
            {
                p2 = particlepool;
            }
        }
    }

    /// <summary>
    /// remove the particle pool from the activated ones
    /// </summary>
    /// <param name="particletype"></param>
    public void ClearPool(ParticleType particletype) {
        if (p1 != null)
        {
            if (p1.type == particletype)
            {
                p1 = p2;
                p2 = null;
            }
        }
        if (p2 != null)
        {
            if (p2.type == particletype)
            {
                p2 = null;
            }
        }
    }

    /// <summary>
    /// Determine the element that should be generated
    /// </summary>
    /// <param name="t1">first type of particle to compose</param>
    /// <param name="t2">second type of particle to compose</param>
    /// <returns>the object type to be generated</returns>
    /*GenerableElements getGeneratedElement(ParticleType t1, ParticleType t2) {
        string result = "";
        CreationtableItem t = new CreationtableItem(t1, t2);
        foreach (CreationtableItem item in EffectManager.instance.creationtable.content)
        {
            if (item == t) {
                result = item.result;
                break;
            }
        }
        blackholeworld = -1;
        if (result.StartsWith("generate"))
        {
            result = result.Split('_').Last();
            if(result.StartsWith("blackhole")){
                blackholeworld = int.Parse(result.Substring(result.Length-2));
                result = result.Substring(0,result.Length-2);
            }
        }
        else {
            result = "none";
        }
        return (GenerableElements)Enum.Parse(typeof(GenerableElements), result);
    }*/

    /// <summary>
    /// transform a roaming element into a planet
    /// </summary>
    /// <param name="asteroid">the gameobject to substitute</param>
    public GameObject SubstitutePlanetToAsteroid(GameObject asteroid, GameObject player)
    {
        CreationEffectManager.instance.GenerationEffect(asteroid.transform.localPosition, asteroid.GetComponent<SingleRoamingElementManager>().type);
        audioplayer.panStereo = -asteroid.transform.localPosition.z / 50;
        audioplayer.PlayOneShot(RoamingObjectType.Planet.GetCreationAudio(), 1);
        GameObject g = GameObject.Instantiate(prefabPlanetToGenerate[0]);
        g.name = g.name + g.GetInstanceID();
        g.transform.parent = transform.GetChild(ApplicationManager.instance.chosenWorld);
        g.transform.position = asteroid.transform.position;
        asteroid.transform.position = Vector3.one * 150;
        List<ParticleType> tps = GeneratePlanet(player, g, asteroid.GetComponent<SingleRoamingElementManager>().type);
        Logger.AddPlanetCretedEventOnLog(player.name, tps.ToArray(), g.name, asteroid.GetComponent<SingleRoamingElementManager>().type, g.transform.position);
        g.layer = LayerMask.NameToLayer("Planets");
        return g;
    }

    /// <summary>
    /// Deactivate the object in the current environment and activate the ones in the new environment
    /// </summary>
    public void ChangeWorld()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == ApplicationManager.instance.chosenWorld)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// transfer a planet or star to another enviornemnt
    /// </summary>
    /// <param name="newworld"> the identifier of the new world in which to move the object</param>
    /// <param name="tomove">object to be moved</param>
    public void MoveToAnotherWorld(int newworld, GameObject tomove)
    {
        tomove.transform.parent = transform.GetChild(newworld);
        Vector2 v = UnityEngine.Random.onUnitSphere * 20;
        tomove.transform.localPosition += new Vector3(v.x, 0, v.y);
        if (Vector3.Distance(tomove.transform.localPosition, Vector3.zero) > 45) {
            tomove.transform.localPosition -= new Vector3(v.x, 0, v.y) * 2;
        }
    }

    /// <summary>
    /// Transform an object into a black hole
    /// </summary>
    /// <param name="p">ne index of the new world the black hole will move to</param>
    /// <param name="gg">the object to be substituted with the black hole</param>
    public void CreateBlackHole(int p, GameObject gg)
    {
        audioplayer.panStereo = -gg.transform.localPosition.z / 50;
        audioplayer.PlayOneShot(RoamingObjectType.Blackhole.GetCreationAudio(), 1);
        CreationEffectManager.instance.DestructionEffect(gg.transform.localPosition,RoamingObjectType.Blackhole, gg.transform.GetChild(gg.transform.childCount-1).GetComponent<MeshRenderer>().material.color);
        GameObject g = GameObject.Instantiate(prefabTunnel);
        g.name = g.name + g.GetInstanceID();
        g.transform.parent = transform.GetChild(ApplicationManager.instance.chosenWorld);
        //g.GetComponent<TunnelManager>().newWorld = p;
        g.GetComponent<TunnelManager>().SetUp(p, true);
        g.transform.position = gg.transform.position + Vector3.down*3;
        GameObject.Destroy(gg);

        g = GameObject.Instantiate(prefabTunnel);
        g.name = g.name + g.GetInstanceID();
        g.transform.parent = transform.GetChild(p);
        g.transform.position = gg.transform.position + Vector3.down * 3;
        //reward.GetComponent<TunnelManager>().newWorld = ApplicationManager.instance.chosenWorld;
        g.GetComponent<TunnelManager>().SetUp(ApplicationManager.instance.chosenWorld, false);

        RewardManager.instance.AddReward(p, g);

        List<string> names = new List<string>();
        for(int i = 0; i<gg.transform.GetChild(0).childCount; i++){
            names.Add(gg.transform.GetChild(0).GetChild(i).name);
        }
        Logger.AddCompleteSolarSystemEventOnLog(gg.name, names.ToArray(), g.name, g.transform.position);
        
    }

    /// <summary>
    /// generate the visual properties of the planets accordign to the player's particles
    /// </summary>
    /// <param name="player">the reference to the player who created the planet</param>
    /// <param name="planet">the panet to act on</param>
    public List<ParticleType> GeneratePlanet(GameObject player, GameObject planet, RoamingObjectType collider)
    {
        List<ParticleType> types = new List<ParticleType>();
        int[] generatorsamount = new int[Enum.GetValues(typeof(ParticleType)).Length - 1];
        float[] generatorsavglife = new float[Enum.GetValues(typeof(ParticleType)).Length - 1];
        for (int i = 0; i < Enum.GetValues(typeof(ParticleType)).Length - 1; i++ )
        {
            generatorsamount[i] = 0;
            generatorsavglife[i] = 0;
        }
        for (int i = 0; i < player.transform.childCount; i++)
        {
            if (player.transform.GetChild(i).GetComponent<ParticleBehaviour>() != null)
            {
                ParticleBehaviour p = player.transform.GetChild(i).GetComponent<ParticleBehaviour>();
                generatorsamount[(int)p.type - 1] = generatorsamount[(int)p.type - 1] + 1;
                generatorsavglife[(int)p.type-1] = generatorsavglife[(int)p.type-1] + (float)(p.lifespan - generatorsavglife[(int)p.type-1]) / (float)generatorsamount[(int)p.type-1];
            }
        }
        int maxindex = 0;
        for (int i = 1; i < Enum.GetValues(typeof(ParticleType)).Length; i++)
        {
            if (generatorsamount[i-1] > generatorsamount[maxindex])
            {
                maxindex = i-1;
            }
        }
        int countmoon = 0;
        int countring = 0;
        for (int i = 1; i < Enum.GetValues(typeof(ParticleType)).Length; i++)
        {
            
            if ((i-1) == maxindex)
            {
                types.Insert(0, (ParticleType)(i));
                //change shape
                planet.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh = collider.GetMesh();
                

                //planet.GetComponent<MeshRenderer>().material = planetmaterial;
                planet.transform.GetChild(0).GetComponent<MeshRenderer>().material = ((ParticleType)i).GetMaterial();
                //NB: *2 in the color set the emission intensity to 1.4 due to internal conversion.
                //planet.GetComponent<MeshRenderer>().material.color = ((ParticleType)i).GetColor() * 2;
                planet.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.clear;
                planet.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", ((ParticleType)i).GetColor() * 1f);
                planet.transform.GetChild(0).localScale = Vector3.one * 0.7f;
            }
            else {
                if (generatorsavglife[i - 1] < ParticleBehaviour.maxlifespan / 2)
                {
                    if(generatorsavglife[i - 1] > 0){
                        countring++;
                        types.Add((ParticleType)(i));
                        GameObject g = GameObject.Instantiate(ringprefab);
                        g.transform.parent = planet.transform.GetChild(1);
                        foreach (MeshRenderer m in g.transform.GetChild(0).GetComponentsInChildren<MeshRenderer>())
                        {
                            //m.material = planetmaterial;
                            m.material.color = ((ParticleType)i).GetColor();
                        }
                        //g.GetComponent<MeshRenderer>().material.color = ((ParticleType)i).GetColor();
                        g.transform.localScale = Vector3.one * 0.04f;
                        g.transform.localPosition = Vector3.zero;
                        g.transform.localRotation = Quaternion.Euler(Vector3.forward * (countmoon) * 30 * (countmoon % 2 == 0 ? -1 : 1));
                    }
                }
                else {
                    countmoon++;
                    types.Add((ParticleType)(i));
                    GameObject g = GameObject.Instantiate(moonprefab);
                    g.transform.parent = planet.transform.GetChild(1);
                    g.GetComponent<MeshRenderer>().material = planetmaterial;
                    g.GetComponent<MeshRenderer>().material.color = ((ParticleType)i).GetColor();
                    g.transform.localScale = Vector3.one * 0.5f;
                    g.transform.localPosition = Vector3.right * ((countmoon) * 0.6f + 0.2f) * (countmoon % 2 == 0 ? -1 : 1);
                    g.GetComponent<OrbitBehaviour>().orbitalDistance = (countmoon) * 0.8f;
                }
                
            }
        }
        planet.GetComponent<GeneratedObjectMovementmanager>().Composedtypes = types.ToArray();
        //change size of planet according to number of types
        planet.transform.localScale = Vector3.one * (2.4f + 0.2f * types.Count);
        return types;
    }

    /// <summary>
    /// dynamic generation of a star according to the two white holes fired
    /// </summary>
    public void GenerateStar() {
        CreationEffectManager.instance.GenerationEffect((p1.transform.localPosition + p2.transform.localPosition) / 2, RoamingObjectType.Star);
        audioplayer.panStereo = -((p1.transform.localPosition + p2.transform.localPosition) / 2).z / 50;
        audioplayer.PlayOneShot(RoamingObjectType.Star.GetCreationAudio(), 1);
        GameObject g = GameObject.Instantiate(prefabStarToGenerate[0]);
        g.name = g.name + g.GetInstanceID();

        g.transform.GetChild(g.transform.childCount-1).GetComponent<MeshRenderer>().material.color = (p1.type.GetColor() + p2.type.GetColor()) / 2;

        g.transform.GetChild(g.transform.childCount-1).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", (p1.type.GetColor() + p2.type.GetColor()) / 2);


        g.transform.GetChild(g.transform.childCount - 1).GetComponent<MeshRenderer>().material.color = (p1.type.GetColor() + p2.type.GetColor()) / 2;
        g.transform.GetChild(g.transform.childCount - 1).localScale = Vector3.one * (p1.type.GetMass() + p2.type.GetMass());
        g.GetComponent<StarManager>().setOrbit(Mathf.RoundToInt(p1.type.GetOrbitValue() + p2.type.GetOrbitValue()));
        g.transform.position = (p1.transform.position + p2.transform.position) / 2;
        g.transform.parent = transform.GetChild(ApplicationManager.instance.chosenWorld);

        Logger.AddStarCretedEventOnLog(g.name, Mathf.RoundToInt(p1.type.GetOrbitValue() + p2.type.GetOrbitValue()), g.transform.position);

        p1.redirectFlow(g.transform);
        p2.redirectFlow(g.transform);
        p1.StopParticles();
        p2.StopParticles();
        p1 = null;
        p2 = null;

        StartCoroutine(setStarUnableToAbsorbParticles(g));
    }

    private IEnumerator setStarUnableToAbsorbParticles(GameObject g)
    {
        yield return new WaitForSeconds(1.5f);
        g.layer = LayerMask.NameToLayer("ExistingStar");
        for (int i = 0; i < g.transform.childCount; i++) {
            g.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("ExistingStar");
        }
    }

    /// <summary>
    /// dynamic generatio of the effects on clashihg particle types on whiteholes
    /// </summary>
    public void GenerateEffect()
    {
        GameObject g = EffectManager.instance.SetEffect(p1.type, p2.type, (p1.transform.position + p2.transform.position) / 2);

        Logger.AddWhiteHoleUsedCollaborativelyEventOnLog(p1.type, p2.type, p1.transform.position, p2.transform.position);

        p1.redirectFlow(g.transform);
        p2.redirectFlow(g.transform);
        p1 = null;
        p2 = null;
    }
}

/// <summary>
/// list of possible elements to be generated (+ none as a default case)
/// </summary>
public enum GenerableElements {
    none, star, twinstar, gigastar, smallstar, bluedwarf, nebula, pulsar, blackhole
}
