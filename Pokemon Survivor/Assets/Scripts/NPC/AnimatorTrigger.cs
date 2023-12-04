using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    // This script its done in case you have animations splitted from a GameObject 
    // and you want to activate at the same time all the animations.

    public List<GameObject> npcs = new List<GameObject>();
    #pragma warning disable CS8632
    public List<GameObject>? npcAnimations;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            foreach(GameObject npc in npcs)
            {
                npc.GetComponent<Animator>().SetBool("Summon", true);
            }
            foreach (GameObject animation in npcAnimations)
            {
                animation.SetActive(true);
                Animator animator = animation.GetComponent<Animator>();
                animator.Rebind();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (GameObject npc in npcs)
            {
                npc.GetComponent<Animator>().SetBool("Summon", false);
            }
            foreach(GameObject animation in npcAnimations)
                animation.SetActive(false);
        }
    }
}
