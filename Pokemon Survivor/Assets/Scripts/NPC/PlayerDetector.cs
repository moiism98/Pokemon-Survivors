using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Animator npcAnimator;
    public DialogueTrigger dialogueTrigger;

    private DialogueManager dialogueManager;
    private bool isInRange = false;
    private GameObject player;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }
    private void Update()
    {
        if(!dialogueManager.answer.activeSelf)
        {
            if (isInRange && Input.GetButtonDown("Dialogue") && !ShopDialogue.shoping) // Where "space" key is stored
            {
                npcAnimator.SetFloat("Horizontal", player.transform.position.x - transform.position.x);
                npcAnimator.SetFloat("Vertical", player.transform.position.y - transform.position.y);
                
                if (!dialogueManager.inConversation)
                    // Start dialogue
                    dialogueManager.StartDialogue(dialogueTrigger.dialogue);
                else
                    // Next dialogue
                    dialogueManager.DisplayNextSentence(dialogueTrigger.dialogue.sentences, dialogueTrigger.dialogue.portrait/*, emotions*/);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerFeets"))
        {
            isInRange = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
