using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagicZoneType
{
    damage,
    heal
}

public class MagicZone : MonoBehaviour
{
    public MagicZoneType magicZone = MagicZoneType.heal;
    public float amount;

    private PlayerStats playerStats;

    private void Update()
    {
        if(playerStats != null)
        {
            switch(magicZone)
            {
                case MagicZoneType.damage:

                    // if player reach the 25% of his max health, we stop the coroutine
                    // in other words: we stop of making damage even if we are on the zone.

                    if (playerStats.playerHealth <= (playerStats.playerMaxHealth * .25f))
                        StopCoroutine("StartMagic");
                break;

                case MagicZoneType.heal:

                    // here we don't stop the coroutine, we simple control the current health is not
                    // over the max health so we cap it at max health

                    if(playerStats.playerHealth >= playerStats.playerMaxHealth)
                        playerStats.playerHealth = playerStats.playerMaxHealth;
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeets"))
        {
            // if we touch the zone we start the coroutine.

            StartCoroutine("StartMagic");

            FindObjectOfType<AudioManager>().PlaySound("MagicZone");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // if we get out of it we stop the coroutine.
            StopCoroutine("StartMagic");
        }
    }

    IEnumerator StartMagic()
    {
        playerStats = FindObjectOfType<PlayerStats>();

        switch (magicZone)
        {
            case MagicZoneType.damage:

                bool stopDamage = false;
                while(!stopDamage) // The loop is never stopping with the variable, but we need the loop in order to take damage every second, same with healing below!
                {
                    playerStats.playerHealth -= amount;

                    yield return new WaitForSeconds(Time.deltaTime);

                }

            break;

            case MagicZoneType.heal:

                bool stopHeal = false;
                while(!stopHeal)
                {
                    playerStats.playerHealth += amount;

                    yield return new WaitForSeconds(Time.deltaTime);

                }

            break;
        }
    }
}
