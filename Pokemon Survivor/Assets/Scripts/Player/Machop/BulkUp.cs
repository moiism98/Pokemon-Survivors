using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkUp : MonoBehaviour
{
    public PlayerStats playerStats;

    void BulkUpPlayer()
    {
        playerStats.goodCondition = GoodCondition.damageup;
    }
}
