using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public void SetDescriptionText(Text description)
    {
        FindObjectOfType<ShopManager>().SetDescriptionText(description);
    }

    public void SetDescriptionIcon(Image icon)
    {
        FindObjectOfType<ShopManager>().SetDescriptionIcon(icon);
    }

    public void SetDescriptionPrice(Text price)
    {
        FindObjectOfType<ShopManager>().SetDescriptionPrice(price);
    }

    public void ShowItemDescription()
    {
        FindObjectOfType<ShopManager>().ShowDescription();
    }

    public void HideItemDescription()
    {
        FindObjectOfType<ShopManager>().HideDescription();
    }

    public void GetItem(Text itemName)
    {
        FindObjectOfType<ShopManager>().GetItem(itemName);
    }

    public void StealItem(Text itemName)
    {
        FindObjectOfType<ShopManager>().StealItem(itemName);
    }

    public void SellItem(Text itemName)
    {
        FindObjectOfType<ShopManager>().SellItem(itemName);
    }
}
