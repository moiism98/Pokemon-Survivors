using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuUpgrade
{
    public Toggle[] toggles;
    public Sprite icon;
    [TextArea(3, 5)]
    public string description;
    public int initialPrice;
    public int upgradePrice;
    public bool selected = false;
    public bool upgraded = false;
}
