using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum Explotion
{
    voltorb,
    electrode
}
public class Explosion : MonoBehaviour
{
    PlayerStats playerStats;
    EnemyStats enemyStats;

    Collider2D[] hitEntities;

    public float explosionRatio = 2f;

    public float voltorbDamage = .25f;

    public float electrodeDamage = .75f;

    public Explotion explotion = Explotion.voltorb;

    void Explode()
    {
        hitEntities = Physics2D.OverlapCircleAll(transform.position, explosionRatio);

        if(explotion == Explotion.voltorb)
            FindObjectOfType<AudioManager>().PlaySound("VoltorbTrap");
        else
            FindObjectOfType<AudioManager>().PlaySound("ElectrodeTrap");

        if (hitEntities != null)
        {
            foreach (Collider2D hitEntity in hitEntities)
            {
                switch (hitEntity.tag)
                {
                    case "Player":
                        Time.timeScale = 0f;
                        playerStats = hitEntity.GetComponent<PlayerStats>();
                        if (explotion == Explotion.voltorb)
                            playerStats.TakeDamage(playerStats.playerMaxHealth * (voltorbDamage / 2));
                        else
                            playerStats.TakeDamage(playerStats.playerMaxHealth * (electrodeDamage / 2));
                        break;
                    case "Enemy":
                        enemyStats = hitEntity.GetComponent<EnemyStats>();
                        if (explotion == Explotion.voltorb)
                            enemyStats.TakeDamage(enemyStats.enemyMaxHealth * (voltorbDamage / 2));
                        else
                            enemyStats.TakeDamage(enemyStats.enemyMaxHealth * (electrodeDamage / 2));
                        break;
                }
            }
        }
    }

    void StopDamageAnimation()
    {
        if (hitEntities.Length != 0)
        {
            foreach (Collider2D hitEntity in hitEntities)
            {
                if (hitEntity != null)
                {
                    if (hitEntity.CompareTag("Enemy") || hitEntity.CompareTag("Player"))
                        hitEntity.GetComponent<Animator>().SetFloat("Damage", 0);
                }
            }
        }
    }
    void DestroyExplsoion()
    {
        Destroy(gameObject);

        Time.timeScale = 1f;
    }
}
