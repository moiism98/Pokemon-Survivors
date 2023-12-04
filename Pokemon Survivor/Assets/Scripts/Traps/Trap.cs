using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Traps
{
    grimerTrap,
    poisonArrow,
    slowEffect,
    voltorbTrap,
    electrodeTrap,
    cleanStatus
}
public class Trap : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Traps traps = Traps.grimerTrap;
    public GameObject trapEffect;
    private GameController gameController;

    PlayerStats player;
    StatusCondition playerCondition;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCondition = FindObjectOfType<StatusCondition>();
        gameController = FindObjectOfType<GameController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerFeets"))
        {
            spriteRenderer.enabled = true; // Show the trap
            playerCondition.gameObject.GetComponent<SpriteRenderer>().enabled = true; // Show the status condition object

            List<RuntimeAnimatorController> conditionControllers = playerCondition.controllers; // Take the list of animators
            Animator statusAnimator = playerCondition.animator; // Take de status condition object animator
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

            FindObjectOfType<AudioManager>().PlaySound("Trap");

            // we take the active value (if the item has been purchased is going to be active)
            // from the item shop item list could be true or false

            bool poisonInmune = gameController.shopItems.Find(item => item.itemEffect.Equals(ShopItemEffect.antidote)).active;
            bool slowInmune = gameController.shopItems.Find(item => item.itemEffect.Equals(ShopItemEffect.antiparalyze)).active;


            switch (traps)
            {
                case Traps.grimerTrap:

                    if(!poisonInmune) // if we got the antidote item, we are poisonInmune to posion, so this code it's not goint to be triggered
                    {
                        player.condition = Condition.poisoned; // To know the player is poisoned

                        Instantiate(trapEffect, transform.position, Quaternion.identity);

                        statusAnimator.runtimeAnimatorController = conditionControllers[0];
                    }

                break;

                case Traps.poisonArrow:

                    if(!poisonInmune)
                    {
                        player.condition = Condition.badlyPoisoned;

                        Instantiate(trapEffect, transform.position, Quaternion.identity);

                        statusAnimator.runtimeAnimatorController = conditionControllers[1];

                        FindObjectOfType<AudioManager>().PlaySound("PoisonTrap");
                    }

                break;

                case Traps.slowEffect:

                    if(!slowInmune)
                    {
                        if (player.condition != Condition.slowed)
                        {
                            player.condition = Condition.slowed;
                            statusAnimator.runtimeAnimatorController = conditionControllers[2];
                        }

                        Instantiate(trapEffect, this.transform.position, Quaternion.identity);
                        
                        playerMovement.movementSpeed -= playerMovement.initialMoveSpeed * .20f;
                    }


                break;

                case Traps.voltorbTrap:

                    Instantiate(trapEffect, this.transform.position, Quaternion.identity);

                break;

                case Traps.electrodeTrap:

                    Instantiate(trapEffect, this.transform.position, Quaternion.identity);

                break;

                case Traps.cleanStatus:
                        
                    switch(player.condition)
                    {
                        case Condition.slowed:

                            player.condition = Condition.healthy;

                            playerMovement.movementSpeed = playerMovement.initialMoveSpeed;

                            statusAnimator.runtimeAnimatorController = null;
                            
                            playerCondition.spriteRenderer.sprite = null;

                        break;
                    }

                break;
            }
        }
    }
}
