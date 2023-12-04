using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Gem : MonoBehaviour, IItems
{
    public static event Action<int> OnGemCollect;
    public int gemValue = 1000;
    public void Collect()
    {
        // with the events we can trigger some functionalities in other scripts to do some actions

        OnGemCollect.Invoke(gemValue); // you can give whatever you want on the invoke and use it, in this case our gem value

        Destroy(gameObject);

        FindObjectOfType<AudioManager>().PlaySound("Gem");
    }
}
