using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recovery : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public PlayerStats playerStats;
    public Animator playerAnimator;

    public float healingAmount = .5f;
    public float initalCooldown;
    private float recoveryCooldown;

    public static bool recovering = false;
    public static bool showAnimation = true;

    private void Start()
    {
        recoveryCooldown = initalCooldown;
    }

    private void Update()
    {
        // we activate the recovery in player attack script, so both animations are played at same time
        // if we are recovering
        if (recovering)
        {
            // we show the animation of the ability, and disable it with the boolean
            // so the animation is not looping every second, only when we activate the ability.
            if (showAnimation) 
                ShowAnimation();

            playerStats.playerHealth += healingAmount; // we heal every second an amount of health
            
            recoveryCooldown -= Time.deltaTime;

            if(recoveryCooldown <= 0) // if the ability time is over, we stop recovering and reset the cooldown again.
            {
                recovering = false;
                recoveryCooldown = initalCooldown;
            }
        }
    }

    // we disable the animation at his last frame, in the unity ui
    void DisableSpriteRenderer() 
    {
        spriteRenderer.enabled = false;
    }

    void ShowAnimation()
    {
        animator.Rebind();

        spriteRenderer.enabled = true;

        showAnimation = false;

    }

}
