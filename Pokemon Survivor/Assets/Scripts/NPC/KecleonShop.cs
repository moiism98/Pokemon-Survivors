using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class ShopKeeper
{
    public GameObject shopKeeper;
    public GameObject shopEnemy;

    [Range(0, 100)]
    public int probability;
}
public class KecleonShop : MonoBehaviour
{
    public List<ShopKeeper> shopKeepers = new List<ShopKeeper>();
    public static bool shoping = false;
    public GameObject enemyIcon;
    private ShopKeeper shopKeeper;
    private GameObject shopEnemy;
    private GameObject[] enemies;
    GameObject spawns;
    private void Start()
    {
        SpawnShopKeeper();
    }

    private void Update()
    {
        //if we stole an item, we disable the shop trigger collider

        if(ShopManager.stolen)
            Destroy(GetComponent<BoxCollider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerFeets"))
        {
            StopEnemies();

            shoping = true;

            spawns = GameObject.Find("Spawners");

            if(spawns != null)
                spawns.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerFeets"))
        {
            ResumeEnemies();

            shoping = false;

            EnemySpawn.loadEnemies = false;

            if(spawns != null)
                spawns.SetActive(true);
        }
    }

    public void StartSecondaryDialogue()
    {
        Dialogue stealDialogue = shopKeeper.shopKeeper.GetComponent<DialogueTrigger>().dialogues.Find(dialogue => dialogue.dialogueName.Equals("Stealing"));

        FindObjectOfType<DialogueManager>().StartDialogue(stealDialogue);
    }

    public IEnumerator SpawnEnemyShop()
    {   
        // wait a little amount of time

        yield return new WaitForSeconds(.25f);

        // destroy the NPC

        GameObject kecleonNPC = GameObject.FindGameObjectWithTag("NPC");

        if(kecleonNPC != null)
            Destroy(kecleonNPC);

        // spawn the enemy

        Instantiate(shopEnemy, transform.position, Quaternion.identity, transform);        
    }

    private void SpawnShopKeeper()
    {
        int sKcount = 0;

        while(sKcount < shopKeepers.Count)
        {
            int randomProbability = Random.Range(1, 101);

            shopKeeper = shopKeepers[sKcount];

            shopEnemy = shopKeepers[sKcount].shopEnemy;

            if(shopKeeper.probability >= randomProbability)
            {
                Instantiate(shopKeeper.shopKeeper, transform.position, Quaternion.identity, transform);

                sKcount = shopKeepers.Count;
            }

            sKcount++;

            if(sKcount == shopKeepers.Count)
                sKcount = 0;
        }
    }

    private void StopEnemies()
    {
        // get all enemies

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies != null)
        {
            foreach(GameObject enemy in enemies)
            {
                if(!enemy.name.ToLower().Contains("kecleon")) // if the enemy it's not a kecleon
                {
                    // enable an enemy mark, you will see where are the enemies (can not disable the components)

                    Instantiate(enemyIcon, enemy.transform.position, Quaternion.identity);

                    // and disable the enemies

                    enemy.SetActive(false);
                }

                
            }

        }
    }

    private void ResumeEnemies()
    {
        if(enemies != null)
        {
            foreach(GameObject enemy in enemies)
            {
                // get the icons and destroy them

                GameObject[] enemyIcons = GameObject.FindGameObjectsWithTag("EnemyIndicator");

                foreach(GameObject icon in enemyIcons)
                    Destroy(icon);

                // enable the enemy again

                enemy.SetActive(true);
            }
        }
    }

}
