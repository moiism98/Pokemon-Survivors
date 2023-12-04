using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public float waitSeconds = 5f;
    public Animator transition;

    [Header("Menus Canvas")]
    public GameObject transitionCanvas;
    public GameObject inGameMenu;
    public GameObject gameOverMenu;

    public List<GameObject> animations = new List<GameObject>();

    void Start()
    {
        ActivateAnimation();
    }

    void ActivateAnimation()
    {
        transitionCanvas.SetActive(false);
        GameObject randomAnimation = animations[Random.Range(0, animations.Count)];

        foreach (GameObject anim in animations)
        {
            if(anim == randomAnimation)
                anim.SetActive(true);
            else 
                anim.SetActive(false);
        }
    }

    public void LoadMainMenu()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
            player.SetActive(false);
        
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex - 1));
    }

    IEnumerator LoadScene(int scene)
    {
        // we set time to normal again (in case we tried to trigger this transition from the game menu)

        Time.timeScale = 1f;

        // hide inGame UI
        
        inGameMenu.SetActive(false);

        gameOverMenu.SetActive(false);

        // show the UI

        transitionCanvas.SetActive(true);

        // activate the animator

        transition.SetTrigger("BackMenu");

        // wait seconds until transition is done

        yield return new WaitForSeconds(waitSeconds);

        // load next scene

        SceneManager.LoadScene(scene);
    }
}
