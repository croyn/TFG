using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// geenral interface for the effect controllers
/// </summary>
public abstract class SpecificEffectManager : MonoBehaviour
{
    /// <summary>
    /// method used to fire an effect
    /// </summary>
    public abstract void Play();
}
