using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameObject buyMenu;
    //public GameObject sellMenu;
    public static bool shoping = false;
    private void Update()
    {
        shoping = KecleonShop.shoping;
    }

    public void ShowBuyMenu()
    {
        buyMenu.SetActive(true);
    }

    public void HideBuyMenu()
    {
        buyMenu.SetActive(false);
    }

    /*public void ShowSellMenu()
    {
        sellMenu.SetActive(true);
    }*/

    public void ShowShopMenu()
    {
        gameObject.SetActive(true);
    }

    public void ExitFromShopping()
    {
        shoping = false;

        dialogueManager.EndDialogue();
    }
}
