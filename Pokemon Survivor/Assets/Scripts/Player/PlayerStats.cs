using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public enum Condition
{
    healthy,
    paralized,
    poisoned,
    badlyPoisoned,
    burnt,
    frozen,
    slowed,
    slept
}
public enum GoodCondition
{
    none,
    damageup,
    speedup,
    defenseup
}
public enum FirstType
{
    water,
    fire,
    grass,
    fly,
    psychic,
    dark,
    ground,
    rock,
    steel,
    bug,
    dragon,
    electric,
    ghost,
    fairy,
    ice,
    fighting,
    poison,
    normal
}
public enum SecondType
{
    none,
    water,
    fire,
    grass,
    fly,
    psychic,
    dark,
    ground,
    rock,
    steel,
    bug,
    dragon,
    electric,
    ghost,
    fairy,
    ice,
    fighting,
    poison,
    normal
}

public class PlayerStats : MonoBehaviour
{
    HealthBar healthBar;
    StatusCondition statusCondition;
    DialogueManager dialogueManager;
    PlayerAttack playerAttack;

    #pragma warning disable CS8632
    public GoodStatusCondition? goodStatus;

    // box collider must be placed before the non trigger box collider (feeets),
    // then when we dragg in the player in this slot will take the correct collider.
    public BoxCollider2D waterDetector; 

    public Animator animator;
    public GameObject healthWarning;

    public GameObject statusEffect;
    public GameObject levelUp;
    private GameController gameController;

    public Condition condition = Condition.healthy;
    public GoodCondition goodCondition = GoodCondition.none;
    public FirstType firstType = FirstType.electric;
    public SecondType secondType = SecondType.none;

    public float playerMaxExp = 150f;
    public float playerExp = 0;
    public int incrementMaxEXP = 50;

    public float playerMaxHealth = 10f;
    public float playerHealth;
    public int incrementMaxHealth = 15;
    public int leftoversTimer = 3;

    public int level = 1;
    public float strongAttackMaxPoints = 200f;
    public float strongAttackPoints = 0;
    public bool isLevelUp = false;

    public static float damagePerLevel = 0;

    public float poison = 0.02f;
    public float greaterPoison = 0.05f;
    private int lifes = 1;
    private float invulneravilityTime = 0;
    public GameObject reviveAnimation;

    [Header("Shop items")]
    private ShopItem leftovers;
    private bool leftoversHealing = false;

    private void Start()
    {
        playerHealth = playerMaxHealth;
        statusCondition = FindObjectOfType<StatusCondition>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        gameController = FindObjectOfType<GameController>();

        // If pokemon is type water or fly (or he can fly by default) he can go through water by default,
        // activating the water detector since the start of the game.
        if (firstType.Equals(FirstType.water) || secondType.Equals(SecondType.water))
            waterDetector.enabled = true;
        
        leftovers = gameController.shopItems.Find(item => item.itemEffect.Equals(ShopItemEffect.health));
    }
    private void Update()
    {
        // take the leftovers item from the list
        
        if(leftovers != null)
        {
            bool active = leftovers.active; // and it's active

            if(active && !leftoversHealing) // if it's active we can heal every X amounts of secs with the leftovers value.
                StartCoroutine(Leftovers(leftovers.value));
        }

        // cap the health at our maxHealth

        if(playerHealth >= playerMaxHealth)
            playerHealth = playerMaxHealth;

        // when we level up

        if (playerExp >= playerMaxExp)
        {
            isLevelUp = true;
            level++; // Level up

            // Only upgrade melee range if level is divisor of 10 (every 10 levels)
            if (level % 10 == 0)
                playerAttack.meleeRange += 0.1f;

            // upgrade damage a bit

            damagePerLevel += 0.2f;

            healthBar = FindObjectOfType<HealthBar>();
            healthBar.playerHealth.maxValue += incrementMaxHealth; // Health up (stats and UI)
            playerMaxHealth += incrementMaxHealth;

            if(playerHealth < playerMaxHealth / 2)
                playerHealth = playerMaxHealth / 2;

            levelUp.SetActive(true); // Enable the gameObject with his animation.
            levelUp.GetComponent<Animator>().Rebind(); // If the player its level up multiple levels, rebind the animator in order to show the animation once.
            FindObjectOfType<AudioManager>().PlaySound("LevelUp");

            playerExp = Mathf.Round(playerExp - playerMaxExp); // Calculating the exceeded exp points. 
            playerMaxExp += Mathf.Round(playerMaxExp / incrementMaxEXP); // Every level a % of exp is added at max exp.
        }

        // we show the health warning when whe hit the 25% of health

        if (playerHealth <= playerMaxHealth * .25f)
            healthWarning.SetActive(true);
        else
            healthWarning.SetActive(false);

        if (strongAttackPoints >= strongAttackMaxPoints)
        {
            strongAttackPoints = strongAttackMaxPoints;
        }

        // it's triggered when player has a bad status condition
        StatusConditionDamage();

        if(invulneravilityTime != 0)
        {
            
            invulneravilityTime -= Time.deltaTime;

            if(invulneravilityTime <= 0)
            {

                EnemyAttack [] enemies = FindObjectsOfType<EnemyAttack>();

                if(enemies != null && enemies.Length > 0)
                {
                    foreach(EnemyAttack enemy in enemies)
                        enemy.attackDamage = enemy.initialAttackDamage;
                }

                invulneravilityTime = 0;
            }
        }
    }

    private IEnumerator Leftovers(float health)
    {
        leftoversHealing = true;

        playerHealth += health;

        yield return new WaitForSeconds(leftoversTimer);

        leftoversHealing = false;
    }
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;

        // we check if damage it's different to 0 because maybe we are reviving and 
        // we have a bit time of invulneravility  when the enemy damage is 0

        if(damage != 0)
            animator.SetFloat("Damage", 1f);

        // we have to check if the health it's below 0

        if (playerHealth <= 0)
        {   

            // if the health is below to 0, we check if we can revive or not the player

            int revive = PlayerPrefs.GetInt(DataController.resurrectionSave);
            if(revive != 0) // if we can, so... we purchased the resurrection upgrade
            {
                if(lifes <= 0) // we check if we revive before, if we did, we simply die
                    Fainted(); 
                else // if not, we revive again!
                    Revive();
            }
            else // if we have not upgraded that, we simply die
                Fainted();
        }
    }

    void StatusConditionDamage()
    {
        switch (condition)
        {
            case Condition.poisoned:

                if(!dialogueManager.inConversation)
                    TakeConditionDamage(poison);

            break;

            case Condition.badlyPoisoned:

                if(!dialogueManager.inConversation)
                    TakeConditionDamage(greaterPoison);
                    
            break;
        }
    }

    void TakeConditionDamage(float damage)
    {
        statusCondition.statusCooldown -= Time.deltaTime;
        if (statusCondition.statusCooldown <= 0)
        {
            FindObjectOfType<AudioManager>().PlaySound("Poison Damage");

            TakeDamage(playerMaxHealth * damage);

            statusCondition.statusCooldown = statusCondition.initialCooldown;
        }
        animator.SetFloat("Damage", 0);
    }

    void Fainted()
    {   
        //stop the stage theme and play the game over sound

        AudioManager audioManager = FindObjectOfType<AudioManager>();

        audioManager.StopSound(GameController.theme);

        audioManager.PlaySound("GameOver");

        // disable the player so enemies can not attack us anymore and the back to menu transition can be triggered

        gameObject.SetActive(false);

        // show the game over menu

        gameController.ShowGameOverMenu();

    }

    void Revive()
    {
        FindObjectOfType<AudioManager>().PlaySound("Recovery");

        // plays the stage theme again

        FindObjectOfType<AudioManager>().PlaySound(GameController.theme);

        // pause the time, so the player can figure it out what's happening

        Time.timeScale = 0f;

        // we trigger revive animation

        Instantiate(reviveAnimation, transform.position, Quaternion.identity);

        // cancel the faint animation, we are alive again

        animator.SetBool("Fainted", false);

        // add non damage time and drop the enemies damage to 0

        EnemyAttack [] enemies = FindObjectsOfType<EnemyAttack>();

        if(enemies != null && enemies.Length > 0)
        {
            foreach(EnemyAttack enemy in enemies)
                enemy.attackDamage = 0;
        }

        invulneravilityTime = 1.75f; // we have a time where the enemies can not damage the player

        playerHealth = playerMaxHealth * .5f; // and now the player has half of his life has health

        lifes--; // and no more extra lifes :(
    }
}
