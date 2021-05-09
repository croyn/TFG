using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//its the manager of all the process of the system
public class timerZone : MonoBehaviour
{
    public static timerZone instance; //singleton
    float timeStartAppear = 0.0f; //float control start time to appear something
    float timeNextAppear = 0.0f; //float control next appear
    public float timeToNext = 1.0f; //public variable to control the next zone appear
    public float timeToOffZones = 1.0f;//public variable to control the next zone dissapear
    float timeNextOff = 0.0f; //control the actual time passing by
    public List<GameObject> zoneList = null; //list of catchZones. Now 4 . One for every user and corner of the game
    public List<GameObject> zoneListMove = null;//list of moving zones. 
    public List<GameObject> zoneScan = null;//list of zoneScans
    private List<Transform> zoneListActive = new List<Transform>(); //internal list to save what zone are active
    private List<Transform> zoneListActiveMove = new List<Transform>();//internal list to save what moving zone are active
    public GameObject pointCentralMandala; //reference to central point mandala
    public GameObject blackCircle;//reference to final black circle
    bool active = false;//boolean to control if is active
    private int ControlWhichZone = 2; //control wich zone is going
    private int ControlWhichMove = 0;//control wich move is going//not used in the actual flow 
    public int controlFase = 3; //control which fase is going on in the flow if the system
    Gradient actualGradient = null; //actual color going on in the flow
    private bool ParticlesDoneAbosorving = false; //check if the particles are done 
    private bool controlOneTimeAppering = false; //check if certain functions are done one time
    private bool controlminiMoveZone = false; //check if the zone are done
    public float timeBetweenFases; //control actual time beetween fases
    private bool timeBetweenFasesControl = false;//control if the time is done
    public Vector3 sizeFinal;//control what size is the final size to the mandala in the end fase
    public Vector3 incrementSize;//the increment vector that will make grow the mandala in the end fase
    public Vector3 sizeFinalCircle;//control what size is the final size to the blackcircle in the end fase
    public Vector3 incrementSizeCircle;//the increment vector that will grow the blackcircle in the end fase
    public float tempMaxFirstLayer;//number of zones we want to be cacthed before the moving fase start -1 in first layer
    public float tempMaxSecondLayer;//number of zones we want to be cacthed before the moving fase start -1 in second layer
    public float tempMaxThirdLayer;//number of zones we want to be cacthed before the moving fase start -1 in third layer
    public float tempMaxCircleLayer;//number of zones we want to be cacthed before the moving fase start -1 in circle layer
    public int contadorIterations; //control number of iterations according to the max that we choose in every layer
    bool firstLog = false;
    bool firstSong = false;
    bool songPlayIt = false;
    public int ControlWhichMovePlayerOne = 0;
    public int contadorIterationsPlayerOne = 0;
    public int ControlWhichMovePlayerTwo = 0;
    public int contadorIterationsPlayerTwo = 0;
    public int ControlWhichMovePlayerThree = 0;
    public int contadorIterationsPlayerThree = 0;
    public int ControlWhichMovePlayerFour = 0;
    public int contadorIterationsPlayerFour = 0;
    public int numItersOnePlayerLayerOne = 0;
    public int numItersOnePlayerLayerTwo = 0;
    public int numItersOnePlayerLayerThree = 0;
    public int numItersOnePlayerLayerCircle = 0;
    public int whatZoneIsPlayerOne = 0;
    public int whatZoneIsPlayerTwo = 1;
    public int whatZoneIsPlayerThree = 2;
    public int whatZoneIsPlayerFour = 3;

    enum usertoActivateEnum { none, one, two, three, four, two_four, one_three, one_two_three, three_four };

    // Start is called before the first frame update
    void Start()
    {
        //set the initial asigments
        instance = this;
        timeNextAppear = 0.0f;
        timeNextOff = 0.0f;
        //start in fase 3
        controlFase = 3;
        firstSong = false;
        contadorIterations = 0;
        changeFaseTo(3);
        whatZoneIsPlayerOne = 0;
        whatZoneIsPlayerTwo = 1;
        whatZoneIsPlayerThree = 2;
        whatZoneIsPlayerFour = 3;
    }


    // Update is called once per frame
    void Update()
    {
        //control time going on
        timeNextAppear = timeNextAppear + Time.deltaTime;

        //if fase is catch particles
        if (controlFase == 0)
        {
            //if the time is what we need
            if (timeNextAppear >= timeToNext)
            {

                if (active)
                {
                    //getAllTimes();
                    //make that the touched particles in every zone go at the same time
                    activeZonesTouched();
                    //deactivate all the zones that where not catched
                    deactivateZones();

                }
                //activate the zone where the particles appear
                activateZones();
                //restart time
                timeNextAppear = 0.0f;



            }



            /* if (timeNextAppear >= timeToNext - timeToOffZones)//this is for deactivate the particles that where doesn catch according to the time we configurate to be active
             {
                 //if we are active
                 if (active)
                 {
                     //getAllTimes();
                     //make that the touched particles in every zone go at the same time
                     activeZonesTouched();
                     //deactivate all the zones that where not catched
                     deactivateZones();

                 }
             }*/




        }
        else if (controlFase == 1) //moving fase
        {
            if (!songPlayIt) {
                // audioController.instance.playAudio(4);
                songPlayIt = true;
            }



            //if particle not donde absorving
            if (!ParticlesDoneAbosorving)
            {
                //deactivate zones 
                deactivateZones();
                //if every zone has finished to move particles
                if (!isAbsorbParticlesDone())
                {
                    //particles done absorving
                    ParticlesDoneAbosorving = true;
                    //restart time
                    timeNextAppear = 0.0f;
                    //play move music

                }
            }

            //if timeBetweenFasesControl is false and the time pass and particles are done moving
            if (!timeBetweenFasesControl && ParticlesDoneAbosorving && timeNextAppear >= timeBetweenFases)
            {
                //deactivate the circles from the zones catch
                deactivateZonesCircles();
                //activate the moving zones between zones catch
                //activaMovezones(0);
                //now its done
                timeBetweenFasesControl = true;
                //restart time
                timeNextAppear = 0.0f;

            }

            //when the moving zones are in the final position and all the previus are ok
            if (isDoneMoving() && ParticlesDoneAbosorving && timeBetweenFasesControl && timeNextAppear >= timeBetweenFases - 0.2f)
            {
                //change where the users are
                changeWhatZoneIsUser();
                //deactivate the moving zones
                deactiveMoveZones();
                if (contadorIterations == 0)
                {
                    mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable = 0.0f;
                    mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = 0.0f;
                }
                //if the mandala is done
                if (mandalamanager.instance.trianglesDone && mandalamanager.instance.circleDone)
                {
                    //go to final scan
                    changeFaseTo(6);
                    return;
                }
                else
                {
                    //activate the circles in the zones catch
                    activateZonesCircles();
                    //restart control of mini moving zones
                    controlminiMoveZone = false;
                    //change to mini moving zones fase
                    changeFaseTo(2);
                    if (firstSong)
                    {
                        //play the song
                        //audioController.instance.playAudio(mandalamanager.instance.whichSong());
                    }
                    return;
                }

            }
            else if (!isDoneMoving() && ParticlesDoneAbosorving && timeBetweenFasesControl) //if not done moving restart time
            {
                timeNextAppear = 0.0f;
            }



        }
        else if (controlFase == 2) //mini moving fase
        {
            //if false
            if (!controlminiMoveZone)
            {
                //activate mini move zones

                activaMiniMovezones();
                //done
                controlminiMoveZone = true;
                //restart time
                timeNextAppear = 0.0f;
                //&& timeNextAppear >= 0.4f
            }
            else if (isDoneMovingMiniZone() && controlminiMoveZone)//if mini move zones are done moving and were activate before
            {
                //deactivate mini moving zones
                deactiveMiniMoveZones();
                //change to fase catch particles
                changeFaseTo(0);
                return;
            }

        }
        else if (controlFase == 3) //first scan fase
        {
            if (!firstLog) {
                firstLog = true;
                //#if !UNITY_EDITOR
                Logger.addChangeFase("First Scan Fase");

                //#endif
            }
            //if false and all the zones are in green
            if (!controlminiMoveZone && checkScanZones())
            {
                //restart time
                timeNextAppear = 0.0f;
                //done
                controlminiMoveZone = true;
                audioController.instance.playAudio(6);
            }

            //wait 2 seconds
            if (controlminiMoveZone && timeNextAppear > 2.0f)
            {
                //restart
                timeNextAppear = 0.0f;
                //declare are done
                timeBetweenFasesControl = true;
                //change fase
                changeFaseTo(4);
                return;
            }

        }
        else if (controlFase == 4) //tutorial catch particles on front of every player fase
        {

            //if a secongs past away 
            if (timeNextAppear >= 1.0f && timeBetweenFasesControl)
            {
                //change to false for next fase
                timeBetweenFasesControl = false;
                //activate the front particles of every user
                activateParticlesZoneScan(0);
            }

            //check if all the front zone are cacthed .
            if (checkPartcilesCatchedZonesFromScanZones())
            {
                //restart zones
                deactivateZoneScan(1);

                changeFaseTo(6);
                return;
            }
        }
        else if (controlFase == 5) //Used in other flow that was denied
        {
            if (!timeBetweenFasesControl)
            {

                activaMovezones(1);

                timeBetweenFasesControl = true;

            }
            if (isDoneMoving() && timeBetweenFasesControl)
            {

                deactiveMoveZones();

                //activateZonesCircles();

                changeFaseTo(6);
                return;


            }

        }
        else if (controlFase == 6) ////second scan and third scan fase
        {
            //check if the foot print in the zones catch are in green
            if (checkScanZonesFromMainZone())
            {
                //audioController.instance.playAudio(6);
                //if the mandala is done
                if (mandalamanager.instance.trianglesDone && mandalamanager.instance.circleDone)
                {
                    deactivateZoneScan(2);
                    //change to final fase
                    changeFaseTo(8);
                    return;
                }
                else
                {
                    //Change to reordering points fase
                    timeNextAppear = 0.0f;
                    changeFaseTo(7);
                    return;
                }

            }
        }
        else if (controlFase == 7) //reordering points fase
        {
            //if all the points on the mandala are in their final position
            if (mandalamanager.instance.checkPointsInPosition() && timeNextAppear >= 13.0f)
            {
                //deactivate scan zone script
                deactivateZoneScan(0);
                //change to catch particles fase
                changeFaseTo(0);
                return;
            }
        }
        else if (controlFase == 8)//final fase
        {
            //if we are in the first 2 seconds make mandala grow 
            if (timeNextAppear <= 2.0f)
            {
                //make bigger the mandala
                reSizeMandala();
            }
            else {
                bool resp1 = reSizeMandala();
                bool resp2 = reSizeBlackCircle();
                if (resp1 && resp2) //when the mandala and the black circle are in their final size we finish the game
                {
                    Debug.Log("SE ACABO");
                }
            }


        }


    }


    //function that allows to reSize the Mandala to their final size
    private bool reSizeMandala() {
        //recolect the local Scale of the mandala
        float x = mandalamanager.instance.gameObject.transform.localScale.x;
        float y = mandalamanager.instance.gameObject.transform.localScale.y;
        float z = mandalamanager.instance.gameObject.transform.localScale.z;
        //check if we increment the final size is reached
        if (x + incrementSize.x <= sizeFinal.x && y + incrementSize.y <= sizeFinal.y && z + incrementSize.z <= sizeFinal.z) {
            //do the increment of size
            mandalamanager.instance.gameObject.transform.localScale = mandalamanager.instance.gameObject.transform.localScale + incrementSize;
            //we are not done yet
            return false;
        }
        else
        {
            //we are on the size we want
            return true;
        }

    }
    //function that allows to reSize the black circle to the final size
    private bool reSizeBlackCircle()
    {

        //make visible the black circle
        blackCircle.SetActive(true);
        //recolect the local scale
        float x = blackCircle.transform.localScale.x;
        float y = blackCircle.transform.localScale.y;
        float z = blackCircle.transform.localScale.z;
        //check if final size reached
        if (x + incrementSizeCircle.x <= sizeFinalCircle.x && y + incrementSizeCircle.y <= sizeFinalCircle.y && z + incrementSizeCircle.z <= sizeFinalCircle.z)
        {
            //increment the size
            blackCircle.transform.localScale = blackCircle.transform.localScale + incrementSizeCircle;
            //not done yet
            return false;
        }
        else
        {
            //done resizing
            return true;
        }

    }

    //function that allows the change of fases in the flow of the program
    public void changeFaseTo(int which) {
        //according to the fase we prepare what we need
        switch (which) {
            case 0://catch particles
                //#if !UNITY_EDITOR
                Logger.addChangeFase("Catch Particles Fase");

                //#endif
                if (!firstSong) {
                    //play the song
                    firstSong = true;
                    //audioController.instance.playAudio(mandalamanager.instance.whichSong());
                    timeNextAppear = 0.0f;
                }
                else
                {
                    timeNextAppear = timeToNext;
                }

                ControlWhichZone = 1;//start in right zone to appear
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = true;//allow the collision in the central point
                //deactivateZonesMove();
                //timeNextAppear = timeToNext;//restart time
                //timeNextAppear = 0.0f;
                activateZonesCircles();//activate circle from catch zone
                break;
            case 1://move
                //#if !UNITY_EDITOR
                Logger.addChangeFase("Moving transition Fase");
                songPlayIt = false;
                
                //#endif
                contadorIterations = 0;//control of iterations done in fase catch particles
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = false;//dont allow collision
                ParticlesDoneAbosorving = false; //restart varible
                timeBetweenFasesControl = false;//restart control

                break;
            case 2://activating circles

                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = false;//dont allow collision
                controlminiMoveZone = false;//restart variable
                break;
            case 3://first scan

                deactivateZonesCircles();//deactivate circle zone
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = false;//dont allow colli
                mandalamanager.instance.deactivateAllPoints();
                controlminiMoveZone = false;
                break;

            case 4://appear and capturing circle
                   //#if !UNITY_EDITOR
                Logger.addChangeFase("First Scan Appear Circle Fase");
                //#endif
                activateParticlesZoneScan(1);
                break;
            case 5://move scan//no se usa pendiente quitar
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = false;
                ParticlesDoneAbosorving = false;
                timeBetweenFasesControl = false;
                deactivateZoneScan(0);
                break;
            case 6://second scan and third scan

                //if mandala done
                if (mandalamanager.instance.trianglesDone && mandalamanager.instance.circleDone)
                {
                    //deactivate scan zone
                    //#if !UNITY_EDITOR
                    Logger.addChangeFase("Third Scan Fase");
                    //#endif
                    deactivateZoneScan(2);
                }
                else
                {
                    //#if !UNITY_EDITOR
                    Logger.addChangeFase("Second Scan Fase");
                    //#endif
                    //deactivate scan zone and circles restarting variable
                    deactivateZoneScan(1);
                }

                controlminiMoveZone = false; //restart variable
                activateZoneScanFromMainZone();//activate the script scanZone on frootprint in zone catch
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = false;//dont allow collision
                break;
            case 7://appear mandala
                   //#if !UNITY_EDITOR
                Logger.addChangeFase("Appear Mandala Fase");
                //#endif
                //deactivate the zoneScan from first scan
                deactivateZoneScan(0);
                //deactivate scan zone and circles restarting variable
                deactivateZoneScan(1);
                audioController.instance.playAudio(5);
                //make visible and make the mandala appear
                mandalamanager.instance.activateAllPoints();
                break;
            case 8://final
                //deactivate scan zone
                Logger.addChangeFase("Final Fase");
                deactivateZoneScan(2);
                //restart time
                audioController.instance.playAudio(7);
                timeNextAppear = 0.0f;
                break;

        }
        //save what fase we are
        controlFase = which;
    }


    //allow to deactivate scan zone according to the arg
    private void deactivateZoneScan(int which) {

        //if arg 0 we set to disable the zoneScan for the first scan fase
        if (which == 0)
        {
            for (int i = 0; i < zoneScan.Count; i++)
            {
                zoneScan[i].SetActive(false);

            }

        }
        else if (which == 1) //if arg 1 we restart the script collison and then disable . For the flow we set visible the front circle of the zone to
        {
            for (int i = 0; i < zoneList.Count; i++)
            {
                Transform tempFoot = zoneList[i].transform.Find("footPrint");
                tempFoot.GetComponent<collisionScan>().timeIn = 0.0f;
                tempFoot.GetComponent<collisionScan>().changeColorTo(0);
                tempFoot.GetComponent<collisionScan>().enabled = false;

                Transform zone = zoneList[i].transform.Find("frenteZone");
                Transform circle = zone.Find("Cube");
                circle.gameObject.SetActive(true);
            }

        }
        else if (which == 2) //if arg 2 only change the color to white and disable the script
        {
            for (int i = 0; i < zoneList.Count; i++)
            {
                Transform tempFoot = zoneList[i].transform.Find("footPrint");
                tempFoot.GetComponent<collisionScan>().changeColorTo(0);
                tempFoot.GetComponent<collisionScan>().enabled = false;

            }
        }


    }


    //activate the zoneScan for the first scan
    private void activateParticlesZoneScan(int which) {

        for (int i = 0; i < zoneScan.Count; i++)
        {
            if (which == 0)//if arg 0 activate the circle and the particle system
            {
                Transform zone = zoneScan[i].transform.Find("frenteZone");
                zone.gameObject.GetComponent<simpleColliderZone>().allowCollision = true;
                Transform particles = zone.Find("Particle System");
                particles.gameObject.GetComponent<ParticleSystem>().Play();
            }
            else if (which == 1)//if arg 1 activate only the circle
            {
                Transform zone = zoneScan[i].transform.Find("frenteZone");
                zone.transform.Find("Cube").gameObject.SetActive(true);
            }


        }

    }


    //activate the footprint and the scritp for the cathc zones
    private void activateZoneScanFromMainZone() {
        for (int i = 0; i < zoneList.Count; i++) {
            // zoneList[i].GetComponent<zoneManager>().activateFootPrint();
            Transform tempFoot = zoneList[i].transform.Find("footPrint");
            tempFoot.gameObject.SetActive(true);
            tempFoot.GetComponent<collisionScan>().enabled = true;
            tempFoot.GetComponent<collisionScan>().playSong = true;

        }
    }


    //function to  check if all the scan zone are in green , so scanned
    public bool checkScanZones() {
        //to check the final result
        bool resp = true;
        //for every scan zone
        for (int i = 0; i < zoneScan.Count; i++) {
            //find the footPrint
            Transform foot = zoneScan[i].transform.Find("footPrint");
            //temp
            bool tempResp = true;
            //get actual color
            Color tempColor = foot.gameObject.GetComponent<collisionScan>().actualColor;

            //if green means that is scanned
            if (tempColor == Color.green) {
                //set true
                tempResp = true;
            }
            else {
                //set false
                tempResp = false;
            }
            //accumulate
            resp = resp && tempResp;

        }
        //final 
        return resp;
    }


    //function to  check if all the scan zone on the catch zone are in green , so scanned
    public bool checkScanZonesFromMainZone()
    {
        //to check the final result
        bool resp = true;

        //for every catch zone
        for (int i = 0; i < zoneList.Count; i++)
        {
            //find the footprint
            Transform foot = zoneList[i].transform.Find("footPrint");
            //temp
            bool tempResp = true;
            //get the color
            Color tempColor = foot.gameObject.GetComponent<collisionScan>().actualColor;

            //if green is scanned
            if (tempColor == Color.green)
            {
                //set true
                tempResp = true;
            }
            else
            {
                //set false
                tempResp = false;
            }
            //acccumulate
            resp = resp && tempResp;

        }

        //final
        return resp;

    }

    //check if all the particles on the front circles in the scan zone are touched
    public bool checkPartcilesCatchedZonesFromScanZones()
    {
        //to check
        bool resp = true;

        //for every scan zone
        for (int i = 0; i < zoneScan.Count; i++)
        {
            //find the front zone
            Transform zone = zoneScan[i].transform.Find("frenteZone");

            //accumulate result of the function that give us as result if the flow of every zone is done
            resp = resp && zone.gameObject.GetComponent<simpleColliderZone>().isZoneOver();

        }

        //final result
        return resp;

    }

    //Activate the moving zone and setup the color configuration the arg for which allows to say if we want the move from scan zone to catch zone or beetween cacth zones.
    public void activaMovezones(int which) {

        //this makes that the color will have the moving zone be the last one that we have in the catch fase
        //ParticleSystem.ColorOverLifetimeModule temp = zoneList[0].transform.Find("derechaZone").GetComponent<zoneManager>().AffectedParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
        //ParticleSystem.MinMaxGradient colorNormal = temp.color;
        //ParticleSystem.MinMaxGradient colorNotouch = zoneList[0].transform.Find("derechaZone").GetComponent<zoneManager>().GradientColorMove;

        //the actual flow will have the color that the next layer will have
        for (int i = 0; i < zoneListMove.Count; i++) {
            //change the color configuration on the moving zone with the actual color that the mandala use,so its the actual next layer already setup
            zoneListMove[i].GetComponent<MoveZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColorMove());
            //for which allows to say if we want the move from scan zone to catch zone or beetween cacth zones.
            zoneListMove[i].GetComponent<MoveZone>().initMoveZone(which);

        }
    }

    //activate mini zones. According to the script enable on the gameobject do the different acces to it. This is because there exists two flows to this mechaninc
    //one that is on use in the actual flow that are the mini rivers . The other actually disable allows us to have 3 little group of particles moving .
    public void activaMiniMovezones()
    {
        //flow used if we want the last color used on catch particles fase
        //recolect the color from right zone of the first catch zone because have the color we want
        //ParticleSystem.ColorOverLifetimeModule temp = zoneList[0].transform.Find("derechaZone").GetComponent<zoneManager>().AffectedParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
        //recolect the gradient
        //ParticleSystem.MinMaxGradient tempColor = temp.color;
        //for every cacth zone
        for (int i = 0; i < zoneList.Count; i++)
        {
            //recolect the references of the 3 gameObjects tranform
            Transform minizone1 = zoneList[i].transform.Find("MiniMoveZone1");
            Transform minizone2 = zoneList[i].transform.Find("MiniMoveZone2");
            Transform minizone3 = zoneList[i].transform.Find("MiniMoveZone3");

            //if the script of the first minizone have enable is moveZone we can assume all of them too
            if (minizone1.gameObject.GetComponent<MoveZone>().isActiveAndEnabled) {

                //change the color configuration of the 3 minizone .
                minizone1.gameObject.GetComponent<MoveZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());
                minizone2.gameObject.GetComponent<MoveZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());
                minizone3.gameObject.GetComponent<MoveZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());

                //init the flow of the script that make move the 3 mini group of particles go(need the right particle system configuration , Look gameObject disable "baseMINI")
                minizone1.gameObject.GetComponent<MoveZone>().initMoveZone(0);
                minizone2.gameObject.GetComponent<MoveZone>().initMoveZone(0);
                minizone3.gameObject.GetComponent<MoveZone>().initMoveZone(0);
            }
            //if the script of the first miniRiverZone have enable is moveZone we can assume all of them too
            if (minizone1.gameObject.GetComponent<miniRiverZone>().isActiveAndEnabled) {
                // change the color configuration of the miniRivers
                minizone1.gameObject.GetComponent<miniRiverZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());
                minizone2.gameObject.GetComponent<miniRiverZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());
                minizone3.gameObject.GetComponent<miniRiverZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());

                //init the flow of the script that make move the 3 mini rivers of particles go(need the right particle system configuration , Look gameObject disable "baseRIVER")
                minizone1.gameObject.GetComponent<miniRiverZone>().PlayRiver();
                minizone2.gameObject.GetComponent<miniRiverZone>().PlayRiver();
                minizone3.gameObject.GetComponent<miniRiverZone>().PlayRiver();
            }

        }

    }

    //control if every particle system is done to moving particles on the catch zones.
    private bool isAbsorbParticlesDone()
    {
        //for accumulate
        bool control = false;
        //for every zone
        for (int i = 0; i < zoneList.Count; i++)
        {
            //check if it is done
            control = control || zoneList[i].transform.Find("derechaZone").GetComponent<zoneManager>().isMovingParticles();
            control = control || zoneList[i].transform.Find("frenteZone").GetComponent<zoneManager>().isMovingParticles();
            control = control || zoneList[i].transform.Find("izquierdaZone").GetComponent<zoneManager>().isMovingParticles();

        }

        //result
        return control;

    }

    //activate the explosion of particles only on the zones that are catched
    private void activeZonesTouched() {
        //temp object
        Transform temp = null;
        bool controlOneActive = false;//control if one zone catched the iteration

        //for every active catch zone 
        for (int i = 0; i < zoneListActive.Count; i++)
        {
            //get the object
            temp = zoneListActive[i];

            //check if it is catched for some player
            if (temp.GetComponent<zoneManager>().isCatched()) {
                //activate the explosion effect
                temp.GetComponent<zoneManager>().activeExplosion();
                //#if !UNITY_EDITOR
                Logger.addParticlesCatch(temp.name, temp.GetComponent<zoneManager>().getUserCatch(), temp.GetComponent<zoneManager>().getWhenCollision());
                //#endif
                //set the control to true
                controlOneActive = true;
            }



        }

        //for set up the iterations that need to make
        float timeTemp = 0.0f;
        //if triangles are not done on the mandala flow
        if (!mandalamanager.instance.trianglesDone)
        {
            //see what layer is the mandala
            switch (mandalamanager.instance.layer)
            {
                case 0://first layer
                    timeTemp = (((float)numItersOnePlayerLayerOne-2.0f) * 4.0f)+2.0f;
                    break;
                case 1://second layer
                    
                    timeTemp = (((float)numItersOnePlayerLayerTwo - 1.0f) * 4.0f)-2.0f ;
                    break;
                case 2://third layer
                    timeTemp = (((float)(numItersOnePlayerLayerThree) - 2.0f) * 4.0f) ;
                    break;
            }
        }
        else
        {
            //this is the circle configuration
            timeTemp = (((float)(numItersOnePlayerLayerCircle) - 2.0f) * 4.0f); ;

        }

       
        //new iteration
        //contadorIterations = contadorIterations + 1;


        //if someone touched a group of particles this iteration
        if (controlOneActive)
        {

            //Debug.Log("Time " + timeTemp);
            //timeTemp = timeTemp / timeToNext;
            //Debug.Log("timeToNext " + timeToNext);
            // Debug.Log("timeTemp calc " + timeTemp);
            float num = mandalamanager.instance.actulgetNumberLines(); //get the number of lines that is configurate in that layer
            // Debug.Log("num lines total " + num);
            float numLines = num / timeTemp; //get the ponderation that we need to make this iteration
            //Debug.Log("numLines calc " + numLines);

            // for the flow system we need to make two calls for each line to happen and we need to have in account if there is some more from before there.
            float calc = mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable + (numLines) * 2.0f;
            // Debug.Log("Tiene point centralDesdeTimer " + calc);

            //accumulate the calc + the number of particles accumulate for not touching it
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable = calc + mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched;
            //restart the particles accumulate not touched
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = 0.0f;
        }
        else if (!controlOneActive && ControlWhichZone != 3) { //if anybody touch a group of particles this 

            // Debug.Log("Time " + timeTemp);
            //timeTemp = timeTemp / timeToNext;
            // Debug.Log("timeToNext " + timeToNext);
            // Debug.Log("timeTemp calc " + timeTemp);
            float num = mandalamanager.instance.actulgetNumberLines();
            //Debug.Log("num lines total " + num);
            float numLines = num / timeTemp;
            // Debug.Log("numLines calc " + numLines);
            float calc = mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched + (numLines) * 2.0f;
            // Debug.Log("Tiene point centralDesdeTimer " + calc);
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = calc;


        }




        /* if (contadorIterations >= (timeTemp + 1) + ((timeTemp + 1) / 3.0f) && !mandalamanager.instance.trianglesDone)
         {
             mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable = mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable + mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched;
             mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = 0.0f;
             contadorIterations = 0;
         }
         else if (contadorIterations >= (timeTemp ) && mandalamanager.instance.trianglesDone) {
             mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable = mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable + mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched;
             mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = 0.0f;
             contadorIterations = 0;
         }*/
        /*  if (contadorIterations >= (timeTemp+1 ) + ((timeTemp + 1) / 3.0f) && !mandalamanager.instance.trianglesDone)
          {
              timerZone.instance.gameObject.GetComponent<timerZone>().changeFaseTo(1);
              audioController.instance.playAudio(4);

          }
          else if (contadorIterations >= (timeTemp+1) && mandalamanager.instance.trianglesDone)
          {
              timerZone.instance.gameObject.GetComponent<timerZone>().changeFaseTo(1);
              audioController.instance.playAudio(4);
          }*/
    }


    private float getNumberIterations(bool sum=false) {
        //for set up the iterations that need to make
        float timeTemp = 0.0f;
        //if triangles are not done on the mandala flow
        if (!mandalamanager.instance.trianglesDone)
        {
            //see what layer is the mandala
            switch (mandalamanager.instance.layer)
            {
                case 0://first layer
                    if (sum)
                    {
                        timeTemp = tempMaxFirstLayer+2.0f;
                    }
                    else {
                        timeTemp = tempMaxFirstLayer;
                    }
                    
                    break;
                case 1://second layer
                    if (sum)
                    {
                        timeTemp = tempMaxSecondLayer+4.0f;
                    }
                    else
                    {
                        timeTemp = tempMaxSecondLayer;
                    }
                   
                    break;
                case 2://third layer
                    if (sum)
                    {
                        timeTemp = tempMaxThirdLayer+6.0f;
                    }
                    else
                    {
                        timeTemp = tempMaxThirdLayer;
                    }
                    
                    break;
            }
        }
        else
        {
            //this is the circle configuration
            timeTemp = tempMaxCircleLayer;

        }

        return timeTemp;

    }

    private void activateTheRest() {

        float timeTemp = getNumberIterations();

        if (contadorIterations >= timeTemp && !mandalamanager.instance.trianglesDone)
        {
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable = mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable + mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched;
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = 0.0f;
            contadorIterations = 0;
           

        }
        else if (contadorIterations >= timeTemp && mandalamanager.instance.trianglesDone)
        {
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable = mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable + mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched;
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = 0.0f;
            contadorIterations = 0;

        }


    }


    //function that controls the flow of the appearing catch zone in terms of which need to appear
    private void ChangeZone() {

        switch (ControlWhichZone) {
            case 0://derecha , right
                ControlWhichZone = 2;
                break;
            case 1://frente ,front
                ControlWhichZone = 3;
                break;
            case 2://izquierda, left
                ControlWhichZone = 1;
                break;
            case 3://silencio, silence
                ControlWhichZone = 0;
                break;
        }

    }


    //check if the moving zones are done in their movement
    private bool isDoneMoving() {

        int control = 0; //check the number of moving zone that are in position

        //for every moving zone in the list
        for (int i = 0; i < zoneListMove.Count; i++) {
            //check if the moving zone is in position
            if (zoneListMove[i].GetComponent<MoveZone>().isInPosition()) {
                control = control + 1;
            }

        }

        //if the number of list in position is the same as the list
        if (control == zoneListMove.Count)
        {
            //is done
            return true;
        }
        else {
            //not yet
            return false;
        }

    }


    //check if the flow of a mini moving zone or MiniRiver is done
    private bool isDoneMovingMiniZone()
    {

        int control = 0;
        //for every catch zone
        for (int i = 0; i < zoneList.Count; i++)
        {
            //find the miniMoveZone
            Transform minizone1 = zoneList[i].transform.Find("MiniMoveZone1");
            Transform minizone2 = zoneList[i].transform.Find("MiniMoveZone2");
            Transform minizone3 = zoneList[i].transform.Find("MiniMoveZone3");
            //set init variable to check
            bool inPos1 = false;
            bool inPos2 = false;
            bool inPos3 = false;

            //we can assume if the first has the script MoveZone active the other too
            if (minizone1.gameObject.GetComponent<MoveZone>().isActiveAndEnabled) {
                //check for every minizone if is in position
                inPos1 = minizone1.gameObject.GetComponent<MoveZone>().isInPosition();
                inPos2 = minizone2.gameObject.GetComponent<MoveZone>().isInPosition();
                inPos3 = minizone3.gameObject.GetComponent<MoveZone>().isInPosition();
            }
            //we can assume if the first has the script miniRiverZone active the other too
            if (minizone1.gameObject.GetComponent<miniRiverZone>().isActiveAndEnabled)
            {
                //check for every minizone if is done moving
                //Debug.Log("Entro miniRiverZone doneMoving:");
                inPos1 = minizone1.gameObject.GetComponent<miniRiverZone>().movingDone();
                inPos2 = minizone2.gameObject.GetComponent<miniRiverZone>().movingDone();
                inPos3 = minizone3.gameObject.GetComponent<miniRiverZone>().movingDone();
            }

            //check the 3 are done
            if (inPos1 && inPos2 && inPos3)
            {
                //check this zone as good
                control = control + 1;
            }

        }

        //if all the zone are done
        if (control == zoneList.Count)
        {
            //Debug.Log("Entro en control true");
            return true;
        }
        else
        {
            //not yet
            return false;
        }

    }

    //deactivate the moving zones
    private void deactiveMoveZones()
    {

        //for every moving zone
        for (int i = 0; i < zoneListMove.Count; i++)
        {
            //deactivate
            zoneListMove[i].GetComponent<MoveZone>().deactiveMoveZone();

        }

    }


    //function tu deactivate the minizones when uses the MoveZone script . MiniRiver doesnt needed
    private void deactiveMiniMoveZones()
    {

        for (int i = 0; i < zoneList.Count; i++)
        {
            Transform minizone1 = zoneList[i].transform.Find("MiniMoveZone1");
            Transform minizone2 = zoneList[i].transform.Find("MiniMoveZone2");
            Transform minizone3 = zoneList[i].transform.Find("MiniMoveZone3");
            minizone1.gameObject.GetComponent<MoveZone>().deactiveMoveZone();
            minizone2.gameObject.GetComponent<MoveZone>().deactiveMoveZone();
            minizone3.gameObject.GetComponent<MoveZone>().deactiveMoveZone();


        }

    }








    //activate the catch zone it need in this time iteration
    void activateZones() {
        //temp variable
        Transform internalZone = null;
        //change what zone it is now
        //ChangeZone();
        contadorIterations = contadorIterations + 1;
        //give the colors that the mandala has configurate in this layer
        WhatMoveToWhatUser(getPlayerFlow());
        
        //#if !UNITY_EDITOR
        // if (internalZone != null) {
        //     Logger.addParticlesAppear(internalZone.name);
        //}
        //#endif
        //for set up the iterations that need to make
        float timeTemp = 0.0f;
        //if triangles are not done on the mandala flow
        if (!mandalamanager.instance.trianglesDone)
        {
            //see what layer is the mandala
            switch (mandalamanager.instance.layer)
            {
                case 0://first layer
                    timeTemp = tempMaxFirstLayer;
                    break;
                case 1://second layer
                    timeTemp = tempMaxSecondLayer;
                    break;
                case 2://third layer
                    timeTemp = tempMaxThirdLayer;
                    break;
            }
        }
        else
        {
            //this is the circle configuration
            timeTemp = tempMaxCircleLayer;

        }

        //control if we need to change of fase
        if (contadorIterations >= timeTemp && !mandalamanager.instance.trianglesDone)
        {
            activateTheRest();
            active = false;
            timerZone.instance.gameObject.GetComponent<timerZone>().changeFaseTo(1);
            //if contadorIterations is 0 need to restart the central point of the mandala
          
            //audioController.instance.playAudio(4);

        }
        else if (contadorIterations >= (timeTemp) && mandalamanager.instance.trianglesDone)
        {
            activateTheRest();
            active = false;
            timerZone.instance.gameObject.GetComponent<timerZone>().changeFaseTo(1);
           
        }

        //when all the zone are active
        timeStartAppear = Time.time;
        //set active to true
        //active = true;

    }



    //deactivate only the catch zone that are active
    void deactivateZones() {


        Transform temp = null;

        //for every active catch zone
        for (int i = 0; i < zoneListActive.Count; i++)
        {

            temp = zoneListActive[i];
            //deactivate
            temp.GetComponent<zoneManager>().deactivateZone();

        }
        //clear the list
        zoneListActive.Clear();

        //the zones are not active anymore
        active = false;
    }


    //deactivate the circles on every catch zone
    void deactivateZonesCircles() {
        Transform temp;
        Transform temp1;
        Transform temp2;

        //for every catch zone
        for (int i = 0; i < zoneList.Count; i++) {

            //find the reference
            //right zone
            temp = zoneList[i].transform.Find("derechaZone");
            //front zone
            temp1 = zoneList[i].transform.Find("frenteZone");
            //left zone
            temp2 = zoneList[i].transform.Find("izquierdaZone");

            //deactivate circle in all 3
            temp.gameObject.GetComponent<zoneManager>().deactiaveCircle();
            temp1.gameObject.GetComponent<zoneManager>().deactiaveCircle();
            temp2.gameObject.GetComponent<zoneManager>().deactiaveCircle();
        }
    }


    //activate the circles on the catch zones
    public void activateZonesCircles()
    {
        Transform temp;
        Transform temp1;
        Transform temp2;

        //for every catch zone
        for (int i = 0; i < zoneList.Count; i++)
        {
            //find the references
            //right zone
            temp = zoneList[i].transform.Find("derechaZone");
            //front zone
            temp1 = zoneList[i].transform.Find("frenteZone");
            //left zone
            temp2 = zoneList[i].transform.Find("izquierdaZone");

            //activate the circles of all 3
            temp.gameObject.GetComponent<zoneManager>().actiaveCircle();
            temp1.gameObject.GetComponent<zoneManager>().actiaveCircle();
            temp2.gameObject.GetComponent<zoneManager>().actiaveCircle();
        }
    }




    //Activate the moving zone and setup the color configuration the arg for which allows to say if we want the move from scan zone to catch zone or beetween cacth zones.
    public void activaMovezonesNoSyncro(int which,GameObject whatInit)
    {

        //this makes that the color will have the moving zone be the last one that we have in the catch fase
        //ParticleSystem.ColorOverLifetimeModule temp = zoneList[0].transform.Find("derechaZone").GetComponent<zoneManager>().AffectedParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
        //ParticleSystem.MinMaxGradient colorNormal = temp.color;
        //ParticleSystem.MinMaxGradient colorNotouch = zoneList[0].transform.Find("derechaZone").GetComponent<zoneManager>().GradientColorMove;

        //the actual flow will have the color that the next layer will have
        for (int i = 0; i < zoneListMove.Count; i++)
        {
            if (zoneListMove[i].GetComponent<MoveZone>().initPoint.Equals(whatInit)) {
                Debug.Log("Encuentro move zone.");
                //change the color configuration on the moving zone with the actual color that the mandala use,so its the actual next layer already setup
                zoneListMove[i].GetComponent<MoveZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColorMove());
                //to calculate the velocity according to the time left to the layer.
                if (which == 0) {
                    float dist = Vector3.Distance(zoneListMove[i].GetComponent<MoveZone>().initPoint.transform.position, zoneListMove[i].GetComponent<MoveZone>().finalPoint.transform.position);
                    float time_left = getNumberIterations(true) - contadorIterations;
                    float velocity = dist / time_left;
                    zoneListMove[i].GetComponent<MoveZone>().velocity = velocity;
                }
                //for which allows to say if we want the move from scan zone to catch zone or beetween cacth zones.
                zoneListMove[i].GetComponent<MoveZone>().initMoveZone(which);
            }
           

        }
    }

    private int whatZoneIsUser(int whichUser) {

        switch(whichUser){

            case 0:
                return whatZoneIsPlayerOne;
            case 1:
                return whatZoneIsPlayerTwo;
            case 2:
                return whatZoneIsPlayerThree;
            case 3:
                return whatZoneIsPlayerFour;
            default:
                return -1;
          

        }

    }


    private void changeWhatZoneIsUser() {

        whatZoneIsPlayerOne = whatZoneIsPlayerOne - 1;
        if (whatZoneIsPlayerOne <0)
        {
            whatZoneIsPlayerOne = 3;
        }
        whatZoneIsPlayerTwo = whatZoneIsPlayerTwo - 1;
        if (whatZoneIsPlayerTwo < 0)
        {
            whatZoneIsPlayerTwo = 3;
        }
        whatZoneIsPlayerThree = whatZoneIsPlayerThree - 1;
        if (whatZoneIsPlayerThree < 0)
        {
            whatZoneIsPlayerThree = 3;
        }
        whatZoneIsPlayerFour = whatZoneIsPlayerFour - 1;
        if (whatZoneIsPlayerFour < 0)
        {
            whatZoneIsPlayerFour = 3;
        }
    }

    private void activateUseDetectingWhatZone(int whichPlayer,int what) {
        int zoneToActive = whatZoneIsUser(whichPlayer);
        GameObject tempZone;
        if (zoneToActive != -1) {
            tempZone = zoneList[zoneToActive];
            Transform tempWhatZone=null;
            
            switch (what)
            {
                case 0://derecha , right
                   tempWhatZone = tempZone.transform.Find("derechaZone");
                    break;
                case 1://frente ,front
                    tempWhatZone = tempZone.transform.Find("frenteZone");
                    break;
                case 2://izquierda, left
                    tempWhatZone = tempZone.transform.Find("izquierdaZone");
                    break;

            }
            if (tempWhatZone != null)
            {
                Gradient colorActual = mandalamanager.instance.switchColor();
                Gradient colorActualMove = mandalamanager.instance.switchColorMove();
                //set the color configuration to the zone
                tempWhatZone.GetComponent<zoneManager>().ChangeColorTo(colorActual, colorActualMove);
                //activate that zone
                tempWhatZone.GetComponent<zoneManager>().activateZone();
                //save in the active list of catchzones
                zoneListActive.Add(tempWhatZone);
                // temp.GetComponent<zoneManager>().updateSpherePoint();
            }
           

        }
        
        

    }

    private int numIterOnePlayerMax() {
        if (!mandalamanager.instance.trianglesDone)
        {

            switch (mandalamanager.instance.layer)
            {
                case 0:
                    return numItersOnePlayerLayerOne;
                case 1:
                    return numItersOnePlayerLayerTwo;
                case 2:
                    return numItersOnePlayerLayerThree;
                default:
                    return 0;
            }
        }
        else {
            return numItersOnePlayerLayerCircle;
        }
        
    }


    private void WhatMoveToWhatUser(usertoActivateEnum usertoActivate) {
        bool playerOne = false;
        bool playerTwo = false;
        bool playerThree = false;
        bool playerFour = false;

        switch (usertoActivate) {

            case usertoActivateEnum.none:
                break;
            case usertoActivateEnum.one:
                playerOne = true;
                break;
            case usertoActivateEnum.two:
                playerTwo = true;
                break;
            case usertoActivateEnum.three:
                playerThree = true;
                break;
            case usertoActivateEnum.four:
                playerFour = true;
                break;
            case usertoActivateEnum.two_four:
                playerTwo = true;
                playerFour = true;
                break;
            case usertoActivateEnum.one_three:
                playerOne = true;
                playerThree = true;
                break;
            case usertoActivateEnum.one_two_three:
                playerOne = true;
                playerTwo = true;
                playerThree = true;
                break;
            case usertoActivateEnum.three_four:
                playerThree = true;
                playerFour = true;
                break;
            default:
                Debug.Log("UserToActivaNum in WhatMoveToWhatUser fail/not contemplated");
                break;
        }


        if (playerOne) {
            //if we are finished

            contadorIterationsPlayerOne = contadorIterationsPlayerOne + 1;
            if (contadorIterationsPlayerOne >= numIterOnePlayerMax())
            {
                int zoneToActive = whatZoneIsUser(0);
                GameObject tempZone=null;
                if (zoneToActive != -1)
                {
                    tempZone = zoneList[zoneToActive].transform.Find("footPrint").gameObject; ;
                    if (tempZone != null) {
                        Debug.Log("intento activar move zone player 1");
                        activaMovezonesNoSyncro(0, tempZone);
                    }
                    
                }
                contadorIterationsPlayerOne = 0;


            }
            else {


                activateUseDetectingWhatZone(0, ControlWhichMovePlayerOne);
                //flow Player One
                switch (ControlWhichMovePlayerOne)
                {
                    case 0://derecha , right
                        ControlWhichMovePlayerOne = 2;
                        break;
                    case 1://frente ,front
                        ControlWhichMovePlayerOne = 0;
                        break;
                    case 2://izquierda, left
                        ControlWhichMovePlayerOne = 1;
                        break;
                    case 3://silencio, silence
                        ControlWhichMovePlayerOne = 0;
                        break;

                }
            }
           
        }

        if (playerTwo)
        {
            //if we are finished

            contadorIterationsPlayerTwo = contadorIterationsPlayerTwo + 1;
            if (contadorIterationsPlayerTwo >= numIterOnePlayerMax())
            {
                int zoneToActive = whatZoneIsUser(1);
                GameObject tempZone = null;
                if (zoneToActive != -1)
                {
                    tempZone = zoneList[zoneToActive].transform.Find("footPrint").gameObject; ;
                    if (tempZone != null) {
                        
                        Debug.Log("intento activar move zone player 2");
                        activaMovezonesNoSyncro(0, tempZone);
                    }
                   
                }
                contadorIterationsPlayerTwo = 0;
            }
            else
            {


                activateUseDetectingWhatZone(1, ControlWhichMovePlayerTwo);
                //flow Player One
                switch (ControlWhichMovePlayerTwo)
                {
                    case 0://derecha , right
                        ControlWhichMovePlayerTwo = 2;
                        break;
                    case 1://frente ,front
                        ControlWhichMovePlayerTwo = 0;
                        break;
                    case 2://izquierda, left
                        ControlWhichMovePlayerTwo = 1;
                        break;
                    case 3://silencio, silence
                        ControlWhichMovePlayerTwo = 0;
                        break;

                }
            }

        }


        if (playerThree)
        {
            //if we are finished

            contadorIterationsPlayerThree = contadorIterationsPlayerThree + 1;
            if (contadorIterationsPlayerThree >= numIterOnePlayerMax())
            {
                int zoneToActive = whatZoneIsUser(2);
                GameObject tempZone=null;
                if (zoneToActive != -1)
                {
                    tempZone = zoneList[zoneToActive].transform.Find("footPrint").gameObject;
                    if (tempZone != null)
                    {

                        Debug.Log("intento activar move zone player 3");
                        activaMovezonesNoSyncro(0, tempZone);
                    }
                }
                contadorIterationsPlayerThree = 0;
            }
            else
            {


                activateUseDetectingWhatZone(2, ControlWhichMovePlayerThree);
                //flow Player One
                switch (ControlWhichMovePlayerThree)
                {
                    case 0://derecha , right
                        ControlWhichMovePlayerThree = 2;
                        break;
                    case 1://frente ,front
                        ControlWhichMovePlayerThree = 0;
                        break;
                    case 2://izquierda, left
                        ControlWhichMovePlayerThree = 1;
                        break;
                    case 3://silencio, silence
                        ControlWhichMovePlayerThree = 0;
                        break;

                }
            }

        }


        if (playerFour)
        {
            //if we are finished

            contadorIterationsPlayerFour = contadorIterationsPlayerFour + 1;
            if (contadorIterationsPlayerFour >= numIterOnePlayerMax())
            {
                int zoneToActive = whatZoneIsUser(3);
                GameObject tempZone;
                if (zoneToActive != -1)
                {
                    tempZone = zoneList[zoneToActive].transform.Find("footPrint").gameObject;
                    if (tempZone != null) {
                        Debug.Log("intento activar move zone player 4");
                        activaMovezonesNoSyncro(0, tempZone);
                    }
                    
                   
                }
                contadorIterationsPlayerFour = 0;
            }
            else
            {


                activateUseDetectingWhatZone(3, ControlWhichMovePlayerFour);
                //flow Player One
                switch (ControlWhichMovePlayerFour)
                {
                    case 0://derecha , right
                        ControlWhichMovePlayerFour = 2;
                        break;
                    case 1://frente ,front
                        ControlWhichMovePlayerFour = 0;
                        break;
                    case 2://izquierda, left
                        ControlWhichMovePlayerFour = 1;
                        break;
                    case 3://silencio, silence
                        ControlWhichMovePlayerFour = 0;
                        break;

                }
            }

        }

        if (playerOne || playerTwo || playerThree || playerFour) {
            active = true;
        }

    }


    private usertoActivateEnum getPlayerFlow() {

        if (!mandalamanager.instance.trianglesDone)
        {
            switch (mandalamanager.instance.layer)
            {
                case 0:
                    return whatUserLayerone();
                case 1:
                    return whatUserLayerTwo();
                case 2:
                    return whatUserLayerTwo();
                default:
                    return usertoActivateEnum.none;
            }

        }
        else {
            return whatUserLayerThree();
        }

       

        
    }


    private usertoActivateEnum whatUserLayerone() {

        switch (contadorIterations)
        {
            case 0:
                return usertoActivateEnum.none;
            case 1:
                return usertoActivateEnum.one;
            case 2:
                return usertoActivateEnum.two;
            case 3:
                return usertoActivateEnum.three;
            case 4:
                return usertoActivateEnum.four;
            case 5:
                return usertoActivateEnum.one;
            case 6:
                return usertoActivateEnum.two;
            case 7:
                return usertoActivateEnum.four;
            case 8:
                return usertoActivateEnum.three;
            case 9:
                return usertoActivateEnum.two;
            case 10:
                return usertoActivateEnum.one;
            case 11:
                return usertoActivateEnum.three;
            case 12:
                return usertoActivateEnum.four;
            case 13:
                return usertoActivateEnum.none;
            case 14:
                return usertoActivateEnum.none;
            case 15:
                return usertoActivateEnum.two;
            case 16:
                return usertoActivateEnum.none;
            case 17:
                return usertoActivateEnum.one;
            case 18:
                return usertoActivateEnum.three;
            case 19:
                return usertoActivateEnum.two;
            case 20:
                return usertoActivateEnum.none;
            case 21:
                return usertoActivateEnum.one;
            case 22:
                return usertoActivateEnum.two_four;//user 2 + 4
            case 23:
                return usertoActivateEnum.three;
            case 24:
                return usertoActivateEnum.none;
            case 25:
                return usertoActivateEnum.four;
            case 26:
                return usertoActivateEnum.one_three;//user 1+3
            case 27:
                return usertoActivateEnum.none;
            case 28:
                return usertoActivateEnum.two;
            case 29:
                return usertoActivateEnum.none;
            case 30:
                return usertoActivateEnum.four;
            case 31:
                return usertoActivateEnum.none;
            case 32:
                return usertoActivateEnum.two;
            case 33:
                return usertoActivateEnum.one_three;
            case 34:
                return usertoActivateEnum.none;
            case 35:
                return usertoActivateEnum.two;
            case 36:
                return usertoActivateEnum.none;
            case 37:
                return usertoActivateEnum.one;
            case 38:
                return usertoActivateEnum.three;
            case 39:
                return usertoActivateEnum.two;
            case 40:
                return usertoActivateEnum.four;
            case 41:
                return usertoActivateEnum.four;
            case 42:
                return usertoActivateEnum.one;
            case 43:
                return usertoActivateEnum.four;
            case 44:
                return usertoActivateEnum.three;
            case 45:
                return usertoActivateEnum.three;
            case 46:
                return usertoActivateEnum.none;
            case 47:
                return usertoActivateEnum.none;
            case 48:
                return usertoActivateEnum.one;
            case 49:
                return usertoActivateEnum.none;
            case 50:
                return usertoActivateEnum.none;
            case 51:
                return usertoActivateEnum.none;
            case 52:
                return usertoActivateEnum.none;
            case 53:
                return usertoActivateEnum.none;
            case 54:
                return usertoActivateEnum.four;
            default:
                return usertoActivateEnum.none;
        }

    }

    private usertoActivateEnum whatUserLayerTwo()
    {
        switch (contadorIterations)
        {
            case 0:
                return usertoActivateEnum.none;
            case 1:
                return usertoActivateEnum.one;
            case 2:
                return usertoActivateEnum.two;
            case 3:
                return usertoActivateEnum.three;
            case 4:
                return usertoActivateEnum.four;
            case 5:
                return usertoActivateEnum.one;
            case 6:
                return usertoActivateEnum.two;
            case 7:
                return usertoActivateEnum.four;
            case 8:
                return usertoActivateEnum.three;
            case 9:
                return usertoActivateEnum.two;
            case 10:
                return usertoActivateEnum.one;
            case 11:
                return usertoActivateEnum.three;
            case 12:
                return usertoActivateEnum.four;
            case 13:
                return usertoActivateEnum.none;
            case 14:
                return usertoActivateEnum.none;
            case 15:
                return usertoActivateEnum.two;
            case 16:
                return usertoActivateEnum.none;
            case 17:
                return usertoActivateEnum.one;
            case 18:
                return usertoActivateEnum.three;
            case 19:
                return usertoActivateEnum.two;
            case 20:
                return usertoActivateEnum.none;
            case 21:
                return usertoActivateEnum.one;
            case 22:
                return usertoActivateEnum.two_four;//user 2 + 4
            case 23:
                return usertoActivateEnum.three;
            case 24:
                return usertoActivateEnum.none;
            case 25:
                return usertoActivateEnum.four;
            case 26:
                return usertoActivateEnum.one_two_three;//user 1+3
            case 27:
                return usertoActivateEnum.none;
            case 28:
                return usertoActivateEnum.none;
            case 29:
                return usertoActivateEnum.none;
            case 30:
                return usertoActivateEnum.three_four;
            case 31:
                return usertoActivateEnum.none;
            case 32:
                return usertoActivateEnum.one;
            case 33:
                return usertoActivateEnum.none;
            case 34:
                return usertoActivateEnum.none;
            case 35:
                return usertoActivateEnum.none;
            case 36:
                return usertoActivateEnum.four;
            default:
                return usertoActivateEnum.none;
        }

    }

    private usertoActivateEnum whatUserLayerThree()
    {
        switch (contadorIterations)
        {
            case 0:
                return usertoActivateEnum.none;
            case 1:
                return usertoActivateEnum.one;
            case 2:
                return usertoActivateEnum.two;
            case 3:
                return usertoActivateEnum.three;
            case 4:
                return usertoActivateEnum.four;
            case 5:
                return usertoActivateEnum.one;
            case 6:
                return usertoActivateEnum.two;
            case 7:
                return usertoActivateEnum.four;
            case 8:
                return usertoActivateEnum.three;
            case 9:
                return usertoActivateEnum.two;
            case 10:
                return usertoActivateEnum.one;
            case 11:
                return usertoActivateEnum.three;
            case 12:
                return usertoActivateEnum.four;
            case 13:
                return usertoActivateEnum.two;
            case 14:
                return usertoActivateEnum.none;
            case 15:
                return usertoActivateEnum.three;
            case 16:
                return usertoActivateEnum.one;
            case 17:
                return usertoActivateEnum.none;
            case 18:
                return usertoActivateEnum.four;
            default:
                return usertoActivateEnum.none;
        }

    }


    #region NotUsedAnyMore
    //function not used. Makes that the number of particles change if all the childs catch all the particles in a time beetwen t.min<0.2<t.max
    /*void getAllTimes() {
        bool control = true;
        Transform temp = null;
        List<float> times = new List<float>();
        float timeCollision = 0.0f;
        Debug.Log("Count list " + zoneListActive.Count);
        for (int i = 0; i < zoneListActive.Count; i++)
        {

            temp = zoneListActive[i];
            timeCollision = temp.GetComponent<zoneManager>().getWhenCollision();
            times.Add(timeCollision);

        }

        while (times.Count >0 && control) {
            times.Sort();
            float timeCalc = times[times.Count - 1] - times[0];
            Debug.Log("time calc" + timeCalc);
            if (timeCalc <= 0.2 && timeCalc > 0.0)
            {
                for (int i = 0; i < zoneListActive.Count; i++)
                {
                    ParticleSystem.MainModule ps = zoneListActive[i].GetComponent<zoneManager>().AffectedParticles.main;
                    ps.maxParticles = 25*times.Count;

                    control = false;
                }
            }
            else {
                times.RemoveAt(0);

            }

        }

        if (times.Count == 0) {
            for (int i = 0; i < zoneListActive.Count; i++)
            {
                ParticleSystem.MainModule ps = zoneListActive[i].GetComponent<zoneManager>().AffectedParticles.main;
                ps.maxParticles = 5;
                control = false;
            }
        }
        times.Clear();


    }*/


    //not used Anymore. Function that active the next circle in the way beetween catch zone in moving fase.
    void activateNextMove()
    {
        Transform temp = null;
        for (int i = 0; i < zoneListMove.Count; i++)
        {
            //int result = Random.Range(0, 3);
            //result = 1;
            Debug.Log("Control num" + ControlWhichMove);
            switch (ControlWhichMove)
            {

                case 0:
                    temp = zoneListMove[i].transform.Find("Move_0");
                    break;
                case 1:
                    temp = zoneListMove[i].transform.Find("Move_1");
                    break;
                case 2:
                    temp = zoneListMove[i].transform.Find("Move_2");
                    break;
                case 3:
                    temp = zoneListMove[i].transform.Find("Move_3");
                    break;
                case 4:
                    temp = zoneListMove[i].transform.Find("Move_4");
                    break;
                case 5:
                    temp = zoneListMove[i].transform.Find("Move_5");
                    break;
                case 6:
                    temp = zoneListMove[i].transform.Find("Move_6");
                    break;
                case 7:
                    temp = null;
                    break;
                default:
                    temp = null;
                    break;
            }

            if (temp != null)
            {
                Debug.Log("no null");
                temp.GetComponent<zoneManagerMove>().actiaveCircle();

                //zoneListActiveMove.Add(temp);
                // temp.GetComponent<zoneManager>().updateSpherePoint();
            }
        }

    }

    //not used Anymore.deactivate only the active move zones that are active
    public void deactivateZonesMove()
    {
        Transform temp = null;

        //
        for (int i = 0; i < zoneListActiveMove.Count; i++)
        {

            temp = zoneListActiveMove[i];
            temp.GetComponent<zoneManagerMove>().deactivateZone();
            temp.GetComponent<zoneManagerMove>().deactiaveCircle();

        }



        zoneListActiveMove.Clear();
        active = false;
    }

    //Not used Anymore.function that allows to make a flow mechanic beetween moving zones.
    private void ChangeZoneMove()
    {
        ControlWhichMove = ControlWhichMove + 1;


        if (ControlWhichMove > 7)
        {
            ControlWhichMove = -1;
        }

    }

    //Not used Anymore.l Function that manage the activation of the moving zones in a mechanic declained that makes a way of circles beetwen every catch zone in moving fase.
    void activateZonesMove()
    {

        Transform temp = null;
        Transform temp1 = null;

        for (int i = 0; i < zoneListMove.Capacity; i++)
        {
            //int result = Random.Range(0, 3);
            //result = 1;

            switch (ControlWhichMove)
            {

                case 0:
                    temp = zoneListMove[i].transform.Find("Move_0");
                    // temp1= zoneListMove[i].transform.Find("Move_1");
                    break;
                case 1:
                    temp = zoneListMove[i].transform.Find("Move_1");
                    //temp1 = zoneListMove[i].transform.Find("Move_2");
                    break;
                case 2:
                    temp = zoneListMove[i].transform.Find("Move_2");
                    //temp1 = zoneListMove[i].transform.Find("Move_3");
                    break;
                case 3:
                    temp = zoneListMove[i].transform.Find("Move_3");
                    temp1 = zoneListMove[i].transform.Find("Move_4");
                    break;
                case 4:
                    temp = zoneListMove[i].transform.Find("Move_4");
                    // temp1 = zoneListMove[i].transform.Find("Move_5");
                    break;
                case 5:
                    temp = zoneListMove[i].transform.Find("Move_5");
                    // temp1 = zoneListMove[i].transform.Find("Move_6");
                    break;
                case 6:
                    temp = zoneListMove[i].transform.Find("Move_6");
                    //temp1 = zoneListMove[i].transform.Find("Move_6");
                    break;
                case 7:
                    temp = null;
                    break;
            }

            if (temp != null)
            {
                temp.GetComponent<zoneManagerMove>().activateZone();
                //temp1.GetComponent<zoneManagerMove>().actiaveCircle();
                zoneListActiveMove.Add(temp);
                // temp.GetComponent<zoneManager>().updateSpherePoint();
            }
        }

        timeStartAppear = Time.time;
        active = true;
        ChangeZoneMove();


    }

    #endregion

}
