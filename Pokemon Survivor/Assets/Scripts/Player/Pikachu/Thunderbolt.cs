using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Thunderbolt : MonoBehaviour
{
    public RangeVisual rangeVisual;
    public Transform hitPoint;

    private Collider2D[] hitEnemies;
    private PlayerAttack playerAttack;
    private GameController gameController;

    public float damage;
    public float range;

    private void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();

        gameController = FindObjectOfType<GameController>();

        // for this range attack we've got the WIDE LENS, increase the attack range by 10%

        ShopItem wideLens = gameController.shopItems.Find(item => item.name.Equals("Lupa"));

        if(wideLens.active)
            range += range * (wideLens.value / 100);
    }

    void ScaleVisual()
    {
        rangeVisual.CalculateScale(range);
    }

    void DamageEnemies()
    {
        hitEnemies = Physics2D.OverlapCircleAll(hitPoint.position, range);

        FindObjectOfType<AudioManager>().PlaySound("Thunderbolt");

        if (hitEnemies != null)
        {
            foreach (Collider2D hitEnemy in hitEnemies)
            {
                if (hitEnemy.CompareTag("Enemy"))
                {
                    EnemyStats enemy = hitEnemy.GetComponent<EnemyStats>();

                    float totalDamage = (damage + PlayerStats.damagePerLevel) * playerAttack.damage;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        
        Gizmos.DrawWireSphere(hitPoint.position, range);
    }
}
