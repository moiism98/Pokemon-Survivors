using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeVisual : MonoBehaviour
{
    public RangeVisual rangeVisual;
    public Summoner summoner;
    public EnemyEnhancer enhancer;

    private void Update()
    {
        if(summoner != null)
            rangeVisual.CalculateScale(summoner.range);
        else
        {
            if(enhancer != null)
                rangeVisual.CalculateScale(enhancer.range);
        }
    }
}
