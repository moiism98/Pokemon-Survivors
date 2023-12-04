using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public PlayerStats stats;
    void EndAnimation()
    {
        this.gameObject.SetActive(false);
        stats.isLevelUp = false;
    }
}
