using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProyectile : MonoBehaviour
{
    public float initialProjLifeTime = 10f;
    private float projectileLifeTime;

    private void Start()
    {
        projectileLifeTime = initialProjLifeTime;
    }

    void Update()
    {
        // If the projectile does not collide with something it will be destroy within X amount of time
        // in order to not accumulate a lot of entities.

        projectileLifeTime -= Time.deltaTime;
        if (projectileLifeTime <= 0)
        {
            Object.Destroy(this.gameObject);
            projectileLifeTime = initialProjLifeTime;
        }
    }
}
