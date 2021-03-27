using UnityEngine;

public class IA : MonoBehaviour
{

    public static IA instance;

    /// <summary>
    /// WhiteHoleState of Player 1 and 2
    /// </summary>
    private whiteHoleState whiteHoleStateP1;
    private whiteHoleState whiteHoleStateP2;

    /// <summary>
    /// Counter for WhiteHoles touched
    /// </summary>
    private int whiteHolesTouching;

    /// <summary>
    /// Counter for WhiteHoles triggered jointly
    /// </summary>
    private int whiteHolesTriggeredJointly;

    /// <summary>
    /// Timer for interactions with WhiteHoles
    /// </summary>
    private int whiteHoleTimer;

    /// <summary>
    /// Time in Game between WhiteHole interactions
    /// </summary>
    private float whiteHoleTime;

    /// <summary>
    /// distance at which the whitehole will be moved
    /// </summary>
    private float whiteHoleThreshold = 10;

    /// <summary>
    /// store the currently triggered Particle Type
    /// </summary>
    private ParticleType triggeredParticleType;

    /// <summary>
    /// store the previous triggered Particle Type
    /// </summary>
    private ParticleType oldtriggeredParticleType;

    /// <summary>
    /// Manage state of indicators
    /// </summary>
    private bool indicatorsNeeded;

    /// <summary>
    /// Manage type of indicators (false=hint, true=pointer)
    /// </summary>
    private bool pointersNeeded;

    /// <summary>
    /// Counters for objects alive within world
    /// </summary>
    [SerializeField] private int planetsAlive;
    private int starsAlive;
    private int blackHolesAlive;

    /// <summary>
    /// Counters for interactions with Planets
    /// </summary>
    private int planetsTouched;
    private int planetsMoved;

    /// <summary>
    /// Switches for differentiating between first and continous touch
    /// </summary>
    private bool planetContinousTouch;
    public bool IsPlanetContinuousTouch() {
        return planetContinousTouch;
    }
    private bool planetContinousConnection;

    /// <summary>
    /// Switches to indicate if players are interacting with planets
    /// </summary>
    private bool planetTouching;
    private bool planetConnecting;

    /// <summary>
    /// Timer for interaction with planets
    /// </summary>
    private float planetTouchTime;
    private float planetConnectionTime;

    //private int starsCompleted;
    //private int starsPlanetsAdded;

    /// <summary>
    /// Counter for interactions with BlackHoles
    /// </summary>
    private int blackHolesTouched;
    private int blackHolesTransitioned;
    private bool blackHoleTouching;
    private bool blackHoleContinousTouch;
    private bool BlackHoleIndicatorsNeeded;


    // Use this for initialization
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

        // initialize indicators as inactive
        indicatorsNeeded = false;
        pointersNeeded = false;

        // initiatlize world as empty
        planetsAlive = 0;
        starsAlive = 0;
        blackHolesAlive = 0;

        // initialize WhiteHole vars
        whiteHoleStateP1 = whiteHoleState.ignorant;
        whiteHoleStateP2 = whiteHoleState.ignorant;
        whiteHolesTouching = 0;
        whiteHolesTriggeredJointly = 0;
        whiteHoleTimer = 30;
        whiteHoleTime = 0f;

        // initialize Planet vars
        planetsTouched = 0;
        planetsMoved = 0;
        planetContinousTouch = false;
        planetContinousConnection = false;
        planetTouching = false;
        planetConnecting = false;
        planetTouchTime = 0f;
        planetConnectionTime = 0f;

        // initialize BlackHole vars
        blackHolesTouched = 0;
        blackHolesTransitioned = 0;
        blackHoleTouching = false;
        blackHoleContinousTouch = false;
        BlackHoleIndicatorsNeeded = false;

    }

    // Update is called once per frame
    void Update()
    {

        // update all time vars
        whiteHoleTime += Time.deltaTime;


        // update object counters
        planetsAlive = GameObject.FindGameObjectsWithTag("AstralElement").Length;
        starsAlive = GameObject.FindGameObjectsWithTag("StarElement").Length;
        blackHolesAlive = GameObject.FindGameObjectsWithTag("TunnelElement").Length;


        // Evaluate interaction with WHITE HOLES
        if (whiteHoleTime >= whiteHoleTimer && (whiteHoleStateP1 == whiteHoleState.ignorant || whiteHoleStateP2 == whiteHoleState.ignorant))
        {
            if (whiteHoleStateP1 == whiteHoleState.ignorant)
            {
                ParticlePoolsManager.instance.BringCloser(GameObject.FindGameObjectWithTag("Player1").transform.position, whiteHoleThreshold);
            }
            else if (whiteHoleStateP2 == whiteHoleState.ignorant)
            {
                ParticlePoolsManager.instance.BringCloser(GameObject.FindGameObjectWithTag("Player2").transform.position, whiteHoleThreshold);
            }
            whiteHoleThreshold -= Mathf.Max(whiteHoleThreshold * 0.5f, 0);
            whiteHoleTime = 0;
        }
        if (starsAlive == 0 && whiteHolesTriggeredJointly < 4 && (whiteHoleStateP1 == whiteHoleState.aware || whiteHoleStateP1 == whiteHoleState.established) && (whiteHoleStateP2 == whiteHoleState.aware || whiteHoleStateP2 == whiteHoleState.established))
        {
            whiteHoleTimer = 60;
            if (whiteHoleTime >= whiteHoleTimer && whiteHolesTouching == 1)
            {
                // Pulsate all Pools
                ParticlePoolsManager.instance.Pulsate(ParticleType.All);
                
            }
        }
        if (starsAlive == 0 && whiteHolesTriggeredJointly >= 8 && whiteHolesTouching == 1)
        {
            // Pulsate only effective for triggeredParticleType
            if (triggeredParticleType != ParticleType.All)
            {
                ParticlePoolsManager.instance.Pulsate(triggeredParticleType);
            }
        }


        // Manage single player interaction with Planets
        if (whiteHoleStateP1 != whiteHoleState.established && whiteHoleStateP2 != whiteHoleState.established)
        {
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Asteroid, 0);
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Comet, 0);
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Meteoroid, 0);
        }
        else if (planetsAlive >= 10)
        {
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Asteroid, 3);
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Comet, 0);
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Meteoroid, 0);
        }
        else if (planetsAlive < 3)
        {
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Asteroid, 0);
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Comet, 1);
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Meteoroid, 2);
        }
        else {
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Asteroid, 1);
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Comet, 1);
            RandomObjectmanager.instance.SetNumberOfRoamingElement(RoamingObjectType.Meteoroid, 1);
        }


        


        // Manage multi-player interaction with Planets
        if ((planetsTouched - planetsMoved*16) > 5 && planetsMoved <= 2)
        {
            indicatorsNeeded = true;
        }
        else
        {
            indicatorsNeeded = false;
        }

        if ((planetsTouched - planetsMoved*16) > 15 && planetsMoved < 1)
        {
            pointersNeeded = true;
            planetsTouched = 16;
        }
        else
        {
            pointersNeeded = false;
        }

        if (planetTouching)
        {
            planetTouchTime += Time.deltaTime;

            if (planetTouchTime > 2)
            {
                if (!planetContinousTouch)
                {
                    planetsTouched++;
                    planetContinousTouch = true;
                }
            }

        }
        else
        {
            planetTouchTime = 0;
        }

        if (planetConnecting)
        {
            planetConnectionTime += Time.deltaTime;
            if (planetConnectionTime > 3)
            {
                if (!planetContinousConnection)
                {
                    planetsMoved++;
                    planetContinousConnection = true;
                    foreach (GameObject g in GameObject.FindGameObjectsWithTag("StarElement"))
                    {
                        g.GetComponent<StarManager>().HighlightFreeOrbit();
                    }
                }
            }
        }
        else
        {
            planetConnectionTime = 0;
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("StarElement"))
            {
                g.GetComponent<StarManager>().StopHighlightFreeOrbit();
            }
        }

        if (blackHoleTouching)
        {
            if(!blackHoleContinousTouch)
            {
                blackHolesTouched++;
                blackHoleContinousTouch = true;
            }
        }

        if (blackHolesTouched >=3 && blackHolesTransitioned == 0)
        {
            BlackHoleIndicatorsNeeded = true;
        }
        else
        {
            BlackHoleIndicatorsNeeded = false;
        }

    }

    // update WhiteHolePlayerState of a player
    public void updateWhiteHolePlayerState(string Player, whiteHoleState oldState, whiteHoleState newState, ParticleType type)
    {
        if (Player == "Player1")
        {
            if (whiteHoleStateP1 == oldState)
            {
                whiteHoleStateP1 = newState;
            }
        }
        else
        {
            if (whiteHoleStateP2 == oldState)
            {
                whiteHoleStateP2 = newState;
            }
        }

        if (whiteHolesTouching < 2 && oldState == whiteHoleState.ignorant)
        {
            whiteHolesTouching++;
        }
        if (triggeredParticleType != ParticleType.All) { 
            oldtriggeredParticleType = triggeredParticleType;
        }
        triggeredParticleType = type;
        
    }

    // reduce count of whiteHoles being touched 
    public void exitingWhiteHoles()
    {
        if (whiteHolesTouching > 0)
        {
            whiteHolesTouching--;
        }
        if (whiteHolesTouching == 0) {
            ParticlePoolsManager.instance.StopPulsate();
            oldtriggeredParticleType = ParticleType.All;
        }
        if (whiteHolesTouching == 1) {
            triggeredParticleType = oldtriggeredParticleType;
        }
    }

    // increase counter of jointly triggered WhiteHoles
    public void increaseWhiteHolesTriggered()
    {
        
        whiteHolesTriggeredJointly++;
        triggeredParticleType = ParticleType.All;
    }

    // Get state of indicators from outside this script
    public bool GetIndicatorsNeeded()
    {
        return indicatorsNeeded;
    }

    // Get type of indicators from outside this script
    public bool GetPointersNeeded()
    {
        return pointersNeeded;
    }

    // Set state for 1 player touching a planet
    public void SetPlanetTouching(bool state)
    {
        planetTouching = state;
    }

    // Set state for 2 players connecting to a planet
    public void SetPlanetConnecting(bool state)
    {
        planetConnecting = state;
    }

    // End continous touch
    public void EndPlanetContinousTouch()
    {
        planetContinousTouch = false;
    }

    // End continous connection
    public void EndPlanetContinousConnection()
    {
        planetContinousConnection = false;
    }

    // Start Black Hole Touch
    public void SetBlackHoleTouching(bool state)
    {
        blackHoleTouching = state;
    }

    // End Black Hole Touch
    public void EndBlackHoleContinousTouch()
    {
        blackHoleContinousTouch = false;
    }

    // Update counter for Black Holes Transitioned
    public void IncreaseBlackHolesTransitioned()
    {
       blackHolesTransitioned++;
    }

    // Get state of Black Hole indicators
    public bool GetBlackHoleIndicatorsNeeded()
    {
        return BlackHoleIndicatorsNeeded;
    }

}

public enum whiteHoleState
{
    ignorant, aware, established
}