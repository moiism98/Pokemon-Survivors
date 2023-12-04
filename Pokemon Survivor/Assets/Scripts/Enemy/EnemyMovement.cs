using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    private GameObject player;
    public float initialMoveSpeed = 3f;
    public float movementSpeed;

    Vector2 playerPosition;

    private void Start()
    {
        movementSpeed = initialMoveSpeed;
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    private void Update()
    {
        float decSpeedCap = initialMoveSpeed * 0.5f; // cap of decrease move speed of the enemy

        if (movementSpeed <= decSpeedCap && !KecleonShop.shoping) // f.e 1 <= 1/2f (0.5f)
            movementSpeed = decSpeedCap;


        if (player != null)
        {
            animator.SetFloat("Horizontal", playerPosition.x - this.transform.position.x);
            animator.SetFloat("Vertical", playerPosition.y - this.transform.position.y);
            animator.SetFloat("Speed", movementSpeed);
        }
    }

    void FixedUpdate()
    {
        if(player != null)
        {
            playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 moveTo = Vector2.MoveTowards(rb.position, playerPosition, movementSpeed * Time.fixedDeltaTime);
            rb.MovePosition(moveTo);
        }
    }
}
