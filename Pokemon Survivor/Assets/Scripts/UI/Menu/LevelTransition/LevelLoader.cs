using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Animator levelTransition;

    public Text stageName;
    public Text stageFloor;

    private void Start()
    {
        // set the stage name and the current floor number to the text
        stageName.text = MenuController.stageUIName;
    }

    public void LoadGame()
    {
        // we load the game scene adding 1 to the menu scene index.
        // our menu scene index is 0 because it is the first one.

        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void LoadNextLevel(int nextFloor)
    {
        // we load the next level

        StartCoroutine(LoadLevel(nextFloor));
    }

    IEnumerator LoadScene(int scene)
    {
        // play the animation
        // we have to have this disabled by default and activate it here
        // because when we go back to menu ending a game the transition is being triggered

        levelTransition.gameObject.SetActive(true);

        levelTransition.SetBool("StartAnim", true);

        // wait an amount of seconds waiting until the transition is over

        yield return new WaitForSeconds(0.3f);

        // load the game scene

        SceneManager.LoadScene(scene);
    }

    IEnumerator LoadLevel(int nextFloor)
    {
        // here we simply plays the animation 

        levelTransition.SetBool("StartAnim", true);

        stageFloor.text = "P. " + nextFloor.ToString();

        // and again, wait an amount of time

        yield return new WaitForSeconds(0.3f);
    }
}
