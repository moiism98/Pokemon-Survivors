using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRangeVisual : MonoBehaviour
{
    // you have to put this script in every single melee player in order to change the scale
    // of the visual range.

    public RangeVisual rangeVisual;
    public PlayerAttack playerAttack;

    private void Update()
    {
        if(playerAttack != null)
            rangeVisual.CalculateScale(playerAttack.meleeRange);
    }

}
