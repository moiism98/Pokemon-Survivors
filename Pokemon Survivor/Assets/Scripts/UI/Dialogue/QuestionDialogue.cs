using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionDialogue : MonoBehaviour
{
    DialogueManager dialogueManager;

    public GameObject npcPortrait;
    public Text npcName;
    
    public Animator dialogueBox;
    public static bool nextSentence;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();

        nextSentence = false;
    }

    public void CloseDialogueBox()
    {
        dialogueBox.SetBool("IsOpen", false);

        dialogueManager.inConversation = false;

        npcPortrait.SetActive(true);

        npcName.enabled = true;
    }
}
