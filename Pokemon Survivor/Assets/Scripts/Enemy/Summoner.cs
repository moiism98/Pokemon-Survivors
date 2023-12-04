using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Summoner : MonoBehaviour
{
    [Header("Pokemon list to summon!")]
    public List<Enemy> summonList = new List<Enemy>();

    [Header("Enemy objects")]
    public Animator animator;
    public EnemyMovement enemyMovement;
    public GameObject enemy;
    public GameObject visualRange;
    public GameObject feets;

    [Header("The target layer mask")]
    public LayerMask playerLayer;

    [Header("Script properties")]
    public float range = 3f;
    public float summonTime = 5f;
    private float initialCooldown = .1f;
    private float cooldown;
    private bool playerFound = false;
    private Transform player;
    private Vector2 direction;
    
    private void Start()
    {
        cooldown = initialCooldown;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // if the enemy find the player, he has to summon another pokemon
        
        if(!KecleonShop.shoping)
        {
            if(enemyMovement == null)
                animator.SetTrigger("Attack");
        
            direction = new Vector2(player.position.x, player.position.y);

            animator.SetFloat("Horizontal", player.position.x - transform.position.x);

            animator.SetFloat("Vertical", player.position.y - transform.position.y);

            if(FindPlayer())
            {
                cooldown -= Time.deltaTime;

                if(cooldown <= 0)
                {
                    Vector2[] spawnPositions = 
                    {
                        new Vector2(transform.position.x + .5f, transform.position.y),
                        new Vector2(transform.position.x - .5f, transform.position.y),
                        new Vector2(transform.position.x, transform.position.y - .5f),
                    };

                    Vector2 randomSpawnPoint = spawnPositions[Random.Range(0, spawnPositions.Length)];
                    
                    Enemy randomEnemy = summonList[Random.Range(0, summonList.Count)];

                    if(randomEnemy.spawnProbability >= Random.Range(0, 101))
                        Instantiate(randomEnemy.pokemon, randomSpawnPoint, Quaternion.identity, GameObject.FindGameObjectWithTag("Ground").transform);

                    cooldown = initialCooldown;
                }


                StartCoroutine(Summon());
            }
        }
    }

    private bool FindPlayer()
    {
        // check if we found the player

        //Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, range, playerLayer);
        
        RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, direction - (Vector2)transform.position, range, playerLayer);
        
        // if I found it we return a true value

        if(hitPlayer)
        {
            FindObjectOfType<AudioManager>().PlaySound("Summoner");

            playerFound = true;
        }

        return playerFound;
    }

    private IEnumerator Summon()
    {
        //enemy has to stop, we destroy the script and his feets 
        // so the spawned enemies don't collide with it and move the summoner one.

        Destroy(feets);

        Destroy(enemyMovement);

        // we trigger the summon animation

        animator.SetTrigger("Attack");

        // he summons some random pokemon from the list in random positions

        // we wait his time to summon

        yield return new WaitForSeconds(summonTime);

        // and after that the pokemon disapears

        Destroy(enemy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
