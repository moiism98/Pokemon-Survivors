using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRevive : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void EndReviveAnimation()
    {
        Animator playerAnimator = player.GetComponent<Animator>();

        // we reset the animator when the player has revived

        if(playerAnimator != null)
            playerAnimator.SetFloat("Damage", 0);

        // we turn back time to normal

        Time.timeScale = 1f;

        // and destroy the animation

        Object.Destroy(gameObject, .5f);
    }
}
