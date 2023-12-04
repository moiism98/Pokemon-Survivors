using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyHorde : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public BoxCollider2D feetCollider;
    private  GameObject player;
    private Vector3 playerPosition;
    public float speed = 10f;
    public float damage = 10f;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerPosition = player.transform.position - transform.position;

        animator.SetFloat("Horizontal", playerPosition.x);

        animator.SetFloat("Vertical", playerPosition.y);

        animator.SetFloat("Speed", speed);

        rb.velocity = playerPosition * speed;

        FindObjectOfType<AudioManager>().PlaySound("Horde");
    }

    private void Update()
    {
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;

        if(!KecleonShop.shoping)
        {
            animator.SetFloat("Horizontal", playerPosition.x);

            animator.SetFloat("Vertical", playerPosition.y);

            animator.SetFloat("Speed", speed);

            rb.velocity = playerPosition * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Player":

                feetCollider.isTrigger = true;

                PlayerStats player = collision.GetComponent<PlayerStats>();

                player.TakeDamage(damage);

                StartCoroutine(StopPlayerHurtAnim());

            break;

            case "Wall": Destroy(gameObject); break;

            case "Lava": Destroy(gameObject); break;

            case "NPC": Destroy(gameObject); break;

            case "Enemy": feetCollider.isTrigger = true; break;

            default: Destroy(gameObject, 5f); break;
        }
    }

    private IEnumerator StopPlayerHurtAnim()
    {
        yield return new WaitForSeconds(.5f);

        player.GetComponent<Animator>().SetFloat("Damage", 0);
    }

}
