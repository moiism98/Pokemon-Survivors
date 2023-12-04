using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public GameObject pokemon;
    
    [Range(1, 100)]
    public float spawnProbability;
}
