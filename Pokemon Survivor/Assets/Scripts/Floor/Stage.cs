using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    public string name;
    public int initialFloor;
    public int lastFloor;
    public List<GameObject> floors;
    public GameObject stageEndFloor;
    public GameObject boss;
    public bool completed;
}
