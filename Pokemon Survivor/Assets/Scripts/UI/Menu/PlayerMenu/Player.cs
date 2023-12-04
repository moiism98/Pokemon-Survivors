using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class Player
{
     public GameObject mainPlayer;
    public int evoloved = 0;
    public int totalEvolutions = 0;
    public bool shiny;
    public List<GameObject> shinies = new List<GameObject>();
    public List<GameObject> evolutions = new List<GameObject>();
}
