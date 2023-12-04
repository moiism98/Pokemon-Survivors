using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject playerMenu;

    public void NextMenu()
    {
        this.gameObject.SetActive(false);
        playerMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
