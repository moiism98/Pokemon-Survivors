using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCondition : MonoBehaviour
{
    public List<RuntimeAnimatorController> controllers = new List<RuntimeAnimatorController>();
    public Animator animator;
    PlayerStats playerStats;
    public SpriteRenderer spriteRenderer;

    public float initialCooldown = 3f;
    public float statusCooldown;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        statusCooldown = initialCooldown;
    }


    public void PlayerIsHealthy()
    {
        if(playerStats.condition == Condition.healthy)
        {
            animator.runtimeAnimatorController = null;
            spriteRenderer.enabled = false;
        }
    }
}
