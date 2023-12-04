using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Psycho_Cut : MonoBehaviour
{
    public RangeVisual rangeVisual;
    private Collider2D[] hitEnemies;
    private PlayerAttack playerAttack;
    private GameController gameController;
    private ShopItem luckyPunch;

    public float damage;
    public float range;

    private void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();

        FindObjectOfType<AudioManager>().PlaySound("PsychoCut");

        gameController = FindObjectOfType<GameController>();

        luckyPunch = gameController.shopItems.Find(item => item.itemEffect.Equals(ShopItemEffect.crits));
    }

    void ScaleVisual()
    {
        rangeVisual.CalculateScale(range);
    }

    void DamageEnemies()
    {
        hitEnemies = Physics2D.OverlapCircleAll(playerAttack.mousePosition, range);

        if (hitEnemies != null)
        {
            foreach (Collider2D hitEnemy in hitEnemies)
            {
                if (hitEnemy.tag == "Enemy")
                {
                    EnemyStats enemy = hitEnemy.GetComponent<EnemyStats>();

                    float totalDamage = (damage + PlayerStats.damagePerLevel) * playerAttack.damage;

                    if(luckyPunch.active) // if we got the crit item
                    {
                        int probability = Random.Range(1, 101); // every time we hit an enemy calculate a random number

                        if(luckyPunch.value >= probability) // if we got a crit chance
                        {
                            totalDamage += totalDamage * (luckyPunch.value / 100); // we add a % damage to our total damage

                            enemy.TakeDamage(totalDamage);
                        }
                        else // if we do not, we take damage normally
                            enemy.TakeDamage(totalDamage);
                    }
                    else
                        enemy.TakeDamage(totalDamage);

                }
            }
        }
    }

    void DestroyAttack()
    {
        Destroy(gameObject);
        if (hitEnemies.Length != 0)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy != null && enemy.CompareTag("Enemy"))
                    enemy.GetComponent<Animator>().SetFloat("Damage", 0);
            }
        }
    }

    void StopAttackAnimation()
    {
        playerAttack.gameObject.GetComponent<Animator>().SetBool("Attack", false);
        playerAttack.gameObject.GetComponent<Animator>().SetBool("SecAttack", false);
    }
}
