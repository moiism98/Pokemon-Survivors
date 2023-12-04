using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodStatusCondition : MonoBehaviour
{
    public List<RuntimeAnimatorController> controllers = new List<RuntimeAnimatorController>();
    public Animator animator;
    public Animator playerAnimator;
    public SpriteRenderer spriteRenderer;
    public PlayerStats playerStats;
    public PlayerAttack playerAttack;
    public PlayerMovement playerMovement;

    public float initialCooldown;
    public float goodStatusCooldown;

    public static bool applyEffect = false;
    bool activateCooldown = false;

    private void Start()
    {
        goodStatusCooldown = initialCooldown;
    }

    private void Update()
    {
        //this can not work like recovery script because attacks like this has a status condition
        // icon with animation, we have to copy the idea but modifying it
        // we check if we are pressing the right click and we have enough strong attack points

        //if we do we apply the effects in our player
        if(applyEffect)
            ApplyEffects();
        
        // the below's methods triggers the power up time effect
        if(activateCooldown)
        {
            goodStatusCooldown -= Time.deltaTime;

            
            if(goodStatusCooldown <= 0) 
            {
                FinishEffects(); // when it rises to 0 we reset to normal the player stats
                ResetCondition(); // and hide the status effect indicator
            }
        }
    }

    void ApplyEffects()
    {
        switch (playerStats.goodCondition)
        {
            case GoodCondition.damageup:

                animator.runtimeAnimatorController = controllers[0];
                playerAttack.damage *= 2;
                
                activateCooldown = true; // effect time starts
                applyEffect = false; // we stop appling the effects, if not they are going to accumulate every frame reaching the infinite, we do not want that.
                
            break;

            case GoodCondition.speedup:

                animator.runtimeAnimatorController = controllers[1];
                playerMovement.movementSpeed *= 2;
                activateCooldown = true;
                applyEffect = false;

            break;

            case GoodCondition.defenseup:

                
                animator.runtimeAnimatorController = controllers[2];
                EnemyAttack[] enemies = FindObjectsOfType<EnemyAttack>();
                foreach (EnemyAttack enemy in enemies)
                    enemy.attackDamage *= 0.5f;
                activateCooldown = true;
                applyEffect = false;

            break;
        }

        // every time we apply the effect we show the sprite render and rebind the animation
        animator.Rebind(); 
        spriteRenderer.enabled = true;
    }

    void FinishEffects() // it is the same method than below but the numbers are the opposite and we also do not need to activate or deactivate anything
    {
        switch (playerStats.goodCondition)
        {
            case GoodCondition.damageup:

                animator.runtimeAnimatorController = controllers[0];
                playerAttack.damage *= .5f;

                break;

            case GoodCondition.speedup:

                animator.runtimeAnimatorController = controllers[1];
                playerMovement.movementSpeed *= .5f;

            break;

            case GoodCondition.defenseup:


                animator.runtimeAnimatorController = controllers[2];
                EnemyAttack[] enemies = FindObjectsOfType<EnemyAttack>();
                foreach (EnemyAttack enemy in enemies)
                    enemy.attackDamage *= 2;

                break;
        }
        animator.runtimeAnimatorController = null;
        spriteRenderer.enabled = false;
    }

    void ResetCondition() // we reset everything in a method
    {
        playerStats.goodCondition = GoodCondition.none;
        goodStatusCooldown = initialCooldown;
        activateCooldown = false;
    }
}
