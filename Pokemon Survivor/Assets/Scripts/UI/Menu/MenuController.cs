using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    [Header("Data Controller")]
    public DataController dataController;

    [Header("Player and Stage Menu")]
    public static string playerName;
    public static string stageName;
    public static string stageUIName;

    private void Start()
    {
        dataController.LoadMenu();

        FindObjectOfType<AudioManager>().PlaySound("Main Menu Theme");
    }
}
