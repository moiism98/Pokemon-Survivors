using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsDialogue : MonoBehaviour
{
    public NextFloor nextFloor;
    GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerFeets"))
        {
            gameController.TriggerTransition();
            
            nextFloor.isTouching = true;

            // deactivate the enemy spawners and destroy enemies

            gameController.DestroyEnemies();

            gameController.DisableSpawners();
        }
    }
}
