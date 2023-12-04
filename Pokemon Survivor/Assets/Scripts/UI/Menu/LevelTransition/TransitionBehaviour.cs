using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBehaviour : MonoBehaviour
{
    GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void LoadLevel()
    {
        if(GameController.nextFloor)
            gameController.LoadNextLevel();
    }

    public void ResetNextLevel()
    {
        GameController.nextFloor = false;
    }
}
