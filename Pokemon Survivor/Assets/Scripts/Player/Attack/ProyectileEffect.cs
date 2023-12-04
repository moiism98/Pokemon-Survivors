using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileEffect : MonoBehaviour
{
    GameObject[] enemies;

    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
    void DestroyEffect()
    {
        if (enemies != null)
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                    enemy.GetComponent<Animator>().SetFloat("Damage", 0f);
            }
        Object.Destroy(this.gameObject);
    }
}
