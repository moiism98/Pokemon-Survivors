using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBackground : MonoBehaviour
{
    public List<Sprite> backgrounds = new List<Sprite>();
    public Image background;

    private void Start()
    {
        SelectBackground();
    }

    void SelectBackground()
    {
        Sprite selectedBackground = backgrounds[Random.Range(0, backgrounds.Count)];
        background.sprite = selectedBackground;
    }
}
