using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GroundType : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Sprite shadow;

    public static bool water = false;

    private void Start()
    {
        spriteRenderer.sprite = shadow;
        animator.enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            animator.enabled = true;

            water = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            spriteRenderer.sprite = shadow;

            animator.enabled = false;

            water = false;
        }
    }
}
