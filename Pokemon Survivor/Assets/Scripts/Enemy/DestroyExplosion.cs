using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Destroy()
    {
        Destroy(gameObject, .15f);

        if(player != null)
            player.GetComponent<Animator>().SetFloat("Damage", 0);
    }
}
