using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStats : MonoBehaviour
{
    public Text damage;
    public Text speed;
    public Text magneto;

    private PlayerAttack playerAttack;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();

        playerMovement = FindObjectOfType<PlayerMovement>();

    }
    void Update()
    {
        damage.text = (MathF.Truncate(playerAttack.damage * 100) / 100).ToString();

        speed.text = (MathF.Truncate(playerMovement.movementSpeed * 100) / 100).ToString();

        magneto.text = (MathF.Truncate(playerMovement.magnetoRange * 100) / 100).ToString();
    }
}
