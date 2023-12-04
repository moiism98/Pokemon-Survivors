using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnPoint : MonoBehaviour
{
    public Transform spawnPoint;
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawnPoint.position;
    }
}
