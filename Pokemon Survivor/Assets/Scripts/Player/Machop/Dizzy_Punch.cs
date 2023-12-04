using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dizzy_Punch : MonoBehaviour
{
    Animator animator;
    PlayerAttack playerAttack;
    Collider2D[] hitEnemies;

    public float damage;
    public float range;

    private void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        playerAttack = FindObjectOfType<PlayerAttack>();
    }

    void Attack()
    {
        hitEnemies = Physics2D.OverlapCircleAll(transform.position, range);

        FindObjectOfType<AudioManager>().PlaySound("DizzyPunch");
        
        if (hitEnemies != null && hitEnemies.Length > 0)
        {
            foreach (Collider2D entity in hitEnemies)
            {
                if (entity.CompareTag("Enemy"))
                {
                    EnemyStats enemy = entity.GetComponent<EnemyStats>();
                    
                    float totalDamage = (damage + PlayerStats.damagePerLevel) * playerAttack.damage;
                    
                    enemy.TakeDamage(totalDamage);
                }
            }
        }
    }

    void StopAttackAnimation()
    {
        animator.SetBool("Attack", false);
    }

    void DestroyMeleeAttack()
    {
        Destroy(gameObject);
        
        if (hitEnemies.Length != 0)
        {
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy != null && enemy.CompareTag("Enemy"))
                    enemy.GetComponent<Animator>().SetFloat("Damage", 0f);
            }
        }
    }
}
