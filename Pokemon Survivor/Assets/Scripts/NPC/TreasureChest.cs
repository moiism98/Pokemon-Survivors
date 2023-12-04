using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    BackToMenu backToMenu;
    DataController dataController;
    bool playerDetected = false;

    private void Start()
    {
        backToMenu = FindObjectOfType<BackToMenu>();
        dataController = FindObjectOfType<DataController>();

    }

    private void Update()
    {
        if(dataController != null && backToMenu != null)
        {
        
            if (playerDetected && Input.GetButtonDown("Dialogue"))
            {
                // we check if we completed the level already or not

                if(!GameController.selectedStage.completed)
                {
                    // if it's not, we set the level as completed

                    GameController.selectedStage.completed = true; // level comlete

                    // add 1 to our player prefs, one more level complete

                    int stagesComplete = PlayerPrefs.GetInt(DataController.completedStages);

                    PlayerPrefs.SetInt(DataController.completedStages, ++stagesComplete);

                    // and save the value on each stage to know wich one is or not complete

                    switch(GameController.selectedStage.name)
                    {
                        case "Cerulean Cave": PlayerPrefs.SetInt(DataController.cerulean, 1); break;
                        case "Lush Jungle": PlayerPrefs.SetInt(DataController.jungle, 1); break;
                        case "Dark Crater": PlayerPrefs.SetInt(DataController.crater, 1); break;
                        default: PlayerPrefs.SetInt(DataController.tutorial, 1); break;
                    }
                }

                dataController.SaveGame();

                backToMenu.LoadMainMenu();

                FindObjectOfType<AudioManager>().PlaySound("TreasureChest");
            }
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // player nearby
            playerDetected = true;

            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

            if(dialogueManager.inConversation)
                dialogueManager.inConversation = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // we player is not nearby 

            playerDetected = false;
        }
    }
}
