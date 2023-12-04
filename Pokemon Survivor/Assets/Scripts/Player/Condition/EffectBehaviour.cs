using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour
{

    private void Start()
    {
        Time.timeScale = 0f;
    }
    public void DestroyEffect()
    {
        Time.timeScale = 1f;

        Destroy(gameObject);
    }

    public void PlayAnimSound(string name)
    {
        FindObjectOfType<AudioManager>().PlaySound(name);
    }
}
