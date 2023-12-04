using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public DataController dataController;
    public ShopManager generateShop;
    public List<Stage> stageList = new List<Stage>();
    public List<GameObject> playerList = new List<GameObject>();
    public static Stage selectedStage;
    public static GameObject playerSelected;
    public static int currentFloor = 0;
    public static bool nextFloor;
    private int stairsValue = 0;
    private bool nextLevel = false;

    public GameObject tutorialPlayer;

    [Header("Game Values")]
    public static int PokeMoney;
    public static int EvoStones;
    public static int ShinyStones;
    public static int Gems;
    public static int Mushrooms = 0;
    public static int Stars = 0;
    public static int Pearls = 0;

    [Header("Pause Menu Action")]
    public DialogueManager dialogueManager;
    public GameObject inGameMenu;
    public GameObject pauseMenu;

    [Header("Game UI")]

    public GameObject gameUI;
    public GameObject gameOverMenu;
    public Animator dialogueAnimator;
    public Text pokeMoneyValue;
    public Text evoStoneValue;
    public Text shinyStoneValue;
    public Text floor;
    public Text gems;
    public Text mushrooms;
    public Text stars;
    public Text pearls;

    [Header("Items UI")]
    public List<ShopItem> shopItems = new List<ShopItem>();
    public GameObject items;

    [Header("Time UI")]

    public GameObject time;
    public Text minutesText;
    public Text secondsText;
    public int maxTime = 15;
    [HideInInspector]
    public static int minutes;
    private float seconds;
    public float spawnTime = 1f;
    private bool decrease = false;

    [Header("Transition animation")]
    public LevelLoader levelLoader;
    public Animator transition;
    public float resetAnimationTime = 2f;
    public Text stageFloor;

    public static string theme;

    [Header("Boss")]
    public List<Transform> bossSpawns = new List<Transform>();

    void Awake()
    {
        // we end the transition animation when we change the scene

        transition.SetBool("StartAnim", false);

        // before the game starts we activate the stage and the player selected

        ActivatePlayer();

        ActivateStage();

        // apply the upgrades values we bought

        dataController.LoadPlayerStats();

        // check if the levels are completed or not at the start of the game.

        LevelComplete();

        // also we activate again all GameObject of the list.

        ReactivateFloors();

        nextFloor = false;
    }

    private void Start()
    {
        pauseMenu.SetActive(false);

        gameOverMenu.SetActive(false);

        Time.timeScale = 1f;

        currentFloor = 0;

        floor.text = "0";

        seconds = 0;

        minutes = 0;

        // we pick the stage theme

        if(selectedStage != null)
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();

            switch(selectedStage.name)
            {
                case "Cerulean Cave": theme = "Cerulean Cave Theme"; audioManager.PlaySound(theme); break;

                case "Lush Jungle": theme = "Lush Jungle Theme"; audioManager.PlaySound(theme); break;

                case "Dark Crater": theme = "Dark Crater Theme"; audioManager.PlaySound(theme); break;

                default: theme = "Tutorial Theme"; audioManager.PlaySound(theme); break;
            }
        }

        // here we can trigger the gem pick up event

        Gem.OnGemCollect += IncreaseGemValue;

        SellItem.OnSellItemCollect += IncreaseSellItemValue;

    }

    private void Update()
    {
        // Start Menu Input

        // we are only be able to pause the game whenever we are NOT in conversation or at game over menu.

        if (!dialogueManager.inConversation && !gameOverMenu.activeSelf && Input.GetButtonDown("Pause"))
        {
            if (!pauseMenu.activeSelf)
                ShowPauseMenu();
            else
                ClosePauseMenu();
        }

        // we save every frame the amount of money in a static variable which
        // we are going to use to save the money at menu

        if (int.TryParse(pokeMoneyValue.text, out int pkMoney))
            PokeMoney = pkMoney;
        
        if(int.TryParse(evoStoneValue.text, out int evoStones))
            EvoStones = evoStones;

        if(int.TryParse(shinyStoneValue.text, out int shinyStones))
            ShinyStones = shinyStones;
        
        gems.text = Gems.ToString();
        
        mushrooms.text = Mushrooms.ToString();

        stars.text = Stars.ToString();
        
        pearls.text = Pearls.ToString();

        // here we control the timer

        if(selectedStage != null && selectedStage.name != "Tutorial")
        {
            if(minutes != maxTime) GameTime();
            else
            {
                if(GameObject.FindGameObjectWithTag("Boss") == null)
                    GameOver();
            }
        }
        else
            time.SetActive(false);

        if(nextLevel)
        {
            if(currentFloor != selectedStage.lastFloor)
                StartCoroutine(ActivateSpawners());
            else
                DisableSpawners();
        }

        if(decrease)
            DecreaseSpawnTime();
    }

    private void LevelComplete()
    {
        foreach(Stage stage in stageList)
        {
            int complete;

            switch (stage.name)
            {
                case "Cerulean Cave":

                    complete = PlayerPrefs.GetInt(DataController.cerulean);

                    if(complete != 0)
                        stage.completed = !stage.completed;

                break;

                case "Lush Jungle":

                    complete = PlayerPrefs.GetInt(DataController.jungle);

                    if(complete != 0)
                        stage.completed = !stage.completed;

                break;

                case "Dark Crater":

                    complete = PlayerPrefs.GetInt(DataController.crater);

                    if(complete != 0)
                        stage.completed = !stage.completed;

                break;
            }
        }
    }

    private void IncreaseGemValue(int gemValue)
    {
        Gems += gemValue;

        if(gems != null)
            gems.text = Gems.ToString();
    }

    private void IncreaseSellItemValue(ESellItem eSellItem)
    {
        switch(eSellItem)
        {
            case ESellItem.mushroom: Mushrooms++; mushrooms.text = Mushrooms.ToString(); break;
            case ESellItem.star: Stars++; stars.text = Stars.ToString(); break;
            case ESellItem.pearl: Pearls++; pearls.text = Pearls.ToString(); break;
        }
    }

    private void DecreaseSpawnTime()
    {
        EnemySpawn[] spawns = FindObjectsOfType<EnemySpawn>();

        if(spawns != null)
        {
            foreach(EnemySpawn spawn in spawns)
                spawn.spawnTime -= spawnTime;
        }

        decrease = false;
    }

    private IEnumerator ActivateSpawners()
    {
        yield return new WaitForSeconds(.75f);

        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

        foreach(GameObject spawner in spawners)
            spawner.GetComponent<EnemySpawn>().enabled = true;
    }

    public void DisableSpawners()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

        foreach(GameObject spawner in spawners)
            spawner.GetComponent<EnemySpawn>().enabled = false;
    }

    public void DestroyEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies != null)
        {
            foreach(GameObject enemy in enemies)
                Destroy(enemy);
        }

        EnemySpawn.spawnCounter = 0;
    }

    private void GameOver()
    {
        // disable the pokemon spawners

        DisableSpawners();

        // kill all pokemon in game

        FindObjectOfType<AudioManager>().PlaySound("DestroyEnemies");

        DestroyEnemies();

        //spawn the boss zone 

        FindObjectOfType<AudioManager>().PlaySound("Boss");

        Transform randomSpawn = bossSpawns[Random.Range(0, bossSpawns.Count)];

        Instantiate(selectedStage.boss, randomSpawn.position, Quaternion.identity);
    }

    private void GameTime()
    {
        
        seconds += Time.deltaTime;

        if(seconds >= 60)
        {
            minutes++;

            if(minutes % 2 == 0)
                decrease = true;

            if(minutes < 10)
                minutesText.text = "0" + minutes.ToString();
            else
                minutesText.text = minutes.ToString();

            seconds = 0;
        }

        if(seconds < 10)
            secondsText.text = "0" + Mathf.FloorToInt(seconds).ToString();
        else
            secondsText.text = Mathf.FloorToInt(seconds).ToString();

    }

    public void LoadNextLevel()
    {
        // we end the transition animation when we change the stage

        transition.SetBool("StartAnim", false);

        nextLevel = true;

        // reset the kecleon's shop value, then game controller can find another one in the next floor.

        generateShop.kecleonShop = null;

        // reset player position to the center
        // here we are checking if we are controlling the tutorial player or
        // our selected player from the main menu        

        if(playerSelected.activeSelf)
            playerSelected.transform.position = transform.position;
        else
            tutorialPlayer.transform.position = transform.position;

        // delete the floor before instantiate another one

        GameObject previousFloor = GameObject.FindGameObjectWithTag("Level");
        Destroy(previousFloor);

        // and show the others UI elements

        gameUI.SetActive(true);

        // we continue to the next floor

        currentFloor += stairsValue;

        // we update the floor text

        floor.text = currentFloor.ToString();

        // load a new floor

        switch(selectedStage.name)
        {
            case "Tutorial": Instantiate(selectedStage.floors[currentFloor], transform.position, Quaternion.identity); break;
            default: 

                // we check if we are in the last stage floor

                if(currentFloor == selectedStage.lastFloor)
                {
                    // load the last floor

                    Instantiate(selectedStage.stageEndFloor, transform.position, Quaternion.identity);
                }
                else
                {
                    // if not we select a random stage floor

                        GameObject randomFloor = selectedStage.floors[Random.Range(0, 
                            selectedStage.floors.Count)];

                        Instantiate(randomFloor, transform.position, Quaternion.identity);
                }
                    
            break;
        }

        nextFloor = false;
    }

    public void RepeatLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TriggerTransition()
    {
        // if we are not reach the last floor we continue goinf through the stage

        if(currentFloor < selectedStage.lastFloor && selectedStage.lastFloor > 0 || currentFloor > selectedStage.lastFloor && selectedStage.lastFloor < 0)
        {
            // next floor animation

            transition.SetBool("StartAnim", true);

            levelLoader.LoadNextLevel(currentFloor);

            // this triggers the load function at the transition animation
            // so if we are loading the scene and not changing the stage we are not loading a wrong stage

            nextFloor = true; 

            NextFloor floor = FindObjectOfType<NextFloor>();

            if (floor != null)
                stairsValue = floor.nextFloor;

            // change the transition UI

            stageFloor.text = "P. " + (currentFloor + stairsValue).ToString();

            // hide the game UI

            gameUI.SetActive(false);
        }
        else //if not we come back to the menu
        {
            // we want to load any other transition not the load level one
            levelLoader.LoadMenu();
        }
    }


    void ActivatePlayer()
    {

        // if our selection has been the tutorial
        if(MenuController.stageName == "Tutorial")
        {
            // we activate the respective player
            tutorialPlayer.SetActive(true);
            playerSelected = tutorialPlayer;
        }
        else // if we not
        {
            // we search our player in the list

            int player = 0;
            while(player < playerList.Count)
            {
                if (playerList[player].name == MenuController.playerName)
                {
                    playerList[player].SetActive(true); // and activates it
                    playerSelected = playerList[player];
                    player = playerList.Count;
                }

                player++;
            }
        }
    }

    void ActivateStage()
    {
        // we loop our stage array to find the seleted one from the menu
        int stage = 0;
        while(stage < stageList.Count)
        {
            if (stageList[stage].name == MenuController.stageName) // if the names are equals we found our stage
            {
                selectedStage = stageList[stage]; // so we select it

                // if the stage is the tutorial one
                if (stageList[stage].name == "Tutorial")
                {
                    // we going through in order

                    Instantiate(stageList[stage].floors[0], this.transform.position, Quaternion.identity);
                }
                else
                {
                    // if not we select a random stage floor

                    GameObject randomFloor = stageList[stage].floors[Random.Range(0, 
                        stageList[stage].floors.Count)];

                    Instantiate(randomFloor, this.transform.position, Quaternion.identity);
                }
                stage = stageList.Count;
            }

            stage++;
        }
    }

    void ReactivateFloors()
    {
        if (selectedStage != null)
        {
            for (int floor = 0; floor < selectedStage.floors.Count; floor++)
            {
                if (!selectedStage.floors[floor].activeSelf)
                    selectedStage.floors[floor].SetActive(true);
            }
        }
    }

    // we have to modify this methods when we add sound!

    void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        inGameMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        inGameMenu.SetActive(true);
        Time.timeScale = 1f;
    }

    public void ShowGameOverMenu()
    {
        inGameMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseGameOverMenu()
    {
        gameOverMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
