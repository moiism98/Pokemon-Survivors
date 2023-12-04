using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<Item> oneSpawn;
    public List<Item> spawnItem = new List<Item>();
    public float xCoord = 0;
    public float negXCoord = 0;
    public float yCoord = 0;
    public float negYCoord = 0;
    public int maxItems = 15;
    int itemCount = 0;
    Vector2 vectorZero = new Vector2(0f, 0f);
    void Awake()
    {
        SpawnItems();

        // we check if the stairs prefab has been assing because the last floor does not have stairs

        if(oneSpawn != null)
        {
            SpawnStairs();
        }
    }

    void SpawnItems()
    {
        int item = 0;
        while(item < maxItems)
        {
            // if we have looped the entire list of items, we reset the counter to start again in the first item

            if(itemCount == spawnItem.Count)
                itemCount = 0;
            
            Item itemToSpawn = spawnItem[itemCount]; // select the current item to spawn

            // and generate a random position

            Vector2 spawnPosition = new Vector2(Random.Range(xCoord, negXCoord), Random.Range(yCoord, negYCoord));
            
            // check if the item has to be spawned and if the random position we've generated its not the 0, 0 (our spawnpoint)

            if(itemToSpawn.probability >= Mathf.Round(Random.Range(0, 101)) && spawnPosition != vectorZero)
                Instantiate(itemToSpawn.item, spawnPosition, Quaternion.identity, gameObject.transform); // we add the gameObject transform to instantiate the prefab as a child of an object
            else // if the item it's not allowed to be spawned, we try again with another one
                item--;

            itemCount++;
            item++;
        }
    }

    void SpawnStairs()
    {
        foreach(Item spawn in oneSpawn)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(xCoord, negXCoord), Random.Range(yCoord, negYCoord));

            if(spawn.probability > Mathf.Round(Random.Range(0, 101)))
                Instantiate(spawn.item, spawnPosition, Quaternion.identity, gameObject.transform);
        }

    }
}
