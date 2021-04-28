using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timerZone : MonoBehaviour
{
    public static timerZone instance;
    float timeStartAppear = 0.0f;
    float timeNextAppear = 0.0f;
    public float timeToNext=1.0f;
    public float timeToOffZones = 1.0f;
    float timeNextOff = 0.0f;
    public List<GameObject> zoneList = null;
    public List<GameObject> zoneListMove = null;
    public List<GameObject> zoneScan = null;
    private List<Transform> zoneListActive = new List<Transform>();
    private List<Transform> zoneListActiveMove = new List<Transform>();
    public GameObject pointCentralMandala;
    public GameObject blackCircle;
    bool active = false;
    private int ControlWhichZone = 2;
    private int ControlWhichMove = 0;
    public int controlFase = 3; //0 absorv , 1 move
    Gradient actualGradient = null;
    private bool ParticlesDoneAbosorving = false;
    private bool controlOneTimeAppering = false;
    private bool controlminiMoveZone = false;
    public float timeBetweenFases;
    private bool timeBetweenFasesControl = false;
    public Vector3 sizeFinal;
    public Vector3 incrementSize;
    public Vector3 sizeFinalCircle;
    public Vector3 incrementSizeCircle;
    public float tempMaxFirstLayer ;
    public float tempMaxSecondLayer;
    public float tempMaxThirdLayer ;
    public float tempMaxCircleLayer ;
    public int contadorIterations;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        timeNextAppear = 0.0f;
        timeNextOff = 0.0f;
        controlFase = 3;
        contadorIterations = 0;
        changeFaseTo(3);
    }

    // Update is called once per frame
    void Update()
    {
        timeNextAppear = timeNextAppear + Time.deltaTime;


        if (controlFase == 0)
        {
            if (timeNextAppear >= timeToNext)
            {

                activateZones();

                timeNextAppear = 0.0f;
            }
            else if (timeNextAppear >= timeToNext - timeToOffZones)
            {
                if (active)
                {
                    //getAllTimes();

                    activeZonesTouched();
                    deactivateZones();

                }
            }
        }
        else if (controlFase == 1)
        {
            if (!ParticlesDoneAbosorving)
            {
                deactivateZones();
                if (!isAbsorbParticlesDone())
                {

                    ParticlesDoneAbosorving = true;
                    timeNextAppear = 0.0f;

                    audioController.instance.playAudio(3);
                }
            }

            if (!timeBetweenFasesControl && timeNextAppear >= timeBetweenFases && ParticlesDoneAbosorving)
            {

                deactivateZonesCircles();
                
                activaMovezones(0);
                timeBetweenFasesControl = true;
                timeNextAppear = 0.0f;

            }


            if (isDoneMoving() && ParticlesDoneAbosorving && timeBetweenFasesControl && timeNextAppear >= timeBetweenFases)
            {

                deactiveMoveZones();


                if (mandalamanager.instance.trianglesDone && mandalamanager.instance.circleDone)
                {
                    changeFaseTo(6);
                }
                else
                {
                    activateZonesCircles();
                    controlminiMoveZone = false;
                    changeFaseTo(2);
                }

            }
            else if (!isDoneMoving() && ParticlesDoneAbosorving && timeBetweenFasesControl)
            {
                timeNextAppear = 0.0f;
            }



        }
        else if (controlFase == 2)
        {
            if (!controlminiMoveZone)
            {
                activaMiniMovezones();
                controlminiMoveZone = true;
                timeNextAppear = 0.0f;
                //&& timeNextAppear >= 0.4f
            }
            else if (isDoneMovingMiniZone() && controlminiMoveZone)
            {
                deactiveMiniMoveZones();
                changeFaseTo(0);
            }

        }
        else if (controlFase == 3)
        {
            if (!controlminiMoveZone && checkScanZones())
            {

                timeNextAppear = 0.0f;
                controlminiMoveZone = true;
            }

            if (controlminiMoveZone && timeNextAppear > 2.0f)
            {
                timeNextAppear = 0.0f;
                timeBetweenFasesControl = true;
                changeFaseTo(4);
            }

        }
        else if (controlFase == 4)
        {


            if (timeNextAppear >= 1.0f && timeBetweenFasesControl)
            {
                timeBetweenFasesControl = false;
                activateParticlesZoneScan(0);
            }

            if (checkPartcilesCatchedZonesFromScanZones())
            {
                deactivateZoneScan(1);
                changeFaseTo(6);
            }
        }
        else if (controlFase == 5) //no se usa pendiente quitar
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


            }

        }
        else if (controlFase == 6)
        {

            if (checkScanZonesFromMainZone())
            {
                if (mandalamanager.instance.trianglesDone && mandalamanager.instance.circleDone)
                {
                    //deactivateZoneScan(2);
                    changeFaseTo(8);
                }
                else
                {
                    changeFaseTo(7);
                }

            }
        }
        else if (controlFase == 7)
        {

            if (mandalamanager.instance.checkPointsInPosition())
            {
                deactivateZoneScan(0);
                changeFaseTo(0);
            }
        }
        else if (controlFase == 8) {
            
            if (timeNextAppear <= 2.0f)
            {
                reSizeMandala();
            }
            else {
                if (reSizeMandala() && reSizeBlackCircle())
                {
                    Debug.Log("SE ACABO");
                }
            }
            

        }


    }

    public bool reSizeMandala() {
        float x = mandalamanager.instance.gameObject.transform.localScale.x;
        float y = mandalamanager.instance.gameObject.transform.localScale.y;
        float z = mandalamanager.instance.gameObject.transform.localScale.z;
        if (x+incrementSize.x <= sizeFinal.x && y+incrementSize.y <= sizeFinal.y && z+incrementSize.z <= sizeFinal.z) {
            mandalamanager.instance.gameObject.transform.localScale = mandalamanager.instance.gameObject.transform.localScale + incrementSize;
            return false;
        }
        else
        {
            return true;
        }

    }

    public bool reSizeBlackCircle()
    {
        
    
        blackCircle.SetActive(true);
        float x = blackCircle.transform.localScale.x;
        float y = blackCircle.transform.localScale.y;
        float z = blackCircle.transform.localScale.z;
        if (x + incrementSizeCircle.x <= sizeFinalCircle.x && y + incrementSizeCircle.y <= sizeFinalCircle.y && z + incrementSizeCircle.z <= sizeFinalCircle.z)
        {
            blackCircle.transform.localScale = blackCircle.transform.localScale + incrementSizeCircle;
            return false;
        }
        else
        {
            return true;
        }

    }


    public void changeFaseTo(int cual) {

        switch (cual) {
            case 0://absorv
                audioController.instance.playAudio(mandalamanager.instance.whichSong());
                ControlWhichZone = 3;
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = true;
                //deactivateZonesMove();
                timeNextAppear = 0.0f;
                activateZonesCircles();
                break;
            case 1://move
                contadorIterations = 0;
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = false;
                ParticlesDoneAbosorving = false;
                timeBetweenFasesControl = false;
                break;
            case 2://activating circles
                
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = false;
                controlminiMoveZone = false;
                break;
            case 3://first scan
                deactivateZonesCircles();
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = false;
                mandalamanager.instance.deactivateAllPoints();
                controlminiMoveZone = false;
                break;

            case 4://appear and capturing circle
                activateParticlesZoneScan(1);
                break;
            case 5://move scan//no se usa pendiente quitar
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = false;
                ParticlesDoneAbosorving = false;
                timeBetweenFasesControl = false;
                deactivateZoneScan(0);
                break;
            case 6://second scan
                if (mandalamanager.instance.trianglesDone && mandalamanager.instance.circleDone)
                {
                    deactivateZoneScan(2);
                }
                else
                {
                    deactivateZoneScan(1);
                }
                    
                controlminiMoveZone = false;
                activateZoneScanFromMainZone();
                pointCentralMandala.GetComponent<PointCentralMandala>().allowAbsorv = false;
                break;
            case 7://appear mandala
                deactivateZoneScan(0);
                deactivateZoneScan(1);
                mandalamanager.instance.activateAllPoints();
                break;
            case 8://final
                deactivateZoneScan(2);
                timeNextAppear = 0.0f;
                break;

        }

        controlFase = cual;
    }


    public void deactivateZoneScan(int cual) {

        if (cual == 0)
        {
            for (int i = 0; i < zoneScan.Count; i++)
            {
                zoneScan[i].SetActive(false);

            }

        }
        else if (cual == 1)
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
        else if (cual == 2) {
            for (int i = 0; i < zoneList.Count; i++)
            {
                Transform tempFoot = zoneList[i].transform.Find("footPrint");
                tempFoot.GetComponent<collisionScan>().changeColorTo(0);
                tempFoot.GetComponent<collisionScan>().enabled = false;

            }
        }
        

    }

    public void activateParticlesZoneScan(int cual) {

        for (int i = 0; i < zoneScan.Count; i++)
        {
            if (cual == 0)
            {
                Transform zone = zoneScan[i].transform.Find("frenteZone");
                zone.gameObject.GetComponent<simpleColliderZone>().allowCollision = true;
                Transform particles = zone.Find("Particle System");
                particles.gameObject.GetComponent<ParticleSystem>().Play();
            }
            else if (cual == 1) {
                Transform zone = zoneScan[i].transform.Find("frenteZone");
                zone.transform.Find("Cube").gameObject.SetActive(true);
            }
            

        }

    }

    

    public void activateZoneScanFromMainZone() {
        for (int i = 0; i < zoneList.Count; i++) {
            // zoneList[i].GetComponent<zoneManager>().activateFootPrint();
            Transform tempFoot = zoneList[i].transform.Find("footPrint");
            tempFoot.gameObject.SetActive(true);
            tempFoot.GetComponent<collisionScan>().enabled = true;

        }
    }

    public bool checkScanZones() {
        bool resp = true;
        for (int i = 0; i < zoneScan.Count; i++) {
            Transform foot = zoneScan[i].transform.Find("footPrint");
            bool tempResp =true;
            Color tempColor = foot.gameObject.GetComponent<collisionScan>().actualColor;
            if (tempColor == Color.green) {
                tempResp = true;
            }
            else {
                tempResp = false;
            }
            resp = resp && tempResp;

        }
        if (resp)
        {

        }
        return resp;
    }

    public bool checkScanZonesFromMainZone()
    {

        bool resp = true;
        for (int i = 0; i < zoneList.Count; i++)
        {
            Transform foot = zoneList[i].transform.Find("footPrint");
            bool tempResp = true;
            Color tempColor = foot.gameObject.GetComponent<collisionScan>().actualColor;
            if (tempColor == Color.green)
            {
                tempResp = true;
            }
            else
            {
                tempResp = false;
            }
            resp = resp && tempResp;

        }
        if (resp)
        {

        }
        return resp;

    }

    public bool checkPartcilesCatchedZonesFromScanZones()
    {

        bool resp = true;
        for (int i = 0; i < zoneScan.Count; i++)
        {
            Transform zone = zoneScan[i].transform.Find("frenteZone");
            
            resp = resp && zone.gameObject.GetComponent<simpleColliderZone>().isZoneOver();

        }
        if (resp)
        {

        }
        return resp;

    }


    public void activaMovezones(int cual) {

        ParticleSystem.ColorOverLifetimeModule temp = zoneList[0].transform.Find("derechaZone").GetComponent<zoneManager>().AffectedParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;

        ParticleSystem.MinMaxGradient colorNormal = temp.color;
        ParticleSystem.MinMaxGradient colorNotouch = zoneList[0].transform.Find("derechaZone").GetComponent<zoneManager>().GradientColorMove;
        for (int i = 0; i < zoneListMove.Count;i++) {
            zoneListMove[i].GetComponent<MoveZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColorMove());
            zoneListMove[i].GetComponent<MoveZone>().initMoveZone(cual);
            
        }
    }


    public void activaMiniMovezones()
    {
        ParticleSystem.ColorOverLifetimeModule temp = zoneList[0].transform.Find("derechaZone").GetComponent<zoneManager>().AffectedParticles.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;

        ParticleSystem.MinMaxGradient tempColor = temp.color;
        for (int i = 0; i < zoneList.Count; i++)
        {
            Transform minizone1 = zoneList[i].transform.Find("MiniMoveZone1");
            Transform minizone2 = zoneList[i].transform.Find("MiniMoveZone2");
            Transform minizone3 = zoneList[i].transform.Find("MiniMoveZone3");
            if (minizone1.gameObject.GetComponent<MoveZone>().isActiveAndEnabled) {
                //Debug.Log("Entro moveZone activate");
                minizone1.gameObject.GetComponent<MoveZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());
                minizone2.gameObject.GetComponent<MoveZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());
                minizone3.gameObject.GetComponent<MoveZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());
                minizone1.gameObject.GetComponent<MoveZone>().initMoveZone(0);
                minizone2.gameObject.GetComponent<MoveZone>().initMoveZone(0);
                minizone3.gameObject.GetComponent<MoveZone>().initMoveZone(0);
            }

            if (minizone1.gameObject.GetComponent<miniRiverZone>().isActiveAndEnabled) {
             //   Debug.Log("Entro miniRiverZone activate");
                minizone1.gameObject.GetComponent<miniRiverZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());
                minizone2.gameObject.GetComponent<miniRiverZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());
                minizone3.gameObject.GetComponent<miniRiverZone>().ChangeColorTo(mandalamanager.instance.switchColor(), mandalamanager.instance.switchColor());
                minizone1.gameObject.GetComponent<miniRiverZone>().PlayRiver();
                minizone2.gameObject.GetComponent<miniRiverZone>().PlayRiver();
                minizone3.gameObject.GetComponent<miniRiverZone>().PlayRiver();
            }
          
        }
 
    }


    private bool isAbsorbParticlesDone()
    {
        bool control = false;
        for (int i = 0; i < zoneList.Count; i++)
        {
            control= control || zoneList[i].transform.Find("derechaZone").GetComponent<zoneManager>().isMovingParticles();
            control = control || zoneList[i].transform.Find("frenteZone").GetComponent<zoneManager>().isMovingParticles();
            control = control || zoneList[i].transform.Find("izquierdaZone").GetComponent<zoneManager>().isMovingParticles();
     
        }

        return control;

    }


    private void activeZonesTouched() {
        Transform temp = null;
        bool controlOneActive = false;
        for (int i = 0; i < zoneListActive.Count; i++)
        {

            temp = zoneListActive[i];
            if (temp.GetComponent<zoneManager>().isCatched()) {
                temp.GetComponent<zoneManager>().activeExplosion();
                Logger.addParticlesCatch(temp.name, temp.GetComponent<zoneManager>().getUserCatch(), temp.GetComponent<zoneManager>().getWhenCollision());
                controlOneActive = true;
            }
            
            

        }


        float timeTemp = 0.0f;
        if (!mandalamanager.instance.trianglesDone)
        {
            switch (mandalamanager.instance.layer)
            {
                case 0:
                    timeTemp = tempMaxFirstLayer;
                    break;
                case 1:
                    timeTemp = tempMaxSecondLayer;
                    break;
                case 2:
                    timeTemp = tempMaxThirdLayer;
                    break;
            }
        }
        else
        {
            timeTemp = tempMaxCircleLayer;
        }

        if (contadorIterations == 0) {
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable = 0.0f;
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = 0.0f;
        }
            contadorIterations = contadorIterations + 1;
        
       

        if (controlOneActive)
        {

            //Debug.Log("Time " + timeTemp);
            //timeTemp = timeTemp / timeToNext;
            //Debug.Log("timeToNext " + timeToNext);
            // Debug.Log("timeTemp calc " + timeTemp);
            float num = mandalamanager.instance.actulgetNumberLines();
            // Debug.Log("num lines total " + num);
            float numLines = num / timeTemp;
            //Debug.Log("numLines calc " + numLines);
            float calc = mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable + (numLines) * 2;
            // Debug.Log("Tiene point centralDesdeTimer " + calc);
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable = calc + mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched;
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = 0.0f;
        }
        else if(!controlOneActive && ControlWhichZone!=3) {

            // Debug.Log("Time " + timeTemp);
            //timeTemp = timeTemp / timeToNext;
            // Debug.Log("timeToNext " + timeToNext);
            // Debug.Log("timeTemp calc " + timeTemp);
            float num = mandalamanager.instance.actulgetNumberLines();
            //Debug.Log("num lines total " + num);
            float numLines = num / timeTemp;
            // Debug.Log("numLines calc " + numLines);
            float calc = mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched + (numLines) * 2;
            // Debug.Log("Tiene point centralDesdeTimer " + calc);
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched +calc;


        }




        if (contadorIterations >= (timeTemp + 1) + ((timeTemp + 1) / 3.0f) && !mandalamanager.instance.trianglesDone)
        {
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable = mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable + mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched;
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = 0.0f;
            contadorIterations = 0;
        }
        else if (contadorIterations >= (timeTemp ) && mandalamanager.instance.trianglesDone) {
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable = mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numActivationAvailable + mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched;
            mandalamanager.instance.centralPoint.GetComponent<PointCentralMandala>().numNotCatched = 0.0f;
            contadorIterations = 0;
        }
        


    }

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

    private void ChangeZone() {

        switch (ControlWhichZone) {
            case 0://derecha
                ControlWhichZone = 2;
                break;
            case 1://frente
                ControlWhichZone = 3;
                break;
            case 2://izquierda
                ControlWhichZone = 1;
                break;
            case 3://silencio
                ControlWhichZone = 0;
                break;
        }
        
    }

    private bool isDoneMoving() {

        int control = 0;
        for (int i = 0; i < zoneListMove.Count; i++) {
            if (zoneListMove[i].GetComponent<MoveZone>().isInPosition()) {
                control = control + 1;
            }

        }

        if (control == zoneListMove.Count)
        {
            return true;
        }
        else {
            return false;
        }

    }
    private bool isDoneMovingMiniZone()
    {

        int control = 0;
        for (int i = 0; i < zoneList.Count; i++)
        {
            Transform minizone1= zoneList[i].transform.Find("MiniMoveZone1");
            Transform minizone2 = zoneList[i].transform.Find("MiniMoveZone2");
            Transform minizone3 = zoneList[i].transform.Find("MiniMoveZone3");
            bool inPos1=false;
            bool inPos2 = false;
            bool inPos3 = false;
            if (minizone1.gameObject.GetComponent<MoveZone>().isActiveAndEnabled) {
                //Debug.Log("Entro movezone doneMoving:");
                inPos1 = minizone1.gameObject.GetComponent<MoveZone>().isInPosition();
                inPos2 = minizone2.gameObject.GetComponent<MoveZone>().isInPosition();
                inPos3 = minizone3.gameObject.GetComponent<MoveZone>().isInPosition();
            }

            if (minizone1.gameObject.GetComponent<miniRiverZone>().isActiveAndEnabled)
            {
                //Debug.Log("Entro miniRiverZone doneMoving:");
                inPos1 = minizone1.gameObject.GetComponent<miniRiverZone>().movingDone();
                inPos2 = minizone2.gameObject.GetComponent<miniRiverZone>().movingDone();
                inPos3 = minizone3.gameObject.GetComponent<miniRiverZone>().movingDone();
            }

            if (inPos1 && inPos2 && inPos3)
            {
                control = control + 1;
            }

        }

        if (control == zoneList.Count)
        {
            //Debug.Log("Entro en control true");
            return true;
        }
        else
        {
            return false;
        }

    }


    private void deactiveMoveZones() {

        for (int i = 0; i < zoneListMove.Count; i++)
        {
            zoneListMove[i].GetComponent<MoveZone>().deactiveMoveZone();

        }

    }
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



    private void ChangeZoneMove()
    {
        ControlWhichMove = ControlWhichMove + 1;


        if (ControlWhichMove > 7) {
            ControlWhichMove = -1;
        }

    }

    void activateZonesMove() {

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

    void activateNextMove() {
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

    void activateZones() {
        Transform internalZone = null;
        ChangeZone();
        Gradient colorActual = mandalamanager.instance.switchColor(); 
         Gradient colorActualMove = mandalamanager.instance.switchColorMove();
        for (int i = 0; i < zoneList.Count; i++)
        {
            //int result = Random.Range(0, 3);
            //result = 1;
            
            switch (ControlWhichZone)
            {

                case 0:

                    internalZone = zoneList[i].transform.Find("derechaZone");
                    break;
                case 1:
                    internalZone = zoneList[i].transform.Find("frenteZone");
                    break;
                case 2:
                    internalZone = zoneList[i].transform.Find("izquierdaZone");
                    break;
            }
            
            if (internalZone != null)
            {
                internalZone.GetComponent<zoneManager>().ChangeColorTo(colorActual, colorActualMove);
                internalZone.GetComponent<zoneManager>().activateZone();
                
                zoneListActive.Add(internalZone);
                // temp.GetComponent<zoneManager>().updateSpherePoint();
            }
        }


        if (internalZone != null) {
            Logger.addParticlesAppear(internalZone.name);
        }

        timeStartAppear = Time.time;
        active = true;

    }


    public void deactivateZonesMove() {
        Transform temp = null;
        
        for (int i = 0; i < zoneListActiveMove.Count; i++)
        {

            temp = zoneListActiveMove[i];
            temp.GetComponent<zoneManagerMove>().deactivateZone();
            temp.GetComponent<zoneManagerMove>().deactiaveCircle();

        }



        zoneListActiveMove.Clear();
        active = false;
    }

    void deactivateZones() {

        
        Transform temp = null;

        for (int i = 0; i < zoneListActive.Count;i++)
        {
            
            temp = zoneListActive[i];
            temp.GetComponent<zoneManager>().deactivateZone();
            
        }
        zoneListActive.Clear();
        active = false;
    }

    void deactivateZonesCircles() {
        Transform temp;
        Transform temp1;
        Transform temp2;


        for (int i = 0; i < zoneList.Count; i++) {

            temp = zoneList[i].transform.Find("derechaZone");
            temp1 = zoneList[i].transform.Find("frenteZone");
            temp2 = zoneList[i].transform.Find("izquierdaZone");

            temp.gameObject.GetComponent<zoneManager>().deactiaveCircle();
            temp1.gameObject.GetComponent<zoneManager>().deactiaveCircle();
            temp2.gameObject.GetComponent<zoneManager>().deactiaveCircle();
        }
    }

    public void activateZonesCircles()
    {
        Transform temp;
        Transform temp1;
        Transform temp2;


        for (int i = 0; i < zoneList.Count; i++)
        {

            temp = zoneList[i].transform.Find("derechaZone");
            temp1 = zoneList[i].transform.Find("frenteZone");
            temp2 = zoneList[i].transform.Find("izquierdaZone");

            temp.gameObject.GetComponent<zoneManager>().actiaveCircle();
            temp1.gameObject.GetComponent<zoneManager>().actiaveCircle();
            temp2.gameObject.GetComponent<zoneManager>().actiaveCircle();
        }
    }

}
