using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrongBarIcons : MonoBehaviour
{
    public List<Sprite> icons = new List<Sprite>();
    public Image image;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            int icon = 0;
            while(icon < icons.Count)
            {
                if (player.name.ToLower().Contains(icons[icon].name))
                {
                    image.sprite = icons[icon];
                    icon = icons.Count;
                }
                    icon++;
            }
        }
    }
}
