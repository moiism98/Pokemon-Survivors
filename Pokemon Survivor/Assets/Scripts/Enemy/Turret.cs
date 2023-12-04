using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public Animator animator;
    public LayerMask playerLayer;
    public float range = 3f;
    public float initialCooldown = .75f;
    private float cooldown;
    private bool playerFound = false;
    private Transform player;

    [HideInInspector]
    public Vector2 direction;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if(FindPlayer())
        {
            cooldown -= Time.deltaTime;

            if(cooldown <= 0)
                Shoot();
        }
    }

    private void FixedUpdate()
    {
        direction = new Vector2(player.position.x, player.position.y);
    }

    private bool FindPlayer()
    {
        // check if we found the player

        RaycastHit2D raycast = Physics2D.Raycast(transform.position, direction - (Vector2)transform.position, range, playerLayer);

        if(raycast.collider && raycast.collider.CompareTag("Player"))
            playerFound = true;

        return playerFound;
    }

    private void Shoot()
    {
        animator.SetTrigger("Attack");

        Instantiate(bullet, transform.position, Quaternion.identity);

        cooldown = initialCooldown;

        playerFound = false;

        switch(gameObject.name)
        {
            case "Beedrill": FindObjectOfType<AudioManager>().PlaySound("Stinger"); break;
            case "ShinyBeedrill": FindObjectOfType<AudioManager>().PlaySound("Stinger"); break;
            case "Flareon": FindObjectOfType<AudioManager>().PlaySound("Swift"); break;
            case "ShinyFlareon": FindObjectOfType<AudioManager>().PlaySound("Swift"); break;
            default: FindObjectOfType<AudioManager>().PlaySound("Turret"); break;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, direction);
    }
}
