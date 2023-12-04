using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UI;

public class DataController: MonoBehaviour
{
    #region PlayerPrefs
    
    [Header("Stats values")]
    public static string pokeMoneySave = "PokeMoney";
    public static string evoStoneSave = "EvoStone";
    public static string shinyStoneSave = "ShinyStone";
    
    public static string healthUpgradeSave = "HealthUpgrade";
    public static string moveSpeedUpgradeSave = "MoveSpeedUpgrade";
    public static string damageUpgradeSave = "DamageUpgrade";
    public static string expUpgradeSave = "ExpUpgrade";
    public static string expRangeSave = "ExpRangeUpgrade";
    public static string resurrectionSave = "ResurrectionUpgrade";

    [Header("Stone Evolution Values")]
    public static string pikachuStones = "pikachuStones";
    public static string machopStones = "machopStones";
    public static string eeveeStones = "eeveeStones";

    [Header("Shiny Stone Values")]
    public static string pikachuShiny = "pikachuShiny";
    public static string machopShiny = "machopShiny";
    public static string eeveeShiny = "eeveeShiny";
    public static string mewShiny = "mewShiny";

    [Header("Block Values")]
    public static string mewLocked = "mewLocked";
    public static string completedStages = "completedStages";

    [Header("Completed Levels")]
    public static string tutorial = "tutorial";
    public static string cerulean = "cerulean";
    public static string jungle = "jungle";
    public static string crater = "crater";

    [Header("Upgrade button text values")]
    public static string healthUpgradeValue = "HealthUpgradeValue";
    public static string moveSpeedUpgradeValue = "MoveSpeedUpgradeValue";
    public static string damageUpgradeValue = "DamageUpgradeValue";
    public static string expUpgradeValue = "ExpUpgradeValue";
    public static string expRangeUpdateValue = "ExpRangeUpdateValue";
    public static string resurrectionUpgradeValue = "ResurrectionUpgradeValue";

    public static string gemAmount = "gems";
    #endregion

    [Header("Stuff to save and load")]
    public Text pokeMoney;
    public Text evoStones;
    public Text shinyStones;
    public SelectPlayer selectPlayer;
    private UpgradeButton[] upgradeButtons;
    private UpgradeButton upgradeButton;

    // we have to check every frame if we have fully upgrade a stat, with this we can show the full upgraded message!
    private void Update()
    {
        if(upgradeButton != null)
            upgradeButton.CheckUpgraded();
        
    }

    #region Data control methods
    // we split the player and the other stuff bc at menu we only want to load the money amount
    // and save the stats which are going to be aplied to our player selected at the beggining
    // of the game, so at menu we only want to load the money and save player stats
    // and at the game, we want the opposite, load the player stats and save the money when the game its over.
    
    public void SaveGame()
    {
        // with PlayerPrefs we can save our type of data (floats, strings, ints or eveng bigger things)

        // saving PokeMoney value

        int savedMoney = PlayerPrefs.GetInt(pokeMoneySave) + GameController.PokeMoney;

        PlayerPrefs.SetInt(pokeMoneySave, savedMoney);

        // saving Evolution Stone value

        int savedEvoStones = PlayerPrefs.GetInt(evoStoneSave) + GameController.EvoStones;

        PlayerPrefs.SetInt(evoStoneSave, savedEvoStones);

        // saving Shiny Stone value

        int shinyStones = PlayerPrefs.GetInt(shinyStoneSave) + GameController.ShinyStones;

        PlayerPrefs.SetInt(shinyStoneSave, shinyStones);

        //saving gems value

        PlayerPrefs.SetInt(gemAmount, GameController.Gems);


    }

    public void SaveMenuMoney()
    {
        int money = int.Parse(pokeMoney.text);
        PlayerPrefs.SetInt(pokeMoneySave, money);
    }

    public void LoadMenu() 
    {
        // we get this in our menu pokemoney variable
        // and display it automatically.

        pokeMoney.text = PlayerPrefs.GetInt(pokeMoneySave).ToString();
        evoStones.text = PlayerPrefs.GetInt(evoStoneSave).ToString();
        shinyStones.text = PlayerPrefs.GetInt(shinyStoneSave).ToString();
        
    } 

    public void LoadPlayerStats()
    {
        // this method should apply directly the stats to the selected player

        // apply new max health value

        PlayerStats stats = FindObjectOfType<PlayerStats>();
        stats.playerMaxHealth += stats.playerMaxHealth * PlayerPrefs.GetFloat(healthUpgradeSave);

        // apply new movement speed and magneto range

        PlayerMovement movement = FindObjectOfType<PlayerMovement>();
        movement.initialMoveSpeed += movement.initialMoveSpeed * PlayerPrefs.GetFloat(moveSpeedUpgradeSave);
        movement.magnetoRange += movement.magnetoRange * PlayerPrefs.GetFloat(expRangeSave);

        // apply new damage

        PlayerAttack attack = FindObjectOfType<PlayerAttack>();
        attack.damage += attack.damage * PlayerPrefs.GetFloat(damageUpgradeSave);

        // load saved gem value.

        Text gems = GameObject.Find("Gems").GetComponent<Text>();

        if(gems != null)
            GameController.Gems = PlayerPrefs.GetInt(gemAmount);

    }

    public void DeleteData()
    {
        // delete all game data

        PlayerPrefs.DeleteAll();

        // update the texts

        pokeMoney.text = "0";
        evoStones.text = "0";
        shinyStones.text = "0";

        // update upgrade UI

        DeleteToggles();

        // update select player menu UI

        selectPlayer.ResetSelectableList(); // reset the list with the current players
        selectPlayer.AddPlayers(); // and load the clean one
    }

    #endregion

    #region Upgrade methods

    // it's called when an upgrade button is pressed
    public void GetUpgradeButtons()
    {
        upgradeButtons = FindObjectsOfType<UpgradeButton>();
        LoadToggles();
    }

    // it's called when you press the buy upgrade button
    public void UpgradeStats()
    {
        int pokeMoneyValue = 0;
        if(int.TryParse(pokeMoney.text, out int pkm))
            pokeMoneyValue = pkm;

        if(upgradeButtons != null && upgradeButtons.Length != 0)
        {
            int upgradeCount = 0;
            while(upgradeCount < upgradeButtons.Length)
            {
                MenuUpgrade upgrade = upgradeButtons[upgradeCount].upgrade;
                upgradeButton = upgradeButtons[upgradeCount];

                if(upgrade.selected && pokeMoneyValue >= upgrade.upgradePrice)
                {

                    FindObjectOfType<AudioManager>().PlaySound("Upgrade");

                    switch(upgradeButton.uType)
                    {
                        case UpgradeType.health:

                            if(!CheckUpgradeToggles(upgrade, upgradeButton))
                            {
                                Upgrade(pokeMoneyValue, upgrade.upgradePrice, upgrade.toggles); // we upgrade the stat

                                // and save his new stat value

                                float healthValue = PlayerPrefs.GetFloat(healthUpgradeSave);

                                healthValue += UpgradeButton.healthValue;

                                PlayerPrefs.SetFloat(healthUpgradeSave, healthValue);

                                // and also the new upgrade value
                                
                                PlayerPrefs.SetInt(healthUpgradeValue, upgrade.upgradePrice * 2);

                                // set this values to change the text button text on update method

                                upgradeButton.valueButton.text = PlayerPrefs.GetInt(healthUpgradeValue).ToString();

                                // we end the loop

                                upgradeCount = upgradeButtons.Length;

                            }
                            
                        break;

                        case UpgradeType.moveSpeed:

                            if(!CheckUpgradeToggles(upgrade, upgradeButton))
                            {
                                Upgrade(pokeMoneyValue, upgrade.upgradePrice, upgrade.toggles);

                                float moveSpeedValue = PlayerPrefs.GetFloat(moveSpeedUpgradeSave);

                                moveSpeedValue += UpgradeButton.speedValue;

                                PlayerPrefs.SetFloat(moveSpeedUpgradeSave, moveSpeedValue);

                                PlayerPrefs.SetInt(moveSpeedUpgradeValue, upgrade.upgradePrice * 2);

                                upgradeButton.valueButton.text = PlayerPrefs.GetInt(moveSpeedUpgradeValue).ToString();

                                upgradeCount = upgradeButtons.Length;

                            }

                        break;

                        case UpgradeType.damage:

                            if(!CheckUpgradeToggles(upgrade, upgradeButton))
                            {
                                Upgrade(pokeMoneyValue, upgrade.upgradePrice, upgrade.toggles);

                                float damageValue = PlayerPrefs.GetFloat(damageUpgradeSave);

                                damageValue += UpgradeButton.damageValue;

                                PlayerPrefs.SetFloat(damageUpgradeSave, damageValue);

                                PlayerPrefs.SetInt(damageUpgradeValue, upgrade.upgradePrice * 2);

                                upgradeButton.valueButton.text = PlayerPrefs.GetInt(damageUpgradeValue).ToString();

                                upgradeCount = upgradeButtons.Length;

                            }

                        break;

                        case UpgradeType.exp:

                            if(!CheckUpgradeToggles(upgrade, upgradeButton))
                            {
                                Upgrade(pokeMoneyValue, upgrade.upgradePrice, upgrade.toggles);

                                float expValue = PlayerPrefs.GetFloat(expUpgradeSave);

                                expValue += UpgradeButton.expValue;

                                PlayerPrefs.SetFloat(expUpgradeSave, expValue);

                                PlayerPrefs.SetInt(expUpgradeValue, upgrade.upgradePrice * 2);

                                upgradeButton.valueButton.text = PlayerPrefs.GetInt(expUpgradeValue).ToString();

                                upgradeCount = upgradeButtons.Length;

                            }

                        break;

                        case UpgradeType.expRange:

                            if(!CheckUpgradeToggles(upgrade, upgradeButton))
                            {
                                Upgrade(pokeMoneyValue, upgrade.upgradePrice, upgrade.toggles);

                                float expRangeValue = PlayerPrefs.GetFloat(expRangeSave);

                                expRangeValue += UpgradeButton.expRangeValue;

                                PlayerPrefs.SetFloat(expRangeSave, expRangeValue);

                                PlayerPrefs.SetInt(expUpgradeValue, upgrade.upgradePrice * 2);

                                upgradeButton.valueButton.text = PlayerPrefs.GetInt(expUpgradeValue).ToString();

                                upgradeCount = upgradeButtons.Length;

                            }

                        break;

                        case UpgradeType.resurrection:

                            if(!CheckUpgradeToggles(upgrade, upgradeButton))
                            {
                                Upgrade(pokeMoneyValue, upgrade.upgradePrice, upgrade.toggles);

                                PlayerPrefs.SetInt(resurrectionSave, 1);

                                PlayerPrefs.SetInt(resurrectionUpgradeValue, upgrade.upgradePrice * 2);

                                upgradeButton.valueButton.text = PlayerPrefs.GetInt(resurrectionUpgradeValue).ToString();

                                upgradeCount = upgradeButtons.Length;

                            }

                        break;
                    }
                }
                upgradeCount++;
            }
        }
    }

    void LoadToggles()
    {
        // take our upgrades values and activates their upgrades toggles 

        if(upgradeButtons != null)
        {
            foreach(UpgradeButton button in upgradeButtons)
            {
                switch(button.uType)
                {
                    case UpgradeType.health:

                        int healthToggleCount = (int)(PlayerPrefs.GetFloat(healthUpgradeSave) / UpgradeButton.healthValue);

                        if(healthToggleCount != 0)
                        {
                            for(int count = 0; count < healthToggleCount; count++)
                                button.upgrade.toggles[count].isOn = true;
                        }

                    break;

                    case UpgradeType.moveSpeed:

                        int moveSpeedToggleCount = (int)(PlayerPrefs.GetFloat(moveSpeedUpgradeSave) / UpgradeButton.speedValue);

                        if(moveSpeedToggleCount != 0)
                        {
                            for(int count = 0; count < moveSpeedToggleCount; count++)
                                button.upgrade.toggles[count].isOn = true;
                        }

                    break;

                    case UpgradeType.damage:

                        int damageToggleCount = (int)(PlayerPrefs.GetFloat(damageUpgradeSave) / UpgradeButton.damageValue);

                        if(damageToggleCount != 0)
                        {
                            for(int count = 0; count < damageToggleCount; count++)
                                button.upgrade.toggles[count].isOn = true;
                        }

                    break;

                    case UpgradeType.exp:

                        int expToggleCount = (int)(PlayerPrefs.GetFloat(expUpgradeSave) / UpgradeButton.expValue);

                        if(expToggleCount != 0)
                        {
                            for(int count = 0; count < expToggleCount; count++)
                                button.upgrade.toggles[count].isOn = true;
                        }

                    break;

                    case UpgradeType.expRange:

                        float expRangeToggleCount = PlayerPrefs.GetFloat(expRangeSave) / UpgradeButton.expRangeValue;
                        if(expRangeToggleCount != 0)
                        {
                            for(int count = 0; count < expRangeToggleCount; count++)
                                button.upgrade.toggles[count].isOn = true;
                        }

                    break;

                    case UpgradeType.resurrection:

                        int resurrectionToggleCount = PlayerPrefs.GetInt(resurrectionSave) / UpgradeButton.resurrection;
                    
                        if(resurrectionToggleCount != 0)
                        {
                            for(int count = 0; count < resurrectionToggleCount; count++)
                                button.upgrade.toggles[count].isOn = true;
                        }

                    break;
                }
            }
        }
    }

    void DeleteToggles()
    {
        // update UI toggles

        if(upgradeButtons != null)
        {
            foreach(UpgradeButton button in upgradeButtons)
            {   
                button.upgrade.upgradePrice = button.upgrade.initialPrice;
                foreach (Toggle toggle in button.upgrade.toggles)
                    toggle.isOn = false;
            }
        }
    }

    void Upgrade(int pokeMoneyValue, int upgradePrice, Toggle[] upgradeToggles)
    {
        // discount the money

        UpdatePokeMoney(pokeMoneyValue, upgradePrice);

        // activate the upgrade (the first toggle off)
        
        ActivateUpgrade(upgradeToggles);
    }

    void UpdatePokeMoney(int pokeMoneyValue, int upgradeValue)
    {
        pokeMoneyValue -= upgradeValue;
        pokeMoney.text = pokeMoneyValue.ToString();
    }

    void ActivateUpgrade(Toggle[] upgrades)
    {
        // with this code we only activate the first toggle he detects its off and exits the loop

        int upgrade = 0;
        while(upgrade < upgrades.Length)
        {
            if(!upgrades[upgrade].isOn)
            {
                upgrades[upgrade].isOn = true;
                upgrade = upgrades.Length;
            }
            upgrade++;
        }
    }

    bool CheckUpgradeToggles(MenuUpgrade currentUpgrade, UpgradeButton upgradeButton)
    {
        bool areAllChecked = false;
        int count = 0;

        foreach(Toggle upgrade in currentUpgrade.toggles)
        {
            if(upgrade.isOn)
                count++;
        }

        // if all toggles are on, we return true in order to stop adding more upgrades than you can buy

        if(count == currentUpgrade.toggles.Length)
            areAllChecked = true;

        return areAllChecked;
    }

    #endregion
}
