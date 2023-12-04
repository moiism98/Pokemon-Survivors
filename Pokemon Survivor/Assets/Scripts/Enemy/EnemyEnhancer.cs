using System;
using UnityEngine;

public class EnemyEnhancer : MonoBehaviour
{
    public Animator animator;
    public EnemyMovement movement;
    public GameObject feets;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public float range;
    private Transform player;
    private bool playerFound = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {

        animator.SetFloat("Horizontal", player.position.x - transform.position.x);

        animator.SetFloat("Vertical", player.position.y - transform.position.y);

        FindPlayer();

        if(playerFound)
            ActivatesEnhancer();
    }

    private void FindPlayer()
    {
        // check if we found the player

        RaycastHit2D raycast = Physics2D.Raycast(transform.position, player.position - transform.position, range, playerLayer);

        if(raycast.collider && raycast.collider.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().PlaySound("Enhanced");
            
            playerFound = true;
        }
    }

    private void ActivatesEnhancer()
    {
        // attack anim

        animator.SetTrigger("Attack");

        // destroy his movement and feets

        Destroy(movement);

        Destroy(feets);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

        if(enemies != null)
        {
            foreach(Collider2D enemy in enemies)
            {
                // if it is we get his stats script

                EnemyStats stats = enemy.gameObject.GetComponent<EnemyStats>();

                if(stats != null && !stats.status)
                {
                    // pick a random enemy condition to apply on enemy

                    // we convert an enum into an SYSTEM array

                    Array eConditions = Enum.GetValues(typeof(EnemyCondition));

                    // create a SYSTEM random which is going to select a random array object
                    // and select it, casting the array to an enemy condition enum value

                    // cast the array to an enemy condition enum value, 
                    // in the GetValue method we are calling the Random SYSTEM method giving it the length of our array
                    // (he is going to pick a random value from the array, the length it's the max value)

                    EnemyCondition eCondition = (EnemyCondition) eConditions.GetValue(new System.Random().Next(eConditions.Length));

                    switch(eCondition) 
                    {
                        case EnemyCondition.damageUp: 

                            stats.condition = EnemyCondition.damageUp;

                            EnemyAttack enemyAttack = enemy.gameObject.GetComponent<EnemyAttack>();

                            if(enemyAttack != null)
                                enemyAttack.attackDamage *= 2;
                            
                        break;

                        case EnemyCondition.speedUp:
                        
                            stats.condition = EnemyCondition.speedUp; 

                            EnemyMovement enemyMovement = enemy.gameObject.GetComponent<EnemyMovement>();
                            
                            if(enemyMovement != null)
                                enemyMovement.movementSpeed *= 2;
                        
                        break;

                        case EnemyCondition.defenseUp: 

                            stats.condition = EnemyCondition.defenseUp; 
                            
                            stats.enemyMaxHealth *= 2;

                            stats.enemyHealth *= 2;

                        break;
                    }

                    stats.status = true;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
