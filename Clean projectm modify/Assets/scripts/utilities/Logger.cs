using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    static string filepath;
    static string fileUserpath;
    static string DateTimeFormat = "yyyy/MM/dd HH:mm:ss.ffff";
    static List<Dictionary<string, string>> listLog = new List<Dictionary<string, string>>();
    static List<Dictionary<string, string>> listPosition = new List<Dictionary<string, string>>();

    // Start is called before the first frame update
    void Start()
    {
       
        filepath = Application.persistentDataPath + "/" + Application.productName + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + "_Mandala.log";
        Debug.Log("Deberia en " + filepath);
        fileUserpath = Application.persistentDataPath + "/" + Application.productName + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + "_M_USERDATA.log";
        addLogLine("[", true);
        addUserPositionLogLine("[", true);
    }

    void OnApplicationQuit()
    {
#if !UNITY_EDITOR

        // return;
         
        // Debug.Log("Guardo en " + filepath);

        for (int i = 0; i < listLog.Count; i++) {
            addLogLine(JsonFormat(listLog[i]));
        }
        for (int i = 0; i < listPosition.Count; i++)
        {
            addUserPositionLogLine(JsonFormat(listPosition[i]));
        }


        string fullog = File.ReadAllText(filepath);
        if (fullog.Length > 1)
        {
            fullog = fullog.Substring(0, fullog.Length - 1);
            fullog += "]";
            File.WriteAllText(filepath, fullog);
        }
        else
        {
            File.Delete(filepath);
        }

        string fullogUser = File.ReadAllText(fileUserpath);
        if (fullogUser.Length > 1)
        {
            fullogUser = fullogUser.Substring(0, fullogUser.Length - 1);
            fullogUser += "]";
            File.WriteAllText(fileUserpath, fullogUser);
        }
        else {
            File.Delete(fileUserpath);
        }
#endif
    }

    private static void addUserPositionLogLine(string content, bool ending = false)
    {
//#if UNITY_EDITOR

///       return;
//#endif
        if (ending)
        {
            File.AppendAllText(fileUserpath, content);
        }
        else
        {
            File.AppendAllText(fileUserpath, content + ",");
        }
    }

    private static void addLogLine(string content, bool ending = false) {
//#if UNITY_EDITOR

//        return;
//#endif
        if (ending)
        {
            File.AppendAllText(filepath, content);
        }
        else
        {
            File.AppendAllText(filepath, content + ",");
        }
    }

     #region ParticlesAppear

    public static void addParticlesAppear(string where) {
        Dictionary<string, string> val = new Dictionary<string, string>();
        val.Add("Time", System.DateTime.Now.ToString(DateTimeFormat));
        val.Add("Event", "Appear particles");
        val.Add("Position", where);
        //addLogLine(JsonFormat(val));
        listLog.Add(val);
    }


    public static void addParticlesCatch(string whatZone,string wichUser,System.DateTime whenIsCatched)
    {
        Dictionary<string, string> val = new Dictionary<string, string>();
        val.Add("Time", whenIsCatched.ToString(DateTimeFormat));
        val.Add("Event", "Particles Catch");
        val.Add("What zone", whatZone);
        val.Add("User", wichUser);
        //addLogLine(JsonFormat(val));
        listLog.Add(val);
    }


    #endregion

    #region changeFase

    #endregion
    public static void addChangeFase(string whatFase)
    {
        Dictionary<string, string> val = new Dictionary<string, string>();
        val.Add("Time", System.DateTime.Now.ToString(DateTimeFormat));
        val.Add("Event", "Change Fase");
        val.Add("What Fase", whatFase);
        //addLogLine(JsonFormat(val));
        listLog.Add(val);
    }

    #region moving

    public static void addTouchingMoving(string whatZone, string wichUser)
    {
        Dictionary<string, string> val = new Dictionary<string, string>();
        val.Add("Time", System.DateTime.Now.ToString(DateTimeFormat));
        val.Add("Event", "Moving Particles Catch");
        val.Add("What moving zone", whatZone);
        val.Add("User", wichUser);
        //addLogLine(JsonFormat(val));
        listLog.Add(val);
    }

    public static void addNotTouchingMoving(string whatZone, string wichUser)
    {
        Dictionary<string, string> val = new Dictionary<string, string>();
        val.Add("Time", System.DateTime.Now.ToString(DateTimeFormat));
        val.Add("Event", "Moving Particles Not Catch");
        val.Add("What moving zone", whatZone);
        val.Add("User", wichUser);
        //addLogLine(JsonFormat(val));
        listLog.Add(val);
    }


    #endregion


    #region Scan

    public static void addParticlesScanZoneAppear(string where)
    {
        Dictionary<string, string> val = new Dictionary<string, string>();
        val.Add("Time", System.DateTime.Now.ToString(DateTimeFormat));
        val.Add("Event", "Appear particles in Scan zone");
        val.Add("Position", where);
        //addLogLine(JsonFormat(val));
        listLog.Add(val);
    }

    public static void addParticlesCatchScan(string whatZone, string wichUser, System.DateTime whenIsCatched)
    {
        Dictionary<string, string> val = new Dictionary<string, string>();
        val.Add("Time", whenIsCatched.ToString(DateTimeFormat));
        val.Add("Event", "Particles Catch");
        val.Add("What zone", whatZone);
        val.Add("User", wichUser);
        //addLogLine(JsonFormat(val));
        listLog.Add(val);
    }

    public static void addScannedIn(string whatZone, string wichUser, System.DateTime whenIsCatched)
    {
        Dictionary<string, string> val = new Dictionary<string, string>();
        val.Add("Time", whenIsCatched.ToString(DateTimeFormat));
        val.Add("Event", "Scanned User in");
        val.Add("What zone", whatZone);
        val.Add("User", wichUser);
        //addLogLine(JsonFormat(val));
        listLog.Add(val);
    }

    public static void addScannedOut(string whatZone, string wichUser, System.DateTime whenIsCatched)
    {
        Dictionary<string, string> val = new Dictionary<string, string>();
        val.Add("Time", whenIsCatched.ToString(DateTimeFormat));
        val.Add("Event", "Scanned User out");
        val.Add("What zone", whatZone);
        val.Add("User", wichUser);
        //addLogLine(JsonFormat(val));
        listLog.Add(val);
    }

    #endregion


    #region PositionLog
    public static void AddUserPosition(string playerID, Vector3 position)
    {
        Dictionary<string, string> val = new Dictionary<string, string>();
        val.Add("Time", System.DateTime.Now.ToString(DateTimeFormat));
        val.Add("Player.Color", playerID.ToString());
        val.Add("Position.X", position.x.ToString());
        val.Add("Position.Y", position.z.ToString());
        //addUserPositionLogLine(JsonFormat(val));
        listPosition.Add(val);
    }
    #endregion

    private static string JsonFormat(Dictionary<string, string> values) {
        string s = "{";
        foreach (string k in values.Keys) {
            s += "\"" + k + "\" : \"" + values[k] + "\",";
        }
        if(values.Count > 0){
            s = s.Substring(0, s.Length - 1);
        }
        s += "}";
        return s;
    }
}
