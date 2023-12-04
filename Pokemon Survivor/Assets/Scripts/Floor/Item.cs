using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public GameObject item;
    
    [Range(0, 100)]
    public float probability;
}
