using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spawner
{
    public string stageName;
    public List<Enemy> enemies = new List<Enemy>();
}
public class EnemySpawn : MonoBehaviour
{
    public List<Spawner> spawners = new List<Spawner>();
    public string stageName;
    public float initialSpawnTime = 15f;
    [HideInInspector]
    public float spawnTime;
    public LayerMask wallLayer;
    public int maxEnemySpawned = 35;
    public static bool loadEnemies = false;
    private List<Enemy> enemies = new List<Enemy>();
    private bool spawning = false;
    private int enemy = 0;
    private float checkRange = 0.3f;
    private bool isTouchingWall = false;
    public static int spawnCounter = 0;
    private int count;
    private bool shinyCharmApply = false;
    

    void Start()
    {
        spawnTime = initialSpawnTime;

        // pick the enemy list from the array

        SelectEnemies();
    }

    void Update()
    {
        if(loadEnemies)
            SelectEnemies();

       // we only want to spawn enemies if the spawner its not colliding with walls

        if(!WallCheck())
        {   
            // if we got the list of enemies

            if(enemies != null)
            {
                // spawns the enemies

                if(!spawning && spawnCounter < maxEnemySpawned)
                    StartCoroutine(SpawnEnemies());
            }
        }
        else // if we are touching a wall we reset the spawn time if not pokemon spawned are going to get stuck in the spawning on it when the spawn stop touching the wall.
            spawnTime = 3.5f;

        EnemyCount();

        // the SHINY CHARM increase the shiny spawn probabilty

        ShopItem shinyCharm = FindObjectOfType<GameController>().shopItems.Find(item => item.name.Equals("Amuleto Iris"));

        if(shinyCharm.active && !shinyCharmApply)
        {
            foreach(Enemy enemy in enemies)
            {
                if(enemy.pokemon.name.Contains("Shiny")) // for every shiny pokemon we increase his spawn prob 5%
                    enemy.spawnProbability += shinyCharm.value;
            }

            shinyCharmApply = true;
        }
    }

    private void EnemyCount()
    {
        GameObject[] enemiesFound = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemiesFound != null)
            spawnCounter = enemiesFound.Length;
    }

    private void SelectEnemies()
    {
        int spawner = 0;

        while(spawner < spawners.Count)
        {
            if(GameController.selectedStage != null && spawners[spawner].stageName.ToLower().Equals(GameController.selectedStage.name.ToLower()))
            {
                enemies = spawners[spawner].enemies;

                spawner = spawners.Count;
            }

            spawner++;
        }

        loadEnemies = true;
    }

    IEnumerator SpawnEnemies()
    {
        spawning = true;

        count = 0;

        while(count < enemies.Count)
        {
            if(enemy == enemies.Count)
                enemy = 0;

            float randomProbability = MathF.Truncate(UnityEngine.Random.Range(1f, 101f) * 100) / 100;

            SpawnPokemon(randomProbability);

            enemy++;
        }

        yield return new WaitForSeconds(spawnTime);

        spawning = false;
    }

    private void SpawnPokemon(float randomProbability)
    {
        if(enemies[enemy].spawnProbability >= randomProbability)
        {
            Instantiate(enemies[enemy].pokemon, transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("Ground").transform);

            count = enemies.Count;
        }
    }

    private bool WallCheck()
    {
        isTouchingWall = Physics2D.OverlapCircle(transform.position, checkRange, wallLayer);

        return isTouchingWall;
    }
}
