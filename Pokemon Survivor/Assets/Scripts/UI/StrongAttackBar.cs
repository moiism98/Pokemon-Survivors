using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StrongAttackBar : MonoBehaviour
{
    public Slider attackBar;
    public Gradient gradient;
    public Image fill;

    PlayerStats player;
    PlayerColors playerColors;

    void Start()
    {
        var colors = new GradientColorKey[2];
        var alphas = new GradientAlphaKey[2];

        alphas[0] = new GradientAlphaKey(1.0f, 0f);
        alphas[1] = new GradientAlphaKey(1.0f, 1.0f);

        player = FindObjectOfType<PlayerStats>();
        playerColors = FindObjectOfType<PlayerColors>();

        if (player != null)
        {
            string gameObjectName = player.gameObject.name.ToLower();
            string pokemonName = playerColors.pokeName.ToLower();

            if (gameObjectName == pokemonName)
            {
                colors[0] = new GradientColorKey(playerColors.colors[0], 0f);
                colors[1] = new GradientColorKey(playerColors.colors[1], 1.0f);
                gradient.SetKeys(colors, alphas);
            }

            attackBar.maxValue = player.strongAttackMaxPoints;
            attackBar.value = player.strongAttackPoints;
            fill.color = gradient.Evaluate(0f);
        }
    }

    void Update()
    {
        if (player != null)
        {
            attackBar.value = player.strongAttackPoints;
            fill.color = gradient.Evaluate(attackBar.normalizedValue);
        }
    }
}
