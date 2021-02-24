using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Audiopan))]
public class TunnelManager : MonoBehaviour
{
    /// <summary>
    /// identifier of the world you will be transported when entering the tunnel
    /// </summary>
    public int newWorld;

    /// <summary>
    /// reference t the player's transform
    /// </summary>
    public Transform Player1, Player2;

    /// <summary>
    /// limit distance of the two players from the center of the object to  interact with it
    /// </summary>
    public float threshold = 3;

    /// <summary>
    /// position of the original tunnel to reposition it
    /// </summary>
    Vector3 oldpos;

    /// <summary>
    /// speed of the animation
    /// </summary>
    public float speed;

    /// <summary>
    /// black screen to cover
    /// </summary>
    public SpriteRenderer overlay;

    /// <summary>
    /// tiem to complite the loading animation before entering the tunnel
    /// </summary>
    public float timetoactivate = 5;

    /// <summary>
    /// state of the tunnel
    /// </summary>
    TunnelState state;

    /// <summary>
    /// variable to set the time flow during the animation
    /// </summary>
    float timeanimation;

    /// <summary>
    /// initial scale of the tunnel to be reset once the effect is abrupted
    /// </summary>
    Vector3 initalscale;
    private Vector3 extendedscale = new Vector3(2, 1, 2);

    /// <summary
    /// Handlers for SinglePulseEffect
    /// </summary>
    private bool SinglePlayerIn = false;
    private bool SingleExpansion = true;
    private bool isFirstTouch = true;

    /// <summary>
    /// reference to the indicator
    /// </summary>
    private Indicator IndicatorPlayer1;
    private Indicator IndicatorPlayer2;

    AudioSource audioPlayer;

    bool set = false;

    public float restrotationspeed = 4;
    public float interactionrotationspeed = 6;

    public bool active = false;

    /// <summary>
    /// recover the reference to the players
    /// </summary>
    void Start()
    {
        Player1 = GameObject.FindGameObjectWithTag("Player1").transform;
        Player2 = GameObject.FindGameObjectWithTag("Player2").transform;
        //transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = ApplicationManager.instance.getBackground(newWorld);
        state = TunnelState.open;
        initalscale = transform.localScale;
        audioPlayer = GetComponent<AudioSource>();

        IndicatorPlayer1 = GameObject.FindGameObjectWithTag("Player1").transform.GetChild(3).GetComponent<Indicator>();
        IndicatorPlayer2 = GameObject.FindGameObjectWithTag("Player2").transform.GetChild(3).GetComponent<Indicator>();
    }

    /// <summary>
    /// determine if the players can interact with the tunnel and eventually staert the aniamtion
    /// </summary>
    void Update()
    {
        if (!set)
        {
            ParticleType[] p = WorldParticleAssingment.gettheworldparticles(newWorld);
            for (int i = 0; i < p.Length; i++)
            {
                ParticleSystem.MainModule m = transform.GetChild(i).GetComponent<ParticleSystem>().main;
                Color c = p[i].GetColor();
                m.startColor = c;
                c.a = 0;
                transform.GetChild(transform.childCount - 1).GetChild(i).GetComponent<SpriteRenderer>().color = c;
            }
            set = true;
        }



        if(state == TunnelState.open && active){
            if (Vector3.Distance(transform.position, Player1.position) < threshold)
            {
                if (Vector3.Distance(transform.position, Player2.position) < threshold)
                {
                    Logger.AddStartUseBlackHoleEventOnLog(gameObject.name, transform.position);
                    oldpos = transform.localPosition;
                    state = TunnelState.inLoading;
                    timeanimation = 0;
                    audioPlayer.Stop();
                    audioPlayer.clip = RoamingObjectType.Blackhole.GetTransitionAudio();
                    audioPlayer.loop = true;
                    audioPlayer.Play();

                    SinglePlayerIn = false;
                    SingleExpansion = true;
                    isFirstTouch = true;

                    IndicatorPlayer1.stopIndicator();
                    IndicatorPlayer2.stopIndicator();
                    IA.instance.SetBlackHoleTouching(false);
                    IA.instance.EndBlackHoleContinousTouch();

                }
                else
                {
                    IA.instance.SetBlackHoleTouching(true);
                    if (IA.instance.GetBlackHoleIndicatorsNeeded())
                    {
                        IndicatorPlayer1.startIndicator(Player2, false);
                    }

                    SinglePlayerIn = true;
                    SinglePulseBlackHole();
                }
            }
            else {
                if (Vector3.Distance(transform.position, Player2.position) < threshold)
                {
                    IA.instance.SetBlackHoleTouching(true);
                    if (IA.instance.GetBlackHoleIndicatorsNeeded())
                    {
                        IndicatorPlayer2.startIndicator(Player1, false);
                    }

                    SinglePlayerIn = true;
                    SinglePulseBlackHole();
                }
                else
                {
                    speedBlackhole(restrotationspeed);

                    IndicatorPlayer1.stopIndicator();
                    IndicatorPlayer2.stopIndicator();
                    IA.instance.SetBlackHoleTouching(false);
                    IA.instance.EndBlackHoleContinousTouch();

                    SinglePlayerIn = false;
                    SingleExpansion = true;

                    audioPlayer.Stop();
                    isFirstTouch = true;
                }
            }
            if (transform.localScale != Vector3.one)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, initalscale, Time.deltaTime);
                transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, Vector3.one, Time.deltaTime);
                transform.GetChild(1).localScale = Vector3.Lerp(transform.GetChild(1).localScale, Vector3.one, Time.deltaTime);
                transform.GetChild(2).localScale = Vector3.Lerp(transform.GetChild(2).localScale, Vector3.one, Time.deltaTime);
                transform.GetChild(3).localScale = Vector3.Lerp(transform.GetChild(3).localScale, Vector3.one, Time.deltaTime);
                if (transform.localScale.x - initalscale.x < 0.1f) {
                    gameObject.GetComponent<SphereCollider>().enabled = true;
                }
            }
        }
        if (state == TunnelState.inLoading) {
            timeanimation += Time.deltaTime;
            for (int i = 0; i < 4; i++)
            {
                Color c = transform.GetChild(transform.childCount - 1).GetChild(i).GetComponent<SpriteRenderer>().color;
                c.a = Mathf.Lerp(c.a, 0, Time.deltaTime*1.5f);
                transform.GetChild(transform.childCount - 1).GetChild(i).GetComponent<SpriteRenderer>().color = c;
            }
            speedBlackhole(restrotationspeed);

            if(Vector3.Distance(transform.position, Player1.position) > threshold || Vector3.Distance(transform.position, Player2.position) > threshold){
                state = TunnelState.open;
                //Logger.AddCancelUseBlackHoleEventOnLog(gameObject.name, transform.position);
                return;
            }
            if (timeanimation >= timetoactivate)
            {
                state = TunnelState.Animation;
                IA.instance.IncreaseBlackHolesTransitioned();
                audioPlayer.Stop();
                audioPlayer.PlayOneShot(RoamingObjectType.Blackhole.GetDestructionAudio(), 1);
            }
            else {
                int i = Mathf.FloorToInt(timeanimation);
                float speedanim = speed / 2;
                if (i % 2 == 0)
                {
                    transform.localScale += new Vector3(1, 0, 1) * speedanim * Time.deltaTime;
                    scaleBlackhole(new Vector3(1, 0, 1) * +(speedanim) * Time.deltaTime);
                }
                else
                {
                    transform.localScale += new Vector3(1, 0, 1) * -speedanim * Time.deltaTime;
                    scaleBlackhole(new Vector3(1, 0, 1) * -(speedanim) * Time.deltaTime);
                }
            }
        }

        if (state == TunnelState.Animation) {

            SinglePlayerIn = false;
            SingleExpansion = true;

            IA.instance.SetBlackHoleTouching(false);
            IA.instance.EndBlackHoleContinousTouch();

            //IA.instance.IncreaseBlackHolesTransitioned();

            gameObject.GetComponent<SphereCollider>().enabled = false;
            ParticlePoolsManager.instance.StopPoolsInTheWorld();
            if (overlay == null)
            {
                overlay = GameObject.FindGameObjectWithTag("Overlay").GetComponent<SpriteRenderer>();
            }
            overlay.color = Color.Lerp(overlay.color, new Color(1, 1, 1, 0.8f), Time.deltaTime * speed);
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime*speed);
            transform.localScale += new Vector3(1, 0, 1)  *speed * Time.deltaTime;
            scaleBlackhole(new Vector3(1, 0, 1) * speed * Time.deltaTime);
            if (transform.localScale.x > 10.3f) {
                Logger.AddUsedBlackHoleEventOnLog(gameObject.name, transform.position);
                ApplicationManager.instance.ChangeWorld(newWorld);
                state = TunnelState.open;
                transform.localPosition = oldpos;
                transform.localScale = Vector3.one * 0.6302389f;
                scaleBlackhole(Vector3.one);
                overlay.color = Color.clear;
            }
        }
    }

    /// <summary>
    /// set the proper scale to the blackhole particles
    /// </summary>
    /// <param name="v"></param>
    private void scaleBlackhole(Vector3 v) {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).localScale += v;
        }
    }

    /// <summary>
    /// set the proper speed to the blackhole particles
    /// </summary>
    /// <param name="v"></param>
    private void speedBlackhole(float amount)
    {
        if (transform.GetChild(0).GetComponent<ParticleSystem>().velocityOverLifetime.orbitalY.constant != amount)
        {
            for (int i = 0; i < 4; i++)
            {
                ParticleSystem.VelocityOverLifetimeModule vm = transform.GetChild(i).GetComponent<ParticleSystem>().velocityOverLifetime;
                vm.orbitalY = amount;
            }
            //audioPlayer.PlayOneShot(RoamingObjectType.Blackhole.GetInteractingAudio(), 1);
        }
    }
    
    /// <summary>
    /// triggered whenever an object enters the event horizon
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AstralElement" || other.tag == "ChosenPlanet")
        {
            ObjectGeneratorManager.instance.MoveToAnotherWorld(newWorld, other.gameObject);
            other.tag = "AstralElement";
            Player1.GetComponent<PlayerStateManager>().releaseObject(other.gameObject);
            Player2.GetComponent<PlayerStateManager>().releaseObject(other.gameObject);
            Logger.AddSendThroughBlackHoleEventOnLog(gameObject.name, other.name, other.transform.position);
            audioPlayer.PlayOneShot(RoamingObjectType.Planet.GetTransitionAudio(),1);
        }
        if (other.tag == "StarElement")
        {
            other.transform.position = other.transform.position - (transform.position - other.transform.position).normalized;
        }
    }
    void OnTriggerStay(Collider other) {
        if (other.tag == "StarElement")
        {
            other.transform.position = other.transform.position - (transform.position - other.transform.position).normalized;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1" || other.tag == "Player2") {
            active = true;
        }
    }


    public void SetUp(int newWorldID, bool activeimmediately = true) {
        newWorld = newWorldID;
        active = activeimmediately;
    }

    private void SinglePulseBlackHole()
    {
        //audioPlayer.clip = RoamingObjectType.Blackhole.GetInteractingAudio();
        //audioPlayer.loop = false;
        //audioPlayer.Play();

        //audioPlayer.PlayOneShot(RoamingObjectType.Blackhole.GetInteractingAudio());
        if (SinglePlayerIn)
        {
            if (SingleExpansion)
            {
                for (int i = 0; i < 4; i++)
                {
                    transform.GetChild(i).localScale = Vector3.Lerp(transform.GetChild(i).localScale, extendedscale, Time.deltaTime * 2);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    transform.GetChild(i).localScale = Vector3.Lerp(transform.GetChild(i).localScale, Vector3.one , Time.deltaTime * 2);
                }
            }

            if (transform.GetChild(0).localScale.x >= 1.5)
            {
                SingleExpansion = false;
            }

            if (isFirstTouch)
            {
                audioPlayer.Stop();
                audioPlayer.PlayOneShot(RoamingObjectType.Blackhole.GetInteractingAudio());
                isFirstTouch = false;
            }

        }
    }

}

public enum TunnelState { 
    open, inLoading, Animation
}
