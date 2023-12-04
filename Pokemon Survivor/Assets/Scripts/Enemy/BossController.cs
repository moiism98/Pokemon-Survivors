using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public float bossSpeed = 10f;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(player != null)
        {
            animator.SetFloat("Horizontal", player.transform.position.x - gameObject.transform.position.x);
            animator.SetFloat("Vertical", player.transform.position.y - gameObject.transform.position.y);
            animator.SetFloat("Speed", bossSpeed);
        }
    }

    private void FixedUpdate()
    {   
        Vector2 bossDirection = player.transform.position;
        rb.MovePosition(Vector2.MoveTowards(rb.position, bossDirection, bossSpeed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            DefeatPlayer();
    }

    private void DefeatPlayer()
    {
        if(player != null)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();

            playerStats.TakeDamage(playerStats.playerMaxHealth);
        }
    }
}
