using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Animator animator;
    public Text nameText;
    public Text dialogeText;
    public Image portrait;
    public GameObject answer;
    public GameObject shopMenu;
    public Queue<string> strSentences;
    public bool inConversation = false;
    public GameObject portraitBox;


    private GameObject player;
    private GameObject[] enemies;

    private void Start()
    {
        strSentences = new Queue<string>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void Update()
    {
        if (inConversation)
            FreezeEntities();
        else
            BackToNormal();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogue.portrait.Length == 0)
            portraitBox.SetActive(false);

        if (dialogue.name == "")
            nameText.enabled = false;

        inConversation = true;

        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name + ":";

        strSentences.Clear(); // Clear the sentences of a previous conversation.

        foreach (Sentence sentence in dialogue.sentences)
            strSentences.Enqueue(sentence.sentence);

        DisplayNextSentence(dialogue.sentences, dialogue.portrait);
    }
    public void DisplayNextSentence(Sentence[] sentences, Sprite[] portraits)
    {
        if (strSentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string strSentence = strSentences.Dequeue();

        // we find the current sentence with the strng one

        Sentence sentence = Array.Find(sentences, sentence => sentence.sentence.Equals(strSentence));

        if(sentence != null) // if we found it
        {
            // show the sentece sprite

            Sprite npcSprite = PickPortait(portraits, sentence.emotion);

            if(npcSprite != null)
                portrait.sprite = npcSprite;

            // if it's a question, we show the question dialogue

            answer.SetActive(sentence.isQuestion);

            // if we are going to shopping, show the shop dialogue

            shopMenu.SetActive(sentence.shoping);
        }

        // IEnumerators are invoke by coroutines, first we have to stop all coroutines
        // (similiar we did clearing all sentencens before add a new one)

        StopAllCoroutines();

        StartCoroutine(TypeSentence(strSentence));
    }

    // Method that write the sentences letter by letter waiting to write the letter waiting a
    // determinated amount of time
    IEnumerator TypeSentence(string sentence)
    {
        dialogeText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogeText.text += letter;
            yield return new WaitForSecondsRealtime(0.01f); // You can wait a single frame with null
        }
    }

    public void EndDialogue()
    {
        inConversation = false;
        animator.SetBool("IsOpen", false);
    }

    private Sprite PickPortait(Sprite[] portraits, Emotion emotion)
    {
        Sprite portrait = null;
        
        switch(emotion)
        {
            case Emotion.angry: portrait = Array.Find(portraits, portrait => portrait.name.ToLower() == "angry"); break;
            case Emotion.emotional: portrait = Array.Find(portraits, portrait => portrait.name.ToLower() == "emotional"); break;
            case Emotion.happy: portrait = Array.Find(portraits, portrait => portrait.name.ToLower() == "happy"); break;
            case Emotion.sad: portrait = Array.Find(portraits, portrait => portrait.name.ToLower() == "sad"); break;
            case Emotion.veryHappy: portrait = Array.Find(portraits, portrait => portrait.name.ToLower() == "veryhappy"); break;
            default: portrait = Array.Find(portraits, portrait => portrait.name.ToLower() == "neutral"); break;
        }

        return portrait;
    }

    private void FreezeEntities()
    {
        if (player)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Animator>().enabled = false;
            player.GetComponent<PlayerAttack>().enabled = false;
        }
        if (enemies.Length != 0)
        {
            foreach (GameObject enemy in enemies)
            {
                if(enemy != null)
                {
                    EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

                    if(movement != null)
                        movement.enabled = false;

                    Animator animator = enemy.GetComponent<Animator>();

                    if(animator != null)
                        animator.enabled = false;
                        
                    EnemyAttack enemyAttack = enemy.GetComponent<EnemyAttack>();

                    if(enemyAttack != null)
                        enemyAttack.enabled = false;
                }
            }
        }
    }

    private void BackToNormal()
    {
        if (player)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<Animator>().enabled = true;
            player.GetComponent<PlayerAttack>().enabled = true;
        }
        if (enemies.Length != 0)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    EnemyMovement enemyMovement =  enemy.GetComponent<EnemyMovement>();

                    if(enemyMovement != null)
                        enemyMovement.enabled = true;

                    enemy.GetComponent<Animator>().enabled = true;

                    EnemyAttack enemyAttack = enemy.GetComponent<EnemyAttack>();
                    
                    if(enemyAttack != null)
                        enemyAttack.enabled = true;
                }
            }
        }
    }
}
