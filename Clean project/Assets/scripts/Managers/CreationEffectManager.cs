using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationEffectManager : MonoBehaviour
{
    public static CreationEffectManager instance;
    public GameObject[] prefabCreationEffect;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerationEffect(Vector3 position, RoamingObjectType t)
    {
        
        float scale = (t == RoamingObjectType.Star) ? 3 : 1;
        DispatchEffect(CreationDestructionEffect.creationfog, position, t, Vector3.one * scale);
    }

    public void DestructionEffect(Vector3 position, RoamingObjectType t,  Color c)
    {
        float scale = (t == RoamingObjectType.Blackhole) ? 3 : 1;
        DispatchEffect(CreationDestructionEffect.destructionfog, position, c, Vector3.one * scale);
    }

    private void DispatchEffect(CreationDestructionEffect type, Vector3 position, RoamingObjectType t, Vector3 scale)
    {
        GameObject g = GameObject.Instantiate(prefabCreationEffect[(int)type]);
        g.transform.localScale = scale;
        for (int i = 0; i < g.transform.childCount; i++) {
            if (g.transform.GetChild(i).GetComponent<ParticleSystem>() != null) {
                g.transform.GetChild(i).localScale = scale;
            }
        }
        g.transform.parent = transform;
        effectdispatchment(type, position, t, g);
    }
    private void DispatchEffect(CreationDestructionEffect type, Vector3 position,  Color c, Vector3 scale)
    {
        GameObject g = GameObject.Instantiate(prefabCreationEffect[(int)type]);
        g.transform.localScale = scale;
        for (int i = 0; i < g.transform.childCount; i++)
        {
            if (g.transform.GetChild(i).GetComponent<ParticleSystem>() != null)
            {
                g.transform.GetChild(i).localScale = scale;
            }
        }
        g.transform.parent = transform;
        effectdispatchment(type, position, g, c);
    }

    private void effectdispatchment(CreationDestructionEffect type, Vector3 position, RoamingObjectType t, GameObject g)
    {
        g.transform.localPosition = position;
        StartCoroutine(waittorepositioning(g));
        ParticleSystem.MainModule main = g.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        main.startColor = t.GetColor();
        g.GetComponent<JustParticleEffect>().Play();
    }

    private void effectdispatchment(CreationDestructionEffect type, Vector3 position, GameObject g, Color c)
    {
        g.transform.localPosition = position;
        StartCoroutine(waittorepositioning(g));
        ParticleSystem.MainModule main = g.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        main.startColor = c;
        g.GetComponent<JustParticleEffect>().Play();
    }

    IEnumerator waittorepositioning(GameObject g) {
        yield return new WaitForSeconds(10);
        GameObject.Destroy(g);
    }
}

public enum CreationDestructionEffect { 
    creationfog, destructionfog
}
