using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
public enum UpgradeType
{
    health,
    damage,
    moveSpeed,
    exp,
    expRange,
    resurrection

}
public class UpgradeButton: MonoBehaviour
{
    [Header("Upgrades values")]
    public UpgradeType uType = UpgradeType.health;
    public static float healthValue = .10f;
    public static float speedValue = .15f;
    public static float damageValue = .50f;
    public static float expValue = .25f;
    public static float expRangeValue = .20f;
    public static int resurrection = 1;

    [Header("Data Controller")]
    public DataController dataController;

    [Header("Upgrade UI")]
    public Image descriptionIcon;
    public Text descriptionText;
    public GameObject fullUpgraded;
    public GameObject upgradeButton;
    public Text valueButton;
    public MenuUpgrade upgrade;

    // update with the button values
    void Update()
    {
        UpdateUpgradeButtonValue();
        CheckUpgraded();
    }
    public void ShowUpgradeDescription()
    {
        descriptionIcon.sprite = upgrade.icon;
        descriptionText.text = upgrade.description;
        valueButton.text = upgrade.upgradePrice.ToString();
    }

    public void SelectUpgrade()
    {
        // we check which upgrade button are we pressing down
        SelectUpgradeButton();
    }

    public void ShowFullUpgradedText()
    {
        upgradeButton.SetActive(false);
        fullUpgraded.SetActive(true);
    }

    public void HideFullUpgradedText()
    {
        upgradeButton.SetActive(true);
        fullUpgraded.SetActive(false);
    }

    void SelectUpgradeButton()
    {
        // when we click on a button, we have to diselect the others

        UpgradeButton[] buttons = FindObjectsOfType<UpgradeButton>();
        if(buttons != null && buttons.Length != 0)
        {
            foreach(UpgradeButton button in buttons) // for each button we diselected them
                button.upgrade.selected = false;
        }

        upgrade.selected = true; // we are selecting the current button

        CheckUpgraded(); // if it's fully upgraded we show the text
    }
    public void CheckUpgraded()
    {
        if(upgrade.selected)
        {
        
            int count = 0;
            foreach(Toggle upgrade in upgrade.toggles)
            {
                if(upgrade.isOn)
                    count++;
            }

            if(count != upgrade.toggles.Length)
                HideFullUpgradedText();
            else
                ShowFullUpgradedText();
        }
    }
    void UpdateUpgradeButtonValue()
    {
        int price;
        switch (uType)
        {
            case UpgradeType.health:

                price = PlayerPrefs.GetInt(DataController.healthUpgradeValue);

                if(price != 0)
                    upgrade.upgradePrice = price;
                else
                    upgrade.upgradePrice = upgrade.initialPrice;

            break;

            case UpgradeType.moveSpeed:

                price = PlayerPrefs.GetInt(DataController.moveSpeedUpgradeValue);
                if(price != 0)
                    upgrade.upgradePrice = price;
                else
                    upgrade.upgradePrice = upgrade.initialPrice;

            break;

            case UpgradeType.damage:

                price = PlayerPrefs.GetInt(DataController.damageUpgradeValue);
                if(price != 0)
                    upgrade.upgradePrice = price;
                else
                    upgrade.upgradePrice = upgrade.initialPrice;

            break;

            case UpgradeType.exp:

                price = PlayerPrefs.GetInt(DataController.expUpgradeValue);
                if(price != 0)
                    upgrade.upgradePrice = price;
                else
                    upgrade.upgradePrice = upgrade.initialPrice;

            break;

            case UpgradeType.expRange:

                price = PlayerPrefs.GetInt(DataController.expRangeUpdateValue);
                if(price != 0)
                    upgrade.upgradePrice = price;
                else
                    upgrade.upgradePrice = upgrade.initialPrice;

            break;

            case UpgradeType.resurrection:

                price = PlayerPrefs.GetInt(DataController.resurrectionUpgradeValue);
                if(price != 0)
                    upgrade.upgradePrice = price;
                else
                    upgrade.upgradePrice = upgrade.initialPrice;

            break;
        }
    }
}
