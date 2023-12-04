using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerColors : MonoBehaviour
{
    StrongBarColors barColors;
    public string pokeName;
    public List<Color> colors;
    private void Start()
    {
        SetBarColors();
    }

    public void SetBarColors()
    {
        if (barColors != null)
        {
            pokeName = barColors.pokeName;
            colors = barColors.pokeColors;
        }
    }
}
