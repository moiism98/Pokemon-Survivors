using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public List<Item> items = new List<Item>();
    public List<Item> oneSpawn;
    public List<Transform> kecleonShopSpawnPoints;
    public int maxItems = 5;
    private List<Vector3> spawnTiles = new List<Vector3>();
    private List<Vector3> savedTilesPositions = new List<Vector3>();
    private int itemCount = 0;

    private void Start()
    {
        // we get our ground tiles

        GetAllTiles();
        
        // spawn our basics first (stairs, some kind of good traps etc)

        if(oneSpawn != null)
        {
            SpawnBasics();
        }

        // and then we spawn the items

        if(spawnTiles.Count > 0)
            SpawnObjects();

    }

    private void GetAllTiles()
    {
        // get the bounds of our tile

        BoundsInt tilemapBounds = tilemap.cellBounds;

        // take all tiles from the tilemap

        TileBase[] allTiles = tilemap.GetTilesBlock(tilemapBounds);

        // take the start position (0, 0 ,0)

        Vector3 startTile = tilemap.CellToWorld(new Vector3Int(tilemapBounds.xMin, tilemapBounds.yMin, 0));

        // and take all tiles positions and save them in our array of spawnPoints
        
        for(int xAxis = 0; xAxis < tilemapBounds.size.x; xAxis++)
        {
            for(int yAxis = 0; yAxis < tilemapBounds.size.y; yAxis++)
            {
                TileBase tile = allTiles[xAxis + yAxis * tilemapBounds.size.x];

                if(tile != null && tile.name.Contains("ground"))
                {
                    Vector3 spawnPosition = startTile + new Vector3(xAxis, yAxis, 0);
                    spawnTiles.Add(spawnPosition);
                }

            }
        }
    }

    private void SpawnObjects()
    {   
        int item = 0;
        while(item < maxItems)
        {

            // we want start looping again our items list if we reach the last item

            if(itemCount == items.Count)
                itemCount = 0;

            // take a random position from our spawn positions array

            Vector3 randomTile = spawnTiles[UnityEngine.Random.Range(0, spawnTiles.Count)];

            // and we build sides positions (we will use them later, they are going to help us to not spawn items nearby)

            Vector3 upPosition = randomTile + Vector3.up;
            Vector3 downPosition = randomTile + Vector3.down;
            Vector3 leftPosition = randomTile + Vector3.left;
            Vector3 rightPosition = randomTile + Vector3.right;

            // we check if in the random tile it's empty, if it is, we spawn the item

            if(!ObjectInTile(upPosition, randomTile) && !ObjectInTile(downPosition, randomTile) && !ObjectInTile(leftPosition, randomTile) && !ObjectInTile(rightPosition, randomTile))
                Spawn(randomTile);
            else // if not we we go one posisition back in our count because we want to be sure we spawn as many items as our limit indicates
                item--;

            itemCount++;
            item++;
        }
    }

    private void Spawn(Vector3 tilePosition)
    {
        int item = 0;

        while(item < items.Count)
        {
            float randomProbability = MathF.Truncate(UnityEngine.Random.Range(1f, 101f) * 10) / 10;

            if(items[itemCount].probability >= randomProbability && tilePosition != Vector3.zero)
            {
                Instantiate(items[itemCount].item, tilePosition, Quaternion.identity, gameObject.transform);

                savedTilesPositions.Add(tilePosition); // after spawn an item, we save the position where it's spawned

                item = items.Count;
            }

            item++;
        }
    }

    private void SpawnBasics()
    {
        foreach(Item spawn in oneSpawn)
        {
            Vector3 randomTile = spawnTiles[UnityEngine.Random.Range(0, spawnTiles.Count)];

            float randomProbability = MathF.Truncate(UnityEngine.Random.Range(1f, 101f) * 10) / 10;

            if(spawn.probability >= randomProbability)
            {
                if(!spawn.item.name.Contains("Shop"))
                {
                    Instantiate(spawn.item, randomTile, Quaternion.identity, gameObject.transform);

                    savedTilesPositions.Add(randomTile);
                }
                else
                {
                    if(kecleonShopSpawnPoints.Count > 0)
                    {
                        Transform spawnPoint = kecleonShopSpawnPoints[UnityEngine.Random.Range(0, kecleonShopSpawnPoints.Count)];

                        Instantiate(spawn.item, spawnPoint.position, Quaternion.identity, gameObject.transform);

                        savedTilesPositions.Add(spawnPoint.position);
                    }
                }
            }
        }
    }

    private bool ObjectInTile(Vector3 nearPosition, Vector3 spawnPosition)
    {
        bool spawned = false;

        int position = 0;

        // we loop our saved positions, in other words where we are instatiate items

        while (position < savedTilesPositions.Count)
        {

            // if the position selected randomly and the nearby positions are in our saved positions list we return
            // true because that means theres an item spawned and we have to tell the code to pick other spawn point

            if(savedTilesPositions[position].Equals(nearPosition) || savedTilesPositions[position].Equals(spawnPosition))
            {
                spawned = true;
                position = savedTilesPositions.Count;
            }

            position++;
        }

        return spawned;
    }
}
