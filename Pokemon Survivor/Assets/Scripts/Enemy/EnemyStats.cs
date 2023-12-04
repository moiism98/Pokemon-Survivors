using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum EnemyCondition
{
    healthy,
    slowed,
    damageUp,
    speedUp,
    defenseUp
}

public enum DropType
{
    experience,
    evoStone,
    shinyStone,
    gem,
    item
}

[System.Serializable]
public class Drop
{
    public DropType type;
    [Range(0, 100)]
    public float probability;
    public GameObject item;
}
public class EnemyStats : MonoBehaviour
{
    PlayerStats player;
    PlayerAttack playerAttack;
    public Animator animator;
    public List<Drop> dropList;
    public Transform evoStoneSpawn;
    public Transform shinyStoneSpawn;

    // box collider must be placed before the non trigger box collider (feeets),
    // then when we dragg in the player in this slot will take the correct collider.
    public BoxCollider2D waterDetector; 

    public float enemyMaxHealth = 10f;
    public float enemyHealth;
    public bool status = false;

    public float strongAttackPoints;

    public FirstType firstType = FirstType.normal;
    public SecondType secondType = SecondType.none;
    public EnemyCondition condition = EnemyCondition.healthy;

    [Header("Damage UI")]
    public GameObject damageUI;
    public GameObject critDamageUI;

    private void Start()
    {
        enemyHealth = enemyMaxHealth;

        // Si el pokemon es de tipo agua o volador podr� de base caminar por el agua, activamos los colliders de los detectores nada m�s arrancar la partida.
        if (firstType.Equals(FirstType.water) || secondType.Equals(SecondType.water) && waterDetector != null)
            waterDetector.enabled = true;

        player = FindObjectOfType<PlayerStats>();
        playerAttack = FindObjectOfType<PlayerAttack>();
    }
    void Update()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {

        // here we apply the LUCKY PUNCH effect

        ShopItem luckyPunch = FindObjectOfType<GameController>().shopItems.Find(item => item.name.Equals("Puño Suerte"));

        float critDamage = damage * (luckyPunch.value / 100); // calculate the crit damage

        bool crit = false;

        if(luckyPunch.active) // if we purchased the lucky punch
        {
            int randomCrit = UnityEngine.Random.Range(1, 101); // calculate a random prob for the crit

            if(luckyPunch.value >= randomCrit)
            {
                damage += critDamage; // apply the damage crit to our damage

                crit = !crit;
            }
        }

        enemyHealth -= damage;

        animator.SetFloat("Damage", damage);

        if(crit)
        {

            Instantiate(critDamageUI, transform.position, Quaternion.identity);

            critDamageUI.GetComponentInChildren<TextMeshPro>().text = Mathf.CeilToInt(damage).ToString();
        }
        else
        {
            Instantiate(damageUI, transform.position, Quaternion.identity);

            damageUI.GetComponentInChildren<TextMeshPro>().text = Mathf.CeilToInt(damage).ToString();
        }




        if (enemyHealth <= 0)
        {
            if(dropList != null && dropList.Count > 0)
                Spawn();
            else
            {
                KecleonDrops kecleonDrops = FindObjectOfType<KecleonDrops>();

                if(kecleonDrops != null)
                    kecleonDrops.Spawn(kecleonDrops.dropList);
            }

            player.strongAttackPoints += strongAttackPoints;

            Destroy(gameObject);
        }
    }

    public void Spawn()
    {
        Transform ground = GameObject.FindGameObjectWithTag("Ground").transform;

        foreach(Drop drop in dropList)
        {
            switch(drop.type)
            {
                case DropType.experience:

                if(SpawnDrop(drop.probability))
                    Instantiate(drop.item, transform.position, Quaternion.identity, ground);

                break;

                case DropType.evoStone:

                if(SpawnDrop(drop.probability))
                    Instantiate(drop.item, evoStoneSpawn.position, Quaternion.identity, ground);

                break;

                case DropType.shinyStone:

                if(SpawnDrop(drop.probability))
                {
                    if(shinyStoneSpawn != null)
                        Instantiate(drop.item, shinyStoneSpawn.position, Quaternion.identity, ground);
                }

                break;
            }
        }
    }
    public bool SpawnDrop(float probability)
    {
        bool spawn = false;

        float randomProbability = MathF.Truncate(UnityEngine.Random.Range(0f, 101f) * 100) / 100;
        
        if(probability >= randomProbability)
            spawn = true;

        return spawn;
    }
}
