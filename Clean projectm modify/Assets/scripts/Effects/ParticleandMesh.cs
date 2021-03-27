using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class to manage effects that coinvolge a particle system and a mesh
/// </summary>
public class ParticleandMesh : SpecificEffectManager
{
    /// <summary>
    /// start the particle effect
    /// </summary>
    public override void Play()
    {
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(AppearAndDisappear());
    }

    private IEnumerator AppearAndDisappear()
    {
        Material m = GetComponentInChildren<MeshRenderer>().material;
        m.color = Color.clear;
        do{
            m.color = Color.Lerp(m.color, Color.white, Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }while(m.color.a > 0.9);
        m.color = Color.white;
        yield return new WaitForSeconds(4);
        do
        {
            m.color = Color.Lerp(m.color, Color.clear, Time.deltaTime);
            yield return new WaitForFixedUpdate();
        } while (m.color.a < 0.1);
        m.color = Color.clear;
    }
}
