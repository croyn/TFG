using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class to manage effects that coinvolge only a particle system
/// </summary>
public class JustParticleEffect : SpecificEffectManager
{
    /// <summary>
    /// start the particle effect
    /// </summary>
    public override void Play() {
        GetComponent<ParticleSystem>().Play();
    }
}
