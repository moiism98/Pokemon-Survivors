using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevel : MonoBehaviour
{
    public Text level;
    PlayerStats player;
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        SetLevel();
    }

    
    void Update()
    {
        SetLevel();
    }

    void SetLevel()
    {
        if(player)
        {
            level.text = player.level.ToString();
        }
    }
}
