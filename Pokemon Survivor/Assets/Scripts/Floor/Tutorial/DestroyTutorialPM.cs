using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTutorialPM : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && PlayerPrefs.GetInt("TutorialComplete") != 0)
        {
            Coins[] pokeMoney = FindObjectsOfType<Coins>();

            foreach(Coins coin in pokeMoney)
            {
                Object.Destroy(coin.gameObject);
            }

            Object.Destroy(this.gameObject);
        }
    }
}
