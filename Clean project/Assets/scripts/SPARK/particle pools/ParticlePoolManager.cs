using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public class ParticlePoolManager : MonoBehaviour
{
    /// <summary>
    /// the type of particle to be geenrated
    /// </summary>
    public ParticleType type;

    /// <summary>
    /// time to replentish the pool
    /// </summary>
    public float reloadTime;

    /// <summary>
    /// time to load the pool
    /// </summary>
    public float activationTime;

    /// <summary>
    /// maximim number of particles to be fired per second
    /// </summary>
    private static float maxratio;

    /// <summary>
    /// reduction ratio of emitted aprticles in time
    /// </summary>
    public float decreaseRatio;

    /// <summary>
    /// current production ratio
    /// </summary>
    private float updatedratio;

    /// <summary>
    /// amount of time passed from the last state change
    /// </summary>
    public float time;

    /// <summary>
    /// determine the state of the white hole
    /// </summary>
    public PoolStates isreadytoburst = PoolStates.idle;

    /// <summary>
    /// reference to the main component of the particle system for Type I particles
    /// </summary>
    private ParticleSystem.MainModule pmain;

    /// <summary>
    /// reference to the particle system for Type I particles
    /// </summary>
    private ParticleSystem particle;

    /// <summary>
    /// reference to the player's objects
    /// </summary>
    private GameObject player1, player2;

    /// <summary>
    /// target towards which send the particles
    /// </summary>
    private Transform target;

    /// <summary>
    /// prefab of Type II particles to be fired, customized on the data from the particle type
    /// </summary>
    public GameObject particlesPrefabs;

    /// <summary>
    /// particle prefab to be fired
    /// </summary>
    private GameObject particleprefabChosen;

    /// <summary>
    /// list of the last emitted particles (used to act on the particle that have already be sent)
    /// </summary>
    private List<GameObject> lastemittedparticle;

    /// <summary>
    /// name of the player object which has fired the pool
    /// </summary>
    private string activatorplayer;

    /// <summary>
    /// numbr of particles to erupt while loading
    /// </summary>
    private int burstemissioncount = 20;

    /// <summary>
    /// AudioSource for auditory feedback on interaction
    /// </summary>
    [HideInInspector]
    public AudioSource audioplayer;

    /// <summary>
    /// syncronization variable to make two pools active symultaneously (means the numebr of simultaneously active pools)
    /// </summary>
    [SerializeField]
    private static int numberOfSimultneouslyActivatedPool = 0;

    /// <summary>
    /// flag to determine if my local timerhs to be reset
    /// </summary>
    [SerializeField]
    private bool startedAlone = false;

    AudioClip clipSingleInteraction, clipDualInteraction;

    bool pulsingActivated = false; // flowredirection

    public int numberplayerinteracting = 0;

    /// <summary>
    /// setup the reference to the players and the correct particle to fire
    /// </summary>
    void Start()
    {
        startedAlone = false;
        setParticleEmissionCounter();
        lastemittedparticle = new List<GameObject>();
        pmain = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        particle = transform.GetChild(0).GetComponent<ParticleSystem>();
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        audioplayer = GetComponent<AudioSource>();
        clipSingleInteraction = RoamingObjectType.Particle1.GetInteractingAudio();
        audioplayer.clip = clipSingleInteraction;
        clipDualInteraction = RoamingObjectType.Particle1_2together.GetInteractingAudio();
        setParticles();
    }

    /// <summary>
    /// check whena  white hole is empty and count time to fill it again
    /// </summary>
    void Update() {

        if (isreadytoburst == PoolStates.inLoading || isreadytoburst == PoolStates.resetted)
        {
            time += Time.deltaTime;
            
            if (time > activationTime)
            {
                isreadytoburst = PoolStates.ready;
                BlowParticles(activatorplayer);
                StopPulsating();
            }
            if (numberOfSimultneouslyActivatedPool >= 2 && startedAlone && isreadytoburst != PoolStates.resetted)
            {
                time = 0;
                isreadytoburst = PoolStates.resetted;
                audioplayer.clip = clipDualInteraction;
                audioplayer.PlayOneShot(clipDualInteraction);

            }
            if (isreadytoburst == PoolStates.resetted && numberOfSimultneouslyActivatedPool < 2)
            {
                isreadytoburst = PoolStates.inLoading;
                audioplayer.clip = clipSingleInteraction;
                audioplayer.Play();
            }
        }
        else {
            if (isreadytoburst != PoolStates.inpulse)
            {
                StopPulsating();
                // Stop Sounds after burst
                audioplayer.Stop();
            }
        }
        
        if (isreadytoburst == PoolStates.exhausted || isreadytoburst == PoolStates.exhaustreset)
        {
            startedAlone = false;
            numberplayerinteracting = 0;
            StopPulsating();

            //flowredirection = false;
            GetComponent<SphereCollider>().enabled = false;
            time += Time.deltaTime;
            if (time > reloadTime) {
                lastemittedparticle.Clear();
                setParticles();
                time = 0;
                pmain.loop = true;
                if (isreadytoburst == PoolStates.exhausted)
                {
                    ParticlePoolsManager.instance.changeOnePoolPosition(gameObject);
                }
                isreadytoburst = PoolStates.idle;
                particle.Play();
                GetComponent<SphereCollider>().enabled = true;
            }
        }
    }

    /// <summary>
    /// Change the number of particles emitted by each whitehole (shared among the different whiteholes)
    /// </summary>
    /// <param name="amount">number of particles to be produced (more or less 1). Default 10</param>
    public static void setParticleEmissionCounter(float amount = 10) {
        maxratio = amount;
    }

    /// <summary>
    /// start interactig with the pool
    /// </summary>
    /// <param name="player"></param>
    public void StartActing(string player) {
        if (isreadytoburst == PoolStates.idle || (isreadytoburst == PoolStates.inpulse))
        {
            isreadytoburst = PoolStates.inLoading;
            activatorplayer = player;
            StartPulsating();
            Logger.AddWhiteHoleActivatedEventOnLog(GameObject.FindGameObjectWithTag(player).name, type, transform.position);

            if (numberOfSimultneouslyActivatedPool == 0)
            {
                startedAlone = true;
            }
            else
            {
                startedAlone = false;
            }
            numberOfSimultneouslyActivatedPool++;
            if (numberOfSimultneouslyActivatedPool >= 2)
            {
                audioplayer.clip = clipDualInteraction;
                numberOfSimultneouslyActivatedPool = 2;
            }
            else
            {
                audioplayer.clip = clipSingleInteraction;
            }
            audioplayer.Play();
        }
        if (numberplayerinteracting < 2)
        {
            numberplayerinteracting++;
        }
    }

    /// <summary>
    /// Start the pulse effect with a cerain frequency
    /// </summary>
    /// <param name="timebetweentwopulses">Time between the pulses in seconds. Default 1</param>
    public void StartPulsating(float timebetweentwopulses = 1, bool fromoutside = false) {
        if (!pulsingActivated && isreadytoburst != PoolStates.exhausted)
        {
            if (fromoutside && isreadytoburst != PoolStates.inLoading)
            {
                isreadytoburst = PoolStates.inpulse;
            }
            pulsingActivated = true;
            StartCoroutine(Pulse(timebetweentwopulses));
        }
    }

    /// <summary>
    /// stop the pulse effect
    /// </summary>
    public void StopPulsating(bool fromoutside = false)
    {
        if (fromoutside && isreadytoburst == PoolStates.inpulse && !startedAlone)
        {
            isreadytoburst = PoolStates.idle;
        }
        if (pulsingActivated)
        {
            if (numberplayerinteracting == 0 || isreadytoburst == PoolStates.exhausted || isreadytoburst == PoolStates.exhaustreset)
            {
                pulsingActivated = false;
            }
        }
    }

    /// <summary>
    /// perform one emission of particles each timetowait seconds
    /// </summary>
    /// <param name="timetowait">amount of time between two emissions in seconds</param>
    /// <returns></returns>
    private IEnumerator Pulse(float timetowait) {
        do
        {
            particle.Emit(burstemissioncount);
            yield return new WaitForSeconds(timetowait);
        } while (pulsingActivated);
    }

    /// <summary>
    /// create a stream of particles
    /// </summary>
    /// <param name="player"></param>
    public void BlowParticles(string player)
    {
        if (isreadytoburst == PoolStates.ready)
        {
            Logger.AddWhiteHoleUsedEventOnLog(GameObject.FindGameObjectWithTag(player).name, type, transform.position);
            ObjectGeneratorManager.instance.ParticleBoolFired(this);
            time = 0;
            updatedratio = maxratio;
            isreadytoburst = PoolStates.firing;
            pmain.loop = false;
            StartCoroutine("burst", player);
        }
    }

    /// <summary>
    /// stop the particle stream
    /// </summary>
    public void StopParticles()
    {
        startedAlone = false;
        if (numberplayerinteracting > 0)
        {
            numberplayerinteracting--;
        }
        if (numberplayerinteracting == 0)
        {
            if (isreadytoburst == PoolStates.inLoading || isreadytoburst == PoolStates.resetted || isreadytoburst == PoolStates.inpulse)
            {
                //Logger.AddWhiteHoleCancelEventOnLog(GameObject.FindGameObjectWithTag(activatorplayer).name, type, transform.position);
                isreadytoburst = PoolStates.idle;
                time = 0;
            }
            else
            {
                StopPulsating();
                if (isreadytoburst == PoolStates.resetposition)
                {
                    isreadytoburst = PoolStates.exhaustreset;
                }
                else
                {
                    isreadytoburst = PoolStates.exhausted;
                }
                particle.Stop();
            }
            IA.instance.exitingWhiteHoles();
            audioplayer.Stop();
            numberOfSimultneouslyActivatedPool--;
            if (numberOfSimultneouslyActivatedPool < 0)
            {
                numberOfSimultneouslyActivatedPool = 0;
            }
            ParticlePoolsManager.instance.changeStartedAlone();
        }
    }

    
    /// <summary>
    /// compute the target of the particles and then geenrate the elements
    /// </summary>
    /// <param name="playername">the name of the player who has interacted with this white hole</param>
    /// <returns></returns>
    IEnumerator burst(string playername) {
        GameObject creator;
        startedAlone = false;
        IA.instance.updateWhiteHolePlayerState(playername, whiteHoleState.aware, whiteHoleState.established, type);
        if (playername == "Player1")
        {
            target = player2.transform;
            creator = player1;
        }
        else
        {
            target = player1.transform;
            creator = player2;
        }

        while(updatedratio > 1 && isreadytoburst == PoolStates.firing){
            updatedratio = updatedratio - decreaseRatio*updatedratio;
            for (int i = 0; i < updatedratio; i++)
            {
                emiteOneParticle(target, creator);
                /*if (i % 2 == 0) {
                 * //gift for yourself to be good to the other player
                    emiteOneParticle(creator.transform, gameObject);
                }*/
                yield return new WaitForSeconds(1.0f/updatedratio);
            }
            
        }
        IA.instance.exitingWhiteHoles();
        isreadytoburst = PoolStates.exhausted;

        numberOfSimultneouslyActivatedPool--;
        if (numberOfSimultneouslyActivatedPool < 0)
        {
            numberOfSimultneouslyActivatedPool = 0;
        }
        audioplayer.Stop();
    }

    /// <summary>
    /// create and start the motion of a single particle
    /// </summary>
    /// <param name="t">target's transform to reach</param>
    /// <param name="c">set the creator object</param>
    private void emiteOneParticle(Transform t, GameObject c) {
        if (t.gameObject.tag == "Player1" || t.gameObject.tag == "Player2")
        {
            ParticleBehaviour.maxlifespan = 30;
        }
        else
        {
            ParticleBehaviour.maxlifespan = 2;
        }
        GameObject g = GameObject.Instantiate(particleprefabChosen);

        g.GetComponent<MeshRenderer>().material.color = ParticleTypeMethods.GetColor(type);
        Gradient grad = new Gradient();
        GradientColorKey[] gk = new GradientColorKey[2];
        gk[0].color = ParticleTypeMethods.GetColor(type);
        gk[0].time = 0f;
        gk[1].color = ParticleTypeMethods.GetColor(type);
        gk[1].time = 1f;
        GradientAlphaKey[] ga = new GradientAlphaKey[2];
        ga[0].alpha = 1f;
        ga[0].time = 0f;
        ga[1].alpha = 0f;
        ga[1].time = 1f;
        grad.SetKeys(gk, ga);
        g.transform.GetChild(0).GetComponent<TrailRenderer>().colorGradient = grad;

        g.GetComponent<ParticleBehaviour>().type = type;

        g.transform.position = transform.position + Random.insideUnitSphere;
        g.transform.LookAt(t);

        g.GetComponent<ParticleBehaviour>().creator = c;
        g.GetComponent<LinearMotionBehaviour>().setTarget(t);
        lastemittedparticle.Add(g);
    }

    /// <summary>
    /// Create an effect with present's particles and those in the white hole
    /// TODO: probably to be removed
    /// </summary>
    /// <param name="presentstype">the type of particles on the player</param>
    /// <returns></returns>
    /*public bool BurstEffect(ParticleType[] presentstype) {
        if (isreadytoburst == PoolStates.exhausted)
        {
            return false;
        }
        foreach(ParticleType t in presentstype){
                EffectManager.instance.SetEffect(EffectManager.instance.computeEffect(type, t), transform.position);
        }
        pmain.loop = false;
        isreadytoburst = PoolStates.exhausted;
        time = 0;
        return true;
    }*/

    /// <summary>
    /// determine the properties of the particles type II to be emitted
    /// </summary>
    private void setParticles() {
        ParticleSystem.MainModule pmain = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        particleprefabChosen = particlesPrefabs;
        pmain.startColor = type.GetColor();
    }

    /// <summary>
    /// redirect the already sent particles towards a new target
    /// </summary>
    /// <param name="newtarget"> the new object the particole need to converge to</param>
    public void redirectFlow(Transform newtarget) {
        target = newtarget;
        foreach (GameObject p in lastemittedparticle) {
            if (gameObject.GetComponent<LinearMotionBehaviour>() != null)
            {
                gameObject.GetComponent<LinearMotionBehaviour>().setTarget(newtarget);
            }
        }
        //flowredirection = true;
        IA.instance.increaseWhiteHolesTriggered();
    }

    public void resetPosition() {
        isreadytoburst = PoolStates.resetposition;
        StopParticles();
    }

    public void changestartedalone() {
        if (numberplayerinteracting > 0)
        {
            startedAlone = true;
        }
    }
}

/// <summary>
/// list of the states in which the particle pool may be
/// </summary>
public enum PoolStates { 
    ready, firing, exhausted, inLoading, idle, resetted, inpulse, resetposition, exhaustreset
}