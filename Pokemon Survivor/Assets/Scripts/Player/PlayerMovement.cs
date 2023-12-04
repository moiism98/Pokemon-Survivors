using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    public float initialMoveSpeed = 5f;
    public float movementSpeed;
    public float maxMoveSpeedCap = 4;
    public float minMoveSpeedCap = 1.5f;
    private Vector2 movement;
    private Vector2 moveVelocity;

    [Header("Drag Experience Variables")]
    private List<Rigidbody2D> expRb = new List<Rigidbody2D>();
    public LayerMask expLayer;
    public float magnetoRange = 5f;
    public float expSpeed = 20f;

    [Header("Enemy Detector")] // to fix the collider bug, player drags the enemies with him with his feets
    public float detectorRange = 1f;
    public LayerMask enemyLayer;

    private void Start()
    {
        movementSpeed = initialMoveSpeed;
    }

    private void Update()
    {
        // Movement Input

        // we only can move if we are not in any game menu!

        if(!pauseMenu.activeSelf && !gameOverMenu.activeSelf)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            moveVelocity = movementSpeed * movement;

            // if we are touching the water and we are moving, we play the water sound!

            if(GroundType.water && moveVelocity != Vector2.zero)
                FindObjectOfType<AudioManager>().PlaySoundUntilEnd("Water");

        }
        

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movementSpeed >= maxMoveSpeedCap) // Cap the speed at 4f
            movementSpeed = maxMoveSpeedCap;

        if(movementSpeed <= minMoveSpeedCap) // Cap the min move speed at 2f
            movementSpeed = minMoveSpeedCap;

        // method which is going to check if there's exp points around and drag them in to the player

        DragExperience();

        TriggerEnemyCollider();
    }

    private void FixedUpdate()
    {
        //Movement
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        if(expRb != null)
        {
            foreach(Rigidbody2D rb in expRb)
            {
                if(rb != null)
                {
                    Vector2 dragInto = Vector2.MoveTowards(rb.position, transform.position, expSpeed * Time.fixedDeltaTime);
                    
                    rb.MovePosition(dragInto);
                }
            }
        }
    }
    
    private void DragExperience()
    {
        Collider2D[] hitExperience = Physics2D.OverlapCircleAll(transform.position, magnetoRange, expLayer);

        if(hitExperience != null)
        {
            foreach(Collider2D exp in hitExperience)
                expRb.Add(exp.GetComponent<Rigidbody2D>());
        }
    }

    private void TriggerEnemyCollider()
    {
        Collider2D[] enemyCols = Physics2D.OverlapCircleAll(transform.position, detectorRange, enemyLayer);

        if(enemyCols != null && enemyCols.Length > 0)
        {
            foreach(Collider2D enemy in enemyCols)
            {
                if(enemy.gameObject.name != "Feets")
                    enemy.isTrigger = true;
            }
        }
        else
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            List<Collider2D> colliders = new List<Collider2D>();

            foreach(GameObject enemy in enemies)
                colliders.Add(enemy.GetComponent<Collider2D>());

            if(colliders != null)
            {
                foreach(Collider2D enemy in colliders)
                {
                    if(enemy.gameObject.name != "Feets" && CheckPokemon(enemy.gameObject.name.ToLower()))
                        enemy.isTrigger = false;
                }
            }
        }
    }

    private bool CheckPokemon(string name)
    {
        bool disable = true;

        switch(name)
        {
            case "gyarados": disable = false; break;
            case "shinygyarados": disable = false; break;
            case "absol": disable = false; break;
            case "shinyabsol": disable = false; break;
            case "golbat": disable = false; break;
            case "shinygolbat": disable = false; break;
            case "shiftry": disable = false; break;
            case "shinyshiftry": disable = false; break;
            case "scizor": disable = false; break;
            case "shinyscizor": disable = false; break;
            case "arcanine": disable = false; break;
            case "shinyarcanine": disable = false; break;
            case "combusken": disable = false; break;
            case "shinycombusken": disable = false; break;
            case "rapidash": disable = false; break;
            case "shinyrapidash": disable = false; break;
            case "mismagius": disable = false; break;
            case "shinymismagius": disable = false; break;
        }

        return disable;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, magnetoRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, detectorRange);
    }
    
}
