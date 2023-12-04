using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    Animator animator;
    PlayerAttack playerAttack;
    Collider2D[] hitEnemies;
    float range = .5f;
    private void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        playerAttack = FindObjectOfType<PlayerAttack>();
    }

    void Attack()
    {
        hitEnemies = Physics2D.OverlapCircleAll(this.transform.position, range);
        if (hitEnemies != null && hitEnemies.Length > 0)
        {
            foreach (Collider2D entity in hitEnemies)
            {
                if (entity.CompareTag("Enemy"))
                {
                    EnemyStats stats = entity.GetComponent<EnemyStats>();
                    stats.TakeDamage(playerAttack.damage);
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
        Object.Destroy(this.gameObject);
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
