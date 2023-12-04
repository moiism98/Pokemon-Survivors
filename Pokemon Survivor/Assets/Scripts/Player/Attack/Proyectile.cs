using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject proyectileExplotion;
    public float projectileForce;
    public float damage;
    private PlayerAttack player;

    private void Start()
    {
        player = FindObjectOfType<PlayerAttack>();

        FindObjectOfType<AudioManager>().PlaySound("Swift");
    }

    private void FixedUpdate()
    {
        rb.AddForce(player.proyectileSpawnPoint.up * projectileForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":

                DestroyProyectile();

                EnemyStats enemy = collision.GetComponent<EnemyStats>();

                float totalDamage = (damage + PlayerStats.damagePerLevel) * player.damage;

                enemy.TakeDamage(totalDamage);
                    
            break;

            case "Wall":
                DestroyProyectile();
                break;

            case "Decoration":
                DestroyProyectile();
                break;

            case "NPC":
                DestroyProyectile();
                break;
        }
    }

    void DestroyProyectile()
    {
        if(proyectileExplotion != null)
            Instantiate(proyectileExplotion, rb.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void StopAttackAnimation()
    {
        player.gameObject.GetComponent<Animator>().SetBool("Attack", false);
    }
}
