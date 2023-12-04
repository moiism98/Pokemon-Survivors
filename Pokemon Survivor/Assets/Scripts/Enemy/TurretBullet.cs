using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject bulletExplosion;
    public Animator animator;
    public float bulletSpeed = 3f;
    public float bulletDamage = 30f;
    private GameObject player;
    private Vector3 direction;
    private void Start()
    {   
        player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            direction = new Vector3(player.transform.position.x, player.transform.position.y);

            rb.velocity = (direction - transform.position) * bulletSpeed;

            if(bulletExplosion == null)
            {
                float angle = Mathf.Atan2((direction - transform.position).y, (direction - transform.position).x) * Mathf.Rad2Deg - 90f;

                rb.rotation = angle;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch(collision.tag)
        {
            case "Player":

                PlayerStats player = collision.GetComponent<PlayerStats>();

                player.TakeDamage(bulletDamage);

                if(bulletExplosion != null)
                {
                    Destroy(gameObject);

                    Instantiate(bulletExplosion, transform.position, Quaternion.identity);
                }
                else
                    StartCoroutine(StopPlayerHurtAnim(player.gameObject.GetComponent<Animator>()));

            break;
            
            case "Wall":

                Destroy(gameObject);

                if(bulletExplosion != null)
                    Instantiate(bulletExplosion, transform.position, Quaternion.identity);

            break; 

            case "NPC":

                Destroy(gameObject);

                if(bulletExplosion != null)
                    Instantiate(bulletExplosion, transform.position, Quaternion.identity);
                
            break; 

            default: Destroy(gameObject, 5f); break;

        }
    }

    IEnumerator StopPlayerHurtAnim(Animator animator)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.2f);

        animator.SetFloat("Damage", 0);

        Destroy(gameObject);
    }
}
