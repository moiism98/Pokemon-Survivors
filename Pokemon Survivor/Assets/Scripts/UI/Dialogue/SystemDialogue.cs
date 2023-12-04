using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemDialogue : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public Dialogue dialogue;
    DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Update()
    {
        // if the invisible wall is off you can press the continue dialogue button
        // to continue with it
        if(!dialogueManager.answer.activeSelf)
        {
            if (!boxCollider.enabled && Input.GetButtonDown("Dialogue"))
                dialogueManager.DisplayNextSentence(dialogue.sentences, dialogue.portrait);
        }

        // whenever the conversation is over and you has touched the invisible wall before
        // it is destroyed not bugging the other walls on the map
        if (!boxCollider.enabled && !dialogueManager.inConversation)
        {
            dialogueManager.portraitBox.SetActive(true);

            dialogueManager.nameText.enabled = true;

            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When player touches the invisible wall
        // the conversation starts
        if (collision.CompareTag("Player"))
        {
            boxCollider.enabled = false;

            dialogueManager.StartDialogue(dialogue);

            FindObjectOfType<AudioManager>().PlaySound("SystemDialog");
        }
    }
}
