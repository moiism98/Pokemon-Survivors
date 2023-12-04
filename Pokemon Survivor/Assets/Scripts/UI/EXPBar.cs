using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{
    public Slider expPoints;
    PlayerStats player;
    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        SetExperienceBar();
    }

    private void Update()
    {
        SetExperienceBar();
    }

    void SetExperienceBar()
    {
        if (player)
        {
            expPoints.maxValue = player.playerMaxExp;
            expPoints.value = player.playerExp;
        }
    }
}
