using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Collector : MonoBehaviour
{
    [Header("Evolution Stone Variables")]
    public GameObject evoStoneUI;
    public Animator evoStone;
    public Text evoStoneCuantity;

    [Header("Shiny Stone Variables")]
    public GameObject shinyStoneUI;
    public Animator shinyStone;
    public Text shinyStoneCuantity;
    int evoStones = 0;
    int shinyStones = 0;

    void Start()
    {
        evoStoneUI.SetActive(false);
        shinyStoneUI.SetActive(false);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        IItems item = collision.GetComponent<IItems>();

        if(item != null)
            item.Collect();

        switch(collision.tag)
        {
            case "EvoStone":

                evoStoneUI.SetActive(true);

                evoStone.SetTrigger("Start");

                FindObjectOfType<AudioManager>().PlaySound("Rocks");

                Destroy(collision.gameObject);

                evoStones++;

                evoStoneCuantity.text =  evoStones.ToString();

                // we do not want to save the value on our player prefs here, because if you end the run you will lose everything, 
                // we save at the end of the level

            break;

            case "ShinyStone":
                
                shinyStoneUI.SetActive(true);

                shinyStone.SetTrigger("Start");

                FindObjectOfType<AudioManager>().PlaySound("Rocks");

                Destroy(collision.gameObject);

                shinyStones++;

                shinyStoneCuantity.text =  shinyStones.ToString();

            break;

            /*case "Gem": 

                IItems item = collision.GetComponent<IItems>();

                if(item != null)
                    item.Collect();

            
            break;*/
        }
    }
}
