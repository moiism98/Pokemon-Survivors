using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ESellItem
{
    mushroom,
    pearl,
    star
}
public class SellItem : MonoBehaviour, IItems
{
    public static event Action<ESellItem> OnSellItemCollect;
    public ESellItem sellItem = ESellItem.mushroom;
    public int sellValue;
    public void Collect()
    {
        Destroy(gameObject);

        OnSellItemCollect.Invoke(sellItem);

        FindObjectOfType<AudioManager>().PlaySound("Gem");
    }
}
