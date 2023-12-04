using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KecleonDrops : MonoBehaviour
{
    public EnemyStats stats;
    public Vector2 coord;
    public List<Drop> dropList;

    public void Spawn(List<Drop> drops)
    {
        for(int drop = 0; drop < drops.Count; drop++)
        {
            Transform ground = GameObject.FindGameObjectWithTag("Ground").transform;

            Vector2 randomSpawnPoint = new Vector2(transform.position.x - (Random.Range(coord.x, -coord.x) * drop), transform.position.y - (Random.Range(coord.y, -coord.y) * drop));

            if(stats.SpawnDrop(drops[drop].probability))
                Instantiate(drops[drop].item, randomSpawnPoint, Quaternion.identity, ground);
        }
    }
}
