using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Execute the play effect on the AudioSource for the audioplayer Effect
/// </summary>
public class AudioEffect : SpecificEffectManager
{
    /// <summary>
    /// start the particle effect
    /// </summary>
    public override void Play()
    {
        GetComponent<AudioSource>().Play();
    }
}
