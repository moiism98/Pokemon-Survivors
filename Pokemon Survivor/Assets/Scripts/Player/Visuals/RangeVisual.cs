using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangeVisual
{
    public Transform visual;

    // attackRange = 1  attackRange = 1.5   attackRange = 2
    // scale = 8        scale = 12           scale = 16 
    // every 0.5f attackRange the scale is 4f more so the multiplier has to be 8f;
    

    // UPDATE: forget the formule, you have to find the correct multiplier, so it has to be public.
    public float multiplier;

    public void CalculateScale(float range)
    {
        visual.localScale = new Vector3((multiplier * range), (multiplier * range));
    }
}
