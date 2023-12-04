using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider playerHealth;
    public Text currentHealth;
    public Text maxHealth;
    PlayerStats player;
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        if (player != null)
        {
            // SLIDER
            playerHealth.maxValue = player.playerMaxHealth;
            playerHealth.value = player.playerMaxHealth;

            // VIDA NUMÉRICA
            maxHealth.text = player.playerMaxHealth.ToString();
            currentHealth.text = player.playerMaxHealth.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            playerHealth.maxValue = player.playerMaxHealth;
            playerHealth.value = player.playerHealth;

            maxHealth.text = Mathf.Round(playerHealth.maxValue).ToString();
            currentHealth.text = Mathf.Round(playerHealth.value).ToString();
        }
    }
}
