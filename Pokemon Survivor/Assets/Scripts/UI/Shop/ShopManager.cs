using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum ShopItemEffect
{
    moreMoney,
    antidote,
    crits,
    antiparalyze,
    health

}
[Serializable]
public class ShopItem
{
    public Sprite icon;
    public string name;
    public bool active;
    [Range(1, 100)]
    public float value;
    public ShopItemEffect itemEffect = ShopItemEffect.moreMoney;
}

[Serializable]
public class PassiveItem
{
    public Sprite icon;
    public string name;
    [TextArea(3, 5)]
    public string description;
    public int price;
    public bool purchased;
    [Range(1, 100)]
    public int stealProbability = 25;

}

[Serializable]
public class SellItm
{
    public Sprite icon;
    public string name;
    public ESellItem eSellItem;
    [Range(1, 1500)]
    public int sellValue;
}
public class ShopManager : MonoBehaviour
{
    public GameController gameController;
    public ShopDialogue shopDialogue;
    [HideInInspector]
    public KecleonShop kecleonShop;

    [Header("BUY SHOP UI")]
    public GameObject itemIconPrefab;
    public GameObject itemPrefab;
    public GameObject itemContainer;
    public GameObject descriptionBox;
    public Text descriptionText;
    public Text itemPrice;
    public Image descriptionIcon;

    [Header("SELL SHOP UI")]
    public GameObject sellItemPrefab;
    public GameObject sellContainer;

    [Header("Item variables")]
    public List<PassiveItem> passiveItems = new List<PassiveItem>();
    public List<SellItm> sellItems = new List<SellItm>();
    public int itemsCap = 3;
    private int itemCount = 0;
    private bool stealButton;
    public static bool stolen = false;
    private bool canPurchase = false;
    private PassiveItem purchasedItem;
    

    void Start()
    {
        GenerateShop();
    }

    void Update()
    {
        // check if we are stealing the item

        if(stolen)
        {
            // then check if the stealing conversation it's done
            // when the conversation end, the NPC now it's an enemy
            // remove NPC

            // intantiate the kecleon enemy, use a coroutine again (wait like 1 sec so u can move until kecleon is spawned)


            if(!FindObjectOfType<DialogueManager>().inConversation)
            {
                StartCoroutine(kecleonShop.SpawnEnemyShop());

                FindObjectOfType<AudioManager>().PlaySound("Boss");

                stolen = false;
            }
        }

        // getting the generated kecleon shop

        if(kecleonShop == null)
            kecleonShop = FindObjectOfType<KecleonShop>();

        // if you do not got all items in a run

        if(itemCount < itemsCap)
        {
            if(canPurchase) // can purchased them
                PurchaseItem();
        }
        else
        {
            if(itemCount == itemsCap)
            {
                // disable every button from every instantiate item

                GameObject[] items = GameObject.FindGameObjectsWithTag("ShopItem");

                foreach(GameObject item in items)
                {
                    Button button = item.GetComponentInChildren<Button>();

                    if(button != null)
                        button.gameObject.SetActive(false);
                }

                // adding 1 more to the count if not it's going to loop the same code every frame

                itemCount++;
            }
        }
    }

    public void GetItem(Text name)
    {
        purchasedItem = passiveItems.Find(item => item.name.Equals(name.text));

        if(purchasedItem != null && GameController.Gems >= purchasedItem.price)
        {
            int gems = GameController.Gems;

            // discount the gems (if we steal it, the item price it's going to be 0, we made it earlier)

            GameController.Gems -= purchasedItem.price;

            // equip the item

            // show purchase text (we can only buy an item once per run)

            purchasedItem.purchased = !purchasedItem.purchased;

            canPurchase = true;

            FindObjectOfType<AudioManager>().PlaySound("Sell Items");
        }
    }

    public void StealItem(Text item)
    {
        // we tell the game stole an item

        stolen = true;

        // take the item

        GetItem(item);

        // and wait a little amunt of secs to take it and then trigger the dialogues and the next actions

        StartCoroutine(StealingActions());
    }

    private IEnumerator StealingActions()
    {
        yield return new WaitForSeconds(.1f);

        // when we steal an item from kecleon's shop:

        // close the buy menu

        shopDialogue.HideBuyMenu();

        // close the current dialogue 

        shopDialogue.ExitFromShopping();

        // and start a new one with the steal dialogue.

        kecleonShop.StartSecondaryDialogue();
    }

    private void PurchaseItem()
    {
        GameObject[] shopItems = GameObject.FindGameObjectsWithTag("ShopItem");

        GameObject item = Array.Find(shopItems, item => item.GetComponentInChildren<Text>().text.Equals(purchasedItem.name));

        if(item != null)
        {
            Text [] texts = item.GetComponentsInChildren<Text>(includeInactive: true);

            Text purchased = Array.Find(texts, text => text.name.Equals("Purchased"));

            if(purchased != null)
                purchased.gameObject.SetActive(purchasedItem.purchased);

            Button[] buttons = item.GetComponentsInChildren<Button>(includeInactive: true);

            foreach(Button button in buttons)
                button.gameObject.SetActive(!purchasedItem.purchased);

            ShopItem shopItem = gameController.shopItems.Find(shopItem => shopItem.name.Equals(purchasedItem.name));

            shopItem.active = purchasedItem.purchased;

            itemIconPrefab.GetComponent<Image>().sprite = shopItem.icon;

            Instantiate(itemIconPrefab, gameController.items.transform.position, Quaternion.identity, gameController.items.transform);
        }

        itemCount++;

        canPurchase = false;
    }

    public void SellItem(Text itemName)
    {
        FindObjectOfType<AudioManager>().PlaySound("Sell Items");

        SellItm itemToSell = sellItems.Find(item => item.name.Equals(itemName.text));

        if(itemToSell != null)
        {
            switch(itemToSell.eSellItem)
            {
                case ESellItem.mushroom: 

                    GameController.Gems += int.Parse(gameController.mushrooms.text) * itemToSell.sellValue;

                    GameController.Mushrooms = 0;

                break;

                case ESellItem.star: 

                    GameController.Gems += int.Parse(gameController.stars.text) * itemToSell.sellValue;

                    GameController.Stars = 0;

                break;

                case ESellItem.pearl:

                    GameController.Gems += int.Parse(gameController.pearls.text) * itemToSell.sellValue;

                    GameController.Pearls = 0;

                break;

            }
        }
    }

    public void ShowSellButton()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("SellItem");

        if(items != null)
        {
            foreach(GameObject item in items)
            {
                Button sellButton = item.GetComponentInChildren<Button>(includeInactive: true);

                Text itemName = item.GetComponentInChildren<Text>();

                if(sellButton != null && itemName != null)
                {
                    switch (itemName.text)
                    {
                        case "Seta Pequeña": 
                        
                            

                            if(GameController.Mushrooms > 0)
                                sellButton.gameObject.SetActive(true);

                        break;

                        case "Tri Estrella": 
                        
                            if(GameController.Stars > 0)
                                sellButton.gameObject.SetActive(true);
                                
                        break;

                        case "Perla Grande": 
                        
                            if(GameController.Pearls > 0)
                                sellButton.gameObject.SetActive(true);
                                
                        break;
                    }
                }
            }
        }
    }
    private void GenerateShop()
    {
        foreach(PassiveItem passiveItem in passiveItems)
        {
            Image image = itemPrefab.GetComponentInChildren<Image>();

            image.sprite = passiveItem.icon;

            Button[] buttons = itemPrefab.GetComponentsInChildren<Button>(includeInactive: true);

            int randomProbability = UnityEngine.Random.Range(1, 101);

            // we have to reset the bool which indicates us if we have to show the steal button or not
            // because if we don't do it it's value it's going to be the same for the next item.

            stealButton = false; 

            foreach(Button button in buttons)
            {
                switch(button.name.ToLower())
                {
                    case "stealbutton":

                        if(passiveItem.stealProbability >= randomProbability)
                        {
                            stealButton = true;

                            passiveItem.price = 0;
                        }

                        button.gameObject.SetActive(stealButton);

                    break;

                    case "buybutton": button.gameObject.SetActive(!stealButton); break;
                }
            }

            Text[] texts = itemPrefab.GetComponentsInChildren<Text>(includeInactive: true);

            foreach(Text text in texts)
            {
                switch(text.name.ToLower())
                {
                    case "itemname": text.text = passiveItem.name; break;

                    case "itemdescription": text.text = passiveItem.description; break;

                    case "itemprice": text.text = passiveItem.price.ToString(); break;

                    case "purchased": text.gameObject.SetActive(passiveItem.purchased); break;
                }
            }

            Instantiate(itemPrefab, transform.position, Quaternion.identity, itemContainer.transform);
        }

        foreach(SellItm sellItem in sellItems)
        {
            Image image = sellItemPrefab.GetComponentInChildren<Image>();

            image.sprite = sellItem.icon;

            Text text = sellItemPrefab.GetComponentInChildren<Text>();

            text.text = sellItem.name;

            Instantiate(sellItemPrefab, transform.position, Quaternion.identity, sellContainer.transform);
        }
    }

    #region "Item Prefab Methods"

    // this methods are called in the Event Trigger of every Item Prefab
    public void SetDescriptionText(Text description) // first setting the description
    {
        descriptionText.text = description.text;
    }

    public void SetDescriptionIcon(Image icon) // then setting the icon
    {
        descriptionIcon.sprite = icon.sprite;
    }

    public void SetDescriptionPrice(Text price) // setting the price also (it would be 0 if the item only can be stolen)
    {
        itemPrice.text = price.text;
    }
    public void ShowDescription() // and finally showing the description box
    {
        descriptionBox.SetActive(true);
    }

    public void HideDescription()
    {
        descriptionBox.SetActive(false);
    }

    #endregion
}
