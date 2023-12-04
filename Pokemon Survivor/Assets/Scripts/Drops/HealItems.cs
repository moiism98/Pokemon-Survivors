using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealingItems
{
    apple,
    goldenApple,
    elixir,
    aranjaBerry,
    zidraBerry,
    safreBerry,
    ataniaBerry,
    melocBerry,
    aranciaBerry,
    zrezaBerry
}
public class HealItems : MonoBehaviour
{
    PlayerStats player;
    StatusCondition statusCondition;
    public HealingItems healingItems = HealingItems.apple;

    public float appleValue;
    public float goldenAppleValue;
    public float elixirValue;
    public float aranjaBerryValue;
    public float recoveredPoints;

    bool arancia = false;
    float aranciaTime = .5f;

    private void Start()
    {
        statusCondition = FindObjectOfType<StatusCondition>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);

            player = collision.GetComponent<PlayerStats>();

            /*SpriteRenderer elixirSprite = gameObject.GetComponent<SpriteRenderer>();

            elixirSprite.enabled = false;

            CircleCollider2D elixirCollider = gameObject.GetComponent<CircleCollider2D>();

            elixirCollider.enabled = false;*/

            Heal();
        }
    }

    private void Update()
    {
        /*if (recoveredPoints > 0)
        {
            recoveredPoints -= Time.deltaTime;

            player.strongAttackPoints += 0.020f;

            IsAtCapHealth();

        }*/

        /*if (recoveredPoints < 0)
            Destroy(gameObject);*/

        if (arancia)
        {
            aranciaTime -= Time.deltaTime;
            if (aranciaTime <= 0)
            {
                player.GetComponent<Animator>().SetFloat("Damage", 0);
                aranciaTime = .5f;
                arancia = false;
            }
        }
    }
    void Heal()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();

        if (player != null)
        {
            switch (healingItems)
            {
                case HealingItems.apple:

                    audioManager.PlaySound("ItemHeal");

                    player.playerHealth += player.playerMaxHealth * appleValue;

                    IsAtCapHealth();

                    break;

                case HealingItems.goldenApple:

                    audioManager.PlaySound("ItemHeal");

                    player.playerHealth = player.playerMaxHealth;

                    player.playerMaxHealth += goldenAppleValue;

                    break;

                case HealingItems.elixir:

                    audioManager.PlaySound("Eat");

                    //recoveredPoints = player.strongAttackMaxPoints * elixirValue;
                    player.strongAttackPoints = player.strongAttackMaxPoints * elixirValue;


                break;

                case HealingItems.aranjaBerry:

                    audioManager.PlaySound("ItemHeal");

                    player.playerHealth += aranjaBerryValue;

                    IsAtCapHealth();

                    break;

                case HealingItems.aranciaBerry:

                    audioManager.PlaySound("AranciaBerry");

                    player.TakeDamage(aranjaBerryValue);

                    arancia = true;

                    break;

                case HealingItems.zidraBerry:

                    audioManager.PlaySound("ItemHeal");

                    if (player.playerHealth >= player.playerMaxHealth)
                        player.playerMaxHealth += 2;

                    player.playerHealth += aranjaBerryValue;

                    IsAtCapHealth();

                    break;

                case HealingItems.zrezaBerry:

                    audioManager.PlaySound("Eat");

                    if (player.condition == Condition.paralized)
                    {
                        player.condition = Condition.healthy;
                        statusCondition.PlayerIsHealthy();
                    }

                    break;

                case HealingItems.ataniaBerry:

                    audioManager.PlaySound("Eat");

                    if (player.condition == Condition.slept)
                    {
                        player.condition = Condition.healthy;
                        statusCondition.PlayerIsHealthy();
                    }

                    break;

                case HealingItems.melocBerry:

                    audioManager.PlaySound("Eat");

                    if (player.condition == Condition.poisoned || player.condition == Condition.badlyPoisoned)
                    {
                        player.condition = Condition.healthy;
                        statusCondition.PlayerIsHealthy();
                    }

                    break;

                case HealingItems.safreBerry:

                    audioManager.PlaySound("Eat");

                    if (player.condition == Condition.burnt)
                    {
                        player.condition = Condition.healthy;
                        statusCondition.PlayerIsHealthy();
                    }

                    break;
            }
        }
    }

    void IsAtCapHealth()
    {
        if (player.playerHealth >= player.playerMaxHealth)
            player.playerHealth = player.playerMaxHealth;
    }
}
