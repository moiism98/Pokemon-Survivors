using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gumis
{
    speedGumi,
    healthGumi,
    damageGumi,
    waterGumi
}
public class Gumi : MonoBehaviour
{
    PlayerStats playerStats;
    PlayerAttack attack;
    PlayerMovement movement;

    public Gumis gumis = Gumis.speedGumi;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        attack = FindObjectOfType<PlayerAttack>();
        movement = FindObjectOfType<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeets"))
        {
            switch (gumis)
            {
                case Gumis.speedGumi:

                    movement.movementSpeed += Random.Range(.1f, .25f);

                break;

                case Gumis.healthGumi:

                    playerStats.playerMaxHealth += Random.Range(1, 5);

                break;

                case Gumis.damageGumi:

                    attack.damage += Random.Range(.1f, .25f);

                break;

                case Gumis.waterGumi: // Si coges el gumi de agua, se activan los colliders de los detectores de agua cercana, el detector detecta si hay agua cerca y te coloca como trigger el collider de los pies para poder pasar sobre ella (tiene que ponerlos como trigger y no desactivarlos para poder cambiar la sombra)
                    
                    playerStats.waterDetector.enabled = true;

                break;
            }

            FindObjectOfType<AudioManager>().PlaySound("Gumi");

            Destroy(gameObject);
        }
    }
}
