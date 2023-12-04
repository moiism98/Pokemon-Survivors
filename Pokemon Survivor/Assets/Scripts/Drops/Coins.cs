using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    public int value;
    private GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        GameObject coin = gameObject;

        if(coin.CompareTag("SuperiorPokeCoin"))
            value = Random.Range(100, 501);
        else
            value = Random.Range(1, 101);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerFeets"))
        {
            FindObjectOfType<AudioManager>().PlaySound("Money");

            Destroy(gameObject);

            // we take the item which boost coins value

            ShopItem item = gameController.shopItems.Find(item => item.itemEffect.Equals(ShopItemEffect.moreMoney));

            bool moneyBoost = item.active; // and take it's active value (if we got or not the item)

            float boostValue = item.value; // then it's value.

            UICoins coinText = FindObjectOfType<UICoins>();
            
            if(int.TryParse(coinText.coinValue.text, out int coin))
            {
                

                GameObject coinAmount = coinText.coinAmount; // Take the GameObject

                coinAmount.SetActive(true); // Enable it to show the animation.

                Animator coinAmountAnimator = coinAmount.GetComponent<Animator>(); // Restart the animation
                coinAmountAnimator.Rebind(); // Every time the player takes a coin the animation gets restarted.

                Text amount = coinAmount.GetComponentInChildren<Text>(); // Get all child components of the GameObject (Texts)
                
                if(!moneyBoost) // if we don't have the money booster we take the money with it's default value
                {
                    coinText.coinValue.text = (coin + value).ToString();

                    amount.text = "+ " + value;
                } 
                else    // if we do, we apply the boost value (it's a %) to the coin's value
                {

                    int boost = Mathf.RoundToInt(value * boostValue / 100);

                    coinText.coinValue.text = (coin + value + boost).ToString();

                    amount.text = "+ " + value + " + " + boost;
                }
                
            }
        }
    }
}
