using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class AttackBehaviour : MonoBehaviour
{
    private Collider2D[] hitEnemies;
    private PlayerAttack playerAttack;

    #pragma warning disable CS8632
    public Transform? hitPoint;

    public float damage;
    public float range;

    private void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();
    }

    void DamageEnemies()
    {
        if(hitPoint != null)
            hitEnemies = Physics2D.OverlapCircleAll(hitPoint.position, range);
        else
            hitEnemies = Physics2D.OverlapCircleAll(playerAttack.mousePosition, range);
        
        if (hitEnemies != null)
        {
            foreach (Collider2D hitEnemy in hitEnemies)
            {
                if (hitEnemy.tag == "Enemy")
                {
                    EnemyStats enemy = hitEnemy.GetComponent<EnemyStats>();
                    enemy.TakeDamage(damage);
                }
            }
        }
    }

    void DestroyAttack()
    {
        Object.Destroy(this.gameObject);
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
