using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator enemyAnimator;
    Animator playerAnimator;
    PlayerStats playerStats;

    public float initialAttackDamage;
    public float attackDamage;
    private bool playerDetected = false;

    void Start()
    {
        attackDamage = initialAttackDamage;
    }
    private void Update()
    {
        if (playerDetected)
            Attack();
        else
            StopAttack();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerDetected = true;
            playerAnimator = collision.GetComponent<Animator>();
            playerStats = collision.GetComponent<PlayerStats>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerDetected = false;
        }
    }

    void Attack()
    {
        if (playerStats != null && playerAnimator != null)
        {
            playerStats.TakeDamage(attackDamage * .0075f);
            enemyAnimator.SetBool("Attack", true);
        }
    }

    void StopAttack()
    {
        if (playerAnimator != null)
        {
            enemyAnimator.SetBool("Attack", false);
            playerAnimator.SetFloat("Damage", 0);
            playerStats = null;
        }
    }
}
