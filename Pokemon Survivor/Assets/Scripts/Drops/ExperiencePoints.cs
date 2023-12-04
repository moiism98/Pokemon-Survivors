using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public enum Experience
{
    exp10,
    exp50,
    exp100,
    exp1000,
    bottle
}

public class ExperiencePoints : MonoBehaviour
{
    EXPBar expBar;
    public Experience experience = Experience.exp10;

    float exp10 = 10f;
    float exp50 = 50f;
    float exp100 = 100f;
    float exp1000 = 1000f;
    private float upgradeValue;
    public int bottleMultiplier;
    private void Start()
    {
        expBar = FindObjectOfType<EXPBar>();
        upgradeValue = PlayerPrefs.GetFloat(DataController.expUpgradeSave);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerStats player = collision.GetComponent<PlayerStats>();

            FindObjectOfType<AudioManager>().PlaySound("Exp");

            Destroy(gameObject);

            switch(experience)
            {
                case Experience.exp10:

                    expBar.expPoints.value += exp10 + (exp10 * upgradeValue);

                    player.playerExp += exp10 + (exp10 * upgradeValue);

                break;

                case Experience.exp50:

                    expBar.expPoints.value += exp50 + (exp50 * upgradeValue);

                    player.playerExp += exp50 + (exp50 * upgradeValue);

                break;

                case Experience.exp100:

                    expBar.expPoints.value += exp100 + (exp100 * upgradeValue);

                    player.playerExp += exp100 + (exp100 * upgradeValue);

                break;

                case Experience.exp1000:

                    expBar.expPoints.value += exp1000 + (exp1000 * upgradeValue);

                    player.playerExp += exp1000 + (exp1000 * upgradeValue);

                break;

                case Experience.bottle:

                    expBar.expPoints.value += player.playerMaxExp * bottleMultiplier;

                    player.playerExp += player.playerMaxExp * bottleMultiplier;

                break;
            }
        }
    }

}
