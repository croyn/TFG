using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RewardManager : MonoBehaviour
{
    /// <summary>
    /// singleton of the class
    /// </summary>
    public static RewardManager instance;

    /// <summary>
    /// list of all possible rward found in the resource folder
    /// </summary>
    public Object[] rewards;

    /// <summary>
    /// speed with which the aniamtion of the entering goes on
    /// </summary>
    public float animationspeed;
    /// <summary>
    /// speed of the rotation effect on the aniamtion entring of the reward
    /// </summary>
    [Range(0, 1)]
    public float rotatinspeed;

    /// <summary>
    /// Reference to the properties a reward should have.
    /// </summary>
    public RewardProps props;

    List<Object> availablerewards = new List<Object>();

    public GameObject rewardToBeMovedintoTheBlackHole = null;
    public Vector3 targetBlackHolePosition;

    /// <summary>
    /// set up the singleton
    /// </summary>
    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    /// <summary>
    /// load form resource folder all the possible visual reward
    /// </summary>
    void Start()
    {
        rewards = Resources.LoadAll("Rewards", typeof(GameObject));
        readTableOfprops();
        availablerewards.AddRange(rewards);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            AddReward(0, gameObject);
        }
        if (rewardToBeMovedintoTheBlackHole != null) {
            rewardToBeMovedintoTheBlackHole.transform.LookAt(targetBlackHolePosition);
            rewardToBeMovedintoTheBlackHole.transform.position += (targetBlackHolePosition - rewardToBeMovedintoTheBlackHole.transform.position).normalized * 0.1f;
            if (Vector3.Distance(rewardToBeMovedintoTheBlackHole.transform.position, targetBlackHolePosition) < 1) {
                GameObject.Destroy(rewardToBeMovedintoTheBlackHole);
                rewardToBeMovedintoTheBlackHole = null;
            }
        }
    }

    /// <summary>
    /// Add a reworld randomply picked up among all the possible alternatives and assign the proper behaviour
    /// </summary>
    /// <param name="worldID">index of the world in which to place the reward</param>
    public void AddReward(int worldID, GameObject blackhole) { 
        //GameObject reward = GameObject.Instantiate((GameObject)rewards[Random.Range(0,rewards.Length)]);
        if (availablerewards.Count == 0) {
            for (int i = 0; i < rewards.Length; i++)
            {
                int rnd = Random.Range(0, rewards.Length);
                Object tempGO = rewards[rnd];
                rewards[rnd] = rewards[i];
                rewards[i] = tempGO;
            }
            availablerewards.AddRange(rewards);
        }
        int index = Random.Range(0, availablerewards.Count);

        createRewardToEnterTheNewBlackHole((GameObject)availablerewards.ElementAt(index), blackhole.transform.position);

        for (int i = 0; i < 2; i++)
        {
            GameObject g = GameObject.Instantiate((GameObject)availablerewards.ElementAt(index));
            g.name = g.name + g.GetInstanceID();
            g.transform.parent = transform.GetChild(worldID);
            g.transform.localPosition = Vector3.zero;
            g.transform.localScale = Vector3.one * 10;
            foreach (MeshRenderer m in g.transform.GetChild(0).GetChild(0).GetComponentsInChildren<MeshRenderer>())
            {
                m.material.color = Color.clear;
            }
            g.AddComponent<Reward>();
            g.GetComponent<Reward>().animationspeed = animationspeed;
            g.GetComponent<Reward>().rotatinspeed = rotatinspeed;
            if (i == 0)
            {
                g.GetComponent<Reward>().SetTargetPlayer(GameObject.FindGameObjectWithTag("Player1").transform);
            }
            else {
                g.GetComponent<Reward>().SetTargetPlayer(GameObject.FindGameObjectWithTag("Player2").transform);
            }
        }

        availablerewards.RemoveAt(index);
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
    /// read the resource file where reward properties are set
    /// </summary>
    private void readTableOfprops()
    {
        string text = Resources.Load<TextAsset>("RewardProps").ToString();
        props = JsonUtility.FromJson<RewardProps>(text);
    }

    public RewardItem getPropertiesOfReward(string name) {
        foreach (RewardItem i in props.content) {
            if (name == i.RewardName) {
                return (i);
            }
        }
        return null;
    }

    private void createRewardToEnterTheNewBlackHole(GameObject chosenreward, Vector3 targetBlackHolePosition) {
        rewardToBeMovedintoTheBlackHole = GameObject.Instantiate(chosenreward);
        RewardItem i = RewardManager.instance.getPropertiesOfReward(rewardToBeMovedintoTheBlackHole.name.Split('(')[0]);
        float x = 0, y = 0;
        if (Random.value > 0.5f)
        {
            if (Random.value > 0.5f)
            {
                x = 0;
            }
            else
            {
                x = 100;
            }
            y = Random.Range(0, 100);
        }
        else {
            if (Random.value > 0.5f)
            {
                y = 0;
            }
            else
            {
                y = 100;
            }
            x = Random.Range(0, 100);
        }
        Vector3 origin = new Vector3(x,5,y);
        rewardToBeMovedintoTheBlackHole.transform.position = origin;
        rewardToBeMovedintoTheBlackHole.transform.localScale = Vector3.one * 1f;
        this.targetBlackHolePosition = targetBlackHolePosition + Vector3.up*3;
        if (i.Reflect) {
            rewardToBeMovedintoTheBlackHole.transform.GetChild(0).localRotation = Quaternion.Euler(0, 180, 0); 
        }
    }
}
